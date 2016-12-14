#if HOVER_INPUT_OCULUSTOUCH

using System;
using Hover.Core.Cursors;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.InputModules.OculusTouch {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverInputOculusTouch : MonoBehaviour {

		private static readonly Vector3 FingerLocalPosFix = new Vector3(0, -0.008f, 0);
		private static readonly Quaternion FingerLocalLeftRotFix = Quaternion.Euler(90, 0, 90);
		private static readonly Quaternion FingerLocalRightRotFix = Quaternion.Euler(90, 0, -90);

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
			public bool IsValid;
			public Vector3 LocalPos;
			public Quaternion LocalRot;
			public Vector2 TouchpadAxis;
			public float TriggerStrength;
		}

		public HoverCursorDataProvider CursorDataProvider;
		public OvrAvatar Avatar;
		public Transform LookCursorTransform;
		public bool UseHandTransforms = true;
		public string AvatarLeftIndexName = "hands:b_l_index_ignore";
		public string AvatarRightIndexName = "hands:b_r_index_ignore";
		public string AvatarLeftThumbName = "hands:b_l_thumb_ignore";
		public string AvatarRightThumbName = "hands:b_r_thumb_ignore";

		////

		public Info LeftPalm = new Info {
			LocalPosition = new Vector3(0, 0.01f, 0),
			MinSize = 0.04f,
			MaxSize = 0.06f
		};

		public Info LeftThumb = new Info {
			LocalPosition = new Vector3(0, 0, -0.09f),
			LocalRotation = new Vector3(-90, 0, 0)
		};

		public Info LeftIndex = new Info {
			LocalPosition = new Vector3(-0.06f, 0, 0.02f),
			LocalRotation = new Vector3(90, -40, 0)
		};

		public Info LeftMiddle = new Info {
			LocalPosition = new Vector3(0, 0, 0.08f),
			LocalRotation = new Vector3(90, 0, 0)
		};

		public Info LeftRing = new Info {
			LocalPosition = new Vector3(0.06f, 0, 0.02f),
			LocalRotation = new Vector3(90, 40, 0)
		};

		public Info LeftPinky = new Info {
			LocalPosition = new Vector3(0.05f, 0, -0.05f),
			LocalRotation = new Vector3(-90, -180, 80)
		};

		////

		public Info RightPalm = new Info {
			LocalPosition = new Vector3(0, 0.01f, 0),
			MinSize = 0.04f,
			MaxSize = 0.06f
		};

		public Info RightThumb = new Info {
			LocalPosition = new Vector3(0, 0, -0.09f),
			LocalRotation = new Vector3(-90, 0, 0)
		};

		public Info RightIndex = new Info {
			LocalPosition = new Vector3(0.06f, 0, 0.02f),
			LocalRotation = new Vector3(90, 40, 0)
		};

		public Info RightMiddle = new Info {
			LocalPosition = new Vector3(0, 0, 0.08f),
			LocalRotation = new Vector3(90, 0, 0)
		};

		public Info RightRing = new Info {
			LocalPosition = new Vector3(-0.06f, 0, 0.02f),
			LocalRotation = new Vector3(90, -40, 0)
		};

		public Info RightPinky = new Info {
			LocalPosition = new Vector3(-0.05f, 0, -0.05f),
			LocalRotation = new Vector3(-90, 180, -80)
		};

		private Transform vLeftIndexTx;
		private Transform vRightIndexTx;
		private Transform vLeftThumbTx;
		private Transform vRightThumbTx;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			InputModuleUtil.FindCursorReference(this, ref CursorDataProvider, false);

			if ( Avatar == null ) {
				Avatar = FindObjectOfType<OvrAvatar>();
			}

			if ( LookCursorTransform == null ) {
				LookCursorTransform = Camera.main.transform;
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

			FindAvatarTransforms();
			CursorDataProvider.MarkAllCursorsUnused();
			UpdateCursorsWithControllers();
			UpdateCursorWithCamera();
			CursorDataProvider.ActivateAllCursorsBasedOnUsage();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void FindAvatarTransforms() {
			if ( !UseHandTransforms || vLeftIndexTx != null ) {
				return;
			}

			//TODO: find a better way to obtain the "hand bones"
			vLeftIndexTx = FindAvatarTransform(Avatar.transform, AvatarLeftIndexName);
			vRightIndexTx = FindAvatarTransform(Avatar.transform, AvatarRightIndexName);
			vLeftThumbTx = FindAvatarTransform(Avatar.transform, AvatarLeftThumbName);
			vRightThumbTx = FindAvatarTransform(Avatar.transform, AvatarRightThumbName);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private Transform FindAvatarTransform(Transform pParentTx, string pName) {
			foreach ( Transform childTx in pParentTx ) {
				if ( childTx.name == pName ) {
					return childTx;
				}

				Transform foundTx = FindAvatarTransform(childTx, pName);

				if ( foundTx != null ) {
					return foundTx;
				}
			}

			return null;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorsWithControllers() {
			ControlState contL = GetControllerState(OVRInput.Controller.LTouch);
			ControlState contR = GetControllerState(OVRInput.Controller.RTouch);

			UpdateCursorWithController(contL, LeftPalm,    CursorType.LeftPalm);
			UpdateCursorWithController(contL, LeftThumb,   CursorType.LeftThumb, vLeftThumbTx);
			UpdateCursorWithController(contL, LeftIndex,   CursorType.LeftIndex, vLeftIndexTx);
			UpdateCursorWithController(contL, LeftMiddle,  CursorType.LeftMiddle);
			UpdateCursorWithController(contL, LeftRing,    CursorType.LeftRing);
			UpdateCursorWithController(contL, LeftPinky,   CursorType.LeftPinky);

			UpdateCursorWithController(contR, RightPalm,   CursorType.RightPalm);
			UpdateCursorWithController(contR, RightThumb,  CursorType.RightThumb, vRightThumbTx);
			UpdateCursorWithController(contR, RightIndex,  CursorType.RightIndex, vRightIndexTx);
			UpdateCursorWithController(contR, RightMiddle, CursorType.RightMiddle);
			UpdateCursorWithController(contR, RightRing,   CursorType.RightRing);
			UpdateCursorWithController(contR, RightPinky,  CursorType.RightPinky);
		}

		/*--------------------------------------------------------------------------------------------*/
		private ControlState GetControllerState(OVRInput.Controller pControlType) {
			OVRInput.Controller contTypes = OVRInput.GetConnectedControllers();
			bool isValid = ((contTypes & pControlType) != 0);

			var state = new ControlState();
			state.IsValid = isValid;

			if ( state.IsValid ) {
				state.LocalPos = OVRInput.GetLocalControllerPosition(pControlType);
				state.LocalRot = OVRInput.GetLocalControllerRotation(pControlType);
				state.TouchpadAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, pControlType);
				state.TriggerStrength = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, pControlType);
			}

			return state;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorWithController(ControlState pState, Info pInfo, CursorType pType, 
																		Transform pHandBoneTx=null) {
			if ( !CursorDataProvider.HasCursorData(pType) ) {
				return;
			}

			ICursorDataForInput data = CursorDataProvider.GetCursorDataForInput(pType);

			data.SetUsedByInput(pState.IsValid);

			if ( !pState.IsValid ) {
				return;
			}

			if ( UseHandTransforms && pHandBoneTx != null ) {
				Quaternion sidedLocalRotFix = (pType < CursorType.RightPalm ? 
					FingerLocalLeftRotFix : FingerLocalRightRotFix);

				data.SetWorldRotation(pHandBoneTx.rotation*sidedLocalRotFix);
				data.SetWorldPosition(pHandBoneTx.position+data.WorldRotation*FingerLocalPosFix);
			}
			else {
				Matrix4x4 txMat = transform.localToWorldMatrix;
				Matrix4x4 txRotMat = txMat*Matrix4x4.TRS(Vector3.zero, pState.LocalRot, Vector3.one);

				data.SetWorldPosition(txMat.MultiplyPoint3x4(pState.LocalPos)+
					txRotMat.MultiplyPoint3x4(pInfo.LocalPosition));
				data.SetWorldRotation(txRotMat.GetRotation()*Quaternion.Euler(pInfo.LocalRotation));
			}

			float sizeProg = 0.5f; //+pState.TouchpadAxis.x/2;

			data.SetSize(Mathf.Lerp(pInfo.MinSize, pInfo.MaxSize, sizeProg));
			data.SetTriggerStrength(pState.TriggerStrength);
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

namespace Hover.InputModules.OculusTouch {

	/*================================================================================================*/
	public class HoverInputOculusTouch : HoverInputMissing {

		public override string ModuleName { get { return "OculusTouch"; } }
		public override string RequiredSymbol { get { return "HOVER_INPUT_OCULUSTOUCH"; } }

	}

}

#endif //HOVER_INPUT_OCULUSTOUCH
