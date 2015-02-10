using System;
using Hovercast.Core.Display.Default;
using Hovercast.Core.Navigation;
using UnityEngine;

namespace Hovercast.Core.Settings {

	/*================================================================================================*/
	public class HovercastDefaultSettingsProvider : HovercastSettingsProvider {

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

		protected readonly ArcSegmentSettings vArcSegment;
		protected readonly CursorSettings vCursor;
		protected readonly InteractionSettings vInteraction;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastDefaultSettingsProvider() {
			vArcSegment = new ArcSegmentSettings();
			vArcSegment.TextSize = TextSize;
			vArcSegment.TextColor = TextColor;
			vArcSegment.TextFont = TextFont;
			vArcSegment.ArrowIconColor = ArrowIconColor;
			vArcSegment.ToggleIconColor = ToggleIconColor;
			vArcSegment.BackgroundColor = BackgroundColor;
			vArcSegment.EdgeColor = EdgeColor;
			vArcSegment.HighlightColor = HighlightColor;
			vArcSegment.SelectionColor = SelectionColor;
			vArcSegment.SliderTrackColor = SliderTrackColor;
			vArcSegment.SliderFillColor = SliderFillColor;
			vArcSegment.SliderTickColor = SliderTickColor;

			vCursor = new CursorSettings();
			vCursor.ColorNorm = CursorColorNormal;
			vCursor.ColorHigh = CursorColorHighlighted;
			vCursor.RadiusNorm = CursorRadiusNormal;
			vCursor.RadiusHigh = CursorRadiusHighlighted;
			vCursor.ThickNorm = CursorThicknessNormal;
			vCursor.ThickHigh = CursorThicknessHighlighted;

			vInteraction = new InteractionSettings();
			vInteraction.IsMenuOnLeftSide = IsMenuOnLeftSide;
			vInteraction.HighlightDistanceMin = HighlightDistanceMin;
			vInteraction.HighlightDistanceMax = HighlightDistanceMax;
			vInteraction.StickyReleaseDistance = StickyReleaseDistance;
			vInteraction.SelectionMilliseconds = SelectionMilliseconds;
			vInteraction.CursorForwardDistance = CursorForwardDistance;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override ArcSegmentSettings GetArcSegmentSettings(NavItem pNavItem) {
			return vArcSegment;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override CursorSettings GetCursorSettings() {
			return vCursor;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override InteractionSettings GetInteractionSettings() {
			return vInteraction;
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
