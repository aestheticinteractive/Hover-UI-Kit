using System;
using System.Linq;
using Hover.Common.Input;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cursor.Input.Look {

	/*================================================================================================*/
	public class HovercursorLookInput : HovercursorInput {

		public Transform HeadsetCameraTransform;
		public float CursorSize = 0.1f;

		private InputCursor vCursor;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			var sett = new InputSettings();
			sett.InputTransform = gameObject.transform;
			sett.CameraTransform = HeadsetCameraTransform;
			sett.CursorSize = CursorSize;

			if ( HeadsetCameraTransform == null ) {
				IsFailure = true;
				throw new Exception("The "+typeof(HovercursorLookInput)+" component "+
					"requires the 'Headset Camera Transform' to be set.");
			}

			vCursor = new InputCursor(CursorType.Look, sett);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput() {
			InteractionPlaneState[] planes = vPlaneMap.Values.ToArray();

			foreach ( InteractionPlaneState plane in planes ) {
				plane.IsHit = false;
				plane.HitPosition = Vector3.zero;
				plane.HitDist = 0;
				plane.IsNearest = false;
			}

			vCursor.UpdateWithPlanes(planes);

			//InteractionPlaneState nearest = planes.FirstOrDefault(x => x.IsNearest);
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

	}

}
