using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyTweenerScale
	: MonoBehaviour,
	IPointerDownHandler, IPointerUpHandler,
	IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	Mode m_mode = Mode.Click;

	[SerializeField]
	Vector3 m_pressRatio = new Vector3( 0.8f, 0.8f, 0.8f );

	[SerializeField]
	float m_duration = 0.1f;

	Selectable m_widget;
	bool m_isInteractale = true;

	/* 单机平台 或 编辑器 两种情况都有可能
	 * 在真机上（判断安卓或苹果） 只会是 clickMode
	 */
	enum Mode
	{
		Click,
		Hover
	}

	Vector3 m_startScale;

	void Awake()
	{
		m_widget = GetComponent<Selectable>();
		if( ObjectHelper.IsNotNull( m_widget ) )
			m_isInteractale = m_widget.IsInteractable();

		m_startScale = transform.localScale;

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)

		m_mode = Mode.Click;

#endif
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if( !m_isInteractale ) return;

		if( m_mode == Mode.Hover ) return;

		transform.DOScale( Vector3.Scale( m_startScale, m_pressRatio ), m_duration );
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if( !m_isInteractale ) return;

		if( m_mode == Mode.Hover ) return;

		transform.DOScale( m_startScale, m_duration );
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if( !m_isInteractale ) return;

		if( m_mode == Mode.Click ) return;

		transform.DOScale( Vector3.Scale( m_startScale, m_pressRatio ), m_duration );
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if( !m_isInteractale ) return;

		if( m_mode == Mode.Click ) return;

		transform.DOScale( m_startScale, m_duration );
	}

	void Update()
	{
		if( m_isInteractale == m_widget.IsInteractable() ) return;

		m_isInteractale = !m_isInteractale;
	}
}