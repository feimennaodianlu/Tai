using Tai.Client;
using UnityEngine;
using UnityEngine.UI;

// 用户名和密码不能有空格
public class LoginWindow : UIWindow
{
	[SerializeField]
	InputField m_userName = null;

	[SerializeField]
	InputField m_password = null;

	IHandler m_handler;

	#region IHandler

	public interface IHandler
	{
		void ClickLogin(string name);
		void ClickRegister(string name, string password);
		void ClickClose();
	}

	#endregion

	#region Create & Initialize

	public static LoginWindow Create(UIWindow parentWindow, Transform parentTransform, IHandler handler)
	{
		var asset = GameManager.Instance.GameLoad.Load<LoginWindow>( Path.LoginWindow );

		var loginWindow = Instantiate<LoginWindow>( asset );
		loginWindow.Initialize( parentWindow, parentTransform, handler );

		return loginWindow;
	}

	void Initialize(UIWindow parentWindow, Transform parentTransform, IHandler handler)
	{
		ObjectHelper.IfNullThrowArgument( handler, "handler" );

		base.InitializeWindow( parentWindow, parentTransform, false );

		m_handler = handler;

		Show = false;
	}

	#endregion

	#region Event

	public void OnClickLogin()
	{
		// 回到开始界面，开始界面用户名变成 accountInput 中的值
		if( string.IsNullOrEmpty( m_userName.text ) || string.IsNullOrEmpty( m_password.text ) )
		{
			//MessageBox.Create( new MessageBox.Content(
			//	GameManager.Instance.Localization["LoginWindow.NullUserNameOrPassword"],
			//	transform.parent,
			//MessageBox.MessageBoxStyle.Confirm ) );

			UIMessageBox.CreateModel( "请检查用户名或账户是否为空", true );

			return;

		}

		m_handler.ClickLogin( m_userName.text );
	}

	public void OnClickRegister()
	{
		m_handler.ClickRegister( m_userName.text, m_password.text );
	}

	public void OnClickClose()
	{
		m_handler.ClickClose();
	}

	#endregion
}