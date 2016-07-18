using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour
{
	string m_playerName;
	Sprite m_portrait;
	int m_enery;
	int m_toughen;
	int m_level;
	int m_exp;
	int m_vip;
	int m_power;
	int m_coin;
	int m_diamond;

	float m_energyTimer;
	float m_toughenTimer;

	// temp
	const int s_maxEnergy = 100;
	const int s_maxToughen = 50;

	#region Properties

	public Sprite Portrait
	{
		get { return m_portrait; }
	}

	public string PlayerName
	{
		get { return m_playerName; }
	}

	public int Energy
	{
		get { return m_enery; }
	}

	public int MaxEnergy
	{
		get { return s_maxEnergy; }
	}

	public int Toughen
	{
		get { return m_toughen; }
	}

	public int MaxToughen
	{
		get { return s_maxToughen; }
	}

	public int Level
	{
		get { return m_level; }
	}

	public int Coin
	{
		get { return m_coin; }
	}

	public int Diamond
	{
		get { return m_diamond; }
	}

	#endregion

	void Update()
	{
		// Energy & toughen automatic growth.
		Accumulator( ref m_enery, ref m_energyTimer, s_maxEnergy, 1f );
		Accumulator( ref m_toughen, ref m_toughenTimer, s_maxToughen, 1f );
	}

	void Accumulator(ref int target, ref float timer, int max, float deltaTime)
	{
		if( target < max )
			timer += Time.deltaTime;
		else
		{
			if( timer != 0f )
				timer = 0f;
		}

		if( timer > deltaTime )
		{
			target++;
			timer = 0;
		}
	}


	// Generate Fake Data
	// Get from the server in fact
	//public void GenerateFakeData()
	//{
	//	m_coin = 9870;
	//	m_diamond = 1234;
	//	m_enery = 78;
	//	m_exp = 123;
	//	m_level = 12;
	//	m_playerName = "千颂伊";
	//	m_portrait = TempSprite;
	//	m_power = 174;
	//	m_toughen = 34;
	//}
}