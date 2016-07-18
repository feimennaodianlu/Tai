using System;

/*  时间： 2016.7.16  周六  v1 提交可使用版本
 * ————————————————————————————————————————————————————————
 *	功能说明：	Director 基类，提供 onPop（带 Dispose） + Update + FindWindow
 * 
 *	使用说明：
 *				1. 子类继承GameDirector需要实现 => override Update
 *				2. director 弹出栈时，如果需要 dispose，则子类需要 => override DisposManaged / DisposeUnmanaged
 *		
 *	后续待扩展：  暂无    
 * 
 *  注意事项：    暂无            
 */

namespace Tai.Client
{
	abstract class GameDirector : TaiObject
	{
		Action<GameDirector> m_onPop;

		protected GameDirector(Action<GameDirector> onPop = null)
		{
			m_onPop = onPop;
		}

		public abstract GameDirector Update();

		public void OnPop()
		{
			if( m_onPop != null )
			{
				m_onPop( this );
				m_onPop = null;
			}

			Dispose();  // 因为这里 Dispose，所以之后 Director 需要 Disposed 的，只要覆写 DisposeManaged 即可
		}

		public static TWindow FindWindow<TWindow>() where TWindow : UIWindow
		{
			return GameManager.Instance.UIRoot.FindWindow<TWindow>();
		}
	}
}