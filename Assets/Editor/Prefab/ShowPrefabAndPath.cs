using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

/*  时间： 2016.7.15  周五  v1 提交可使用版本
 * ————————————————————————————————————————————————————————
 * 
 *	功能说明：	选中场景中的物体，显示其对应的prefab及路径
 * 
 *	使用说明：
 *				1. ctrl + q 调出该窗口
 *				2. 选中多个物体时，以 Hierarchy 中最上方的为准
 *		
 *	后续待扩展： 
 *				1. 选中多个物体时，显示有prefab的物体及其prefab和路径
 * 
 *  注意事项：  
 *  
 *				1. Selection.objects 不会为null，只会数组长度为0
 *				2. Selection.objects[0] 不是指第一个选中的，而是 Hierarchy 中从上至下的第一个
 */

public class ShowPrefabAndPath : ScriptableWizard
{
	GUIStyle m_styleRichText;

	UObject m_prefab = null;
	string m_prefabPath;

	[MenuItem( "Wa/Prefab/Show Prefab Asset if exists %q" )]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<ShowPrefabAndPath>( "Show Prefab Asset if exists" );
	}

	void OnEnable()
	{
		m_styleRichText = new GUIStyle()
		{
			richText = true
		};

		ResetPrefabAndPath();
	}

	void OnGUI()
	{
		EditorGUILayout.BeginVertical();

		// Title
		if( Selection.objects.Length == 0 )
			EditorGUILayout.LabelField( "<color=Yellow>请选中 Hierarchy 视图中的一个物体</color><color=white>（选择多个则以Hierarchy视图中第一个为准）</color>", m_styleRichText );
		else
			EditorGUILayout.LabelField( string.Format( "<color=white>当前场景选中的物体是</color> <color=cyan>{0}</color>", Selection.objects[0].name ), m_styleRichText );

		// Prefab
		if( Selection.objects.Length != 0 )
		{
			if( m_prefab != null )
			{
				EditorGUILayout.ObjectField( m_prefab, typeof( UObject ), false );
				EditorGUILayout.TextField( "含前缀   Assets/", m_prefabPath );
				EditorGUILayout.TextField( "不含前缀 Assets/", m_prefabPath.TrimStart( "Assets/".ToCharArray() ) );
			}
			else
				EditorGUILayout.LabelField( "<color=yellow>没有对应的prefab</color>", m_styleRichText );
		}

		EditorGUILayout.EndVertical();
	}

	void OnSelectionChange()
	{
		ResetPrefabAndPath();
	}

	void OnInspectorUpdate()
	{
		Repaint();  // 必须有，否则选中场景物体，再打开窗口不会正确显示。因为此时没有触发 OnSelectionChange
	}

	void ResetPrefabAndPath()
	{
		if( Selection.objects.Length == 0 )
		{
			m_prefab = null;
			m_prefabPath = null;
			return;
		}

		m_prefab = PrefabUtility.GetPrefabParent( Selection.objects[0] );  // Selection.objects[0] 不是指第一个选中的，而是 Hierarchy 中从上至下的第一个
		m_prefabPath = AssetDatabase.GetAssetPath( m_prefab );

		if( Selection.objects.Length > 1 )
			Debug.LogWarning( "选中多个物体只有第一个有效" );
	}
}