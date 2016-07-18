using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/* 目前的作用就是保证 GameApp 初始化（GameManager初始化是在 Awake 中，由 Launcher 切换到 startMenu 是在 Start 中）
 * 。。这和 cp 的 Launcher 不太一样，具体需要查看 cp 的launcher作用 
 * 
 */
public class Launcher : MonoBehaviour
{
	LoadingWindow m_loadingWindow;
	AsyncOperation m_async;

	void Awake()
	{
		GameManager.Create();
	}

	void Start()
	{
		m_loadingWindow = LoadingWindow.Create(
			new LoadingWindow.Content( null, null ) );

		StartCoroutine( LoadScene( Scene.StartMenu ) );
	}

	IEnumerator LoadScene(string sceneName)
	{
		//var sceneAsync = GameManager.Instance.GameLoad.LoadSceneAsync( sceneName );
		var sceneAsync = SceneManager.LoadSceneAsync( sceneName );
		yield return m_loadingWindow.Play( sceneAsync, 1f );

		//var asset = GameManager.Instance.GameLoad.Load<StartMenuManager>( Path.StartMenu );
		//ObjectHelper.IfNullThrowArgument( asset );
		//var startMenu = Instantiate<StartMenuManager>( asset );
		//startMenu.Initialize();
	}
}