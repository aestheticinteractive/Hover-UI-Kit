#if HOVER_INPUT_VIVE

using Hover.Core.Cursors;
using UnityEngine;
using Valve.VR;

namespace Hover.InputModules.Vive {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverInputVive : MonoBehaviour {

		public struct ControlState {
			public SteamVR_TrackedObject Controller;
			public Transform Tx;
			public bool IsValid;
			public Vector2 TouchpadAxis;
			public Vector2 TriggerAxis;
			public bool TouchpadTouch;
			public bool TouchpadPress;
			public bool GripPress;
			public bool MenuPress;
		}

		public ControlState StateLeft { get; private set; }
		public ControlState StateRight { get; private set; }

		public HoverCursorDataProvider CursorDataProvider;
		public SteamVR_ControllerManager SteamControllers;

		[Space(12)]

		public FollowCursor Look = new FollowCursor(CursorType.Look);

		[Space(12)]

		public ViveCursor LeftPalm = new ViveCursor(CursorType.LeftPalm) {
			LocalPosition = new Vector3(0, 0.01f, 0),
			LocalRotation = new Vector3(90, 0, 0),
			CursorSizeInput = ViveCursor.InputSourceType.TouchpadX,
			MinSize = 0.04f,
			MaxSize = 0.06f
		};

		public ViveCursor LeftThumb = new ViveCursor(CursorType.LeftThumb) {
			LocalPosition = new Vector3(0, 0, -0.17f),
			LocalRotation = new Vector3(-90, 0, 0)
		};

		public ViveCursor LeftIndex = new ViveCursor(CursorType.LeftIndex) {
			LocalPosition = new Vector3(-0.05f, 0, 0.03f),
			LocalRotation = new Vector3(90, -40, 0)
		};

		public ViveCursor LeftMiddle = new ViveCursor(CursorType.LeftMiddle) {
			LocalPosition = new Vector3(0, 0, 0.06f),
			LocalRotation = new Vector3(90, 0, 0)
		};

		public ViveCursor LeftRing = new ViveCursor(CursorType.LeftRing) {
			LocalPosition = new Vector3(0.05f, 0, 0.03f),
			LocalRotation = new Vector3(90, 40, 0)
		};

		public ViveCursor LeftPinky = new ViveCursor(CursorType.LeftPinky) {
			LocalPosition = new Vector3(0.08f, 0, -0.06f),
			LocalRotation = new Vector3(-90, -180, 80),
			TriggerStrengthInput = ViveCursor.InputSourceType.TouchpadLeft //for Hovercast
		};

		[Space(12)]

		public ViveCursor RightPalm = new ViveCursor(CursorType.RightPalm) {
			LocalPosition = new Vector3(0, 0.01f, 0),
			LocalRotation = new Vector3(90, 0, 0),
			CursorSizeInput = ViveCursor.InputSourceType.TouchpadX,
			MinSize = 0.04f,
			MaxSize = 0.06f
		};

		public ViveCursor RightThumb = new ViveCursor(CursorType.RightThumb) {
			LocalPosition = new Vector3(0, 0, -0.17f),
			LocalRotation = new Vector3(-90, 0, 0)
		};

		public ViveCursor RightIndex = new ViveCursor(CursorType.RightIndex) {
			LocalPosition = new Vector3(0.05f, 0, 0.03f),
			LocalRotation = new Vector3(90, 40, 0)
		};

		public ViveCursor RightMiddle = new ViveCursor(CursorType.RightMiddle) {
			LocalPosition = new Vector3(0, 0, 0.06f),
			LocalRotation = new Vector3(90, 0, 0)
		};

		public ViveCursor RightRing = new ViveCursor(CursorType.RightRing) {
			LocalPosition = new Vector3(-0.05f, 0, 0.03f),
			LocalRotation = new Vector3(90, -40, 0)
		};

		public ViveCursor RightPinky = new ViveCursor(CursorType.RightPinky) {
			LocalPosition = new Vector3(-0.08f, 0, -0.06f),
			LocalRotation = new Vector3(-90, 180, -80),
			TriggerStrengthInput = ViveCursor.InputSourceType.TouchpadRight //for Hovercast
		};


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			CursorUtil.FindCursorReference(this, ref CursorDataProvider, false);

			if ( Look.FollowTransform == null ) {
				Look.FollowTransform = Camera.main.transform;
			}

			if ( SteamControllers == null ) {
				SteamControllers = FindObjectOfType<SteamVR_ControllerManager>();
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
			StateLeft = GetControllerState(SteamControllers.left);
			StateRight = GetControllerState(SteamControllers.right);

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
		private ControlState GetControllerState(GameObject pControlGo) {
			SteamVR_TrackedObject control = pControlGo.GetComponent<SteamVR_TrackedObject>();
			SteamVR_Controller.Device input = null;

			var state = new ControlState();
			state.Controller = control;
			state.Tx = control.transform;
			state.IsValid = control.isValid;

			if ( control.index < 0 ) {
				state.IsValid = false;
			}
			else {
				input = SteamVR_Controller.Input((int)control.index);
				state.IsValid = (state.IsValid && input.valid);
			}

			if ( state.IsValid ) {
				state.TouchpadAxis = input.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
				state.TriggerAxis = input.GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger);
				state.TouchpadTouch = input.GetTouch(EVRButtonId.k_EButton_SteamVR_Touchpad);
				state.TouchpadPress = input.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad);
				state.GripPress = input.GetPress(EVRButtonId.k_EButton_Grip);
				state.MenuPress = input.GetPress(EVRButtonId.k_EButton_ApplicationMenu);
			}

			return state;
		}

	}

}

#else

using Hover.Core.Utils;

namespace Hover.InputModules.Vive {

	/*================================================================================================*/
	public class HoverInputVive : HoverInputMissing {

		public override string ModuleName { get { return "Vive"; } }
		public override string RequiredSymbol { get { return "HOVER_INPUT_VIVE"; } }

	}

}

#endif //HOVER_INPUT_VIVE
