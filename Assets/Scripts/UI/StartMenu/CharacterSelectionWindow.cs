using UnityEngine;
using System.Collections;
using Tai.Client;
using UnityEngine.UI;

// !!! startwindow Create 前要验证
public class CharacterSelectionWindow : UIWindow
{
	[SerializeField]
	Text m_characterName = null;

	[SerializeField]
	Text m_characterLevel = null;

	[SerializeField]
	GameObject m_boy = null;

	[SerializeField]
	GameObject m_girl = null;

	[SerializeField]
	Text m_poster = null;

	// 模型


	IHandler m_handler;

	#region Content & IHandler

	public class Content
	{
		public UIWindow ParentWindow;
		public Transform ParentTransform;
		public string Name;
		public int Level;
		public string Poster;

		public Content(UIWindow window, Transform parent, string name, int level, string poster)
		{
			ParentWindow = window;
			ParentTransform = parent;
			Name = name;
			Level = level;
			Poster = poster;
		}
	}

	public interface IHandler
	{
		void ClickChangeCharacter(GameObject go1, GameObject go2);
		void ClikcEnterGame();
	}

	#endregion

	#region Create & Initialize

	public static CharacterSelectionWindow Create(Content content, IHandler handler)
	{
		var asset = GameManager.Instance.GameLoad.Load<CharacterSelectionWindow>( Path.CharacterSelectionWindow );

		var window = Instantiate<CharacterSelectionWindow>( asset );
		window.Initialize( content, handler );

		return window;
	}

	void Initialize(Content content, IHandler handler)
	{
		ObjectHelper.IfNullThrowArgument( content, "content" );
		ObjectHelper.IfNullThrowArgument( handler, "handler" );

		base.InitializeElement( content.ParentWindow, content.ParentTransform );

		m_handler = handler;

		m_characterName.text = content.Name;
		m_characterLevel.text = content.Level.ToString();
		m_poster.text = content.Poster;

		// 模型，显示男或女随机
		var flag = Random.value < 0.5f;
		ObjectHelper.SetActive( m_boy, flag );
		ObjectHelper.SetActive( m_girl, !flag );
		Show = false;
	}

	#endregion

	#region Event

	public void OnClickChangeCharacter()
	{
		m_handler.ClickChangeCharacter( m_boy, m_girl );
	}

	public void OnClickEnterGame()
	{
		m_handler.ClikcEnterGame();
	}

	#endregion
}
