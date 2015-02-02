using System;
using Hovercast.Core.Display.Default;
using Hovercast.Core.Navigation;
using UnityEngine;

namespace Hovercast.Core.Settings {

	/*================================================================================================*/
	public class HovercastDefaultSettingsComponent : HovercastSettingsComponent {

		public int TextSize = 30;
		public Color TextColor = new Color(1, 1, 1);
		public string TextFont = "Tahoma";
		public Color ArrowIconColor = new Color(1, 1, 1);
		public Color ToggleIconColor = new Color(1, 1, 1);
		public Color BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
		public Color EdgeColor = new Color(1, 1, 1, 0.5f);
		public Color HighlightColor = new Color(0.1f, 0.5f, 0.9f);
		public Color SelectionColor = new Color(0.1f, 0.9f, 0.2f);
		public Color SliderTrackColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
		public Color SliderFillColor = new Color(0.1f, 0.9f, 0.2f, 0.5f);
		public Color SliderTickColor = new Color(1, 1, 1, 0.1f);

		public Color CursorColorNormal = new Color(1, 1, 1, 0.6f);
		public Color CursorColorHighlighted = new Color(1, 1, 1, 1);
		public float CursorRadiusNormal = 0.12f;
		public float CursorRadiusHighlighted = 0.06f;
		public float CursorThicknessNormal = 0.1f;
		public float CursorThicknessHighlighted = 0.4f;

		public bool IsMenuOnLeftSide = true;
		public float HighlightDistanceMin = 0.05f;
		public float HighlightDistanceMax = 0.1f;
		public float StickyReleaseDistance = 0.07f;
		public float SelectionMilliseconds = 600;
		public float CursorForwardDistance = 0.0f;


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
			sett.EdgeColor = EdgeColor;
			sett.HighlightColor = HighlightColor;
			sett.SelectionColor = SelectionColor;
			sett.SliderTrackColor = SliderTrackColor;
			sett.SliderFillColor = SliderFillColor;
			sett.SliderTickColor = SliderTickColor;
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
			inter.IsMenuOnLeftSide = IsMenuOnLeftSide;
			inter.HighlightDistanceMin = HighlightDistanceMin;
			inter.HighlightDistanceMax = HighlightDistanceMax;
			inter.StickyReleaseDistance = StickyReleaseDistance;
			inter.SelectionMilliseconds = SelectionMilliseconds;
			inter.CursorForwardDistance = CursorForwardDistance;
			return inter;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Type GetUiArcSegmentRendererTypeInner(NavItem pNavItem) {
			switch ( pNavItem.Type ) {
				case NavItem.ItemType.Parent:
					return typeof(UiParentRenderer);

				case NavItem.ItemType.Selector:
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
