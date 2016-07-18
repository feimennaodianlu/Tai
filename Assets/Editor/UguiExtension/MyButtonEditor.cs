using UnityEditor;
using UnityEditor.UI;

[CustomEditor( typeof( MyButton ), true )]
[CanEditMultipleObjects]
public class MyButtonEditor : ButtonEditor
{
	SerializedProperty m_onPressProperty;
	SerializedProperty m_onUpProperty;

	protected override void OnEnable()
	{
		base.OnEnable();

		m_onPressProperty = serializedObject.FindProperty( "m_onPressed" );
		m_onUpProperty = serializedObject.FindProperty( "m_onUp" );
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField( m_onPressProperty );

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField( m_onUpProperty );

		serializedObject.ApplyModifiedProperties();
	}
}
