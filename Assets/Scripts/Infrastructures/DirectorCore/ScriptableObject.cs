using System;
using System.Collections.Generic;
using System.Threading;

/*  时间： 2016.7.16  周六  v1 提交可使用版本
 * ————————————————————————————————————————————————————————
 *	功能说明：	作为所有需要实现 IDisposable 接口的基类，这样之后的类需要实现该接口时只要继承自它 或 TaiObject 即可
 * 
 *	使用说明：
 *				某个类需要实现 IDisposable 接口，则继承自 DiposableObject
 *						
 *	后续待扩展：  暂无
 * 
 *  注意事项：    学习 System.Threading            
 */

namespace Dvm
{
	public class DisposableObject : IDisposable
	{
		int m_disposed;

		protected void CheckDisposed()
		{
			if( Disposed )
				throw new ObjectDisposedException( base.GetType().Name );
		}

		public void Dispose()
		{
			Dispose( true );
		}

		// true： dispose Managed + Unmanaged
		// false: only dispose Unmanaged
		void Dispose(bool disposing)
		{
			if( Interlocked.CompareExchange( ref m_disposed, 1, 0 ) == 0 )
			{
				if( disposing )
					DisposeManaged();
				DisposeUnmanaged();
				GC.SuppressFinalize( this );
			}
		}

		protected virtual void DisposeManaged() { }

		protected virtual void DisposeUnmanaged() { }

		~DisposableObject()
		{
			Dispose( false );
		}

		public static void SafeDispose<T>(ref T obj) where T : IDisposable
		{
			if( obj != null )
			{
				obj.Dispose();
				obj = default( T );
			}
		}

		public static void SafeDispose<T>(IEnumerable<T> objects) where T : IDisposable
		{
			if( objects != null )
			{
				foreach( T item in objects )
				{
					if( item != null )
					{
						item.Dispose();
						//item = default( T );  // ！！ foreach 不允许改变迭代变量
					}
				}
			}
		}

		public static bool SafeDisposeReturn<T>(ref T obj) where T : IDisposable
		{
			if( obj != null )
			{
				obj.Dispose();
				obj = default( T );

				return true;
			}

			return false;
		}

		public bool Disposed
		{
			get { return m_disposed != 0; }
		}
	}
}