using System;
using System.Collections.Generic;
using Hover.Board.Navigation;
using UnityEngine;

namespace Hover.Board.Custom {

	/*================================================================================================*/
	public class HoverboardCustomizationProvider : MonoBehaviour, ICustom {

		private class CustomItem {
			public HoverboardCustomButton Seg;
		}

		private HoverboardCustomButton vMainSeg;
		private HoverboardCustomCursor vCursor;
		private HoverboardCustomInteraction vInteract;
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
				throw new ArgumentException("Hoverboard | NavItem cannot be null.", "NavItem");
			}

			InitOnce();

			HoverboardCustomButton seg = FindCustom(vMainSeg, pNavItem, (c => c.Seg));
			return seg.GetRendererForNavItemType(pNavItem.Type);
		}

		/*--------------------------------------------------------------------------------------------*/
		public ButtonSettings GetSegmentSettings(NavItem pNavItem) {
			InitOnce();

			HoverboardCustomButton seg = FindCustom(vMainSeg, pNavItem, (c => c.Seg));
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

			vMainSeg = gameObject.GetComponent<HoverboardCustomButton>();
			vCursor = gameObject.GetComponent<HoverboardCustomCursor>();
			vInteract = gameObject.GetComponent<HoverboardCustomInteraction>();

			if ( vMainSeg == null ) {
				Debug.LogWarning("Hoverboard | No '"+typeof(HoverboardCustomButton).Name+
					"' provided; using default.");
				vMainSeg = gameObject.AddComponent<HoverboardDefaultButton>();
			}

			if ( vCursor == null ) {
				Debug.LogWarning("Hoverboard | No '"+typeof(HoverboardCustomCursor).Name+
					"' provided; using default.");
				vCursor = gameObject.AddComponent<HoverboardDefaultCursor>();
			}

			if ( vInteract == null ) {
				Debug.LogWarning("Hoverboard | No '"+typeof(HoverboardCustomInteraction).Name+
					"' provided; using default.");
				vInteract = gameObject.AddComponent<HoverboardCustomInteraction>();
			}

			////

			vCustomMap = new Dictionary<int, CustomItem>();

			HoverboardCustomButton[] segList = 
				gameObject.GetComponentsInChildren<HoverboardCustomButton>();

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
