using Tai.Client;

class LoginDirector : GameDirector, StartWindow.IHandler, LoginWindow.IHandler, RegisterWindow.IHandler
{
	StartWindow m_startWindow;
	LoginWindow m_loginWindow;
	RegisterWindow m_registerWindow;
	ServerListWindow m_serverListWindow;
	CharacterSelectionWindow m_characterSelectionWindow;

	Stage m_stage;
	enum Stage
	{
		Noop,

		Show_StartWindow,
		Show_LoginWindow,
		Show_RegisterWindow,
		Show_ServerListWindow,
		Show_Show_CharacterSelectionWindow,
	}

	public override GameDirector Update()
	{
		switch( m_stage )
		{
			case Stage.Noop:
				break;

			case Stage.Show_StartWindow:

				break;
			case Stage.Show_LoginWindow:

				break;
			case Stage.Show_RegisterWindow:

				break;
			case Stage.Show_ServerListWindow:

				break;
			case Stage.Show_Show_CharacterSelectionWindow:

				break;

			default:
				ObjectHelper.ThrowEnumSpecializedArgument( m_stage.ToString() );
				break;
		}

		return this;
	}



	void StartWindow.IHandler.ClickUserName()
	{
		m_startWindow.Show = false;
		m_loginWindow.Show = true;
	}

	void StartWindow.IHandler.ClickServerList()
	{
		m_startWindow.Show = false;
		m_serverListWindow.Show = true;
	}

	void StartWindow.IHandler.ClickEnterGame()
	{
		m_startWindow.Show = false;
		m_characterSelectionWindow.Show = true;
	}

	void LoginWindow.IHandler.ClickLogin(string name)
	{
		m_startWindow.SetUserName( name );
		m_startWindow.Show = true;
		m_loginWindow.Show = false;
	}

	void LoginWindow.IHandler.ClickRegister(string name, string password)
	{
		m_registerWindow.OnShow( name, password );
		m_registerWindow.Show = true;
		m_loginWindow.Show = false;
	}

	void LoginWindow.IHandler.ClickClose()
	{
		m_startWindow.Show = true;
		m_loginWindow.Show = false;
	}

	void RegisterWindow.IHandler.ClickCancel()
	{
		m_loginWindow.Show = true;
		m_registerWindow.Show = false;
	}

	void RegisterWindow.IHandler.ClickRegisterAndLogin(string name)
	{
		m_startWindow.SetUserName( name );
		m_startWindow.Show = true;
		m_registerWindow.Show = false;
	}

	void RegisterWindow.IHandler.ClickClose()
	{
		m_loginWindow.Show = true;
		m_registerWindow.Show = false;
	}
}