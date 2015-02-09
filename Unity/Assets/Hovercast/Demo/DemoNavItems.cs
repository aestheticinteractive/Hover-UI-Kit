using System;
using System.Linq;
using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;
using UnityEngine;

namespace Hovercast.Demo {

	/*================================================================================================*/
	public class DemoNavItems {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
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

			darkObj.GetComponent<HovercastNavItem>().RadioValue = CustomizeDark.Value = true;
			CustomizeDark.OnSelected += HandleDarkThemeSelected;
			CustomizeLight.OnSelected += HandleLightThemeSelected;
			CustomizeColor.OnSelected += HandleColorThemeSelected;

			CustomizeFontsize.ValueToLabel = (s =>
				"Size: "+Math.Round(Mathf.Lerp(MinFontSize, MaxFontSize, s.SnappedValue)));
			sizeObj.GetComponent<HovercastNavItem>().SliderValue = CustomizeFontsize.Value = 
				Mathf.InverseLerp(MinFontSize, MaxFontSize, 30);
			CustomizeFontsize.OnValueChanged += HandleFontSizeChanged;

			vMenuOpacity = 0.5f;
			CustomizeOpacity.ValueToLabel = (s => "Bg Alpha: "+Math.Round(s.Value*100)+"%");
			opacObj.GetComponent<HovercastNavItem>().SliderValue = 
				CustomizeOpacity.Value = vMenuOpacity;
			CustomizeOpacity.OnValueChanged += HandleOpacityChanged;

			CustomizeSwitch.OnSelected += HandleSwitchHands;
		}

		/*--------------------------------------------------------------------------------------------* /
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
		/*--------------------------------------------------------------------------------------------* /
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

		/*--------------------------------------------------------------------------------------------* /
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

		/*--------------------------------------------------------------------------------------------* /
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
		
		/*--------------------------------------------------------------------------------------------* /
		private void HandleFontSizeChanged(NavItem<float> pItem) {
			DemoSettingsComponent.ArcSegmentSettings.TextSize = 
				(int)Mathf.Lerp(MinFontSize, MaxFontSize, ((NavItemSlider)pItem).SnappedValue);
		}
		
		/*--------------------------------------------------------------------------------------------* /
		private void HandleOpacityChanged(NavItem<float> pItem) {
			Debug.Log("SIZE: "+pItem.Value);
			vMenuOpacity = pItem.Value;
			UpdateMenuColorOpacity();
		}

		/*--------------------------------------------------------------------------------------------* /
		private void HandleSwitchHands(NavItem pNavItem) {
			DemoSettingsComponent.InteractionSettings.IsMenuOnLeftSide = 
				!DemoSettingsComponent.InteractionSettings.IsMenuOnLeftSide;
		}

		/*--------------------------------------------------------------------------------------------* /
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
		/*--------------------------------------------------------------------------------------------* /
		private void HandleHideMenuCValueChanged(NavItem<bool> pItem) {
			if ( pItem.Value ) {
				//Nested.ChildLevel.Items = new NavItem[] { NestedA, NestedB };
			}
			else {
				//Nested.ChildLevel.Items = new NavItem[] { NestedA, NestedB, NestedC };
			}
		}*/

	}

}
