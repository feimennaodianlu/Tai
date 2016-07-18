using Tai.Client;
using UnityEngine;
using UnityEngine.UI;

/*  时间： 2016.6.28  周二  v1 提交可使用版本
 * —————————————————————————————————————————————————————— * 
 *	功能说明：	所有 ui 的根目录。所有 window 全都放在 rootCanvas 下
 * 
 *	使用说明：
 *				1.  通过 GameManager.Instance.UIRoot 去获取
 *		
 *	后续待扩展：暂无 
 * 
 *  注意事项：  
 *				1. 注意 GameManager.Instance 的初始化，防止在后续场景中由于 GameManager.Instance 为 Null 而获取 uiroot 失败 
 *				2. prefab 中 RootCanvas 和 UICamera 不要轻易改名字，否则要在代码中适配 
 *  
 */

public class UIRoot : MonoBehaviour
{
	Canvas m_rootCanvas;
	Camera m_uiCamera;
	CanvasScaler m_canvasScaler;

	public static UIRoot Create()
	{
		var asset = GameManager.Instance.GameLoad.Load<GameObject>( Path.UIRoot );

		var go = Instantiate<GameObject>( asset ).GetComponent<UIRoot>();
		go.Initialize();

		return go;
	}

	void Initialize()
	{
		DontDestroyOnLoad( gameObject );

		m_rootCanvas = transform.Find( "RootCanvas" ).GetComponent<Canvas>();
		m_canvasScaler = m_rootCanvas.GetComponent<CanvasScaler>();
		m_uiCamera = transform.Find( "UICamera" ).GetComponent<Camera>();
	}

	public TWindow FindWindow<TWindow>() where TWindow : UIWindow
	{
		return m_rootCanvas.transform.FindWindow<TWindow>();
	}

	#region Properties

	public Canvas RootCanvas
	{
		get { return m_rootCanvas; }
	}

	public Camera UICamera
	{
		get { return m_uiCamera; }
	}

	public CanvasScaler CanvasScaler
	{
		get { return m_canvasScaler; }
	}

	#endregion
}

static class WindowFinderExtension
{
	public static TWindow FindWindow<TWindow>(this Transform t)
		where TWindow : UIWindow
	{
		var go = t.Find( typeof( TWindow ).Name ) ??
			t.Find( typeof( TWindow ).Name + "(Clone)" );

		return go ? go.GetComponent<TWindow>() : null;
	}
}