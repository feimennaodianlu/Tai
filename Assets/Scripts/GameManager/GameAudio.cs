using System;
using UnityEngine;

[RequireComponent( typeof( AudioListener ) )]
public class GameAudio : MonoBehaviour
{
	AudioSource m_uiAudioSource;
	AudioSource m_bgmAudioSource;

	public void Initialize()
	{
		m_uiAudioSource = gameObject.AddComponent<AudioSource>();
		m_bgmAudioSource = gameObject.AddComponent<AudioSource>();

		if( null == m_uiAudioSource )
			throw new ArgumentNullException( "m_uiAudioSource should not be NULL" );
		if( null == m_bgmAudioSource )
			throw new ArgumentNullException( "m_bgmAudioSource should not be NULL" );

		m_uiAudioSource.playOnAwake = false;

		m_bgmAudioSource.playOnAwake = false;
		m_bgmAudioSource.loop = true;
	}

	public void PlayUiAudioClip(AudioClip clip)
	{
		if( null == clip ) return;

		m_uiAudioSource.PlayOneShot( clip );
	}

	public void PlayBgmAudioClip(AudioClip clip)
	{
		if( null == clip ) return;

		m_bgmAudioSource.clip = clip;
		m_bgmAudioSource.Play();
	}

	public void StopBgmAudioClip()
	{
		if( m_bgmAudioSource.isPlaying )
			m_bgmAudioSource.Stop();
	}
}
