using UnityEngine;
using System.Collections;

public class MainWindowDirector : MonoBehaviour
{
	MainWindow m_mainWindow;

	public MainWindowDirector Create()
	{
		var asset = GameManager.Instance.GameLoad.Load<GameObject>( Path.MainWindowDirector );

		var go = Instantiate<GameObject>( asset ).GetComponent<MainWindowDirector>();
		go.Initialize();

		return go;
	}

	void Initialize()
	{
		//m_mainWindow = MainWindow.Create( null, transform );
	}
}
