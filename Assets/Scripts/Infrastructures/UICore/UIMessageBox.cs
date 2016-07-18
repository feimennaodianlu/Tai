using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/*   时间： 2016.6.28  周二  v1 提交可使用版本 
 * ——————————————————————————————————————————————————————
 * 
 *  功能说明：
 *			1. ModelBg 已在 UIWindow.cs 中设置。
 *			2. 点击按钮时，如果从 Dictionary 中找不到按钮类型对应的事件，则一律认为是关闭 UIWindow
 * 
 *  使用说明：
 *			1. 直接调用 Create() 方法即可。
 *			2. Action 设置时，如果不设置，则一律认为是关闭 UIWindow
 *			3. 设置 Content.Action 时，style 必须是 Action.Keys 的子集。当 enum Button 存在于 enum style 中，但不存在于 Action.Keys 中此时使用默认事件（点击事件）。如果存在于 Action.Keys 中，但不存在与 style 中，则报错
 *	
 *  后续待扩展：
 *   
 * 
 *  注意事项：
 *			1. 目前设置 点击按钮后，不管执行什么事件，最后都会执行 UIWindow.Show = false。如果有地方不需要 UIMessageBox 执行该操作，则修改相应代码。并在所有 UIMessageBox.Create 地方做适配
 *			2. 想要修改 button 的显示顺序 => Initialize()
 *  
 */

namespace Tai.Client
{
	public class UIMessageBox : UIWindow
	{
		[SerializeField]
		Text m_message = null;

		[SerializeField]
		Transform m_buttonParent = null;

		Content m_content;

		Button[] m_buttons;  // button 显示顺序同索引顺序（click event, localization）

		#region Enum

		[Flags]
		public enum Button
		{
			Ok = 1 << 1,
			Cancel = 1 << 2,
			Yes = 1 << 3,
			No = 1 << 4,
			Retry = 1 << 5,
			Custom = 1 << 6  // 自定义名字
		}

		public enum Style
		{
			Ok = Button.Ok,
			OKCancel = Button.Ok | Button.Cancel,
			YesNo = Button.Yes | Button.No,
			YesNoCancel = Button.Yes | Button.No | Button.Cancel,
			Retry = Button.Retry,
			Custom = Button.Custom,
			CustomCancel = Button.Custom | Button.Cancel
		}

		#endregion

		#region Content & IHandler

		public class Content
		{
			public string Text;
			public Style Style;
			public IDictionary<Button, Action> Actions;
			public string CustomName;
			public bool IsNeedModelBg;

			Content(string text, Style style, bool isNeedModelBg, string customName)
			{
				// Check customName
				// Unity 使用 .net framwork v3.5。从 v4 开始才增加了 Enum实例.HasFlag() 方法
				if( ( style & Style.Custom ) == Style.Custom && null == customName )
					ObjectHelper.ThrowArgument( "customName should not be NULL" );

				Text = text;
				Style = style;
				IsNeedModelBg = isNeedModelBg;
				CustomName = customName;
			}

			/// <summary>
			/// 用于 Style 为多个 enum Button 合成的情况
			/// </summary>
			public Content(string text, Style style, IDictionary<Button, Action> actions = null, bool isNeedModelBg = true, string customName = null)
				: this( text, style, isNeedModelBg, customName )
			{
				if( null == actions ) return;

				if( actions.Keys != null && CheckParamsIsFault( style, actions.Keys.ToArray() ) )
					ObjectHelper.ThrowArgument( "传入的枚举与位域枚举不匹配" );

				Actions = actions;
			}

			/// <summary>
			/// 用于 Style 仅为 1 个 enum Button 的情况。使用该构造函数时，如果需要歧义，使用 ()=>{}
			/// </summary>
			public Content(string text, Style style, Action action = null, bool isNeedModelBg = true, string customName = null)
				: this( text, style, isNeedModelBg, customName )
			{
				if( null == action ) return;

				Button buttonStyle;

				// Unity 使用 .net framwork v3.5。从 v4 开始才增加了 TryParse 方法                                                                                                                           
				try
				{
					//buttonStyle = (Button)Enum.Parse( typeof( Button ), style.ToString(), true );
					buttonStyle = (Button)style;
				}
				catch( Exception e )
				{
					throw new ArgumentException( "enum cast failure", e );
				}

				Actions = new Dictionary<Button, Action>();
				Actions.Add( buttonStyle, action );
			}

			#region Helper

			// 传入的 button[] 已经保证有值
			// 改变 button 和 Style 枚举不需要适配本函数
			bool CheckParamsIsFault(Style style, Button[] buttonStyle)
			{
				var styleToButton = (Button)style;

				var parseResult = new List<Button>();
				foreach( var b in buttonStyle )
				{
					// if true => buttonStyle 有位域枚举不存在于 style 中
					if( ( styleToButton & b ) != b )
					{
						parseResult.Add( b );
						break;
					}
				}

				return parseResult.Count > 0;
			}

			#endregion
		}

		#endregion

		#region Create & Initialize

		public static UIMessageBox Create(Content content)
		{
			ObjectHelper.IfNullThrowArgument( content, "content" );

			var asset = AssetDatabase.LoadAssetAtPath<UIMessageBox>( Path.UIMessageBox );
			var go = Instantiate<UIMessageBox>( asset );
			go.Initialize( content );

			return go;
		}

		public static UIMessageBox CreateModel(string message, bool isNeedModelBg)
		{
			var asset = GameManager.Instance.GameLoad.Load<UIMessageBox>( Path.UIMessageBox );
			var go = Instantiate<UIMessageBox>( asset );
			go.Initialize( new Content( message, Style.Ok, () => { }, isNeedModelBg, null ) );

			return go;
		}

		void Initialize(Content content)
		{
			base.InitializeWindow( null, null, content.IsNeedModelBg );  // true => 需要模态背景
			transform.SetAsLastSibling();

			m_content = content;

			#region Button Sequence

			// Message
			m_message.text = content.Text;

			// 需要几个button
			switch( content.Style )
			{
				// 下面参数的数值的顺序是按钮显示顺序
				case Style.Ok:
					SetButtonGroup( Button.Ok );
					break;

				case Style.OKCancel:
					SetButtonGroup( Button.Cancel, Button.Ok );
					break;

				case Style.YesNo:
					SetButtonGroup( Button.No, Button.Yes );
					break;

				case Style.YesNoCancel:
					SetButtonGroup( Button.Cancel, Button.No, Button.Yes );
					break;

				case Style.Retry:
					SetButtonGroup( Button.Retry );
					break;

				case Style.Custom:
					SetButtonGroup( Button.Custom );
					break;

				case Style.CustomCancel:
					SetButtonGroup( Button.Cancel, Button.Custom );
					break;

				default:
					ObjectHelper.ThrowEnumSpecializedArgument( "content.Style" );
					break;
			}

			#endregion
		}

		#endregion

		// 找到需要的 button go 并与 enum Button 配对
		void SetButtonGroup(params Button[] buttons)
		{
			m_buttons = buttons;

			var t = m_buttonParent.Find( buttons.Length.ToString() );

			ObjectHelper.SetActive( t.gameObject, true );

			// Set 哪个 button 显示什么文字
			for( var i = 1; i <= buttons.Length; i++ )
				SetButtonText( t.Find( i.ToString() ), buttons[i - 1] );
		}

		// 给 button 和 buttonStyles，设置 text
		void SetButtonText(Transform button, Button buttonStyle)
		{
			var label = button.Find( "Text" ).GetComponent<Text>();
			if( Button.Custom == buttonStyle )
				label.text = m_content.CustomName;
			else
			{
				label.text = GameManager.Instance.Localization["MessageBox." + buttonStyle.ToString()];  // 
			}
		}

		#region Event

		void ClickButton(Button button)
		{
			if( m_content.Actions != null && m_content.Actions.ContainsKey( button ) )
				m_content.Actions[button].Invoke();

			Show = false;
		}

		public void OnButton1Click()
		{
			ClickButton( m_buttons[0] );
		}

		public void OnButton2Click()
		{
			ClickButton( m_buttons[1] );
		}

		public void OnButton3Click()
		{
			ClickButton( m_buttons[2] );
		}

		#endregion

		// [Tai]
		#region Override

		protected override void OnClickModelBg()
		{
		}

		#endregion
	}
}