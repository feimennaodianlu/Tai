using UnityEngine;

/*  时间： 2016.6.28  周二  v1 提交可使用版本
 * ————————————————————————————————————————————————————— 
 * 
 *	功能说明：	作为自制 ui 元素的基类，是一个抽象类。
 * 
 *	使用说明：
 *				1. 提供最基础功能： Destroy, Show, Initialize, ParentWindow, ParentTransform 
 *	
 * 
 *	后续待扩展：暂无 
 *				 
 * 
 * 
 *  注意事项：
 *				1. 如果子类中某个函数或属性需要改变其代码，则考虑把本脚本中对应部分变成 virtual
 */

namespace Tai.Client
{
	public abstract class UIElement : MonoBehaviour
	{
		RectTransform m_rectTransform;

		protected void InitializeElement(UIWindow parentWindow, Transform parentTransform)
		{
			Transform parent;
			if( null == parentWindow )
				parent = parentTransform ?? UIRoot.RootCanvas.transform;
			else
				parent = parentTransform ?? parentWindow.transform ?? UIRoot.RootCanvas.transform;

			transform.SetParent( parent, false );
		}

		public virtual void Destroy()
		{
			Destroy( gameObject );
		}

		public virtual void OnShow(bool show)
		{
			ObjectHelper.SetActive( gameObject, show );
		}

		#region Properties

		public bool Show
		{
			get { return gameObject.activeSelf; }
			set
			{
				if( value != Show )
					OnShow( value );
			}
		}

		public UIRoot UIRoot
		{
			get { return GameManager.Instance.UIRoot; }
		}

		public RectTransform RectTransform
		{
			get
			{
				if( null == m_rectTransform )
					m_rectTransform = transform as RectTransform;

				return m_rectTransform;
			}
		}

		#endregion
	}
}