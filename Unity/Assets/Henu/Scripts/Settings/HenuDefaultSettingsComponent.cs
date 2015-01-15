using System;
using Henu.Display.Default;
using Henu.Navigation;
using UnityEngine;

namespace Henu.Settings {

	/*================================================================================================*/
	public class HenuDefaultSettingsComponent : HenuSettingsComponent {

		public int TextSize = 26;
		public Color TextColor = new Color(1, 1, 1);
		public string TextFont = "Tahoma";
		public Color ArrowIconColor = new Color(1, 1, 1);
		public Color ToggleIconColor = new Color(1, 1, 1);
		public Color BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
		public Color HighlightColor = new Color(0.1f, 0.5f, 0.9f);
		public Color SelectionColor = new Color(0.1f, 0.9f, 0.2f);
		public Color SliderTrackColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
		public Color SliderFillColor = new Color(0.1f, 0.9f, 0.2f, 0.5f);

		public Color CursorColorNormal = new Color(1, 1, 1, 0.75f);
		public Color CursorColorHighlighted = new Color(1, 1, 1, 1);
		public float CursorRadiusNormal = 0.1f;
		public float CursorRadiusHighlighted = 0.1f;
		public float CursorThicknessNormal = 0.1f;
		public float CursorThicknessHighlighted = 0.3f;

		public float NavigationBackGrabThreshold = 0.5f;
		public float NavigationBackUngrabThreshold = 0.25f;

		public float HighlightDistanceMin = 0.05f;
		public float HighlightDistanceMax = 0.15f;
		public float SelectionMilliseconds = 600;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override ArcSegmentSettings GetArcSegmentSettings(NavItem pNavItem) {
			var sett = new ArcSegmentSettings();
			sett.TextSize = TextSize;
			sett.TextColor = TextColor;
			sett.TextFont = TextFont;
			sett.ArrowIconColor = ArrowIconColor;
			sett.ToggleIconColor = ToggleIconColor;
			sett.BackgroundColor = BackgroundColor;
			sett.HighlightColor = HighlightColor;
			sett.SelectionColor = SelectionColor;
			sett.SliderTrackColor = SliderTrackColor;
			sett.SliderFillColor = SliderFillColor;
			return sett;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override CursorSettings GetCursorSettings() {
			var sett = new CursorSettings();
			sett.ColorNorm = CursorColorNormal;
			sett.ColorHigh = CursorColorHighlighted;
			sett.RadiusNorm = CursorRadiusNormal;
			sett.RadiusHigh = CursorRadiusHighlighted;
			sett.ThickNorm = CursorThicknessNormal;
			sett.ThickHigh = CursorThicknessHighlighted;
			return sett;
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
					return typeof(UiParentRenderer);

				case NavItem.ItemType.Selection:
					return typeof(UiSelectRenderer);

				case NavItem.ItemType.Sticky:
					return typeof(UiStickyRenderer);

				case NavItem.ItemType.Checkbox:
					return typeof(UiCheckboxRenderer);

				case NavItem.ItemType.Radio:
					return typeof(UiRadioRenderer);

				case NavItem.ItemType.Slider:
					return typeof(UiSliderRenderer);
			}

			return typeof(UiSelectRenderer);
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
