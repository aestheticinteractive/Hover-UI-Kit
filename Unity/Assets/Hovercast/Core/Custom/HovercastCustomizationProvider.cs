using System;
using System.Collections.Generic;
using Hovercast.Core.Navigation;
using UnityEngine;

namespace Hovercast.Core.Custom {

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
		public Type GetSegmentRenderer(NavItem pNavItem) {
			if ( pNavItem == null ) {
				throw new ArgumentException("Hovercast | NavItem cannot be null.", "NavItem");
			}

			InitOnce();

			HovercastCustomSegment seg = FindCustom(vMainSeg, pNavItem, (c => c.Seg));
			return seg.GetRendererForNavItemType(pNavItem.Type);
		}

		/*--------------------------------------------------------------------------------------------*/
		public SegmentSettings GetSegmentSettings(NavItem pNavItem) {
			InitOnce();

			HovercastCustomSegment seg = FindCustom(vMainSeg, pNavItem, (c => c.Seg));
			return seg.GetSettings();
		}

		/*--------------------------------------------------------------------------------------------*/
		public Type GetPalmRenderer(NavItem pNavItem) {
			InitOnce();

			HovercastCustomPalm palm = FindCustom(vMainPalm, pNavItem, (c => c.Palm));
			return palm.GetRenderer();
		}

		/*--------------------------------------------------------------------------------------------*/
		public SegmentSettings GetPalmSettings(NavItem pNavItem) {
			InitOnce();

			HovercastCustomPalm palm = FindCustom(vMainPalm, pNavItem, (c => c.Palm));
			return (palm.GetSettings() ?? GetSegmentSettings(pNavItem));
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
				Debug.LogWarning("Hovercast | No '"+typeof(HovercastDefaultPalm).Name+
					"' provided; using default.");
				vMainPalm = gameObject.AddComponent<HovercastDefaultPalm>();
			}

			if ( vCursor == null ) {
				Debug.LogWarning("Hovercast | No '"+typeof(HovercastDefaultCursor).Name+
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
				HovercastNavItem hni = comp.gameObject.GetComponent<HovercastNavItem>();

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
		private T FindCustom<T>(T pMain, NavItem pNavItem, Func<CustomItem, T> pGetPropFunc)
																				where T : Component {
			T comp = pMain;

			if ( pNavItem == null ) {
				return comp;
			}

			CustomItem cust;
			vCustomMap.TryGetValue(pNavItem.AutoId, out cust);

			if ( cust != null && pGetPropFunc(cust) != null ) {
				comp = pGetPropFunc(cust);
			}

			return comp;
		}

	}

}
