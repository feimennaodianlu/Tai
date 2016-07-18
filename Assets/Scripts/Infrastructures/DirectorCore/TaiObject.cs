using Dvm;
using UnityEngine;
using UObject = UnityEngine.Object;

/*  时间： 2016.7.16  周六  v1 提交可使用版本
 * ————————————————————————————————————————————————————————
 *	功能说明：	 实质上和 SriptableObject.cs 的封装
 * 
 *	使用说明：   需要实现 IDiposable 接口的类继承自 TaiObject.cs
 *						
 *	后续待扩展： 暂无
 * 
 *  注意事项：   暂无
 */

public class TaiObject : DisposableObject
{
	// 传 null，则不高亮。不会报错
	public static void Log(string message, UObject obj = null)
	{
		Debug.Log( message, obj );
	}

	// 以命名规范来说需要大写，这里 p 小写是为了模仿 MonoBehaviour.print()
	public static void print(string message, UObject obj = null)
	{
		Log( message, obj );
	}

	public static void LogWarning(string message, UObject obj = null)
	{
		Debug.LogWarning( message, obj );
	}

	public static void LogError(string message, UObject obj = null)
	{
		Debug.LogError( message, obj );
	}

	public static void LogAssertion(string message, UObject obj = null)
	{
		Debug.LogAssertion( message, obj );
	}
}