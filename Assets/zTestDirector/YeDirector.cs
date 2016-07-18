using System;
using Tai.Client;
using UnityEngine;

class YeDirector : GameDirector, YeWindow.IHandler
{
	YeWindow m_yeWindow;

	bool m_isEnd;

	public YeDirector(Transform parent, string title, Action<GameDirector> onPop)
		: base( onPop )
	{
		m_yeWindow = YeWindow.Create( parent, title );

		m_yeWindow.Reset( this );

		m_isEnd = false;
	}

	public override GameDirector Update()
	{
		return m_isEnd ? null : this;
	}

	protected override void DisposeManaged()
	{
		m_yeWindow.Destroy();

		base.DisposeManaged();
	}

	void YeWindow.IHandler.ClickReturn()
	{
		m_isEnd = true;
	}
}
