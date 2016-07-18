using UnityEngine;
using System.Collections;

/*  时间： xxxx.xx.xx      v1 提交可使用版本
          2016.7.7 周四   v2 增加备注
 * ————————————————————————————————————————————————————————
 * 
 *	功能说明：	代码中所有需要加载的资源路径名（除场景在 class Scene 中）从本脚本获取。
 * 
 *	使用说明：
 *				1.  情况1： 使用 AssetDatabase.Load 加载资源 => Path.XXX。 添加路径时，路径均为project视图中的路径（需要扩展名），不需要 "Assets/"。
 *					情况2： 使用 Resources.Load 加载的资源 => Path.Resource.XXX。 添加路径时，需要扩展名
 *		            
 *	后续待扩展：  
 *				1. 当加入 ab 可能需要调整使用方法
 *				
 *  注意事项：   
 *				1. 当前资源主要是使用 AssetDatabase.Load，这个方法不能打包，所以发布时肯定要修改 ！！！
 */
public class Path
{
	// Test(Director)
	#region Test

	public static class Test
	{
		public static readonly string HaoWindow = "zTestDirector/Prefabs/HaoWindow.prefab";
		public static readonly string YeWindow = "zTestDirector/Prefabs/YeWindow.prefab";
		public static readonly string MainWindowTestWindow = "zTestDirector/Prefabs/MainWindowTestWindow.prefab";
	}

	#endregion

	// Located in Resources folder ( without extension )
	public static class Resource
	{
		public static readonly string GameManager = "GameManager";
		public static readonly string MainStatus = "UI/Prefabs/Main/MainStatus";
	}

	// UICore
	public const string UIRoot = @"Prefabs/GameManager/UIRoot.prefab";
	public const string UIMessageBox = "Prefabs/UI/Common/UIMessageBox.prefab";
	public const string ModelBackground = "Prefabs/UI/Common/ModelBg.prefab";
	public const string LoadingWindow = "Prefabs/UI/LoadingWindow/LoadingWindow.prefab";
	public const string UIMessageTip = "Prefabs/UI/Common/UIMessageTip.prefab";

	// Start Menu
	public const string StartMenuDirector = "Prefabs/UI/StartMenu/StartMenuDirector.prefab";
	public const string StartWindow = "Prefabs/UI/StartMenu/StartWindow.prefab";
	public const string LoginWindow = "Prefabs/UI/StartMenu/LoginWindow.prefab";
	public const string RegisterWindow = "Prefabs/UI/StartMenu/RegisterWindow.prefab";
	public const string ServerListWindow = "Prefabs/UI/StartMenu/ServerListWindow/ServerListWindow.prefab";
	public const string NewServer = "Prefabs/UI/StartMenu/ServerListWindow/NewServer.prefab";
	public const string HotServer = "Prefabs/UI/StartMenu/ServerListWindow/HotServer.prefab";
	public const string CharacterSelectionWindow = @"Prefabs/UI/StartMenu/CharacterSelectionWindow.prefab";

	// Main Window
	public const string MainWindowDirector = "Prefabs/UI/MainWindow/MainWindowDirector.prefab";
}