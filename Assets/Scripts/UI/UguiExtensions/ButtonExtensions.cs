using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonExtensions 
	: MonoBehaviour, 
	IPointerDownHandler, IPointerUpHandler
{
	[SerializeField]
	Labels[] m_labels = null;

	Button m_button;
	bool m_isInteractable;

	[Serializable]
	class Labels
	{
		public Text Text = null;

		public Color NormalColor;
		public Color PressedColor = Color.white;
		public Color DisabledColor = Color.grey;

		public void Initialize()
		{
			if( null == Text )
				throw new ArgumentNullException( "Text should not be NULL" );
			NormalColor = Text.color;
		}

		public void SetNormal()
		{
			Text.color = NormalColor;
		}

		public void SetPress()
		{
			Text.color = PressedColor;
		}

		public void SetDisable()
		{
			Text.color = DisabledColor;
		}
	}

	void Awake()
	{
		m_button = GetComponent<Button>();

		if( null == m_button )
			throw new ArgumentNullException( "m_button should not be NULL" );

		m_isInteractable = m_button.IsInteractable();

		foreach( var t in m_labels )
			t.Initialize();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if( !m_isInteractable ) return;

		foreach( var t in m_labels )
			t.SetPress();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if( !m_isInteractable ) return;

		foreach( var t in m_labels )
			t.SetNormal();
	}

	void Update()
	{
		if( m_isInteractable == m_button.IsInteractable() ) return;

		m_isInteractable = !m_isInteractable;

		foreach( var t in m_labels )
		{
			if( m_isInteractable )
				t.SetNormal();
			else
				t.SetDisable();
		}
	}
}