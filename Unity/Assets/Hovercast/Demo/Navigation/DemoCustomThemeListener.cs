using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;
using UnityEngine;

namespace Hovercast.Demo.Navigation {

	/*================================================================================================*/
	public class DemoCustomThemeListener : DemoBaseListener<NavItemRadio> {

		public enum ThemeType {
			Dark,
			Light,
			Color
		}

		public ThemeType Type;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(NavItem<bool> pNavItem) {
			if ( !pNavItem.Value ) {
				return;
			}

			ArcSegmentSettings sett = DemoSettingsComponent.ArcSegmentSettings;

			switch ( Type ) {
				case ThemeType.Dark:
					sett.TextColor = new Color(1, 1, 1);
					sett.ArrowIconColor = new Color(1, 1, 1);
					sett.ToggleIconColor = new Color(1, 1, 1);
					sett.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
					sett.EdgeColor = new Color(0.5f, 0.5f, 0.5f, 1);
					sett.HighlightColor = new Color(0.25f, 0.25f, 0.25f, 0.43f);
					sett.SelectionColor = new Color(0.5f, 0.5f, 0.5f, 1);
					sett.SliderTrackColor = new Color(0.1f, 0.1f, 0.1f, 0.25f);
					sett.SliderFillColor = new Color(0.5f, 0.5f, 0.5f, 0.25f);
					sett.SliderTickColor = new Color(1, 1, 1, 0.25f);
					break;

				case ThemeType.Light:
					sett.TextColor = new Color(0, 0, 0);
					sett.ArrowIconColor = new Color(0, 0, 0);
					sett.ToggleIconColor = new Color(0, 0, 0);
					sett.BackgroundColor = new Color(0.9f, 0.9f, 0.9f, 0.5f);
					sett.EdgeColor = new Color(1, 1, 1, 1);
					sett.HighlightColor = new Color(0.75f, 0.75f, 0.75f, 0.57f);
					sett.SelectionColor = new Color(0.5f, 0.5f, 0.5f, 1);
					sett.SliderTrackColor = new Color(0.9f, 0.9f, 0.9f, 0.25f);
					sett.SliderFillColor = new Color(0.5f, 0.5f, 0.5f, 0.25f);
					sett.SliderTickColor = new Color(0, 0, 0, 0.25f);
					break;

				case ThemeType.Color:
					sett.TextColor = new Color(1, 1, 0.7f);
					sett.ArrowIconColor = new Color(1, 1, 0.7f);
					sett.ToggleIconColor = new Color(1, 1, 0.7f);
					sett.BackgroundColor = new Color(0.05f, 0.25f, 0.45f, 0.5f);
					sett.EdgeColor = new Color(0.1f, 0.9f, 0.2f);
					sett.HighlightColor = new Color(0.1f, 0.5f, 0.9f);
					sett.SelectionColor = new Color(0.1f, 0.9f, 0.2f);
					sett.SliderTrackColor = new Color(0.1f, 0.5f, 0.9f, 0.5f);
					sett.SliderFillColor = new Color(0.1f, 0.9f, 0.2f, 0.5f);
					sett.SliderTickColor = new Color(1, 1, 1, 0.2f);
					break;
			}

			DemoSettingsComponent.UpdateSettingsWithBgAlpha();
		}

	}

}
