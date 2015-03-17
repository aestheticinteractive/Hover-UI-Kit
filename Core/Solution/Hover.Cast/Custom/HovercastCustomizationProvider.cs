using System;
using System.Collections.Generic;
using Hover.Cast.Items;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Cast.Custom {

	/*================================================================================================*/
	public class HovercastCustomizationProvider : MonoBehaviour, ICustom {

		private class CustomItem {
			public HovercastCustomSegment Seg;
			public HovercastCustomPalm Palm;
		}

		private HovercastCustomSegment vMainSeg;
		private HovercastCustomPalm vMainPalm;
		private HovercastCustomCursor vCursor;
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
				throw new ArgumentException("Hovercast | NavItem cannot be null.", "NavItem");
			}

			InitOnce();

			HovercastCustomSegment seg = FindCustom(vMainSeg, pItem, (c => c.Seg));
			return seg.GetRendererForItem(pItem);
		}

		/*--------------------------------------------------------------------------------------------*/
		public SegmentSettings GetSegmentSettings(IBaseItem pItem) {
			InitOnce();

			HovercastCustomSegment seg = FindCustom(vMainSeg, pItem, (c => c.Seg));
			return seg.GetSettings();
		}

		/*--------------------------------------------------------------------------------------------*/
		public Type GetPalmRenderer(IBaseItem pItem) {
			InitOnce();

			HovercastCustomPalm palm = FindCustom(vMainPalm, pItem, (c => c.Palm));
			return palm.GetRenderer();
		}

		/*--------------------------------------------------------------------------------------------*/
		public SegmentSettings GetPalmSettings(IBaseItem pItem) {
			InitOnce();

			HovercastCustomPalm palm = FindCustom(vMainPalm, pItem, (c => c.Palm));
			return (palm.GetSettings() ?? GetSegmentSettings(pItem));
		}

		/*--------------------------------------------------------------------------------------------*/
		public Type GetCursorRenderer() {
			InitOnce();
			return vCursor.GetRenderer();
		}

		/*--------------------------------------------------------------------------------------------*/
		public CursorSettings GetCursorSettings() {
			InitOnce();
			return vCursor.GetSettings();
		}

		/*--------------------------------------------------------------------------------------------*/
		public InteractionSettings GetInteractionSettings() {
			InitOnce();
			return vInteract.GetSettings();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void InitOnce() {
			if ( vMainSeg != null ) {
				return;
			}

			vMainSeg = gameObject.GetComponent<HovercastCustomSegment>();
			vMainPalm = gameObject.GetComponent<HovercastCustomPalm>();
			vCursor = gameObject.GetComponent<HovercastCustomCursor>();
			vInteract = gameObject.GetComponent<HovercastCustomInteraction>();

			if ( vMainSeg == null ) {
				Debug.LogWarning("Hovercast | No '"+typeof(HovercastCustomSegment).Name+
					"' provided; using default.");
				vMainSeg = gameObject.AddComponent<HovercastDefaultSegment>();
			}

			if ( vMainPalm == null ) {
				Debug.LogWarning("Hovercast | No '"+typeof(HovercastCustomPalm).Name+
					"' provided; using default.");
				vMainPalm = gameObject.AddComponent<HovercastDefaultPalm>();
			}

			if ( vCursor == null ) {
				Debug.LogWarning("Hovercast | No '"+typeof(HovercastCustomCursor).Name+
					"' provided; using default.");
				vCursor = gameObject.AddComponent<HovercastDefaultCursor>();
			}

			if ( vInteract == null ) {
				Debug.LogWarning("Hovercast | No '"+typeof(HovercastCustomInteraction).Name+
					"' provided; using default.");
				vInteract = gameObject.AddComponent<HovercastCustomInteraction>();
			}

			////

			vCustomMap = new Dictionary<int, CustomItem>();

			HovercastCustomSegment[] segList = 
				gameObject.GetComponentsInChildren<HovercastCustomSegment>();
			HovercastCustomPalm[] palmList = 
				gameObject.GetComponentsInChildren<HovercastCustomPalm>();

			FillCustomItems(segList, ((c, s) => { c.Seg = s; }));
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
