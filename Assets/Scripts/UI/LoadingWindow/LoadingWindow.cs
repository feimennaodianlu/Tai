using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tai.Client;
using UnityEngine;
using UnityEngine.UI;

/*  时间： 2016.6.28  周二  v1 提交可使用版本
 * ————————————————————————————————————————————————————————
 * 
 *	功能说明：	LoadingWindow 可定制背景（ModelBg、CustomImage、None一般不用）。
 * 
 *	使用说明：
 *			步骤1： var loadingWindow = LoadingWindow.Create("xxx");
 *			步骤2： IEnumerator LoadScene(xxx)
					{
						.....
						yield return m_loadingWindow.Play(xxx);
					}				
 *		
 *	后续待扩展：  progressbar 在加载过程中平滑过渡	        
 * 
 *  注意事项：  暂无
 *  
 */

public class LoadingWindow : UIWindow, IEnumerator
{
	[SerializeField]
	Slider m_progressbar = null;

	[SerializeField]
	RawImage m_modelBackground = null;

	List<Task> m_tasks = new List<Task>();

	public enum BackgroundType
	{
		ModelBackground,
		CustomImage,
		None
	}

	#region Content

	public class Content
	{
		public UIWindow ParentWindow;
		public Transform ParentTransform;
		public BackgroundType BgType;
		public Texture2D CustomTextuer;

		public Content(UIWindow parentWindow, Transform parentTransform, BackgroundType bgType = BackgroundType.ModelBackground, Texture2D image = null)
		{
			ParentWindow = parentWindow;
			ParentTransform = parentTransform;
			BgType = bgType;
		}
	}

	#endregion

	#region Task

	public class Task
	{
		public AsyncOperation Asyncs;
		public float Weight;
		public Func<float> ProgressGetter;

		public Task(AsyncOperation async, float weight, Func<float> progressGetter)
		{
			Weight = weight;
			ProgressGetter = progressGetter;
			Asyncs = async;
		}
	}

	#endregion

	#region Create & Initialize

	public static LoadingWindow Create(Content content)
	{
		ObjectHelper.IfNullThrowArgument( content, "content" );

		var asset = GameManager.Instance.GameLoad.Load<LoadingWindow>( Path.LoadingWindow );

		ObjectHelper.IfNullThrowArgument( asset, "asset" );

		var loadingWindow = Instantiate<LoadingWindow>( asset );
		loadingWindow.Initialize( content );

		return loadingWindow;
	}

	void Initialize(Content content)
	{
		base.InitializeWindow( content.ParentWindow, content.ParentTransform, false );  // 此处始终为false。LoadingWindow 的模态背景由 enum BackgroundType 定

		m_progressbar.value = 0f;

		switch( content.BgType )
		{
			case BackgroundType.ModelBackground:
				UIModelBackground.Create( this );
				break;

			case BackgroundType.CustomImage:
				ObjectHelper.SetActive( m_modelBackground.gameObject, true );
				m_modelBackground.texture = content.CustomTextuer;
				break;

			case BackgroundType.None:
				break;

			default:
				ObjectHelper.ThrowEnumSpecializedArgument( content.BgType.ToString() );
				break;
		}
	}

	#endregion

	/// <summary>
	/// 所有 Task 的 Weight 之和应为 1。eg: weight = 1f / TaskCount
	/// </summary>
	public IEnumerator Play(AsyncOperation async, float weight)
	{
		m_tasks.Add( new Task( async, weight, () => async.progress ) );
		return this;
	}

	void Update()
	{
		m_progressbar.value = m_tasks.Sum( t => t.ProgressGetter() * t.Weight );

		if( m_progressbar.value >= 1f )
			Destroy();
	}

	#region IEnumerator

	object IEnumerator.Current
	{
		get
		{
			var current = m_tasks.FirstOrDefault( t => t.ProgressGetter() < 1 );
			return current != null ? current.Asyncs : null;
		}
	}

	bool IEnumerator.MoveNext()
	{
		return m_tasks.Any( t => t.ProgressGetter() < 1 );
	}

	void IEnumerator.Reset()
	{
		// 一般用于重置 index = -1
		throw new NotImplementedException();
	}

	#endregion
}