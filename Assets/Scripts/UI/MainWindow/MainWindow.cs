using DG.Tweening;
using System;
using Tai.Client;
using UnityEngine;

public class MainWindow : EntryWindow
{
	[SerializeField]
	Playerbar m_playerBar = null;

	[SerializeField]
	Bottom m_bottomPart = null;

	Tweener m_spreadTweener;
	Tweener m_shrinkTweener;

	public interface IHandler
	{
		void ClickTask();
		void ClickSystem();
		void ClickBattle();
		void ClickSkill();
		void ClickShop();
		void ClickTreasreChest();
	}

	#region Create & Initialize

	public static MainWindow Create(UIWindow window, Transform parent, PlayerInfo info)
	{
		return null;
	}

	void Initialize(UIWindow window, Transform parent, PlayerInfo info)
	{
		base.InitializeWindow( window, parent, false );

		m_shrinkTweener = m_bottomPart.Transform.DOLocalMoveX( m_bottomPart.Xshrink, m_bottomPart.Duration ).OnComplete( TweenerShrinkCompleted ).SetAutoKill( false );
		m_spreadTweener = m_bottomPart.Transform.DOLocalMoveX( m_bottomPart.Xspread, m_bottomPart.Duration ).OnComplete( TweenerSpreadCompleted ).SetAutoKill( false );

		m_shrinkTweener.Pause();
		m_spreadTweener.Pause();

		m_bottomPart.Transform.localPosition = m_bottomPart.ShrinkButton.transform.localPosition;
	}

	public void Reset(PlayerInfo info)
	{
		//m_playerBar.Initialize(new Playerbar.Content.
	}

	#endregion

	#region Event

	public void OnClickShrinkSpread(string type)
	{
		ObjectHelper.SetActive( m_bottomPart.Mask, true );

		switch( type )
		{
			case "SpreadButton":
				m_spreadTweener.PlayForward();
				break;

			case "ShrinkButton":
				m_shrinkTweener.PlayForward();
				break;

			default:
				ObjectHelper.ThrowEnumSpecializedArgument( type );
				break;
		}
	}

	#endregion

	#region Helper

	void TweenerSpreadCompleted()
	{
		ObjectHelper.SetActive( m_bottomPart.SpreadButton, false );
		ObjectHelper.SetActive( m_bottomPart.ShrinkButton, true );
		ObjectHelper.SetActive( m_bottomPart.Mask, false );
	}

	void TweenerShrinkCompleted()
	{
		ObjectHelper.SetActive( m_bottomPart.SpreadButton, true );
		ObjectHelper.SetActive( m_bottomPart.ShrinkButton, false );
		ObjectHelper.SetActive( m_bottomPart.Mask, false );
	}

	#endregion

	#region Inspector Helper

	[Serializable]
	class Bottom
	{
		public Transform BottomGo = null;
		public float Duration = 1f;  // Tweener duration
		[SerializeField]
		Transform m_spreadPos = null;  // After spreading pos
		[SerializeField]
		Transform m_shrinkPos = null;
		public GameObject Mask = null;  // When bottom part is moving, we can't click it.

		public GameObject SpreadButton = null;
		public GameObject ShrinkButton = null;

		public float Xspread
		{
			get { return m_spreadPos.localPosition.x; }
		}
		public float Xshrink
		{
			get { return m_shrinkPos.localPosition.x; }
		}
		public Transform Transform
		{
			get { return BottomGo.transform; }
		}
	}

	#endregion
}