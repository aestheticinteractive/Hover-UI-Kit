#if HOVER_INPUT_VIVE

using System;
using Hover.Core.Cursors;
using Hover.Core.Utils;
using UnityEngine;
using Valve.VR;

namespace Hover.InputModules.Vive {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverInputVive : MonoBehaviour {

		[Serializable]
		public class Info {
			public Vector3 LocalPosition = Vector3.zero;
			public Vector3 LocalRotation = new Vector3(90, 0, 0);

			[Range(0.01f, 0.1f)]
			public float MinSize = 0.01f;

			[Range(0.02f, 0.2f)]
			public float MaxSize = 0.03f;
		}

		private struct ControlState {
			public Transform Tx;
			public bool IsValid;
			public bool IsTouchpadTouched;
			public Vector2 TouchpadAxis;
			public Vector2 TriggerAxis;
		}

		public HoverCursorDataProvider CursorDataProvider;
		public SteamVR_ControllerManager SteamControllers;
		public Transform LookCursorTransform;

		////

		public Info LeftPalm = new Info {
			LocalPosition = new Vector3(0, 0.01f, 0),
			MinSize = 0.04f,
			MaxSize = 0.06f
		};

		public Info LeftThumb = new Info {
			LocalPosition = new Vector3(0, 0, -0.17f),
			LocalRotation = new Vector3(-90, 0, 0)
		};

		public Info LeftIndex = new Info {
			LocalPosition = new Vector3(-0.05f, 0, 0.03f),
			LocalRotation = new Vector3(90, -40, 0)
		};

		public Info LeftMiddle = new Info {
			LocalPosition = new Vector3(0, 0, 0.06f),
			LocalRotation = new Vector3(90, 0, 0)
		};

		public Info LeftRing = new Info {
			LocalPosition = new Vector3(0.05f, 0, 0.03f),
			LocalRotation = new Vector3(90, 40, 0)
		};

		public Info LeftPinky = new Info {
			LocalPosition = new Vector3(0.08f, 0, -0.06f),
			LocalRotation = new Vector3(-90, -180, 80)
		};

		////

		public Info RightPalm = new Info {
			LocalPosition = new Vector3(0, 0.01f, 0),
			MinSize = 0.04f,
			MaxSize = 0.06f
		};

		public Info RightThumb = new Info {
			LocalPosition = new Vector3(0, 0, -0.17f),
			LocalRotation = new Vector3(-90, 0, 0)
		};

		public Info RightIndex = new Info {
			LocalPosition = new Vector3(0.05f, 0, 0.03f),
			LocalRotation = new Vector3(90, 40, 0)
		};

		public Info RightMiddle = new Info {
			LocalPosition = new Vector3(0, 0, 0.06f),
			LocalRotation = new Vector3(90, 0, 0)
		};

		public Info RightRing = new Info {
			LocalPosition = new Vector3(-0.05f, 0, 0.03f),
			LocalRotation = new Vector3(90, -40, 0)
		};

		public Info RightPinky = new Info {
			LocalPosition = new Vector3(-0.08f, 0, -0.06f),
			LocalRotation = new Vector3(-90, 180, -80)
		};


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			InputModuleUtil.FindCursorReference(this, ref CursorDataProvider, false);

			if ( LookCursorTransform == null ) {
				LookCursorTransform = Camera.main.transform;
			}

			if ( SteamControllers == null ) {
				SteamControllers = FindObjectOfType<SteamVR_ControllerManager>();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !InputModuleUtil.FindCursorReference(this, ref CursorDataProvider, true) ) {
				return;
			}

			if ( !Application.isPlaying ) {
				return;
			}

			CursorDataProvider.MarkAllCursorsUnused();
			UpdateCursorsWithControllers();
			UpdateCursorWithCamera();
			CursorDataProvider.ActivateAllCursorsBasedOnUsage();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorsWithControllers() {
			ControlState contL = GetControllerState(SteamControllers.left);
			ControlState contR = GetControllerState(SteamControllers.right);

			UpdateCursorWithController(contL, LeftPalm,    CursorType.LeftPalm);
			UpdateCursorWithController(contL, LeftThumb,   CursorType.LeftThumb);
			UpdateCursorWithController(contL, LeftIndex,   CursorType.LeftIndex);
			UpdateCursorWithController(contL, LeftMiddle,  CursorType.LeftMiddle);
			UpdateCursorWithController(contL, LeftRing,    CursorType.LeftRing);
			UpdateCursorWithController(contL, LeftPinky,   CursorType.LeftPinky);

			UpdateCursorWithController(contR, RightPalm,   CursorType.RightPalm);
			UpdateCursorWithController(contR, RightThumb,  CursorType.RightThumb);
			UpdateCursorWithController(contR, RightIndex,  CursorType.RightIndex);
			UpdateCursorWithController(contR, RightMiddle, CursorType.RightMiddle);
			UpdateCursorWithController(contR, RightRing,   CursorType.RightRing);
			UpdateCursorWithController(contR, RightPinky,  CursorType.RightPinky);
		}

		/*--------------------------------------------------------------------------------------------*/
		private ControlState GetControllerState(GameObject pControlGo) {
			SteamVR_TrackedObject control = pControlGo.GetComponent<SteamVR_TrackedObject>();

			var state = new ControlState();
			state.Tx = control.transform;
			state.IsValid = control.isValid;
			SteamVR_Controller.Device input = null;

			if ( control.index < 0 ) {
				state.IsValid = false;
			}
			else {
				input = SteamVR_Controller.Input((int)control.index);
				state.IsValid = (state.IsValid && input.valid);
			}

			if ( state.IsValid ) {
				state.IsTouchpadTouched = input.GetTouch(EVRButtonId.k_EButton_SteamVR_Touchpad);
				state.TouchpadAxis = input.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
				state.TriggerAxis = input.GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger);
			}

			return state;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorWithController(ControlState pState, Info pInfo, CursorType pType) {
			if ( !CursorDataProvider.HasCursorData(pType) ) {
				return;
			}

			ICursorDataForInput data = CursorDataProvider.GetCursorDataForInput(pType);

			data.SetUsedByInput(pState.IsValid);

			if ( !pState.IsValid ) {
				return;
			}

			float sizeProg = 0.5f; //+(pState.IsTouchpadTouched ? pState.TouchpadAxis.x/2 : 0);
			Vector3 worldOffset = pState.Tx.TransformVector(pInfo.LocalPosition);

			data.SetWorldPosition(pState.Tx.position+worldOffset);
			data.SetWorldRotation(pState.Tx.rotation*Quaternion.Euler(pInfo.LocalRotation));
			data.SetSize(Mathf.Lerp(pInfo.MinSize, pInfo.MaxSize, sizeProg));
			data.SetTriggerStrength(pState.TriggerAxis.x);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorWithCamera() {
			if ( !CursorDataProvider.HasCursorData(CursorType.Look) ) {
				return;
			}

			ICursorDataForInput data = CursorDataProvider.GetCursorDataForInput(CursorType.Look);
			data.SetWorldPosition(LookCursorTransform.position);
			data.SetWorldRotation(LookCursorTransform.rotation);
			data.SetUsedByInput(true);
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
