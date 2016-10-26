using Hover.Core.Cursors;
using Hover.Core.Utils;
using Hover.InterfaceModules.Cast;
using UnityEngine;

namespace HoverDemos.Common {

	/*================================================================================================*/
	public class DemoAdjustCursorsForHovercastVive : HoverSceneAdjust {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool IsReadyToAdjust() {
			return (FindObjectOfType<HovercastInterface>() != null);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void PerformAdjustments() {
			HoverCursorDataProvider cursProv = FindObjectOfType<HoverCursorDataProvider>();
			ICursorDataForInput pinL = cursProv.GetCursorDataForInput(CursorType.LeftPinky);
			ICursorDataForInput midR = cursProv.GetCursorDataForInput(CursorType.RightMiddle);
			ICursorDataForInput look = cursProv.GetCursorDataForInput(CursorType.Look);
			HovercastInterface cast = FindObjectOfType<HovercastInterface>();
			HoverCursorFollower castFollow = cast.GetComponent<HoverCursorFollower>();
			HoverCursorRendererUpdater[] cursorRendUps =
				Resources.FindObjectsOfTypeAll<HoverCursorRendererUpdater>();

			foreach ( HoverCursorRendererUpdater rendUp in cursorRendUps ) {
				CursorType ct = rendUp.GetComponent<HoverCursorFollower>().CursorType;
				rendUp.gameObject.SetActive(ct != CursorType.LeftPinky && ct != CursorType.RightPinky);
			}

			foreach ( ICursorData cursorData in cursProv.Cursors ) {
				ICursorDataForInput cursorDataInp = cursProv.GetCursorDataForInput(cursorData.Type);
				cursorDataInp.SetCapability(CursorCapabilityType.None);
			}

			pinL.SetCapability(CursorCapabilityType.TransformOnly);
			midR.SetCapability(CursorCapabilityType.Full);
			look.SetCapability(CursorCapabilityType.TransformOnly);

			cast.transform.GetChild(0).localPosition = Vector3.zero; //moves "TransformAdjuster"
			castFollow.CursorType = pinL.Type;

			//for non-playing editor...

			pinL.SetWorldRotation(Quaternion.Euler(0, 160, 80)); //face the camera (for editor)
			castFollow.Update(); //moves interface to the correct cursor transform
			cast.GetComponent<TreeUpdater>().Update(); //forces entire interface to update
		}

	}

}
