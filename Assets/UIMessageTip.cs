using DG.Tweening;
using Tai.Client;
using UnityEngine;
using UnityEngine.UI;

/*  时间： 2016.7.7  周四 2：27  家中		v1	 提交可使用版本
 *		   2016.7.9  周六 17：28 八佰伴		v1.1 修改模态背景显示尺寸不正确的bug
 * ————————————————————————————————————————————————————————
 * 
 *	功能说明：		一般提示的 tip 面板。有 static fade 和 bubble fade 两种模式
 * 
 *	使用说明：		1. 直接 Create 即可
 					2. 有 CreateModel 方法，快速创建带有 modelbg + staticFade 模式的UIMessageTip 
 *					
 *	后续待扩展：	增加不同的 Mode        
 * 
 *  注意事项：      若需要让中间的 tip 底边跑到 canvas 上边缘外，则其移动距离为 ( 0.5f * rootCanvasHeight + 0.5f * rectHeight ) 不是 screenHeight 
 */
[RequireComponent( typeof( CanvasGroup ) )]
public class UIMessageTip : UIWindow
{
	[SerializeField]
	Transform m_realMessageTip = null;  // 脚本挂在父物体上，但真正的uimessagetip是其子物体开始。m_realMessageTip 变量保存的就是真正的 uimessagetip

	[SerializeField]
	Text m_tip = null;

	[SerializeField]
	float m_fadeDuration = 2f;

	[SerializeField]
	float m_delayTime = 2f;

	CanvasGroup m_canvasGroup;

	public enum Mode
	{
		StaticFade,  // 在中间出现逐渐消失模式
		BubbleFade,  // 在中间出现向上逐渐消失，模态背景和该枚举是互斥事件
	}

	#region Content

	public class Content
	{
		public string Tip;
		public Mode Mode;
		public bool IsNeedModelBg;

		public Content(string tip, Mode mode = Mode.StaticFade, bool isNeedModelBg = true)
		{
			Tip = tip;
			Mode = mode;
			IsNeedModelBg = isNeedModelBg;
		}
	}

	#endregion

	#region Create * 2 & Initialize

	public static UIMessageTip Create(Content content)
	{
		var asset = GameManager.Instance.GameLoad.Load<UIMessageTip>( Path.UIMessageTip );

		var go = Instantiate<UIMessageTip>( asset );
		go.Initialize( content );

		return go;
	}

	/// <summary>
	/// 需要modelBackground，且为staticFade模式 
	/// </summary>
	public static UIMessageTip CreateModel(string tip)
	{
		return UIMessageTip.Create( new Content( tip, Mode.StaticFade, true ) );
	}

	void Initialize(Content content)
	{
		if( null == content )
			ObjectHelper.IfNullThrowArgument( content, "content" );

		if( content.IsNeedModelBg )
			base.InitializeWindow( null, null, content.IsNeedModelBg );
		else
			base.InitializeWindow( null, null, null, 0f );

		m_canvasGroup = GetComponent<CanvasGroup>();

		m_tip.text = content.Tip;

		transform.localPosition = Vector3.zero;
		transform.SetAsLastSibling();

		switch( content.Mode )
		{
			case Mode.StaticFade:
				{
					DOTween.To( () => m_canvasGroup.alpha, a => m_canvasGroup.alpha = a, 0f, m_fadeDuration ).SetDelay( m_delayTime ).OnKill( () => Destroy() );
				}
				break;

			case Mode.BubbleFade:
				{
					var t = DOTween.To( () => m_canvasGroup.alpha, a => m_canvasGroup.alpha = a, 0f, m_fadeDuration ).SetDelay( m_delayTime )/*.OnKill( () => Destroy() )*/;

					m_realMessageTip.DOLocalMoveY(
						0.5f * UIRoot.CanvasScaler.referenceResolution.y + 0.5f * RectTransform.rect.height,
						m_fadeDuration ).SetDelay( m_delayTime );
				}
				break;

			default:
				ObjectHelper.ThrowEnumSpecializedArgument( content.Mode.ToString() );
				break;
		}
	}

	#endregion

	#region Override

	protected override void OnClickModelBg()
	{
		base.OnClickModelBg();

		Destroy();
	}

	#endregion
}