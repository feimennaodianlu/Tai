using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlaySound : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	AudioClip m_audioClip = null;

	public void OnPointerClick(PointerEventData eventData)
	{
		GameManager.Instance.GameAudio.PlayUiAudioClip( m_audioClip );
	}
}