using System;
using System.Collections.Generic;
using Hover.Cast.Items;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public class HovercastCustomizationProvider : MonoBehaviour, ICustom {

		private class CustomItem {
			public HovercastCustomItem Item;
			public HovercastCustomPalm Palm;
		}

		private HovercastCustomItem vMainItem;
		private HovercastCustomPalm vMainPalm;
		private HovercastCustomInteraction vInteract;
		private IDictionary<int, CustomItem> vCustomMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			InitOnce();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Type GetSegmentRenderer(IBaseItem pItem) {
			if ( pItem == null ) {
				throw new ArgumentException("Hovercast | Item cannot be null.", "Item");
			}

			InitOnce();

			HovercastCustomItem seg = FindCustom(vMainItem, pItem, (c => c.Item));
			return seg.GetRendererForItem(pItem);
		}

		/*--------------------------------------------------------------------------------------------*/
		public ItemVisualSettings GetSegmentSettings(IBaseItem pItem) {
			InitOnce();

			HovercastCustomItem seg = FindCustom(vMainItem, pItem, (c => c.Item));
			return seg.GetSettings();
		}

		/*--------------------------------------------------------------------------------------------*/
		public Type GetPalmRenderer(IBaseItem pItem) {
			InitOnce();

			HovercastCustomPalm palm = FindCustom(vMainPalm, pItem, (c => c.Palm));
			return palm.GetRenderer();
		}

		/*--------------------------------------------------------------------------------------------*/
		public ItemVisualSettings GetPalmSettings(IBaseItem pItem) {
			InitOnce();

			HovercastCustomPalm palm = FindCustom(vMainPalm, pItem, (c => c.Palm));
			return (palm.GetSettings() ?? GetSegmentSettings(pItem));
		}

		/*--------------------------------------------------------------------------------------------*/
		public InteractionSettings GetInteractionSettings() {
			InitOnce();
			return vInteract.GetSettings();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void InitOnce() {
			if ( vMainItem != null ) {
				return;
			}

			vMainItem = gameObject.GetComponent<HovercastCustomItem>();
			vMainPalm = gameObject.GetComponent<HovercastCustomPalm>();
			vInteract = gameObject.GetComponent<HovercastCustomInteraction>();

			if ( vMainItem == null ) {
				Debug.LogWarning("Hovercast | No '"+typeof(HovercastCustomItem).Name+
					"' provided; using default.");
				vMainItem = gameObject.AddComponent<HovercastDefaultItem>();
			}

			if ( vMainPalm == null ) {
				Debug.LogWarning("Hovercast | No '"+typeof(HovercastCustomPalm).Name+
					"' provided; using default.");
				vMainPalm = gameObject.AddComponent<HovercastDefaultPalm>();
			}

			if ( vInteract == null ) {
				Debug.LogWarning("Hovercast | No '"+typeof(HovercastCustomInteraction).Name+
					"' provided; using default.");
				vInteract = gameObject.AddComponent<HovercastCustomInteraction>();
			}

			////

			vCustomMap = new Dictionary<int, CustomItem>();

			HovercastCustomItem[] segList = 
				gameObject.GetComponentsInChildren<HovercastCustomItem>();
			HovercastCustomPalm[] palmList = 
				gameObject.GetComponentsInChildren<HovercastCustomPalm>();

			FillCustomItems(segList, ((c, s) => { c.Item = s; }));
			FillCustomItems(palmList, ((c, p) => { c.Palm = p; }));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void FillCustomItems<T>(T[] pComponentList, Action<CustomItem, T> pFillAction)
																				where T : Component {
			foreach ( T comp in pComponentList ) {
				HovercastItem hni = comp.gameObject.GetComponent<HovercastItem>();

				if ( hni == null ) {
					continue;
				}

				int key = hni.GetItem().AutoId;
				CustomItem cust;

				if ( vCustomMap.ContainsKey(key) ) {
					cust = vCustomMap[key];
				}
				else {
					cust = new CustomItem();
					vCustomMap.Add(key, cust);
				}

				pFillAction(cust, comp);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private T FindCustom<T>(T pMain, IBaseItem pItem, Func<CustomItem, T> pGetPropFunc)
																				where T : Component {
			T comp = pMain;

			if ( pItem == null ) {
				return comp;
			}

			CustomItem cust;
			vCustomMap.TryGetValue(pItem.AutoId, out cust);

			if ( cust != null && pGetPropFunc(cust) != null ) {
				comp = pGetPropFunc(cust);
			}

			return comp;
		}

	}

}
