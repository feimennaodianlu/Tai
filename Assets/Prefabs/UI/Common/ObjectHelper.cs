using System;
using UnityEngine;
using UObject = UnityEngine.Object;

// 主要用于 启用/禁用组件前做个判断。激活或不激活物体前做个判断
public static class ObjectHelper
{
	public static void SetEnable(Component c, bool isEnable)
	{
		// 使用反射获得是否存在 enabled方法
	}

	// 这里的参数不是 UObject 的原因是，某些 UObject 没有 .gameObject
	public static bool SetActive(GameObject go, bool isActive)
	{
		if( go.activeInHierarchy != isActive )
			go.SetActive( isActive );

		return isActive;
	}

	public static bool IsNull(object obj)
	{
		return null == obj;
	}

	public static bool IsNotNull(object obj)
	{
		return obj != null;
	}

	#region Exception

	public static void IfNullThrowArgument(object obj, string objName)
	{
		if( null == obj )
			throw new ArgumentNullException( "variable" + objName + " should not be NULL" );
	}

	public static void ThrowEnumSpecializedArgument(string argName)
	{
		throw new ArgumentOutOfRangeException( "enum " + argName + "does not exist." );
	}

	public static void ThrowArgument(string message)
	{
		throw new ArgumentException( message );
	}

	#endregion
}