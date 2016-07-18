using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*  时间： 2016.6.28  周二  v1 提交可使用版本
 *		   2016.7.9   周六  可定制颜色
 * ————————————————————————————————————————————————————————
 * 
 *	功能说明：		模态背景（可定制颜色，不能定制图片）
 *  
 *	使用说明：
 *					1. 直接调用 Create 即可。 
 *					2. Window 中的模态背景已在 UIWindow 中 Create，我们只需要在 base.Initilize 中设置 bool
 *					3. 一般需要再调用 Create 的情况，eg: UIMessageBox
 *					4. 初始化有两个重载 1. 传颜色，2. 传透明度
 *				
 *		
 *	后续待扩展：	定制图片
 * 
 *  注意事项：		无
 */

namespace Tai.Client
{
	[RequireComponent( typeof( Image ) )]
	public class UIModelBackground : UIWidget, IPointerClickHandler
	{
		Action m_action;
		Image m_modelBg;

		/// <summary>
		/// Color优先级高于alpha.即color和alpha同时赋值时，以color为准.alpha is [0,1]
		/// </summary>
		public static UIModelBackground Create(UIWindow window, Action action = null, Color? color = null, float? alpha = null)
		{
			ObjectHelper.IfNullThrowArgument( window, "window" );

			var asset = GameManager.Instance.GameLoad.Load<UIModelBackground>( Path.ModelBackground );

			var go = Instantiate<UIModelBackground>( asset );

			if( color.HasValue )
				go.Initialize( window, color.Value, action );
			else if( alpha.HasValue )
				go.Initialize( window, alpha.Value, action );
			else
				go.Initialize( window, action );

			return go;
		}

		#region Initialize * 3

		void Initialize(UIWindow window, Action action = null)
		{
			base.InitialzeWidget( window, window.transform );

			m_action = action;
		}

		void Initialize(UIWindow window, Color color, Action action = null)
		{
			Initialize( window, action );

			m_modelBg = GetComponent<Image>();
			m_modelBg.color = color;
		}

		/// <summary>
		/// alpha is [0,1]
		/// </summary>
		void Initialize(UIWindow window, float alpha, Action action = null)
		{
			Initialize( window, action );

			m_modelBg = GetComponent<Image>();
			var color = m_modelBg.color;
			color.a = alpha;
			m_modelBg.color = color;
		}

		#endregion

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			if( m_action != null )
				m_action.Invoke();
		}
	}
}