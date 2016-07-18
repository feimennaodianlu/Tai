using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TEST : MonoBehaviour, IPointerClickHandler
{
	public Transform m_image = null;


	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		return;

		var go = Instantiate<Transform>( m_image );
		go.gameObject.SetActive( true );
		go.SetParent( transform, false );
		//go.localPosition = eventData.position;
		Vector2 v;
		if( RectTransformUtility.ScreenPointToLocalPointInRectangle( (RectTransform)transform, eventData.position, eventData.pressEventCamera, out  v ) )
		{
			go.localPosition = v;
		}
	}

	UIMessageTip m_tipWindow;
	public void OnClickButton()
	{
		var tip = "打算考了几分当然看鲨卷风扣篮大赛就阿福卡傻大姐看会电视是打开后击杀掉就好人卡就是打了卡加上了就";
		//UIMessageTip.CreateModel( "打算考了几分当然看鲨卷风扣篮大赛就阿福卡傻大姐看会电视是打开后击杀掉就好人卡就是打了卡加上了就" );'

		var content = new UIMessageTip.Content( tip, UIMessageTip.Mode.BubbleFade, true );

		UIMessageTip.Create( content );
	}
}
