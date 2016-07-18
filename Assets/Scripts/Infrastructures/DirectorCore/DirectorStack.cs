using System.Collections;
using System.Collections.Generic;

/*  时间： 2016.7.16  周六  v1 提交可使用版本
 * ————————————————————————————————————————————————————————
 *	功能说明：	管理所有的 Director。
 * 
 *	使用说明：
 *				1. 在 GameManager 的 Init() 中实例化
 *				2. 其 Update 函数如果返回 false，则游戏结束
 *				3. 在 GameManager 中需要实现 IHandler.CreateRootDirector()
 *				
 *		
 *	后续待扩展：  暂无    
 * 
 *  注意事项：  1. DirecotrStack 不用 stack 改用 list 实现的 原因之一是： 不能 得到 BottomDirector
 *				2. Director 架构我们需要关注两个返回值。1. 栈顶Director返回null表明弹出栈 2. 本类中的 Update 返回false游戏结束
 */

namespace Tai.Client
{
	sealed class DirectorStack : TaiObject, IEnumerable<GameDirector>
	{
		IHandler m_handler;
		List<GameDirector> m_stack = new List<GameDirector>();

		public interface IHandler
		{
			GameDirector CreateRootDirector();
			void OnPop(GameDirector pop, GameDirector newTop);
			void OnPush(GameDirector push, GameDirector lastTop);
		}

		public DirectorStack(IHandler handler)
		{
			m_handler = handler;

			m_stack.Add( m_handler.CreateRootDirector() );
		}

		public bool Update()
		{
			for( ; ; )
			{
				var top = TopDirector;

				var result = top.Update();  // 有三种结果。1. null => 弹出栈 2. 等于top => Noop 3. newDirector => 压入栈
				if( result == null )
				{
					m_stack.RemoveAt( m_stack.Count - 1 );
					top.OnPop();   // 栈顶 director 弹出栈时调用

					m_handler.OnPop( top, result );  // DirectorStack 在有元素弹出时的操作

					if( m_stack.Count == 0 )
						return false;
				}
				else if( result != top )
				{
					m_stack.Add( result );
					m_handler.OnPush( result, top );
				}

				return true;
			}
		}

		public bool Add(GameDirector director)
		{
			var top = TopDirector;

			if( top != null && top.GetType() == director.GetType() )  // 判断是否是同一个类
				return false;

			m_stack.Add( director );

			return true;
		}

		public void Reset()
		{
			if( m_stack.Count > 0 )
			{
				foreach( var director in m_stack )
					director.Dispose();

				m_stack.Clear();
			}

			m_stack.Add( m_handler.CreateRootDirector() );
		}

		#region IEnumerable

		IEnumerator<GameDirector> IEnumerable<GameDirector>.GetEnumerator()
		{
			return m_stack.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_stack.GetEnumerator();
		}

		#endregion

		#region Properties

		public GameDirector TopDirector
		{
			get { return m_stack.Count != 0 ? m_stack[m_stack.Count - 1] : null; }
		}

		public GameDirector BottomDirector
		{
			get { return m_stack.Count != 0 ? m_stack[0] : null; }
		}

		#endregion
	}
}