using System;
using System.Collections.Generic;
using Hover.Common.Input;
using Hover.Common.State;
using Hover.Cursor;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Demo.CursorTest {

	/*================================================================================================*/
	public class DemoCursorToggle : MonoBehaviour, IHovercursorDelegate {

		private struct Bundle {
			public ICursorState State;
			public Func<bool> ShowFunc; 
		}

		public bool IsInteractionEnabled = true;

		public bool ShowLeftPalm = true;
		public bool ShowLeftThumb = true;
		public bool ShowLeftIndex = true;
		public bool ShowLeftMiddle = true;
		public bool ShowLeftRing = true;
		public bool ShowLeftPinky = true;
		public bool ShowRightPalm = true;
		public bool ShowRightThumb = true;
		public bool ShowRightIndex = true;
		public bool ShowRightMiddle = true;
		public bool ShowRightRing = true;
		public bool ShowRightPinky = true;

		public float DisplayStrength = 1;
		public float HighlightProgress = 0;

		private HovercursorSetup vSetup;
		private Bundle[] vBundles;
		private FakeItemState vFakeItem;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vSetup = gameObject.GetComponent<HovercursorSetup>();
			vFakeItem = new FakeItemState();
			vFakeItem.ItemAutoId = 123;

			ActiveCursorTypes = new CursorType[0];
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

			vSetup.State.AddDelegate(this);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			var cursorTypes = new List<CursorType>();

			foreach ( Bundle bundle in vBundles ) {
				if ( bundle.ShowFunc() ) {
					cursorTypes.Add(bundle.State.Type);
				}

				vFakeItem.MaxHighlightProgress = HighlightProgress;
			}

			ActiveCursorTypes = cursorTypes.ToArray();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Bundle GetBundle(CursorType pType, Func<bool> pShowFunc) {
			var bundle = new Bundle();
			bundle.State = vSetup.State.GetCursorState(pType);
			bundle.ShowFunc = pShowFunc;
			return bundle;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		// IHovercursorDelegate
		/*--------------------------------------------------------------------------------------------*/
		public CursorDomain Domain {
			get {
				return CursorDomain.Hovercursor;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsCursorInteractionEnabled {
			get {
				return IsInteractionEnabled;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public CursorType[] ActiveCursorTypes { get; private set; }

		/*--------------------------------------------------------------------------------------------*/
		public float CursorDisplayStrength {
			get {
				return DisplayStrength;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IBaseItemInteractionState[] GetActiveCursorInteractions(CursorType pCursorType) {
			return new [] { vFakeItem };
		}

		/*--------------------------------------------------------------------------------------------*/
		public PlaneData[] GetActiveCursorPlanes(CursorType pCursorType) {
			return new PlaneData[0];
		}

	}


	/*================================================================================================*/
	public class FakeItemState : IBaseItemInteractionState {

		public int ItemAutoId { get; set; }
		public bool IsSelectionPrevented { get; set; }
		public float MaxHighlightProgress { get; set; }
		public float SelectionProgress { get; set; }

	}

}

