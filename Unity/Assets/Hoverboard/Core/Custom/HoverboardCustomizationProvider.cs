using System;
using System.Collections.Generic;
using Hoverboard.Core.Navigation;
using UnityEngine;

namespace Hoverboard.Core.Custom {

	/*================================================================================================*/
	public class HoverboardCustomizationProvider : MonoBehaviour, ICustom {

		private class CustomItem {
			public HovercastCustomButton Seg;
		}

		private HovercastCustomButton vMainSeg;
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

			HovercastCustomButton seg = FindCustom(vMainSeg, pNavItem, (c => c.Seg));
			return seg.GetRendererForNavItemType(pNavItem.Type);
		}

		/*--------------------------------------------------------------------------------------------*/
		public ButtonSettings GetSegmentSettings(NavItem pNavItem) {
			InitOnce();

			HovercastCustomButton seg = FindCustom(vMainSeg, pNavItem, (c => c.Seg));
			return seg.GetSettings();
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

			vMainSeg = gameObject.GetComponent<HovercastCustomButton>();
			vCursor = gameObject.GetComponent<HovercastCustomCursor>();
			vInteract = gameObject.GetComponent<HovercastCustomInteraction>();

			if ( vMainSeg == null ) {
				Debug.LogWarning("Hovercast | No '"+typeof(HovercastCustomButton).Name+
					"' provided; using default.");
				vMainSeg = gameObject.AddComponent<HovercastDefaultButton>();
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

			HovercastCustomButton[] segList = 
				gameObject.GetComponentsInChildren<HovercastCustomButton>();

			FillCustomItems(segList, ((c, s) => { c.Seg = s; }));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void FillCustomItems<T>(T[] pComponentList, Action<CustomItem, T> pFillAction)
																				where T : Component {
			foreach ( T comp in pComponentList ) {
				HoverboardNavItem hni = comp.gameObject.GetComponent<HoverboardNavItem>();

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
