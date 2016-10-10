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

		public HoverCursorDataProvider CursorDataProvider;
		public SteamVR_ControllerManager SteamControllers;
		public Transform LookCursorTransform;

		////

		public Info LeftPalm = new Info {
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
			LocalRotation = new Vector3(90, 0, -90)
		};

		////

		public Info RightPalm = new Info {
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
			LocalRotation = new Vector3(90, 0, 90)
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
			UpdateCursorsWithDevices();
			UpdateCursorWithCamera();
			CursorDataProvider.ActivateAllCursorsBasedOnUsage();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorsWithDevices() {
			int objectCount = SteamControllers.objects.Length;
			int indexL = -1;
			int indexR = -1;

			for ( int i = 0 ; i < objectCount ; i++ ) {
				GameObject deviceGo = SteamControllers.objects[i];

				if ( deviceGo == SteamControllers.left ) {
					indexL = i;
				}
				else if ( deviceGo == SteamControllers.right ) {
					indexR = i;
				}
			}

			Transform deviceTxL = SteamControllers.left.transform;
			Transform deviceTxR = SteamControllers.right.transform;

			SteamVR_Controller.Device deviceL = SteamVR_Controller.Input(indexL);
			SteamVR_Controller.Device deviceR = SteamVR_Controller.Input(indexR);

			UpdateCursorWithDevice(deviceTxL, deviceL, LeftPalm,    CursorType.LeftPalm);
			UpdateCursorWithDevice(deviceTxL, deviceL, LeftThumb,   CursorType.LeftThumb);
			UpdateCursorWithDevice(deviceTxL, deviceL, LeftIndex,   CursorType.LeftIndex);
			UpdateCursorWithDevice(deviceTxL, deviceL, LeftMiddle,  CursorType.LeftMiddle);
			UpdateCursorWithDevice(deviceTxL, deviceL, LeftRing,    CursorType.LeftRing);
			UpdateCursorWithDevice(deviceTxL, deviceL, LeftPinky,   CursorType.LeftPinky);

			UpdateCursorWithDevice(deviceTxR, deviceR, RightPalm,   CursorType.RightPalm);
			UpdateCursorWithDevice(deviceTxR, deviceR, RightThumb,  CursorType.RightThumb);
			UpdateCursorWithDevice(deviceTxR, deviceR, RightIndex,  CursorType.RightIndex);
			UpdateCursorWithDevice(deviceTxR, deviceR, RightMiddle, CursorType.RightMiddle);
			UpdateCursorWithDevice(deviceTxR, deviceR, RightRing,   CursorType.RightRing);
			UpdateCursorWithDevice(deviceTxR, deviceR, RightPinky,  CursorType.RightPinky);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorWithDevice(Transform pDeviceTx, SteamVR_Controller.Device pDevice,
															Info pInfo, CursorType pCursorType) {
			if ( !CursorDataProvider.HasCursorData(pCursorType) ) {
				return;
			}

			ICursorDataForInput data = CursorDataProvider.GetCursorDataForInput(pCursorType);

			data.SetUsedByInput(pDevice.valid);

			if ( !pDevice.valid ) {
				return;
			}

			Vector2 touchAxis = pDevice.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
			Vector2 triggerAxis = pDevice.GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger);
			bool isTouch = pDevice.GetTouch(EVRButtonId.k_EButton_SteamVR_Touchpad);
			float sizeProg = (isTouch ? touchAxis.x*2+1 : 0.5f);
			Vector3 worldOffset = pDeviceTx.TransformVector(pInfo.LocalPosition);

			data.SetWorldPosition(pDeviceTx.position+worldOffset);
			data.SetWorldRotation(pDeviceTx.rotation*Quaternion.Euler(pInfo.LocalRotation));
			data.SetSize(Mathf.Lerp(pInfo.MinSize, pInfo.MaxSize, sizeProg));
			data.SetTriggerStrength(triggerAxis.x);
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

#endif //HOVER_INPUT_VIVE
