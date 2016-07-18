using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

public class ShowAssetsPath : ScriptableWizard
{
	string[] paths;
	string path = "";
	UObject go;

	GUIStyle style1;
	GUIStyle style2;

	[MenuItem( "Wa/Show Path %w" )]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<ShowAssetsPath>( "Show Assets Path" );
	}

	void OnEnable()
	{
		style1 = new GUIStyle()
		{
			fontStyle = FontStyle.Bold,
			richText = true
		};

		style2 = new GUIStyle()
		{
			richText = true
		};

		SetPaths( Selection.objects );
	}

	void OnSelectionChange()
	{
		SetPaths( Selection.objects );
		Repaint();
	}

	void OnGUI()
	{
		EditorGUILayout.BeginVertical();

		EditorGUILayout.LabelField( "<color=white>Find asset by path</color>", style1 );

		path = EditorGUILayout.TextField( path );

		if( GUILayout.Button( "Find" ) )
			OnClickBtn();

		if( go != null )
			EditorGUILayout.ObjectField( go, typeof( UObject ), false ); // allow scene objects： true 表示可以把 Hierarchy view 中的物体拖入。
		else
			EditorGUILayout.LabelField( "<color=yellow>资源未找到</color>", style2 );

		EditorGUILayout.EndVertical();

		EditorGUILayout.Space();

		EditorGUILayout.LabelField( "<color=white>Find path by selection asset</color>", style1 );

		if( paths == null ) return;

		foreach( var p in paths )
		{
			EditorGUILayout.TextField( "含Assets/前缀", p );
			EditorGUILayout.TextField( "不含前缀", p.TrimStart( "Assets/".ToCharArray() ) );
			EditorGUILayout.Space();
		}
	}

	void SetPaths(UObject[] objs)
	{
		if( objs == null ) return;

		paths = new string[Selection.objects.Length];

		for( var i = 0; i < Selection.objects.Length; i++ )
			paths[i] = AssetDatabase.GetAssetPath( Selection.objects[i] );
	}

	void OnClickBtn()
	{
		go = AssetDatabase.LoadAssetAtPath<UObject>( path );

		if( go != null )
			Repaint();
	}
}