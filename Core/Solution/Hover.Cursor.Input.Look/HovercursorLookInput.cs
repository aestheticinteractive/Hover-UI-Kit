using System;
using System.Collections.ObjectModel;
using Hover.Common.Input;
using Hover.Common.Util;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cursor.Input.Look {

	/*================================================================================================*/
	public class HovercursorLookInput : HovercursorInput {

		public Transform HeadsetCameraTransform;
		public float CursorSize = 0.1f;
		public bool UseMouseForTesting = false;
		public float MousePositionMultiplier = 0.4f;

		private InputCursor vCursor;
		private CacheList<PlaneState> vPlaneStates;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			var sett = new InputSettings();
			sett.InputTransform = gameObject.transform;
			sett.CameraTransform = HeadsetCameraTransform;
			sett.CursorSize = CursorSize;
			sett.UseMouseForTesting = UseMouseForTesting;
			sett.MousePositionMultiplier = MousePositionMultiplier;
			
			if ( HeadsetCameraTransform == null ) {
				IsFailure = true;
				throw new Exception("The "+typeof(HovercursorLookInput)+" component "+
					"requires the 'Headset Camera Transform' to be set.");
			}

			vCursor = new InputCursor(CursorType.Look, sett);
			vPlaneStates = new CacheList<PlaneState>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput() {
			ReadOnlyCollection<PlaneData> planeDataList = vPlaneProviderFunc(vCursor.Type);

			//TODO: check this for GC allocations

			//vPlaneStates.RebuildWith(planeDataList, InitState);
			//vCursor.UpdateWithPlanes(vPlaneStates.ReadOnly);

			//PlaneState nearest = planes.FirstOrDefault(x => x.IsNearest);
			//Debug.Log("NEAREST: "+(nearest == null ? "---" : nearest.Id+" / "+nearest.HitDist));
		}

		/*--------------------------------------------------------------------------------------------*/
		public override IInputCursor GetCursor(CursorType pType) {
			if ( pType != CursorType.Look ) {
				throw new Exception("The "+typeof(HovercursorLookInput)+" component does not support "+
					"the use of "+typeof(CursorType)+"."+pType+".");
			}

			return vCursor;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void InitState(PlaneState pState, PlaneData pData) {
			pState.Init(pData);
		}

	}

}
