using UnityEngine;

public class StartMenuLauncher : MonoBehaviour
{
	void Awake()
	{
		StartMenuDirector.Create( GameManager.Instance.UIRoot.RootCanvas.transform );
	}

	void Start()
	{
		Destroy( gameObject );
	}
}