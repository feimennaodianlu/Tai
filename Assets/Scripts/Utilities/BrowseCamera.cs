using System;
using UnityEngine;

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!! [Tai] 适配 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

/*  时间： 2016.6.29  周三  v1 提交可使用版本
 * ————————————————————————————————————————————————————————
 * 
 *	功能说明：	1. 用于展示、浏览商品。
 * 
 *	使用说明：
 *				1. 在 unitEditor、standalone 中可以使用鼠标，在手机上使用触屏滑动
 *				2. 在 play 前需要把相机固定到 play 后的起始位置。该位置目前不能通过代码设置
 *				3. BrowseCamera 下有子物体 Direction light
 *				
 *	后续待扩展：  
 *				1. 代码设置 camera 的 play 后的起始位置
 *				2. 在横向限制角度
 *				3. 107 行 // !!!! Quaterion * Vector3 = Vector3
 * 
 *   注意事项：  
 *				1. 设置 m_target 时，赋的是展示对象的几何中心
 *				2. 无法在横向限制角度
 *				3. 实现 ICheckParams 的类，会在本脚本初始化时检查参数，如果设置不正确会使用默认值（此时会有 warning）
 *				4. 实现原理： 每一帧都根据滑动得到新的 x,y,distance，然后根据 target.position 算出 camera 的位置
 */

[RequireComponent( typeof( Camera ) )]
[DisallowMultipleComponent]
public class BrowseCamera : MonoBehaviour
{
	[SerializeField, Tooltip( "特指 target 的几何中心" )]
	Transform m_target = null;

	[SerializeField]
	XAxis m_xAxis = null;

	[SerializeField, Space( 5 )]
	YAxis m_yAxis = null;

	[SerializeField, Space( 5 )]
	ScrollWheel m_scrollWheel = null;

	[SerializeField, Space( 5 )]
	Speed m_speed = null;

	[SerializeField, Space( 5 )]
	Damping m_damping = null;

	enum MouseType
	{
		Left,
		Right
	}

	float m_distance;
	float m_x;  // 保存当前的角度（是实际值，不是delta。类似 to 和 by 的关系）。 2d 中 x,y 轴，但不是实际3d的x，轴
	float m_y;

	void Start()
	{
		Initialize();
	}

	void Initialize()
	{
		if( null == m_target ) return;

		if( m_damping != null )
			m_damping.SelfCheckParams();

		m_scrollWheel.SelfCheckParams();

		m_yAxis.SelfCheckParams();

		m_x = transform.eulerAngles.y;
		m_y = transform.eulerAngles.x;
		m_distance = Vector3.Distance( transform.position, m_target.position );
	}

	void Update()
	{
		if( null == m_target ) return;

#if UNITY_EDITOR || UNITY_STANDALONE

		if( Input.GetMouseButton( (int)m_xAxis.Type ) )
		{
			m_x += m_speed.Xspeed * Input.GetAxis( "Mouse X" );
			m_y -= m_speed.Yspeed * Input.GetAxis( "Mouse Y" );
		}

#elif UNITY_ANDROID || UNITY_IPONE

		if( Input.touchCount != 0 && Input.GetTouch( 0 ).phase == TouchPhase.Moved )
		{
			m_x += m_speed.Xspeed * Input.GetAxis( "Mouse X" );
			m_y -= m_speed.Yspeed * Input.GetAxis( "Mouse Y" );

			m_y = ClampAngle( m_y, m_yAxis.Ymin, m_yAxis.Ymax );
		}
#endif

		m_distance -= m_speed.ScrollWheelSpeed * Input.GetAxis( "Mouse ScrollWheel" );
		m_distance = Mathf.Clamp( m_distance, m_scrollWheel.MinDistance, m_scrollWheel.MaxDistance );

		var rotation = Quaternion.Euler( m_y, m_x, 0 );
		var position = m_target.position + rotation * new Vector3( 0, 0, -m_distance );  // !!!! Quaterion * Vector3 = Vector3

		AdjustCamera( position, rotation );
	}


	void AdjustCamera(Vector3 position, Quaternion rotation)
	{
		if( m_damping != null && m_damping.IsNeedDamp )
		{
			transform.position = Vector3.Lerp( transform.position, position, m_damping.DampValue * Time.deltaTime );
			transform.rotation = Quaternion.Lerp( transform.rotation, rotation, m_damping.DampValue * Time.deltaTime );
		}
		else
		{
			transform.position = position;
			transform.rotation = rotation;
		}
	}

	static float ClampAngle(float angle, float min, float max)
	{
		if( angle < -360f ) angle += 360f;
		if( angle > 360f ) angle -= 360f;

		return Mathf.Clamp( angle, min, max );
	}

	#region About Inspector Show

	[Serializable]
	class Speed
	{
		public float Xspeed = 10f;
		public float Yspeed = 10f;
		public float ScrollWheelSpeed = 10f;
	}

	[Serializable]
	class Damping : ICheckParams
	{
		public bool IsNeedDamp = false;
		public float DampValue = 5f;  // 需要>0，如果<=0，则认为不需要

		public void SelfCheckParams()
		{
			if( !IsNeedDamp && DampValue <= 0f )
			{
				IsNeedDamp = false;

				Debug.LogWarning( "Damping params setting is fault" );
			}
		}
	}

	[Serializable]
	class ScrollWheel : ICheckParams
	{
		public float MinDistance = 0f;
		public float MaxDistance = 30f;

		public void SelfCheckParams()
		{
			if( MinDistance < 0f )
			{
				MinDistance = 0f;
				Debug.LogWarning( "ScrollWheel params setting is fault" );
			}
			if( MaxDistance <= 0 )
			{
				MaxDistance = 30f;
				Debug.LogWarning( "ScrollWheel params setting is fault" );
			}
		}
	}

	[Serializable]
	class XAxis
	{
		[SerializeField]
		protected MouseType MouseType = MouseType.Left;

		public int Type
		{
			get { return (int)MouseType; }
		}
	}

	[Serializable]
	class YAxis : XAxis, ICheckParams
	{
		public float Ymin = -60f;
		public float Ymax = 60f;

		public void SelfCheckParams()
		{
			if( Ymin < Ymax ) return;

			Ymin = -60f;
			Ymax = 60f;

			Debug.LogWarning( "YAxis params setting is fault" );
		}
	}

	interface ICheckParams
	{
		void SelfCheckParams();
	}

	#endregion
}