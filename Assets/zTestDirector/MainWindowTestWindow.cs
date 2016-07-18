using Tai.Client;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainWindowTestWindow : UIWindow
{
	[SerializeField]
	Text m_title = null;

	IHandler m_handler;

	public interface IHandler
	{
		void ClickYe();
		void ClickHao();
	}

	public static MainWindowTestWindow Create(Transform parent, string title)
	{
		var asset = GameManager.Instance.GameLoad.Load<GameObject>( Path.Test.MainWindowTestWindow );

		var go = Instantiate<GameObject>( asset ).GetComponent<MainWindowTestWindow>();
		go.Initialize( parent, title );

		return go;
	}

	void Initialize(Transform parent, string title)
	{
		base.InitializeWindow( null, parent, false );

		m_title.text = title;
	}

	public void Reset(IHandler handler)
	{
		m_handler = handler;
	}

	public void OnClickYe()
	{
		m_handler.ClickYe();
	}

	public void OnClickHao()
	{
		m_handler.ClickHao();
	}
}
