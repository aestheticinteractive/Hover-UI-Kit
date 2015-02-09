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

		private GameObject vRootObj;
		private float vMenuOpacity;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(GameObject pRootObj) {
			vRootObj = pRootObj;

			while ( vRootObj.transform.childCount > 0 ) {
				UnityEngine.Object.DestroyImmediate(vRootObj.transform.GetChild(0).gameObject);
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
		public static T AddItem<T>(GameObject pParentObj, out GameObject pObj, string pLabel,
																float pRelSize=1) where T : NavItem {
			pObj = new GameObject(pLabel);
			pObj.transform.SetParent(pParentObj.transform, false);

			HovercastNavItem item = pObj.AddComponent<HovercastNavItem>();

			if ( typeof(T) == typeof(NavItemCheckbox) ) {
				item.Type = NavItem.ItemType.Checkbox;
			}
			else if ( typeof(T) == typeof(NavItemParent) ) {
				item.Type = NavItem.ItemType.Parent;
			}
			else if ( typeof(T) == typeof(NavItemRadio) ) {
				item.Type = NavItem.ItemType.Radio;
			}
			else if ( typeof(T) == typeof(NavItemSelector) ) {
				item.Type = NavItem.ItemType.Selector;
			}
			else if ( typeof(T) == typeof(NavItemSlider) ) {
				item.Type = NavItem.ItemType.Slider;
			}
			else if ( typeof(T) == typeof(NavItemSticky) ) {
				item.Type = NavItem.ItemType.Sticky;
			}

			item.Label = pLabel;
			item.RelativeSize = pRelSize;
			return (T)item.GetItem();
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T AddItem<T>(GameObject pParentObj, string pLabel, float pRelSize=1) 
																					where T : NavItem {
			GameObject obj;
			return AddItem<T>(pParentObj, out obj, pLabel, pRelSize);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static NavItemParent AddParent(GameObject pParentObj, out GameObject pObj, 
		                                      						string pLabel, float pRelSize=1) {
			return AddItem<NavItemParent>(pParentObj, out pObj, pLabel, pRelSize);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static bool IsItemWithin(NavItem pItem, NavItemParent pParent, NavItem.ItemType pType) {
			VerifyParent(pParent);
			return (pItem.Type == pType && pParent.ChildLevel.Items.Contains(pItem));
		}

		/*--------------------------------------------------------------------------------------------* /
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
			GameObject colorObj;

			Color = AddParent(vRootObj, out colorObj, "Color");
			ColorWhite = AddItem<NavItemRadio>(colorObj, "White");
			ColorRandom = AddItem<NavItemRadio>(colorObj, "Random");
			ColorCustom = AddItem<NavItemRadio>(colorObj, "Custom");
			ColorHue = AddItem<NavItemSlider>(colorObj, "Hue", 3);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildMotions() {
			GameObject motionObj;

			Motion = AddParent(vRootObj, out motionObj, "Motion");
			MotionOrbit = AddItem<NavItemCheckbox>(motionObj, "Orbit");
			MotionSpin = AddItem<NavItemCheckbox>(motionObj, "Spin");
			MotionBob = AddItem<NavItemCheckbox>(motionObj, "Bob");
			MotionGrow = AddItem<NavItemCheckbox>(motionObj, "Grow");
			MotionSpeed = AddItem<NavItemSlider>(motionObj, "Speed", 4);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildLight() {
			GameObject lightObj;

			Light = AddParent(vRootObj, out lightObj, "Lighting");
			LightPos = AddItem<NavItemSlider>(lightObj, "Position", 2);
			LightInten = AddItem<NavItemSlider>(lightObj, "Power", 2);
			LightSpot = AddItem<NavItemSticky>(lightObj, "Spotlight");
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildCamera() {
			GameObject cameraObj;

			Camera = AddParent(vRootObj, out cameraObj, "Camera");
			CameraCenter = AddItem<NavItemRadio>(cameraObj, "Center");
			CameraBack = AddItem<NavItemRadio>(cameraObj, "Back");
			CameraTop = AddItem<NavItemRadio>(cameraObj, "Top");
			CameraReorient = AddItem<NavItemSelector>(cameraObj, "Re-orient");
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildCustomize() {
			GameObject customObj;
			GameObject darkObj;
			GameObject sizeObj;
			GameObject opacObj;

			Customize = AddParent(vRootObj, out customObj, "Customize");
			CustomizeDark = AddItem<NavItemRadio>(customObj, out darkObj, "Dark Theme");
			CustomizeLight = AddItem<NavItemRadio>(customObj, "Light Theme");
			CustomizeColor = AddItem<NavItemRadio>(customObj, "Color Theme");
			CustomizeFontsize = AddItem<NavItemSlider>(customObj, out sizeObj, "Size", 3);
			CustomizeOpacity = AddItem<NavItemSlider>(customObj, out opacObj, "Bg Alpha", 3);
			CustomizeSwitch = AddItem<NavItemSelector>(customObj, "Switch Hands!");

			////

			darkObj.GetComponent<HovercastNavItem>().ValueBool = CustomizeDark.Value = true;
			CustomizeDark.OnSelected += HandleDarkThemeSelected;
			CustomizeLight.OnSelected += HandleLightThemeSelected;
			CustomizeColor.OnSelected += HandleColorThemeSelected;

			CustomizeFontsize.ValueToLabel = ((v, sv) =>
				"Size: "+Math.Round(Mathf.Lerp(MinFontSize, MaxFontSize, sv)));
			sizeObj.GetComponent<HovercastNavItem>().ValueFloat = CustomizeFontsize.Value = 
				Mathf.InverseLerp(MinFontSize, MaxFontSize, 30);
			CustomizeFontsize.OnValueChanged += HandleFontSizeChanged;

			vMenuOpacity = 0.5f;
			CustomizeOpacity.ValueToLabel = ((v, sv) => "Bg Alpha: "+Math.Round(v*100)+"%");
			opacObj.GetComponent<HovercastNavItem>().ValueFloat = 
				CustomizeOpacity.Value = vMenuOpacity;
			CustomizeOpacity.OnValueChanged += HandleOpacityChanged;

			CustomizeSwitch.OnSelected += HandleSwitchHands;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildNested() {
			GameObject nestObj;
			GameObject nestAObj;
			GameObject nestBObj;
			GameObject nestCObj;
			GameObject nestB4Obj;

			Nested = AddParent(vRootObj, out nestObj, "Nested Menu");

			NestedA = AddParent(nestObj, out nestAObj, "Menu A");
			NestedA1 = AddItem<NavItemCheckbox>(nestAObj, "Checkbox A1");
			NestedA2 = AddItem<NavItemCheckbox>(nestAObj, "Checkbox A2");
			NestedA3 = AddItem<NavItemCheckbox>(nestAObj, "Checkbox A3");

			NestedB = AddParent(nestObj, out nestBObj, "Menu B");
			NestedB1 = AddItem<NavItemCheckbox>(nestBObj, "Checkbox B1");
			NestedB2 = AddItem<NavItemCheckbox>(nestBObj, "Checkbox B2");
			NestedB3 = AddItem<NavItemCheckbox>(nestBObj, "Checkbox B3");
			NestedB4 = AddItem<NavItemSelector>(nestBObj, out nestB4Obj, "Go Back");

			NestedC = AddParent(nestObj, out nestCObj, "Menu C");
			NestedC1 = AddItem<NavItemCheckbox>(nestCObj, "Checkbox C1");
			NestedC2 = AddItem<NavItemCheckbox>(nestCObj, "Checkbox C2");
			NestedC3 = AddItem<NavItemCheckbox>(nestCObj, "Checkbox C3");
			NestedC4 = AddItem<NavItemCheckbox>(nestCObj, "Checkbox C4");
			NestedC5 = AddItem<NavItemCheckbox>(nestCObj, "Checkbox C5");

			////

			NestedB3.OnValueChanged += HandleHideMenuCValueChanged;
			nestB4Obj.GetComponent<HovercastNavItem>().NavigateBackUponSelect = 
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
			Debug.Log("SIZE: "+pItem.Value);
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
