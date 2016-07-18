using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent( typeof( Text ) )]
public class UILocalize : MonoBehaviour
{
	[SerializeField]
	string m_key;

	void Start()
	{
		if( null == m_key )
			throw new ArgumentNullException( "m_key should not be NULL" );

		var widget = GetComponent<Text>();

		try
		{
			widget.text = GameManager.Instance.Localization[m_key];
		}
		catch( Exception e )
		{
			throw e;
		}
	}
}