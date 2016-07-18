using System;
using UnityEditor;
using UnityEngine;

/*  时间： 2016.7.15  周五  v1 提交可使用版本
 * ————————————————————————————————————————————————————————
 * 
 *	功能说明：	拖入场景中的物体，创建相应的prefab
 * 
 *	使用说明：
 *			步骤1： 调出窗口
 *			步骤2： 给数组赋值，填写需要生成prefab的物体的数量				
 *			步骤3： 把每个场景物体拖入相应栏
 *			步骤4： 填写prefab路径
 *			步骤5： 点击创建 => 会得到prefab的引用
 *		
 *	后续待扩展： 暂无	        
 * 
 *  注意事项：   
 *			1.  PrefabUtility.CreatePrefab( m_prefabPath, m_sceneGo );  // m_prefabPath 必须以Assets/开头， .prefab 结尾，即需要扩展名
 *	
 *			2.  [Seriablizable] 类在 Inpector 是创建时，其字段的值是默认值，不是我们在定义类时设的值（eg: 即使定义了某bool字段为true，在生成类对象时，该bool字段仍为false）
 *			
 *			3.  EditorWindow.OnInspectorUpdate() 每秒调用10次
 *				MonoBehaviour.OnGUI()  每 帧 调用多次		
 *				EditorWindow.OnGUI() 文档没说
 *			
 *			5.  PrefabUtility.CreatePrefab 默认创建出的prefab和场景中的物体没有关联，还需要添加 PrefabUtility.ConnectGameObjectToPrefab， 在创建时 设置 ReplacePrefabOptions.ConnectToPrefab 
 *			
 *			6.  base.DrawWizardGUI();  // 如果没有该句，则定义了 OnGUI 后会把父类的视图给去掉
 */

public class CreatePrefab : ScriptableWizard
{
	[SerializeField]
	PrefabCreation[] m_creations = null;

	[MenuItem( "Wa/Prefab/Create Prefab" )]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<CreatePrefab>( "Create Prefab" );
	}

	void OnGUI()
	{
		base.DrawWizardGUI();  // 如果没有该句，则定义了 OnGUI 后会把父类的视图给去掉

		if( m_creations == null ) return;

		EditorGUILayout.BeginVertical();

		for( var i = 0; i < m_creations.Length; i++ )
		{
			var c = m_creations[i];

			c.Show = EditorGUILayout.Foldout( c.Show, i.ToString() );

			if( !c.Show )
				continue;

			c.SceneGo = EditorGUILayout.ObjectField( "场景中物体", c.SceneGo, typeof( GameObject ), true ) as GameObject;

			if( c.SceneGo != null )
			{
				c.PrefabPath = EditorGUILayout.TextArea( string.Format( "Assets/{0}.prefab", c.SceneGo.name ) );

				c.Option = (ReplacePrefabOptions)EditorGUILayout.EnumPopup( c.Option );

				if( GUILayout.Button( "创建prefab" ) )
					c.Prefab = PrefabUtility.CreatePrefab( c.PrefabPath, c.SceneGo, c.Option );

				if( c.Prefab != null )
				{
					EditorGUILayout.LabelField( "创建成功" );
					EditorGUILayout.ObjectField( c.Prefab, typeof( GameObject ), false );
				}
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}

		EditorGUILayout.EndVertical();
	}

	void OnInspectorUpdate()
	{
		Repaint();
	}

	[Serializable]
	class PrefabCreation
	{
		public GameObject SceneGo = null;
		public GameObject Prefab = null;
		public string PrefabPath = "";
		public bool Show = true;
		public ReplacePrefabOptions Option = ReplacePrefabOptions.ConnectToPrefab;
	}
}