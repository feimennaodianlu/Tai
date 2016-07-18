using System;
using Tai.Client;
using UnityEngine;

class MainWindowTestDirector : GameDirector, MainWindowTestWindow.IHandler
{
	Transform m_parent;
	MainWindowTestWindow m_mainWindow;

	Stage m_stage = Stage.Noop;
	enum Stage
	{
		Noop,
		GoToYeDirector,
		GoToHaoDirector
	}

	public MainWindowTestDirector(Action<GameDirector> onPop)
		: base( onPop )
	{

		if( GameManager.Instance.UIRoot == null )
			Debug.Log( "uiroot is null" );
		else if( GameManager.Instance.UIRoot.RootCanvas == null )
			Debug.Log( "rootCanvas is null" );

		m_parent = GameManager.Instance.UIRoot.RootCanvas.transform;

		m_mainWindow = MainWindowTestWindow.Create( m_parent, "MainWindow" );
		m_mainWindow.Reset( this );

		m_stage = Stage.Noop;
	}

	public override GameDirector Update()
	{
		switch( m_stage )
		{
			case Stage.Noop:
				break;

			case Stage.GoToYeDirector:
				m_mainWindow.Show = false;
				return new YeDirector( m_parent, "YeWindow", ReturnFromDirector );

			case Stage.GoToHaoDirector:
				m_mainWindow.Show = false;
				return new HaoDirector( ReturnFromDirector );

			default:
				break;
		}

		return this;
	}

	void ReturnFromDirector(GameDirector director)
	{
		m_mainWindow.Show = true;
		m_stage = Stage.Noop;
	}

	#region IHandler

	void MainWindowTestWindow.IHandler.ClickYe()
	{
		m_stage = Stage.GoToYeDirector;
	}

	void MainWindowTestWindow.IHandler.ClickHao()
	{
		m_stage = Stage.GoToHaoDirector;
	}

	#endregion
}
