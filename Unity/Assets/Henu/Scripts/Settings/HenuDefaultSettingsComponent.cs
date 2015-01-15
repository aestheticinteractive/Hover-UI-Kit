using System;
using Henu.Display.Default;
using Henu.Navigation;
using UnityEngine;

namespace Henu.Settings {

	/*================================================================================================*/
	public class HenuDefaultSettingsComponent : HenuSettingsComponent {

		public int TextSize = 26;
		public Color TextColor = new Color(1, 1, 1);
		public Color ArrowIconColor = new Color(1, 1, 1);
		public Color ToggleIconColor = new Color(1, 1, 1);
		public Color BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
		public Color HighlightColor = new Color(0.1f, 0.5f, 0.9f);
		public Color SelectionColor = new Color(0.1f, 0.9f, 0.2f);

		public float NavigationBackGrabThreshold = 0.5f;
		public float NavigationBackUngrabThreshold = 0.25f;

		public float HighlightDistanceMin = 0.05f;
		public float HighlightDistanceMax = 0.15f;
		public float SelectionMilliseconds = 600;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override ArcSegmentSettings GetArcSegmentSettings(NavItem pNavItem) {
			var colors = new ArcSegmentSettings();
			colors.TextSize = TextSize;
			colors.TextColor = TextColor;
			colors.ArrowIconColor = ArrowIconColor;
			colors.ToggleIconColor = ToggleIconColor;
			colors.BackgroundColor = BackgroundColor;
			colors.HighlightColor = HighlightColor;
			colors.SelectionColor = SelectionColor;
			return colors;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override InteractionSettings GetInteractionSettings() {
			var inter = new InteractionSettings();
			inter.NavBackGrabThreshold = NavigationBackGrabThreshold;
			inter.NavBackUngrabThreshold = NavigationBackUngrabThreshold;
			inter.HighlightDistanceMin = HighlightDistanceMin;
			inter.HighlightDistanceMax = HighlightDistanceMax;
			inter.SelectionMilliseconds = SelectionMilliseconds;
			return inter;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Type GetUiArcSegmentRendererTypeInner(NavItem pNavItem) {
			switch ( pNavItem.Type ) {
				case NavItem.ItemType.Parent:
					return typeof(UiArcSegmentParentRenderer);

				case NavItem.ItemType.Checkbox:
					return typeof(UiArcSegmentCheckboxRenderer);

				case NavItem.ItemType.Radio:
					return typeof(UiArcSegmentRadioRenderer);
			}

			return typeof(UiArcSegmentRenderer);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override Type GetUiCursorRendererTypeInner() {
			return typeof(UiCursorRenderer);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override Type GetUiPalmRendererTypeInner() {
			return typeof(UiPalmRenderer);
		}

	}

}
