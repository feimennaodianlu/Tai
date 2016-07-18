using System;
using UnityEngine;
using UnityEngine.UI;

/*  时间： 2016.06.xx  xx    v1 提交可使用版本
 *		   2016.07.17  周日	 v2 重构及添加备注
 * ————————————————————————————————————————————————————————
 *	功能说明：		表示 ServerListWindow.SelectedServer。 
 * 
 *	使用说明：		xxxxxx
 *		
 *	后续待扩展：	暂无
 *
 *  注意事项：		本类同时存有 SelectedServer 图片 和 ServerToBeSelected 图片。而不是像 ServerToBeSelected 那样根据传入的 server 类型来加载 server asset.
 */

class SelectedServer : Server
{
	[SerializeField]
	ButtonImages m_newServerImages = null;

	[SerializeField]
	ButtonImages m_hotServerImages = null;



	public override void Reset(Content content)
	{
		base.Reset( content );

		switch( content.Type )
		{
			case ServerType.NewServer:
				SetImages( m_newServerImages );
				break;

			case ServerType.HotServer:
				SetImages( m_hotServerImages );
				break;

			default:
				ObjectHelper.ThrowEnumSpecializedArgument( content.Type.ToString() );
				break;
		}
	}

	void SetImages(ButtonImages images)
	{
		var spriteState = new SpriteState()
		{
			pressedSprite = images.Pressed,
			highlightedSprite = null,
			disabledSprite = images.Disable
		};

		m_server.spriteState = spriteState;

		//( (Image)( m_server.targetGraphic ) ).sprite = images.Normal;
		if( m_server.targetGraphic is Image )
			( (Image)( m_server.targetGraphic ) ).sprite = images.Normal;
		else
			print( "can't cast" );
	}

	public void OnClick()
	{
		PlayerPrefs.SetString( "Server", m_content.Name );
		m_action.Invoke( m_content.Name );
	}

	#region About Inspector Show

	[Serializable]
	class ButtonImages
	{
		public Sprite Normal = null;
		public Sprite Pressed = null;
		public Sprite Disable = null;
	}

	#endregion
}