using System.Linq;
using Tai.Client;
using UnityEngine;
using UnityEngine.UI;

public class ServerListWindow : UIWindow
{
	[SerializeField]
	SelectedServer m_selectedServer = null;

	[SerializeField]
	ScrollRect m_scrollView = null;

	Server.Content[] m_serverListToBeSelectedContents;  // ScrollView 中所有 server 的 info

	IHandler m_handler;

	#region IHandler

	public interface IHandler
	{
		void ClickServerSelected(string serverName);
		void ClickServerAvailable();
	}

	#endregion

	#region Create & Initialize

	// 没有 using System; 是为了防止 UnityEngine.Random 和 System.Range 引起歧义。因为本类中使用 UnityEngine.Random 频率更高
	public static ServerListWindow Create(UIWindow parentWindow, Transform parentTransform, System.Action<string> action)
	{
		var asset = GameManager.Instance.GameLoad.Load<GameObject>( Path.ServerListWindow );

		var window = Instantiate<GameObject>( asset ).GetComponent<ServerListWindow>();
		window.Initialize( parentWindow, parentTransform, action );

		return window;
	}

	void Initialize(UIWindow parentWindow, Transform parentTransform, System.Action<string> action)
	{
		base.InitializeWindow( parentWindow, parentTransform, false );

		m_selectedServer.Initialize( action );

		m_serverListToBeSelectedContents = GenerateServerFakeData( Random.Range( 7, 12 ) );

		// Selected Server
		// 第一次登陆默认一个流畅的服务器，选过一个后就一直是选的那个服务器
		#region Selected Server;

		SetSelectedServer();

		#endregion

		// Server List
		#region Server List

		foreach( var c in m_serverListToBeSelectedContents )
		{
			switch( c.Type )
			{
				case Server.ServerType.NewServer:
					var newServer = Server.Create( m_scrollView.content, m_selectedServer, Server.ServerType.NewServer );
					newServer.Reset( c );

					break;
				case Server.ServerType.HotServer:
					var hotServer = Server.Create( m_scrollView.content, m_selectedServer, Server.ServerType.HotServer );
					hotServer.Reset( c );

					break;
				default:
					ObjectHelper.ThrowEnumSpecializedArgument( c.Type.ToString() );
					break;
			}
		}

		#endregion
	}

	#endregion

	// 如果是从没选过，则分配第一个流畅的服务器
	// 如果选过且存在，则分配该服务器
	// 如果选过但因某种原因找不到同名服务器，则分配最后一个流畅的服务器
	// 如果流畅的服务器找不到，这分配第一个火爆的服务器
	void SetSelectedServer()
	{
		// 之前没选过（即 第一次进入游戏）
		if( !PlayerPrefs.HasKey( "Server" ) )
		{
			// todo 分配一个默认的流畅服务器
			var content = m_serverListToBeSelectedContents.FirstOrDefault( c => c.Type == Server.ServerType.NewServer )
						  ?? m_serverListToBeSelectedContents[Random.Range( 0, m_serverListToBeSelectedContents.Length )];

			m_selectedServer.Reset( content );
		}
		else
		{
			// 之前选过
			var serverName = PlayerPrefs.GetString( "Server" );

			var content = m_serverListToBeSelectedContents.FirstOrDefault( c => c.Name == serverName )
							?? m_serverListToBeSelectedContents.LastOrDefault( c => c.Type == Server.ServerType.NewServer )
							?? m_serverListToBeSelectedContents[Random.Range( 0, m_serverListToBeSelectedContents.Length )];

			m_selectedServer.Reset( content );
		}
	}

	#region Event

	public void OnClickServerSelected()
	{
		m_handler.ClickServerSelected( "" );
	}

	public void OnClickServerAvailable()
	{
		m_handler.ClickServerAvailable();
	}

	#endregion




	// ....................................................................................................................................
	// Generate Fake data
	// cnt 数量需要小于12
	Server.Content[] GenerateServerFakeData(int cnt)
	{
		var fakeContents = new Server.Content[] 
		{
			new Server.Content("1区 马达加斯加", null, (Server.ServerType)Random.Range(0,2)),
			new Server.Content("2区 后大家是假", null, (Server.ServerType)Random.Range(0,2)),
			new Server.Content("3区 的路径奥斯", null, (Server.ServerType)Random.Range(0,2)),
			new Server.Content("4区 的是哪款了", null, (Server.ServerType)Random.Range(0,2)),
			new Server.Content("5区 的速溶咖啡", null, (Server.ServerType)Random.Range(0,2)),
			new Server.Content("6区 到哪神经内", null, (Server.ServerType)Random.Range(0,2)),
			new Server.Content("7区 的日刷卡机", null, (Server.ServerType)Random.Range(0,2)),
			new Server.Content("8区 莲富大厦家", null, (Server.ServerType)Random.Range(0,2)),
			new Server.Content("9区 到哪数据库", null, (Server.ServerType)Random.Range(0,2)),
			new Server.Content("10区 的抗裂砂浆", null, (Server.ServerType)Random.Range(0,2)),
			new Server.Content("11区 你懂啥净空", null, (Server.ServerType)Random.Range(0,2)),
		};

		if( cnt >= fakeContents.Length )
			return fakeContents;

		return fakeContents.Take( cnt ).ToArray();
	}
}