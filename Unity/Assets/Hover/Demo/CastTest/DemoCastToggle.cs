using System;
using System.Collections.Generic;
using Hover.Cast;
using Hover.Cast.Custom;
using UnityEngine;

namespace Hover.Demo.CastTest {

	/*================================================================================================*/
	public class DemoCastToggle : MonoBehaviour {

		private struct Bundle {
			public HovercastCursorType CursorType;
			public Func<bool> ShowFunc; 
		}

		public bool ShowPalm = false;
		public bool ShowThumb = true;
		public bool ShowIndex = true;
		public bool ShowMiddle = false;
		public bool ShowRing = false;
		public bool ShowPinky = true;

		private HovercastSetup vSetup;
		private Bundle[] vBundles;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vSetup = gameObject.GetComponent<HovercastSetup>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vBundles = new[] {
				GetBundle(HovercastCursorType.Palm, () => ShowPalm),
				GetBundle(HovercastCursorType.Thumb, () => ShowThumb),
				GetBundle(HovercastCursorType.Index, () => ShowIndex),
				GetBundle(HovercastCursorType.Middle, () => ShowMiddle),
				GetBundle(HovercastCursorType.Ring, () => ShowRing),
				GetBundle(HovercastCursorType.Pinky, () => ShowPinky),
			};
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			var cursorTypes = new List<HovercastCursorType>();

			foreach ( Bundle bundle in vBundles ) {
				if ( !bundle.ShowFunc() ) {
					continue;
				}

				cursorTypes.Add(bundle.CursorType);
			}

			vSetup.InteractionSettings.GetSettings().Cursors = cursorTypes.ToArray();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Bundle GetBundle(HovercastCursorType pType, Func<bool> pShowFunc) {
			var bundle = new Bundle();
			bundle.CursorType = pType;
			bundle.ShowFunc = pShowFunc;
			return bundle;
		}

	}

}
