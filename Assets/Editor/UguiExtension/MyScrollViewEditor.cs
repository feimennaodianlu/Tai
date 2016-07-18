using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.UI;

[CustomEditor( typeof( MyScrollView ) ), CanEditMultipleObjects]
public class MyScrollViewEditor : ScrollRectEditor
{
	SerializedProperty m_dragMask;

	protected override void OnEnable()
	{
		base.OnEnable();

		m_dragMask = base.serializedObject.FindProperty( "m_dragMask" );
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUILayout.Space();

		var guiStyle = new GUIStyle()
		{
			richText = true
		};
		EditorGUILayout.LabelField( "<color=yellow>派生的地方</color>", guiStyle );
		EditorGUILayout.PropertyField( m_dragMask, new GUILayoutOption[0] );

		serializedObject.ApplyModifiedProperties();
	}
}
