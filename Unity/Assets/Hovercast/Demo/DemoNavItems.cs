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

		private GameObject vRootObj;
		private float vMenuOpacity;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(GameObject pRootObj) {
			vRootObj = pRootObj;

			while ( vRootObj.transform.childCount > 0 ) {
				GameObject.DestroyImmediate(vRootObj.transform.GetChild(0).gameObject);
			}

			////

			BuildColors();
			BuildMotions();
			BuildLight();
			BuildCamera();
			BuildCustomize();
			BuildNested();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T AddItem<T>(GameObject pParentObj, string pLabel, float pRelSize=1) 
																					where T : NavItem {
			var obj = new GameObject(pLabel);
			obj.transform.SetParent(pParentObj.transform, false);

			T item = obj.AddComponent<T>();
			item.BaseLabel = pLabel;
			item.RelativeSize = pRelSize;
			return item;
		}

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
			Color = AddItem<NavItemParent>(vRootObj, "Color");
			ColorWhite = AddItem<NavItemRadio>(Color.gameObject, "White");
			ColorRandom = AddItem<NavItemRadio>(Color.gameObject, "Random");
			ColorCustom = AddItem<NavItemRadio>(Color.gameObject, "Custom");
			ColorHue = AddItem<NavItemSlider>(Color.gameObject, "Hue", 3);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildMotions() {
			Motion = AddItem<NavItemParent>(vRootObj, "Motion");
			MotionOrbit = AddItem<NavItemCheckbox>(Motion.gameObject, "Orbit");
			MotionSpin = AddItem<NavItemCheckbox>(Motion.gameObject, "Spin");
			MotionBob = AddItem<NavItemCheckbox>(Motion.gameObject, "Bob");
			MotionGrow = AddItem<NavItemCheckbox>(Motion.gameObject, "Grow");
			MotionSpeed = AddItem<NavItemSlider>(Motion.gameObject, "Speed", 4);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildLight() {
			Light = AddItem<NavItemParent>(vRootObj, "Lighting");
			LightPos = AddItem<NavItemSlider>(Light.gameObject, "Position", 2);
			LightInten = AddItem<NavItemSlider>(Light.gameObject, "Power", 2);
			LightSpot = AddItem<NavItemSticky>(Light.gameObject, "Spotlight");
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildCamera() {
			Camera = AddItem<NavItemParent>(vRootObj, "Camera");
			CameraCenter = AddItem<NavItemRadio>(Camera.gameObject, "Center");
			CameraBack = AddItem<NavItemRadio>(Camera.gameObject, "Back");
			CameraTop = AddItem<NavItemRadio>(Camera.gameObject, "Top");
			CameraReorient = AddItem<NavItemSelector>(Camera.gameObject, "Re-orient");
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildCustomize() {
			Customize = AddItem<NavItemParent>(vRootObj, "Customize");
			CustomizeDark = AddItem<NavItemRadio>(Customize.gameObject, "Dark Theme");
			CustomizeLight = AddItem<NavItemRadio>(Customize.gameObject, "Light Theme");
			CustomizeColor = AddItem<NavItemRadio>(Customize.gameObject, "Color Theme");
			CustomizeFontsize = AddItem<NavItemSlider>(Customize.gameObject, "Size", 3);
			CustomizeOpacity = AddItem<NavItemSlider>(Customize.gameObject, "Bg Alpha", 3);
			CustomizeSwitch = AddItem<NavItemSelector>(Customize.gameObject, "Switch Hands!");

			////

			CustomizeDark.Value = true;
			CustomizeDark.OnSelected += HandleDarkThemeSelected;
			CustomizeLight.OnSelected += HandleLightThemeSelected;
			CustomizeColor.OnSelected += HandleColorThemeSelected;

			CustomizeFontsize.ValueToLabel = ((v, sv) =>
				"Size: "+Math.Round(Mathf.Lerp(MinFontSize, MaxFontSize, sv)));
			CustomizeFontsize.Value = Mathf.InverseLerp(MinFontSize, MaxFontSize, 30);
			CustomizeFontsize.OnValueChanged += HandleFontSizeChanged;

			vMenuOpacity = 0.5f;
			CustomizeOpacity.ValueToLabel = ((v, sv) => "Bg Alpha: "+Math.Round(v*100)+"%");
			CustomizeOpacity.Value = vMenuOpacity;
			CustomizeOpacity.OnValueChanged += HandleOpacityChanged;

			CustomizeSwitch.OnSelected += HandleSwitchHands;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildNested() {
			Nested = AddItem<NavItemParent>(vRootObj, "Nested Menu");

			NestedA = AddItem<NavItemParent>(Nested.gameObject, "Menu A");
			NestedA1 = AddItem<NavItemCheckbox>(NestedA.gameObject, "Checkbox A1");
			NestedA2 = AddItem<NavItemCheckbox>(NestedA.gameObject, "Checkbox A2");
			NestedA3 = AddItem<NavItemCheckbox>(NestedA.gameObject, "Checkbox A3");

			NestedB = AddItem<NavItemParent>(Nested.gameObject, "Menu B");
			NestedB1 = AddItem<NavItemCheckbox>(NestedB.gameObject, "Checkbox B1");
			NestedB2 = AddItem<NavItemCheckbox>(NestedB.gameObject, "Checkbox B2");
			NestedB3 = AddItem<NavItemCheckbox>(NestedB.gameObject, "Checkbox B3");
			NestedB4 = AddItem<NavItemSelector>(NestedB.gameObject, "Go Back");

			NestedC = AddItem<NavItemParent>(Nested.gameObject, "Menu C");
			NestedC1 = AddItem<NavItemCheckbox>(NestedC.gameObject, "Checkbox C1");
			NestedC2 = AddItem<NavItemCheckbox>(NestedC.gameObject, "Checkbox C2");
			NestedC3 = AddItem<NavItemCheckbox>(NestedC.gameObject, "Checkbox C3");
			NestedC4 = AddItem<NavItemCheckbox>(NestedC.gameObject, "Checkbox C4");
			NestedC5 = AddItem<NavItemCheckbox>(NestedC.gameObject, "Checkbox C5");

			////

			NestedB3.OnValueChanged += HandleHideMenuCValueChanged;
			NestedB4.NavigateBackUponSelect = true;
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
				//Nested.ChildLevel.Items = new NavItem[] { NestedA, NestedB };
			}
			else {
				//Nested.ChildLevel.Items = new NavItem[] { NestedA, NestedB, NestedC };
			}
		}

	}

}
