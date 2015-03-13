using System;
using Hover.Board.Devices.Leap;
using Leap;
using UnityEngine;

namespace Hover {

	/*================================================================================================*/
	public class LeapHandControllerHelper : MonoBehaviour, ILeapHandControllerHelper {

		private HandController vHandControl;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			vHandControl = gameObject.GetComponent<HandController>();

			if ( vHandControl == null ) {
				throw new Exception("The "+GetType().Name+" component must be added to the "+
					"same GameObject that contains the Leap Motion HandController component.");
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Frame GetFrame() {
			return vHandControl.GetFrame();
		}

		/*--------------------------------------------------------------------------------------------*/
		public Controller GetController() {
			return vHandControl.GetLeapController();
		}

	}

}
