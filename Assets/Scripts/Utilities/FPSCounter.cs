using UnityEngine;
using UnityEngine.UI;

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!! [Tai] 适配 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

/*  时间： 2016.6.28  周二  v1 提交可使用版本
 * ————————————————————————————————————————————————————————
 * 
 *	功能说明：	用于显示当前游戏的帧率
 * 
 *	使用说明：
 *				直接把 FPSCounter.prefab 挂在 rootCanvas 下即可
 *		
 *	后续待扩展： 	        
 * 
 *  注意事项：  不需要每帧都更新，
 *				且倘若每帧都更新，则 Text 会因为不断地变化反而看不清楚
 *  
 */

[RequireComponent( typeof( Text ) )]
public class FPSCounter : MonoBehaviour
{
	Text m_counter;

	[SerializeField]
	string m_displayFormat = "{0} FPS";
	[SerializeField]
	float m_deltaTime = 0.5f;

	int m_accumulator;
	float m_timer;

	void Awake()
	{
		m_counter = GetComponent<Text>();
	}

	void Update()
	{
		if( m_timer > m_deltaTime )
		{
			SetFPS( m_accumulator / m_timer );

			m_accumulator = 0;
			m_timer = 0;
		}
		else
		{
			m_accumulator++;
			m_timer += Time.deltaTime;
		}
	}

	void SetFPS(float fps)
	{
		m_counter.text = string.Format( m_displayFormat, Mathf.RoundToInt( fps ) );  // 四舍五入六取偶
	}
}