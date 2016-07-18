using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!! [Tai] 移到笔记本适配 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

/*  时间： 2016.6.28  周二  v1 提交可使用版本
 * ——————————————————————————————————————————————————
 * 
 *	功能说明：	1. 所有 window 的基类。
 *					   其子类有： EntryWindow（有 mainStatus）、UIMessageBox
 *				2. 每个 window 看成一个 Canvas，上面不能有任何 其它image等控件（Eg: 这样messagebox可以直接挂在window下面并 SetAsFirstSibling），因此在代码中需要设置 canvas's sorting order
 *				
 * 
 *	使用说明：  1. 作为抽象类被子类继承
 *				2. 是否 需要模态背景 在本脚本中处理
 *				3. 如果 继承的window 需要定制模态背景的点击事件，则 override OnClickModelBg()
 *				
 *		
 *	后续待扩展：  暂无。关注 Car 中的变化
 *	
 * 
 *  注意事项：  暂无
 * 
 */

namespace Tai.Client
{
	[RequireComponent( typeof( Canvas ) )]
	[RequireComponent( typeof( GraphicRaycaster ) )]
	public abstract class UIWindow : UIElement
	{
		Canvas m_canvas;
		int m_accumulativeDepth;

		// [Tai-dev] parentWindow 主要用来设置 sorting order，如果 parentTransform 为 null 则用来设置 parent。如果 parentWindow 为 null 则挂在 rootCanvas 上
		protected void InitializeWindow(UIWindow parentWindow, Transform parentTransform, bool isNeedModelBg)
		{
			base.InitializeElement( parentWindow, parentTransform );

			// Model Bg
			if( isNeedModelBg )
				UIModelBackground.Create( this, OnClickModelBg );

			m_canvas = GetComponent<Canvas>();
			m_canvas.overrideSorting = true;

			var canvases = GetComponentsInChildren<Canvas>( true )
							.OrderBy( c => c.sortingOrder )
							.ToArray();

			int baseDepth;
			if( parentWindow != null )
				baseDepth = parentWindow.LocalMaxDepth + 1;
			else
				baseDepth = canvases[0].sortingOrder;

			for( var i = 0; i < canvases.Length; i++ )
				canvases[i].sortingOrder = baseDepth + i;

			m_accumulativeDepth = canvases.Length - 1;
		}
 
        // 用于定制模态背景的颜色
		protected void InitializeWindow(UIWindow parentWindow, Transform parentTransform, Color? modelBgColor, float? modelBgAlpha = null)
		{
			base.InitializeElement( parentWindow, parentTransform );

			InitializeWindow( parentWindow, parentTransform, false );

			UIModelBackground.Create( this, OnClickModelBg, modelBgColor, modelBgAlpha );
		}

		#region Override

		protected virtual void OnClickModelBg()
		{
		}

		#endregion

		#region Property

		// ( 挂新的 canvas 前，当前 canvas 下深度最深的 canvas 的 sorting order )
		// 保证新挂上去的 canvas 的深度是极大值
		public int LocalMaxDepth
		{
			get { return m_canvas.sortingOrder + m_accumulativeDepth; }
		}

		#endregion
	}
}