using DG.Tweening;
using System;
using Tai.Client;
using UnityEngine;
using UnityEngine.UI;

// MainStatus 有 Create、Initialize，没有 Reset，用 ResetXXX 代替。


/*  进入到主界面开始Create，然后在整个游戏中都不销毁。
 *	在Create的时候从服务器端获取信息，之后mainstatus的状态由客户端维护。
 *	
 *  每一分钟增加一个coin、每1.5分钟增加一个diamond。
 * 
 * 
 * 
 * 
 * 
 */

public class MainStatus : UIWindow
{
	static MainStatus s_instance;

	[SerializeField]
	GameObject m_backButton = null;

	[SerializeField]
	GameObject m_coinItem = null;

	[SerializeField]
	Text m_coinText = null;

	[SerializeField]
	GameObject m_diamondItem = null;

	[SerializeField]
	Text m_diamondText = null;

	[SerializeField]
	DOTweenAnimation m_coinTweenAnimation = null;

	[SerializeField]
	DOTweenAnimation m_diamondTweenAnimation = null;

	uint m_currentCoin;
	uint m_currentDiamond;

	Style m_style;

	[Flags]
	public enum Style
	{
		None = 0,
		Coin = 1 << 0,
		Diamond = 1 << 1,
		Back = 1 << 2,

		MainWindowStyle = Coin | Diamond | Back,
		EntryWindowStyle = Coin | Diamond
	}

	#region Content & IHandler

	public class Content
	{
		public int Coin;
		public int Diamond;
		public Style Style;

		public Content(int gold, int diamond, Style mode)
		{
			Coin = gold;
			Diamond = diamond;
			Style = mode;
		}
	}

	public interface IHandler
	{
		void ClickGoldAdd();
		void ClickDiamondAdd();
		void ClickBack();
	}

	#endregion

	#region Create & Initialize

	public MainStatus Create(UIWindow window, Transform parent, Content content, IHandler handler)
	{
		if( s_instance != null )
			ObjectHelper.ThrowArgument( "MainStatus should be singleton" );

		var asset = GameManager.Instance.GameLoad.Load<GameObject>( Path.Resource.MainStatus ).GetComponent<MainStatus>();

		var go = Instantiate<MainStatus>( asset );
		go.Initialize( window, parent, content, handler );

		return go;
	}

	void Initialize(UIWindow window, Transform parent, Content content, IHandler handler)
	{
		base.InitializeWindow( window, parent, false );

		DontDestroyOnLoad( gameObject );

		Reset( content, handler );
	}
	#endregion

	#region Reset

	// 分总的 Reset 和 AddXXXX。
	// Reset 中设置 SetActive 和 content。而 AddXXX 只负责设置 content
	public void Reset(Content content, IHandler handler)
	{
		ObjectHelper.IfNullThrowArgument( content, "content" );
		ObjectHelper.IfNullThrowArgument( handler, "handler" );

		// Back
		var show = ( content.Style & Style.Back ) != 0;
		ObjectHelper.SetActive( m_backButton, show );

		// Coin
		show = ( content.Style & Style.Coin ) != 0;
		ObjectHelper.SetActive( m_coinItem, show );
		if( show )
			m_coinText.text = content.Coin.ToString();

		// Diamond
		show = ( content.Style & Style.Diamond ) != 0;
		ObjectHelper.SetActive( m_diamondItem, show );
		if( show )
			m_diamondText.text = content.Diamond.ToString();
	}

	// It does not need to play animation when hiding.
	public void AddCoin(uint cnt = 1)
	{
		if( m_coinItem.activeSelf )
		{
			m_currentCoin += cnt;

			if( m_coinItem.activeInHierarchy )
			{
				m_coinTweenAnimation.DOPlay();
				m_coinText.text = m_currentCoin.ToString();
			}
		}
	}

	public void AddDiamond(uint cnt = 1)
	{
		if( m_diamondItem.activeSelf )
		{
			m_currentDiamond += cnt;
			if( m_diamondItem.activeInHierarchy )
			{
				m_diamondText.text = m_currentDiamond.ToString();
				m_diamondTweenAnimation.DOPlay();
			}
		}
	}

	#endregion

	#region Property

	public static MainStatus Instance
	{
		get { return s_instance; }
	}

	#endregion
}