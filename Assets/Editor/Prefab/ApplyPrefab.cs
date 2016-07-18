using UnityEditor;
using UnityEngine;

/*  时间： 2016.7.15  周五  v1 提交可使用版本
 * ————————————————————————————————————————————————————————
 * 
 *	功能说明：	 通过快捷键 => 把对场景物体的修改应用到prefab
 * 
 *	使用说明：   选中场景物体，然后执行快捷键 ctrl + alt + RightArrow	
 *		
 *	后续待扩展： 打开面板时，箭头自动展开。创建若干 Creation 后，箭头自动展开
 * 
 *  注意事项：   暂无            
 */

public class ApplyPrefab : Editor
{
	[MenuItem( "Wa/Prefab/Apply Prefab %&RIGHT" )]
	static void Apply()
	{
		if( Selection.objects.Length != 0 )
		{
			var sceneGo = Selection.objects[0] as GameObject;

			var prefab = PrefabUtility.GetPrefabParent( sceneGo );	// 只对 hierarchy 视图中的prefab有用，若传入project视图中物体返回null。 => 可以通过场景中的物体找到其在project视图中的prefab

			if( prefab == null )
				Debug.LogWarning( "不是prefab的实例" );
			else
			{
				var path = AssetDatabase.GetAssetPath( prefab );

				Debug.Log( "Apply 成功 -----> " + path );
				PrefabUtility.ReplacePrefab( sceneGo, prefab );  // Core
			}

			AssetDatabase.Refresh();  // In case that  "Preferences -> Auto Fresh" are not set true
		}
		else
			Debug.LogWarning( "没有选中场景物体" );
	}
}