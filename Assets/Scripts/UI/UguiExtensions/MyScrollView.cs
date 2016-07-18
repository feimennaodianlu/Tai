using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyScrollView : ScrollRect
{
	[SerializeField]
	RectTransform m_dragMask = null;

	protected override void Start()
	{
		base.Start();

		//m_dragMask.SetAsFirstSibling();
	}

	public override void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
	{
		//m_dragMask.SetAsLastSibling();
		//m_dragMask.gameObject.SetActive( true );
	}

	public override void OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
	{
		//m_dragMask.SetAsFirstSibling();
		//m_dragMask.gameObject.SetActive( false );
	}

	//public override void OnInitializePotentialDrag(UnityEngine.EventSystems.PointerEventData eventData)
	//{
	//	m_dragMask.SetAsLastSibling();
	//}
}