using UnityEngine;
using UnityEngine.UI;

// 主界面左上角的控件
public class Playerbar : MonoBehaviour
{
	[SerializeField]
	Image Portrait = null;

	[SerializeField]
	Text PlayerName = null;

	[SerializeField]
	string m_displayFormat = "{0} / {1}";

	[SerializeField]
	Slider EnergySlider = null;

	[SerializeField]
	Text EnergySliderText = null;

	[SerializeField]
	Slider ToughenSlider = null;

	[SerializeField]
	Text ToughenSliderText = null;

	[SerializeField]
	Text Level = null;

	Content m_content;
	IHandler m_handler;

	#region Content & IHandler

	public class Content
	{
		public Sprite Portrait;
		public string PlayerName;
		public int Energy;
		public int MaxEnergy;
		public int Toughen;
		public int MaxToughen;
		public int Level;

		public Content(Sprite portrait, string name, int energy, int maxEnergy, int toughen, int maxToughen, int level)
		{
			Portrait = portrait;
			PlayerName = name;
			Energy = energy;
			MaxEnergy = maxEnergy;
			Toughen = toughen;
			MaxToughen = maxToughen;
			Level = level;
		}
	}

	public interface IHandler
	{
		void ClickEnergyAdd();
		void ClickToughenAdd();

		void ClickGamePlay(string gamePlayName);
		void ClickToSpread();
		void ClickToShrink();
	}

	#endregion

	public void Initialize(Content content, IHandler handler)
	{
		ObjectHelper.IfNullThrowArgument( content, "content" );
		ObjectHelper.IfNullThrowArgument( handler, "handler" );

		m_handler = handler;

		Portrait.sprite = content.Portrait;
		PlayerName.text = content.PlayerName;
		EnergySlider.value = content.Energy;
		EnergySliderText.text = string.Format( m_displayFormat, content.Energy.ToString(), content.MaxEnergy.ToString() );
		ToughenSlider.value = content.Toughen;
		ToughenSliderText.text = string.Format( m_displayFormat, content.Toughen.ToString(), content.MaxToughen.ToString() );
		Level.text = content.Level.ToString();
	}

	#region Event

	public void OnClickGamePlay(string gamePlayName)
	{
		m_handler.ClickGamePlay( gamePlayName );
	}

	#endregion
}
