using UnityEngine;

// 负责管理所有 start menu 界面， 例如： 界面间跳转
public class StartMenuDirector
	:
	MonoBehaviour,
	StartWindow.IHandler, LoginWindow.IHandler,
	RegisterWindow.IHandler, CharacterSelectionWindow.IHandler
{
	StartWindow m_startWindow;
	LoginWindow m_loginWindow;
	RegisterWindow m_registerWindow;
	ServerListWindow m_serverListWidow;
	CharacterSelectionWindow m_characterWindow;  // 创建前要验证

	public static StartMenuDirector Create(Transform parent)
	{
		var asset = GameManager.Instance.GameLoad.Load<StartMenuDirector>( Path.StartMenuDirector );

		var director = Instantiate<StartMenuDirector>( asset );
		director.Initialize( parent );

		return director;
	}

	void Initialize(Transform parent)
	{
		transform.SetParent( parent, false );

		m_startWindow = StartWindow.Create( null, transform, this );
		m_loginWindow = LoginWindow.Create( null, transform, this );
		m_registerWindow = RegisterWindow.Create( null, transform, this );
		m_serverListWidow = ServerListWindow.Create( null, transform, s => FromServerListToStart( s ) );

		//// temp 需要验证（没账户也进不了该界面）。此处不验证
		m_characterWindow = CharacterSelectionWindow.Create(
			new CharacterSelectionWindow.Content(
				null, transform, "沙拉酱", 30,
				"当然看啥叫好可怜的杀菌后了扩大数据库黑龙江的沙坑里很骄傲的" ), this );
	}

	#region Start & Login

	void FromStartToLogin()
	{
		ObjectHelper.SetActive( m_startWindow.gameObject, false );
		ObjectHelper.SetActive( m_loginWindow.gameObject, true );
	}

	void FromLoginToStart(string name)
	{
		m_startWindow.SetUserName( name );
		CloseLoginWindow();
	}

	void CloseLoginWindow()
	{
		m_loginWindow.gameObject.SetActive( false );
		m_startWindow.gameObject.SetActive( true );
	}

	#endregion

	#region Login & Register

	void FromLoginToRegister(string name, string password)
	{
		m_registerWindow.OnShow( name, password );
		ObjectHelper.SetActive( m_loginWindow.gameObject, false );
		ObjectHelper.SetActive( m_registerWindow.gameObject, true );
	}

	void FromRegisterToLogin()
	{
		ObjectHelper.SetActive( m_registerWindow.gameObject, false );
		ObjectHelper.SetActive( m_loginWindow.gameObject, true );
	}

	void FromRegisterToStart(string name)
	{
		m_startWindow.SetUserName( name );
		m_registerWindow.gameObject.SetActive( false );
		m_startWindow.gameObject.SetActive( true );
	}

	#endregion

	#region Start & ServerList

	void FromServerListToStart(string serverName)
	{
		ObjectHelper.SetActive( m_serverListWidow.gameObject, false );
		ObjectHelper.SetActive( m_startWindow.gameObject, true );
		m_startWindow.SetServerName( serverName );
	}

	void FromStartToServerList()
	{
		ObjectHelper.SetActive( m_startWindow.gameObject, false );
		ObjectHelper.SetActive( m_serverListWidow.gameObject, true );
	}

	#endregion

	#region Start & CharacterSelection

	void FromStartToCharacterSelection()
	{
		ObjectHelper.SetActive( m_startWindow.gameObject, false );
		ObjectHelper.SetActive( m_characterWindow.gameObject, true );
	}

	#endregion

	#region CharacterSelection & MainScene

	void FromCharacterSelectionToMainScene()
	{
		print( "enter game" );
	}

	#endregion

	#region IHandler

	void StartWindow.IHandler.ClickUserName()
	{
		FromStartToLogin();
	}

	void StartWindow.IHandler.ClickServerList()
	{
		FromStartToServerList();
	}

	void StartWindow.IHandler.ClickEnterGame()
	{
		FromStartToCharacterSelection();
	}

	void LoginWindow.IHandler.ClickLogin(string name)
	{
		FromLoginToStart( name );
	}

	void LoginWindow.IHandler.ClickRegister(string name, string password)
	{
		FromLoginToRegister( name, password );
	}

	void LoginWindow.IHandler.ClickClose()
	{
		CloseLoginWindow();
	}

	void RegisterWindow.IHandler.ClickCancel()
	{
		FromRegisterToLogin();
	}

	void RegisterWindow.IHandler.ClickRegisterAndLogin(string name)
	{
		FromRegisterToStart( name );
	}

	void RegisterWindow.IHandler.ClickClose()
	{
		FromRegisterToLogin();
	}

	void CharacterSelectionWindow.IHandler.ClickChangeCharacter(GameObject go1, GameObject go2)
	{
		var flag = go1.activeSelf;
		ObjectHelper.SetActive( go1, !flag );
		ObjectHelper.SetActive( go2, flag );
	}

	void CharacterSelectionWindow.IHandler.ClikcEnterGame()
	{
		FromCharacterSelectionToMainScene();
	}

	#endregion
}