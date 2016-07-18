using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;

public class GameLoad : MonoBehaviour
{
	/// <summary>
	/// 参数 path 为project视图中的路径不需要 "Assets/"
	/// </summary>
	public T Load<T>(string path) where T : UObject
	{
		var asset = AssetDatabase.LoadAssetAtPath<T>( "Assets/" + path );

		ObjectHelper.IfNullThrowArgument( asset, "asset" );

		return asset;
	}

	public AsyncOperation LoadSceneAsync(string sceneName)
	{
		return SceneManager.LoadSceneAsync( sceneName );
	}
}