using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UObject = UnityEngine.Object;

public class TestLog : MonoBehaviour
{
	BoxCollider b;
	public UObject m_obj;

	void Awake()
	{
		b = GetComponent<BoxCollider>();

		TaiObject.LogAssertion( "ssss" );
	}
}
