using Tai.Client;
using UnityEngine;
using UnityEngine.UI;

public class YeWindow : UIWindow
{
	[SerializeField]
	Text m_title = null;

	IHandler m_handler;

	public interface IHandler
	{
		void ClickReturn();
	}

	#region Create & Initialize

	public static YeWindow Create(Transform parent, string title)
	{
		var asset = GameManager.Instance.GameLoad.Load<GameObject>( Path.Test.YeWindow );

		var go = Instantiate<GameObject>( asset ).GetComponent<YeWindow>();
		go.Initalize( parent, title );

		return go;
	}

	void Initalize(Transform parent, string title)
	{
		base.InitializeWindow( null, parent, false );

		m_title.text = title;
	}

	#endregion

	public void Reset(IHandler handler)
	{
		ObjectHelper.IfNullThrowArgument( handler, "handler" );

		m_handler = handler;
	}

	public void OnClickReturn()
	{
		m_handler.ClickReturn();
	}
}