using UnityEngine;
using System.Collections;
using Tai.Client;

abstract class EntryDirector : GameDirector, MainStatus.IHandler
{
	void MainStatus.IHandler.ClickGoldAdd()
	{
		ClickGoldAdd();
	}

	void MainStatus.IHandler.ClickDiamondAdd()
	{
		ClickDiamondAdd();
	}

	void MainStatus.IHandler.ClickBack()
	{
		ClickBack();
	}

	protected virtual void ClickGoldAdd()
	{
		print( "Click add gold" );
	}

	protected virtual void ClickDiamondAdd()
	{
		print( "Click add diamond" );
	}

	protected virtual void ClickBack()
	{
		print( "Click main status Back" );
	}
}
