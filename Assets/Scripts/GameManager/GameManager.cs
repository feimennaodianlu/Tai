using System;
using UnityEditor;
using UnityEngine;
using Tai.Client;

public class GameManager : MonoBehaviour, DirectorStack.IHandler  // Director
{
	static GameManager s_instance = null;

	[SerializeField]
	Language m_language = Language.Chinese_CHS;

	enum Language
	{
		Chinese_CHS,
		English
	}

	StringTable m_localization;

	GameAudio m_gameAudio;
	GameLoad m_gameLoad;
	UIRoot m_uiRoot;  // 不需要像 GameAudio 和 GameLoad 那样在本脚本的 Awake 中初始化。只需要在第一次用到的时候初始化

	DirectorStack m_directors;  // Director

	#region Create & Initialize * 5

	public static GameManager Create()
	{
		var asset = Resources.Load<GameManager>( Path.Resource.GameManager );

		ObjectHelper.IfNullThrowArgument( asset, "asset" );

		var go = Instantiate<GameManager>( asset );
		go.Initialize();

		return go;
	}

	void Initialize()
	{
		DontDestroyOnLoad( gameObject );

		// When having two or more scripts in the scene, it will throw the following exception
		if( s_instance != null )
			ObjectHelper.ThrowArgument( "class GameManager shoud be Singleton" );
		s_instance = this;
		InitializeGameLoad();
		InitializeLocalization();
		InitializeUIRoot();
		InitializeGameAudio();
		InitializeDirectorStack();
	}

	void InitializeUIRoot()
	{
		m_uiRoot = UIRoot.Create();
	}

	void InitializeGameAudio()
	{
		m_gameAudio = transform.Find( "Audio" ).GetComponent<GameAudio>();
		m_gameAudio.Initialize();
	}

	void InitializeLocalization()
	{
		string path;

		switch( m_language )
		{
			case Language.Chinese_CHS:
				path = "Language_ch_CHS";
				break;

			case Language.English:
				path = "Language_en";
				break;

			default:
				throw new ArgumentOutOfRangeException( m_language.ToString() );
		}

		m_localization = StringTable.Create( "Main", fileName =>
			{
				var asset = AssetDatabase.LoadAssetAtPath<TextAsset>( string.Format( "Assets/Localization/{0}/{1}.xml", path, fileName ) );

				return asset.text;
			} );
	}

	void InitializeGameLoad()
	{
		try
		{
			m_gameLoad = transform.Find( "Load" ).GetComponent<GameLoad>();
		}
		catch( ArgumentNullException e )
		{
			throw e;
		}
	}

	// Director
	void InitializeDirectorStack()
	{
		m_directors = new DirectorStack( this );
	}

	#endregion

	void Update()
	{

		if( !m_directors.Update() )
		{
			print( "<color=cyan>Quit Application</color>" );
			Application.Quit();
		}
	}

	#region Director Stack IHandler

	// Director
	GameDirector DirectorStack.IHandler.CreateRootDirector()
	{
		return new RootDirector();
	}

	void DirectorStack.IHandler.OnPop(GameDirector pop, GameDirector newTop)
	{
	}

	void DirectorStack.IHandler.OnPush(GameDirector push, GameDirector lastTop)
	{
	}

	#endregion

	#region Properties

	public static GameManager Instance
	{
		get { return s_instance; }
	}

	public GameAudio GameAudio
	{
		get { return m_gameAudio; }
	}

	public StringTable Localization
	{
		get { return m_localization; }
	}

	public GameLoad GameLoad
	{
		get { return m_gameLoad; }
	}

	public UIRoot UIRoot
	{
		get { return m_uiRoot; }
	}

	#endregion
}