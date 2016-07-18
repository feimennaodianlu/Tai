using UnityEngine;

/*  时间： 2016.6.28  周二  v1 提交可使用版本
 * ————————————————————————————————————————————————————————
 *	功能说明：		所有可复用组件的基类。
 *						其子类有： UIModelBackground 
 * 
 *	使用说明：		需要复用的组件继承该类即可
 *		
 *	后续待扩展：	暂无
 *
 *  注意事项：		暂无
 */

namespace Tai.Client
{
	public class UIWidget : UIElement
	{
		public void InitialzeWidget(UIWindow parentWindow, Transform parentTransform)
		{
			base.InitializeElement( parentWindow, parentTransform );

			transform.SetAsFirstSibling();  // [Tai-wangyehao] 目前仅对于 UIModelBackground 有用，其它的组件没有该句也可
			transform.localPosition = Vector3.zero;
		}
	}
}