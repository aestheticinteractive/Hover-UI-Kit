using System;
using System.Collections.Generic;
using Hover.Board;
using Hover.Common.Input;
using UnityEngine;

namespace Hover.Demo.BoardTest {

	/*================================================================================================*/
	public class DemoBoardToggle : MonoBehaviour {

		private struct Bundle {
			public CursorType CursorType;
			public Func<bool> ShowFunc; 
		}

		public bool ShowLeftPalm = false;
		public bool ShowLeftThumb = true;
		public bool ShowLeftIndex = true;
		public bool ShowLeftMiddle = false;
		public bool ShowLeftRing = false;
		public bool ShowLeftPinky = true;
		public bool ShowRightPalm = false;
		public bool ShowRightThumb = true;
		public bool ShowRightIndex = true;
		public bool ShowRightMiddle = false;
		public bool ShowRightRing = true;
		public bool ShowRightPinky = true;

		private HoverboardSetup vSetup;
		private Bundle[] vBundles;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vSetup = gameObject.GetComponent<HoverboardSetup>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vBundles = new[] {
				GetBundle(CursorType.LeftPalm, () => ShowLeftPalm),
				GetBundle(CursorType.LeftThumb, () => ShowLeftThumb),
				GetBundle(CursorType.LeftIndex, () => ShowLeftIndex),
				GetBundle(CursorType.LeftMiddle, () => ShowLeftMiddle),
				GetBundle(CursorType.LeftRing, () => ShowLeftRing),
				GetBundle(CursorType.LeftPinky, () => ShowLeftPinky),
				GetBundle(CursorType.RightPalm, () => ShowRightPalm),
				GetBundle(CursorType.RightThumb, () => ShowRightThumb),
				GetBundle(CursorType.RightIndex, () => ShowRightIndex),
				GetBundle(CursorType.RightMiddle, () => ShowRightMiddle),
				GetBundle(CursorType.RightRing, () => ShowRightRing),
				GetBundle(CursorType.RightPinky, () => ShowRightPinky)
			};
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			IList<CursorType> cursorTypes = vSetup.InteractionSettings.GetSettings().Cursors;
			cursorTypes.Clear();

			foreach ( Bundle bundle in vBundles ) {
				if ( !bundle.ShowFunc() ) {
					continue;
				}

				cursorTypes.Add(bundle.CursorType);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Bundle GetBundle(CursorType pType, Func<bool> pShowFunc) {
			var bundle = new Bundle();
			bundle.CursorType = pType;
			bundle.ShowFunc = pShowFunc;
			return bundle;
		}

	}

}
