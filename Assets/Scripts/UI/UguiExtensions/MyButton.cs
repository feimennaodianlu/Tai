using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyButton 
	: Button,
	IPointerDownHandler, IPointerUpHandler
{
	// Press
	[Serializable]
	public class ButtonPressedEvent : UnityEvent { }

	[SerializeField]
	ButtonPressedEvent m_onPressed = new ButtonPressedEvent();  // 字段名也是 Inspector 视图中的事件名

	// Up
	[Serializable]
	public class ButtonUpEvent : UnityEvent { }

	[SerializeField]
	ButtonUpEvent m_onUp = new ButtonUpEvent();

	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown( eventData );

		if( !IsActive() || !IsInteractable() )
			return;

		m_onPressed.Invoke();
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp( eventData );

		if( !IsActive() || !IsInteractable() )
			return;

		m_onUp.Invoke();
	}

	#region Properites

	public ButtonPressedEvent OnPressed
	{
		get { return m_onPressed; }
		set { m_onPressed = value; }
	}

	public ButtonUpEvent OnUp
	{
		get { return m_onUp; }
		set { m_onUp = value; }
	}

	#endregion
}