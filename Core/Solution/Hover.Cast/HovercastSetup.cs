using Hover.Cast.Custom;
using Hover.Cast.Display;
using Hover.Cast.Input;
using Hover.Cast.Items;
using Hover.Cast.State;
using Hover.Common.Items.Types;
using Hover.Common.Util;
using Hover.Cursor;
using UnityEngine;

namespace Hover.Cast {
	
	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HovercastSetup : MonoBehaviour {
		
		private const string Domain = "Hovercast";
		
		public HovercastInteractionSettings InteractionSettings;
		public HovercastItemHierarchy ItemHierarchy;
		public HovercursorSetup Hovercursor;
		public HovercastInput Input;
		public HovercastMenuDisplay MenuDisplay;
		
		//private HovercastState vState;
		private bool vInputErrorReported;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		public void Awake() {
			//vState = new HovercastState(ItemHierarchy.Root, Hovercursor, 
			//	InteractionSettings.GetSettings(), Input, gameObject.transform);
			
			//MenuDisplay.Build(vState, null);
			//vState.SetReferences(MenuDisplay.gameObject.transform);
			//Hovercursor.State.AddDelegate(vState);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			TryFindInteractionSettings();
			TryBuildHovercursor();
			TryBuildItemHierarchy();
			TryBuildMenuDisplay();
			TryFindInput();
			
			if ( Input != null ) {
				Input.UpdateInput();
				//vState.UpdateAfterInput();
			}
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void TryFindInteractionSettings() {
			if ( InteractionSettings != null ) {
				return;
			}
			
			InteractionSettings = gameObject.GetComponent<HovercastInteractionSettings>();
			
			if ( InteractionSettings == null ) {
				InteractionSettings = gameObject.AddComponent<HovercastInteractionSettings>();
				Debug.Log(UnityUtil.GetComponentCreatedText(Domain, InteractionSettings));
			}
			else {
				Debug.Log(UnityUtil.GetComponentFoundText(Domain, InteractionSettings));
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void TryBuildHovercursor() {
			if ( Hovercursor != null ) {
				return;
			}
			
			Hovercursor = FindObjectOfType<HovercursorSetup>();
			
			if ( Hovercursor == null ) {
				var cursorSetupGo = new GameObject("Hovercursor");
				cursorSetupGo.transform.SetParent(gameObject.transform.parent, false);
				Hovercursor = cursorSetupGo.AddComponent<HovercursorSetup>();
				Debug.Log(UnityUtil.GetComponentCreatedText(Domain, Hovercursor));
			}
			else {
				Debug.Log(UnityUtil.GetComponentFoundText(Domain, Hovercursor));
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void TryBuildItemHierarchy() {
			if ( ItemHierarchy != null ) {
				return;
			}
			
			ItemHierarchy = gameObject.GetComponentInChildren<HovercastItemHierarchy>();
			
			if ( ItemHierarchy == null ) {
				var itemsGo = new GameObject("MenuData");
				itemsGo.transform.SetParent(gameObject.transform, false);
				ItemHierarchy = itemsGo.AddComponent<HovercastItemHierarchy>();
				Debug.Log(UnityUtil.GetComponentCreatedText(Domain, ItemHierarchy));

				var itemGo = new GameObject("ItemA");
				itemGo.transform.SetParent(itemsGo.transform, false);
				HoverCheckboxItem item = itemGo.AddComponent<HoverCheckboxItem>();
				item.Label = "Item A";
				item.Value = true;
				Debug.Log(UnityUtil.GetComponentCreatedText(Domain, item));

				itemGo = new GameObject("ItemB");
				itemGo.transform.SetParent(itemsGo.transform, false);
				item = itemGo.AddComponent<HoverCheckboxItem>();
				item.Label = "Item B";
				Debug.Log(UnityUtil.GetComponentCreatedText(Domain, item));

				itemGo = new GameObject("ItemC");
				itemGo.transform.SetParent(itemsGo.transform, false);
				item = itemGo.AddComponent<HoverCheckboxItem>();
				item.Label = "Item C";
				Debug.Log(UnityUtil.GetComponentCreatedText(Domain, item));
			}
			else {
				Debug.Log(UnityUtil.GetComponentFoundText(Domain, ItemHierarchy));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void TryBuildMenuDisplay() {
			if ( MenuDisplay != null ) {
				return;
			}

			Transform menuDispTx = gameObject.transform.FindChild("MenuDisplay");
			GameObject menuDispGo;

			if ( menuDispTx == null ) {
				menuDispGo = new GameObject("MenuDisplay");
				menuDispGo.transform.SetParent(gameObject.transform, false);
			}
			else {
				menuDispGo = menuDispTx.gameObject;
			}

			MenuDisplay = menuDispGo.GetComponent<HovercastMenuDisplay>();
			
			if ( MenuDisplay == null ) {
				MenuDisplay = menuDispGo.gameObject.AddComponent<HovercastMenuDisplay>();
				Debug.Log(UnityUtil.GetComponentCreatedText(Domain, MenuDisplay));
			}

			//MenuDisplay.Build(vState, null);
			//vState.SetReferences(MenuDisplay.gameObject.transform);
			//Hovercursor.State.AddDelegate(vState);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void TryFindInput() {
			if ( Input != null ) {
				return;
			}
			
			Input = FindObjectOfType<HovercastInput>();
			
			if ( Input == null && !vInputErrorReported ) {
				Debug.LogError(UnityUtil.GetComponentMissingText<HovercastInput>(Domain));
				vInputErrorReported = true;
			}
			else if ( Input != null ) {
				Debug.Log(UnityUtil.GetComponentFoundText(Domain, Input));
				vInputErrorReported = false;
			}
		}
		
	}
	
}
