#if HOVER_INPUT_VRTK

using Hover.Core.Cursors;
using UnityEngine;
using VRTK;

namespace Hover.InputModules.VRTK {

	/*================================================================================================*/

	[ExecuteInEditMode]
	public class HoverInputVrtk : MonoBehaviour {

		public struct ControlState {
			public VRTK_ControllerEvents Controller;
			public Transform Tx;
			public bool IsValid;
			public Vector2 TouchpadAxis;
			public float TriggerAxis;
			public bool TouchpadTouch;
			public bool TouchpadPress;
			public bool GripPress;
			public bool MenuPress;
		}

		public ControlState StateLeft { get; private set; }
		public ControlState StateRight { get; private set; }

		public HoverCursorDataProvider CursorDataProvider;
		public VRTK_ControllerEvents Left;
		public VRTK_ControllerEvents Right;

		[Space(12)]

		public FollowCursor Look = new FollowCursor(CursorType.Look);

		[Space(12)]

		public VrtkCursor LeftPalm = new VrtkCursor(CursorType.LeftPalm) {
			LocalPosition = new Vector3(0, 0.01f, 0),
			LocalRotation = new Vector3(90, 0, 0),
			CursorSizeInput = VrtkCursor.InputSourceType.TouchpadX,
			MinSize = 0.04f,
			MaxSize = 0.06f
		};

		public VrtkCursor LeftThumb = new VrtkCursor(CursorType.LeftThumb) {
			LocalPosition = new Vector3(0, 0, -0.17f),
			LocalRotation = new Vector3(-90, 0, 0)
		};

		public VrtkCursor LeftIndex = new VrtkCursor(CursorType.LeftIndex) {
			LocalPosition = new Vector3(-0.05f, 0, 0.03f),
			LocalRotation = new Vector3(90, -40, 0)
		};

		public VrtkCursor LeftMiddle = new VrtkCursor(CursorType.LeftMiddle) {
			LocalPosition = new Vector3(0, 0, 0.06f),
			LocalRotation = new Vector3(90, 0, 0)
		};

		public VrtkCursor LeftRing = new VrtkCursor(CursorType.LeftRing) {
			LocalPosition = new Vector3(0.05f, 0, 0.03f),
			LocalRotation = new Vector3(90, 40, 0)
		};

		public VrtkCursor LeftPinky = new VrtkCursor(CursorType.LeftPinky) {
			LocalPosition = new Vector3(0.08f, 0, -0.06f),
			LocalRotation = new Vector3(-90, -180, 80),
			TriggerStrengthInput = VrtkCursor.InputSourceType.TouchpadLeft //for Hovercast
		};

		[Space(12)]

		public VrtkCursor RightPalm = new VrtkCursor(CursorType.RightPalm) {
			LocalPosition = new Vector3(0, 0.01f, 0),
			LocalRotation = new Vector3(90, 0, 0),
			CursorSizeInput = VrtkCursor.InputSourceType.TouchpadX,
			MinSize = 0.04f,
			MaxSize = 0.06f
		};

		public VrtkCursor RightThumb = new VrtkCursor(CursorType.RightThumb) {
			LocalPosition = new Vector3(0, 0, -0.17f),
			LocalRotation = new Vector3(-90, 0, 0)
		};

		public VrtkCursor RightIndex = new VrtkCursor(CursorType.RightIndex) {
			LocalPosition = new Vector3(0.05f, 0, 0.03f),
			LocalRotation = new Vector3(90, 40, 0)
		};

		public VrtkCursor RightMiddle = new VrtkCursor(CursorType.RightMiddle) {
			LocalPosition = new Vector3(0, 0, 0.06f),
			LocalRotation = new Vector3(90, 0, 0)
		};

		public VrtkCursor RightRing = new VrtkCursor(CursorType.RightRing) {
			LocalPosition = new Vector3(-0.05f, 0, 0.03f),
			LocalRotation = new Vector3(90, -40, 0)
		};

		public VrtkCursor RightPinky = new VrtkCursor(CursorType.RightPinky) {
			LocalPosition = new Vector3(-0.08f, 0, -0.06f),
			LocalRotation = new Vector3(-90, 180, -80),
			TriggerStrengthInput = VrtkCursor.InputSourceType.TouchpadRight //for Hovercast
		};


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			CursorUtil.FindCursorReference(this, ref CursorDataProvider, false);

			if ( Look.FollowTransform == null ) {
				Look.FollowTransform = Camera.main.transform;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !CursorUtil.FindCursorReference(this, ref CursorDataProvider, true) ) {
				return;
			}

			if ( !Application.isPlaying ) {
				return;
			}

			CursorDataProvider.MarkAllCursorsUnused();
			UpdateCursorsWithControllers();
			Look.UpdateData(CursorDataProvider);
			CursorDataProvider.ActivateAllCursorsBasedOnUsage();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorsWithControllers() {
			StateLeft = GetControllerState(Left);
			StateRight = GetControllerState(Right);

			LeftPalm.UpdateData(CursorDataProvider, StateLeft);
			LeftThumb.UpdateData(CursorDataProvider, StateLeft);
			LeftIndex.UpdateData(CursorDataProvider, StateLeft);
			LeftMiddle.UpdateData(CursorDataProvider, StateLeft);
			LeftRing.UpdateData(CursorDataProvider, StateLeft);
			LeftPinky.UpdateData(CursorDataProvider, StateLeft);

			RightPalm.UpdateData(CursorDataProvider, StateRight);
			RightThumb.UpdateData(CursorDataProvider, StateRight);
			RightIndex.UpdateData(CursorDataProvider, StateRight);
			RightMiddle.UpdateData(CursorDataProvider, StateRight);
			RightRing.UpdateData(CursorDataProvider, StateRight);
			RightPinky.UpdateData(CursorDataProvider, StateRight);
		}

		/*--------------------------------------------------------------------------------------------*/
		private ControlState GetControllerState(VRTK_ControllerEvents pControl) {
			//TODO: handle "isValid" states?

			var state = new ControlState {
				Controller = pControl,
				Tx = pControl.transform,
				TouchpadAxis = pControl.GetTouchpadAxis(),
				TriggerAxis = pControl.GetTriggerAxis(),
				TouchpadTouch = pControl.touchpadTouched,
				TouchpadPress = pControl.touchpadPressed,
				GripPress = pControl.gripPressed,
				MenuPress = pControl.menuPressed
			};

			return state;
		}

	}

}

#else

using Hover.Core.Utils;

namespace Hover.InputModules.VRTK {

	/*================================================================================================*/
	public class HoverInputVrtk : HoverInputMissing {

		public override string ModuleName { get { return "VRTK"; } }
		public override string RequiredSymbol { get { return "HOVER_INPUT_VRTK"; } }

	}

}

#endif //HOVER_INPUT_VRTK