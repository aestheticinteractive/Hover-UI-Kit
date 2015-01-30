using System;
using System.Linq;
using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;
using UnityEngine;

namespace Hovercast.Demo {

	/*================================================================================================*/
	public class DemoNavItems {

		private const float MinFontSize = 20;
		private const float MaxFontSize = 40;

		public NavItemParent Color { get; private set; }
		public NavItemRadio ColorWhite { get; private set; }
		public NavItemRadio ColorRandom { get; private set; }
		public NavItemRadio ColorCustom { get; private set; }
		public NavItemSlider ColorHue { get; private set; }

		public NavItemParent Motion { get; private set; }
		public NavItemCheckbox MotionOrbit { get; private set; }
		public NavItemCheckbox MotionSpin { get; private set; }
		public NavItemCheckbox MotionBob { get; private set; }
		public NavItemCheckbox MotionGrow { get; private set; }
		public NavItemSlider MotionSpeed { get; private set; }

		public NavItemParent Light { get; private set; }
		public NavItemSlider LightPos { get; private set; }
		public NavItemSlider LightInten { get; private set; }
		public NavItemSticky LightSpot { get; private set; }

		public NavItemParent Camera { get; private set; }
		public NavItemRadio CameraCenter { get; private set; }
		public NavItemRadio CameraBack { get; private set; }
		public NavItemRadio CameraTop { get; private set; }
		public NavItemSelector CameraReorient { get; private set; }

		public NavItemParent Customize { get; private set; }
		public NavItemRadio CustomizeDark { get; private set; }
		public NavItemRadio CustomizeLight { get; private set; }
		public NavItemRadio CustomizeColor { get; private set; }
		public NavItemSlider CustomizeFontsize { get; private set; }
		public NavItemSlider CustomizeOpacity { get; private set; }
		public NavItemSelector CustomizeSwitch { get; private set; }

		public NavItemParent Nested { get; private set; }
		public NavItemParent NestedA { get; private set; }
		public NavItemCheckbox NestedA1 { get; private set; }
		public NavItemCheckbox NestedA2 { get; private set; }
		public NavItemCheckbox NestedA3 { get; private set; }
		public NavItemParent NestedB { get; private set; }
		public NavItemCheckbox NestedB1 { get; private set; }
		public NavItemCheckbox NestedB2 { get; private set; }
		public NavItemCheckbox NestedB3 { get; private set; }
		public NavItemSelector NestedB4 { get; private set; }
		public NavItemParent NestedC { get; private set; }
		public NavItemCheckbox NestedC1 { get; private set; }
		public NavItemCheckbox NestedC2 { get; private set; }
		public NavItemCheckbox NestedC3 { get; private set; }
		public NavItemCheckbox NestedC4 { get; private set; }
		public NavItemCheckbox NestedC5 { get; private set; }

		public NavLevel TopLevel { get; private set; }

		private float vMenuOpacity;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoNavItems() {
			BuildColors();
			BuildMotions();
			BuildLight();
			BuildCamera();
			BuildCustomize();
			BuildNested();

			TopLevel = new NavLevel(Color, Motion, Light, Camera, Customize, Nested);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static bool IsItemWithin(NavItem pItem, NavItemParent pParent, NavItem.ItemType pType) {
			VerifyParent(pParent);
			return (pItem.Type == pType && pParent.ChildLevel.Items.Contains(pItem));
		}

		/*--------------------------------------------------------------------------------------------*/
		public static NavItemRadio GetChosenRadioItem(NavItemParent pParent) {
			VerifyParent(pParent);

			foreach ( NavItem item in pParent.ChildLevel.Items ) {
				NavItemRadio radItem = (item as NavItemRadio);

				if ( radItem != null && radItem.Value ) {
					return radItem;
				}
			}

			return null;
		}

		/*--------------------------------------------------------------------------------------------*/
		private static void VerifyParent(NavItemParent pParent) {
			if ( pParent.ChildLevel != null && pParent.ChildLevel.Items.Length != 0 ) {
				return;
			}

			throw new Exception("Parent item '"+pParent.Label+"' has no children.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildColors() {
			ColorWhite = new NavItemRadio("White");
			ColorRandom = new NavItemRadio("Random");
			ColorCustom = new NavItemRadio("Custom");
			ColorHue = new NavItemSlider("Hue", 3);

			Color = new NavItemParent("Color");
			Color.ChildLevel.Items = new NavItem[] { ColorWhite, ColorRandom, ColorCustom, ColorHue };
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildMotions() {
			MotionOrbit = new NavItemCheckbox("Orbit");
			MotionSpin = new NavItemCheckbox("Spin");
			MotionBob = new NavItemCheckbox("Bob");
			MotionGrow = new NavItemCheckbox("Grow");
			MotionSpeed = new NavItemSlider("Speed", 4);

			Motion = new NavItemParent("Motion");
			Motion.ChildLevel.Items = new NavItem[] { MotionOrbit, MotionSpin, MotionBob, MotionGrow, 
				MotionSpeed };
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildLight() {
			LightPos = new NavItemSlider("Position", 2);
			LightInten = new NavItemSlider("Power", 2);
			LightSpot = new NavItemSticky("Spotlight");

			Light = new NavItemParent("Lighting");
			Light.ChildLevel.Items = new NavItem[] { LightPos, LightInten, LightSpot };
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildCamera() {
			CameraCenter = new NavItemRadio("Center");
			CameraBack = new NavItemRadio("Back");
			CameraTop = new NavItemRadio("Top");
			CameraReorient = new NavItemSelector("Re-orient");

			Camera = new NavItemParent("Camera");
			Camera.ChildLevel.Items = new NavItem[] { CameraCenter, CameraBack, CameraTop, 
				CameraReorient };
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildCustomize() {
			vMenuOpacity = 0.5f;

			CustomizeDark = new NavItemRadio("Dark Theme");
			CustomizeDark.OnSelected += HandleDarkThemeSelected;
			CustomizeDark.Value = true;
			CustomizeLight = new NavItemRadio("Light Theme");
			CustomizeLight.OnSelected += HandleLightThemeSelected;
			CustomizeColor = new NavItemRadio("Color Theme");
			CustomizeColor.OnSelected += HandleColorThemeSelected;

			CustomizeFontsize = new NavItemSlider("Size", 3);
			CustomizeFontsize.ValueToLabel = ((v, sv) =>
				"Size: "+Math.Round(Mathf.Lerp(MinFontSize, MaxFontSize, sv)));
			CustomizeFontsize.Value = Mathf.InverseLerp(MinFontSize, MaxFontSize, 30);
			CustomizeFontsize.OnValueChanged += HandleFontSizeChanged;

			CustomizeOpacity = new NavItemSlider("Bg Alpha", 3);
			CustomizeOpacity.ValueToLabel = ((v, sv) => "Bg Alpha: "+Math.Round(v*100)+"%");
			CustomizeOpacity.Value = vMenuOpacity;
			CustomizeOpacity.OnValueChanged += HandleOpacityChanged;

			CustomizeSwitch = new NavItemSelector("Switch Hands!");
			CustomizeSwitch.OnSelected += HandleSwitchHands;

			Customize = new NavItemParent("Customize");
			Customize.ChildLevel.Items = new NavItem[] { CustomizeDark, CustomizeLight, CustomizeColor,
			CustomizeFontsize, CustomizeOpacity, CustomizeSwitch };
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildNested() {
			NestedA1 = new NavItemCheckbox("Checkbox A1");
			NestedA2 = new NavItemCheckbox("Checkbox A2");
			NestedA3 = new NavItemCheckbox("Checkbox A3");

			NestedA = new NavItemParent("Menu A");
			NestedA.ChildLevel.Items = new NavItem[] { NestedA1, NestedA2, NestedA3 };

			NestedB1 = new NavItemCheckbox("Checkbox B1");
			NestedB2 = new NavItemCheckbox("Checkbox B2");
			NestedB3 = new NavItemCheckbox("Hide Menu C");
			NestedB3.OnValueChanged += HandleHideMenuCValueChanged;
			NestedB4 = new NavItemSelector("Go Back");
			NestedB4.NavigateBackUponSelect = true;

			NestedB = new NavItemParent("Menu B");
			NestedB.ChildLevel.Items = new NavItem[] { NestedB1, NestedB2, NestedB3, NestedB4 };

			NestedC1 = new NavItemCheckbox("Checkbox C1");
			NestedC2 = new NavItemCheckbox("Checkbox C2");
			NestedC3 = new NavItemCheckbox("Checkbox C3");
			NestedC4 = new NavItemCheckbox("Checkbox C4");
			NestedC5 = new NavItemCheckbox("Checkbox C5");

			NestedC = new NavItemParent("Menu C");
			NestedC.ChildLevel.Items = new NavItem[] { NestedC1, NestedC2, NestedC3, NestedC4,
				NestedC5 };

			Nested = new NavItemParent("Nested Menu");
			Nested.ChildLevel.Items = new NavItem[] { NestedA, NestedB, NestedC };
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleDarkThemeSelected(NavItem pItem) {
			ArcSegmentSettings sett = DemoSettingsComponent.ArcSegmentSettings;
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

			UpdateMenuColorOpacity();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleLightThemeSelected(NavItem pItem) {
			ArcSegmentSettings sett = DemoSettingsComponent.ArcSegmentSettings;
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

			UpdateMenuColorOpacity();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleColorThemeSelected(NavItem pItem) {
			ArcSegmentSettings sett = DemoSettingsComponent.ArcSegmentSettings;
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

			UpdateMenuColorOpacity();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void HandleFontSizeChanged(NavItem<float> pItem) {
			DemoSettingsComponent.ArcSegmentSettings.TextSize = 
				(int)Mathf.Lerp(MinFontSize, MaxFontSize, ((NavItemSlider)pItem).SnappedValue);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void HandleOpacityChanged(NavItem<float> pItem) {
			vMenuOpacity = pItem.Value;
			UpdateMenuColorOpacity();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleSwitchHands(NavItem pNavItem) {
			DemoSettingsComponent.InteractionSettings.IsMenuOnLeftSide = 
				!DemoSettingsComponent.InteractionSettings.IsMenuOnLeftSide;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMenuColorOpacity() {
			ArcSegmentSettings sett = DemoSettingsComponent.ArcSegmentSettings;

			Color c = sett.BackgroundColor;
			c.a = vMenuOpacity;
			sett.BackgroundColor = c;

			c = sett.SliderFillColor;
			c.a = 0.5f*vMenuOpacity;
			sett.SliderFillColor = c;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleHideMenuCValueChanged(NavItem<bool> pItem) {
			if ( pItem.Value ) {
				Nested.ChildLevel.Items = new NavItem[] { NestedA, NestedB };
			}
			else {
				Nested.ChildLevel.Items = new NavItem[] { NestedA, NestedB, NestedC };
			}
		}

	}

}
