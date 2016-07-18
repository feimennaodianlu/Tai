using Tai.Client;
using UnityEngine;
using UnityEngine.UI;

public class RegisterWindow : UIWindow
{
	[SerializeField]
	InputField m_userName = null;

	[SerializeField]
	InputField m_password = null;

	[SerializeField]
	InputField m_confirmPassword = null;

	[SerializeField]
	InputField m_phoneNumber = null;

	IHandler m_handler;

	#region IHandler

	public interface IHandler
	{
		void ClickCancel();
		void ClickRegisterAndLogin(string name);
		void ClickClose();
	}

	#endregion

	#region Create & Initialize

	public static RegisterWindow Create(UIWindow parentWindow, Transform parentTransform, IHandler handler)
	{
		var asset = GameManager.Instance.GameLoad.Load<RegisterWindow>( Path.RegisterWindow );

		var window = Instantiate<RegisterWindow>( asset );
		window.Initialze( parentWindow, parentTransform, handler );

		return window;
	}

	void Initialze(UIWindow parentWindow, Transform parentTransform, IHandler handler)
	{
		ObjectHelper.IfNullThrowArgument( handler, "handler" );

		base.InitializeWindow( parentWindow, parentTransform, false );

		m_handler = handler;
	}

	#endregion

	#region Event

	public void OnClickRegisterAndLogin()
	{
		if( Is4Null() )
			return;

		if( !IsPasswordEquality() )
			return;

		m_handler.ClickRegisterAndLogin( m_userName.text );
	}

	public void OnClickClose()
	{
		m_handler.ClickClose();
	}

	public void OnClickCancel()
	{
		m_handler.ClickCancel();
	}

	#endregion

	public void OnShow(string name, string password)
	{
		m_userName.text = name;
		m_password.text = password;
		m_confirmPassword.text = "";
		m_phoneNumber.text = "";
	}

	bool Is4Null()
	{
		if( string.IsNullOrEmpty( m_userName.text ) ||
			string.IsNullOrEmpty( m_password.text ) ||
			string.IsNullOrEmpty( m_confirmPassword.text ) ||
			string.IsNullOrEmpty( m_phoneNumber.text ) )
		{
			//MessageBox.Create( new MessageBox.Content(
			//	   GameManager.Instance.Localization["RegisterWindow.NullFourInput"],
			//	   transform.parent ) );
			UIMessageBox.Create(
				new UIMessageBox.Content(
					GameManager.Instance.Localization["RegisterWindow.NullFourInput"],
					UIMessageBox.Style.Ok, () => { } ) );

			return true;
		}

		return false;
	}

	bool IsPasswordEquality()
	{
		if( m_password.text != m_confirmPassword.text )
		{
			UIMessageBox.Create(
				new UIMessageBox.Content(
					GameManager.Instance.Localization["RegisterWindow.PasswordNotEqual"],
					UIMessageBox.Style.Ok,
					() => { } ) );

			return false;
		}

		return true;
	}
}