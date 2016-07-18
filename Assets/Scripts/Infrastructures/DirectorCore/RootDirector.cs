using UnityEngine;
using System.Collections;
using Tai.Client;

// 负责从游戏开始到 mainWindow之前的阶段，之后由 MainDirector负责
class RootDirector : GameDirector
{
	#region Test

	//Stage m_stage;
	//enum Stage
	//{
	//	UpdateVersion,
	//	LoginWindow,
	//	MainWindowTest
	//}

	//public RootDirector()
	//{
	//	m_stage = Stage.UpdateVersion;
	//}

	//public override GameDirector Update()
	//{
	//	switch( m_stage )
	//	{
	//		case Stage.UpdateVersion:
	//			{
	//				// 暂无
	//				m_stage = Stage.LoginWindow;
	//			}
	//			break;

	//		case Stage.LoginWindow:
	//			{
	//				//
	//				m_stage = Stage.MainWindowTest;
	//			}
	//			break;

	//		case Stage.MainWindowTest:
	//			{
	//				return new MainWindowTestDirector( null );
	//			}

	//		default:
	//			ObjectHelper.IfNullThrowArgument( m_stage, "m_stage" );
	//			break;
	//	}

	//	return this;
	//}

	#endregion

	Stage m_stage;
	enum Stage
	{
		UpdateVersion, Login, MainWindow
	}

	public RootDirector()
	{
		m_stage = Stage.UpdateVersion;
	}

	public override GameDirector Update()
	{
		switch( m_stage )
		{
			case Stage.UpdateVersion:
				{
					// .....UpdateVersion   // [Dev-wangyeaho] 之后有了 UpdateVersion阶段要改
					m_stage = Stage.Login;
				}
				break;

			case Stage.Login:
				break;

			case Stage.MainWindow:
				{

				}
				break;

			default:
				ObjectHelper.ThrowEnumSpecializedArgument( m_stage.ToString() );
				break;
		}

		return this;
	}

}
