using System;
using Tai.Client;
using UnityEngine;
using UnityEngine.UI;

/*  时间： 2016.6.xx  xx   v1   提交可使用版本
 *		   2016.7.17  周日 v2   重构，并增加此备注
 * ——————————————————————————————————————————————————————————————
 *	功能说明：		作为 Server 基类（SelectedServer继承该基类），同时也是 ServerListWindow 中 ScrollView 下所有可选的Server。
 *	
 *	使用说明：		创建 Server 对象时，其 asset 路径有两个。因为 newServer 和 hotServer 使用的图片不用，所以做成两个 prefab
 *		
 *	后续待扩展：	暂无
 *
 *  注意事项：		暂无
 *  
 */

class Server : UIWidget
{
	[SerializeField]
	Text m_serverName = null;

	protected Content m_content;
	protected IHandler m_handler;

	SelectedServer m_selectedServer;

	public enum ServerType
	{
		NewServer,  // 流畅
		HotServer	// 火爆
	}

	#region Content & IHandler

	public class Content
	{
		public string Name;
		public string Ip;
		public ServerType Type;

		public Content(string name, string ip, ServerType type)
		{
			Name = name;
			Ip = ip;
			Type = type;
		}
	}

	public interface IHandler
	{
		void ClickServer(Content content);
	}

	#endregion

	public static Server Create(UIWindow window, Transform parent, Content content, IHandler handler)
	{
		GameObject asset = null;
		switch( content.Type )
		{
			case ServerType.NewServer:
				asset = GameManager.Instance.GameLoad.Load<GameObject>( Path.NewServer );
				break;

			case ServerType.HotServer:
				asset = GameManager.Instance.GameLoad.Load<GameObject>( Path.HotServer );
				break;

			default:
				ObjectHelper.ThrowEnumSpecializedArgument( content.Type.ToString() );
				break;
		}

		var go = Instantiate<GameObject>( asset ).GetComponent<Server>();
		go.Initialize( window, parent, content, handler );

		return go;
	}

	protected virtual void Initialize(UIWindow window, Transform parent, Content content, IHandler handler)
	{
		ObjectHelper.IfNullThrowArgument( content, "content" );
		ObjectHelper.IfNullThrowArgument( handler, "handler" );

		base.InitialzeWidget( window, parent );

		m_content = content;
		m_handler = handler;
	}

	public virtual void Click()
	{
		m_handler.ClickServer( m_content );
	}
}