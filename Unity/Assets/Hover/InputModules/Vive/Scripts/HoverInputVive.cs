#if HOVER_INPUT_VIVE

using Hover.Core.Cursors;
using UnityEngine;
using Valve.VR;

namespace Hover.InputModules.Vive {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverInputVive : MonoBehaviour {

		private static readonly Quaternion RotationFix = Quaternion.Euler(90, 0, 0);

		public HoverCursorDataProvider CursorDataProvider;
		public SteamVR_ControllerManager SteamControllers;
		public Transform LookCursorTransform;

		[Range(0.01f, 0.1f)]
		public float MinSize = 0.04f;

		[Range(0.02f, 0.2f)]
		public float MaxSize = 0.06f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				CursorDataProvider = FindObjectOfType<HoverCursorDataProvider>();
			}

			if ( SteamControllers == null ) {
				SteamControllers = FindObjectOfType<SteamVR_ControllerManager>();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !Application.isPlaying ) {
				return;
			}

			if ( CursorDataProvider == null ) {
				Debug.LogError("References to "+typeof(HoverCursorDataProvider).Name+" and "+
					typeof(SteamVR_ControllerManager).Name+" must be set.", this);
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
			int leftIndex = -1;
			int rightIndex = -1;

			for ( int i = 0 ; i < objectCount ; i++ ) {
				GameObject deviceGo = SteamControllers.objects[i];

				if ( deviceGo == SteamControllers.left ) {
					leftIndex = i;
				}
				else if ( deviceGo == SteamControllers.right ) {
					rightIndex = i;
				}
			}

			UpdateCursorWithDevice(SteamControllers.left, leftIndex, CursorType.LeftPalm);
			UpdateCursorWithDevice(SteamControllers.right, rightIndex, CursorType.RightPalm);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorWithDevice(GameObject pDeviceGo, int pIndex, CursorType pCursorType) {
			SteamVR_TrackedObject tracked = pDeviceGo.GetComponent<SteamVR_TrackedObject>();

			if ( !CursorDataProvider.HasCursorData(pCursorType) ) {
				return;
			}

			ICursorDataForInput data = CursorDataProvider.GetCursorDataForInput(pCursorType);
			data.SetUsedByInput(tracked.isValid);

			if ( !tracked.isValid ) {
				return;
			}

			SteamVR_Controller.Device device = SteamVR_Controller.Input(pIndex);
			Vector2 touchAxis = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
			Vector2 triggerAxis = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger);
			bool isTouch = device.GetTouch(EVRButtonId.k_EButton_SteamVR_Touchpad);
			float sizeProg = (isTouch ? touchAxis.x*2+1 : 0.5f);

			data.SetWorldPosition(tracked.transform.position);
			data.SetWorldRotation(tracked.transform.rotation*RotationFix);
			data.SetSize(Mathf.Lerp(MinSize, MaxSize, sizeProg));
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
