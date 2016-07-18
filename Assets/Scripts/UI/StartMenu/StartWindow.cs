using Tai.Client;
using UnityEngine;
using UnityEngine.UI;

public class StartWindow : UIWindow
{
	[SerializeField]
	Text m_userName = null;

	[SerializeField]
	Text m_serverName = null;

	IHandler m_handler;

	#region IHandler

	public interface IHandler
	{
		void ClickUserName();
		void ClickServerList();
		void ClickEnterGame();
	}

	#endregion

	#region Create & Initialize

	public static StartWindow Create(UIWindow parentWindow, Transform parentTransform, IHandler handler)
	{
		var asset = GameManager.Instance.GameLoad.Load<StartWindow>( Path.StartWindow );

		var startWindow = Instantiate<StartWindow>( asset );
		startWindow.Initialize( parentWindow, parentTransform, handler );

		return startWindow;
	}

	void Initialize(UIWindow parentWindow, Transform parentTransform, IHandler handler)
	{
		ObjectHelper.IfNullThrowArgument( handler, "handler" );

		base.InitializeWindow( parentWindow, parentTransform, false );

		m_handler = handler;

		if( PlayerPrefs.HasKey( "UserName" ) )
			SetUserName( PlayerPrefs.GetString( "UserName" ) );
		else
			SetUserName( GameManager.Instance.Localization["StartWindow.PlsLogin"] );

		if( PlayerPrefs.HasKey( "Server" ) )
			SetServerName( PlayerPrefs.GetString( "Server" ) );
		else
			SetServerName( GameManager.Instance.Localization["StartWindow.ChooseServer"] );
	}

	#endregion

	public void SetUserName(string name)
	{
		if( string.IsNullOrEmpty( name ) )
			m_userName.text = GameManager.Instance.Localization["StartWindow.PlsLogin"];
		else
			m_userName.text = name;
	}

	public void SetServerName(string name)
	{
		if( string.IsNullOrEmpty( name ) )
			m_serverName.text = GameManager.Instance.Localization["StartWindow.ChooseServer"];
		else
			m_serverName.text = name;
	}

	#region Event

	public void OnClickUserName()
	{
		m_handler.ClickUserName();
	}

	public void OnClickServerList()
	{
		m_handler.ClickServerList();
	}

	public void OnClickEnterGame()
	{
		m_handler.ClickEnterGame();
	}

	#endregion
}