using Hover.Core.Cursors;
using Hover.Core.Utils;
using Hover.InterfaceModules.Cast;
using UnityEngine;

namespace HoverDemos.Common {

	/*================================================================================================*/
	public class DemoAdjustCursorsForHovercastDefault : HoverSceneAdjust {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool IsReadyToAdjust() {
			return (FindObjectOfType<HovercastInterface>() != null);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void PerformAdjustments() {
			HoverCursorDataProvider cursProv = FindObjectOfType<HoverCursorDataProvider>();
			ICursorDataForInput palL = cursProv.GetCursorDataForInput(CursorType.LeftPalm);
			ICursorDataForInput indR = cursProv.GetCursorDataForInput(CursorType.RightIndex);
			ICursorDataForInput look = cursProv.GetCursorDataForInput(CursorType.Look);
			HovercastInterface cast = FindObjectOfType<HovercastInterface>();
			HoverCursorFollower castFollow = cast.GetComponent<HoverCursorFollower>();
			HoverCursorRendererUpdater[] cursorRendUps = 
				Resources.FindObjectsOfTypeAll<HoverCursorRendererUpdater>();

			foreach ( HoverCursorRendererUpdater rendUp in cursorRendUps ) {
				CursorType ct = rendUp.GetComponent<HoverCursorFollower>().CursorType;
				rendUp.gameObject.SetActive(ct != CursorType.LeftPalm && ct != CursorType.RightPalm);
			}

			foreach ( ICursorData cursorData in cursProv.Cursors ) {
				ICursorDataForInput cursorDataInp = cursProv.GetCursorDataForInput(cursorData.Type);
				cursorDataInp.SetCapability(CursorCapabilityType.None);
			}

			palL.SetCapability(CursorCapabilityType.TransformOnly);
			indR.SetCapability(CursorCapabilityType.Full);
			look.SetCapability(CursorCapabilityType.TransformOnly);

			cast.transform.GetChild(0).localPosition = 
				new Vector3(0, 0, 0.02f); //moves "TransformAdjuster"
			castFollow.CursorType = palL.Type;

			//for non-playing editor...

			palL.SetWorldRotation(Quaternion.Euler(0, 160, 80)); //face the camera (for editor)
			castFollow.Update(); //moves interface to the correct cursor transform
			cast.GetComponent<TreeUpdater>().Update(); //forces entire interface to update
		}

	}

}
