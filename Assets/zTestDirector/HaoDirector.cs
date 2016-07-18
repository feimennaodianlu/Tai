using System;
using Tai.Client;

class HaoDirector : GameDirector, HaoWindow.IHandler
{
	HaoWindow m_haoWindow;
	Stage m_stage;

	enum Stage
	{
		Noop,
		GoToMainDirector
	}

	public HaoDirector(Action<GameDirector> onPop)
		: base( onPop )
	{
		m_haoWindow = HaoWindow.Create( GameManager.Instance.UIRoot.RootCanvas.transform, "Hao Window" );

		m_haoWindow.Reset( this );

		m_stage = Stage.Noop;
	}

	public override GameDirector Update()
	{
		switch( m_stage )
		{
			case Stage.Noop:
				break;

			case Stage.GoToMainDirector:
				return null;

			default:
				break;
		}

		return this;
	}

	protected override void DisposeManaged()
	{
		m_haoWindow.Destroy();

		base.DisposeManaged();
	}

	void HaoWindow.IHandler.ClickReturn()
	{
		m_stage = Stage.GoToMainDirector;
	}
}