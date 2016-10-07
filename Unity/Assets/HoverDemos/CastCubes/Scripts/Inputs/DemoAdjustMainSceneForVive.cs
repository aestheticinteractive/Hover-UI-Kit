﻿using Hover.Core.Cursors;
using Hover.Core.Utils;
using Hover.InterfaceModules.Cast;
using UnityEngine;

namespace HoverDemos.CastCubes.Inputs {

	/*================================================================================================*/
	public class DemoAdjustMainSceneForVive : HoverSceneAdjust {


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
			DemoHovercastCustomizer castCustom = cast.GetComponent<DemoHovercastCustomizer>();

			Deactivate(UnityUtil.FindComponentAll<Camera>("MainCamera").gameObject);
			Deactivate(UnityUtil.FindComponentAll<HoverCursorRendererUpdater>("LeftPinky"));
			Deactivate(UnityUtil.FindComponentAll<HoverCursorRendererUpdater>("RightPinky"));

			foreach ( ICursorData cursorData in cursProv.Cursors ) {
				ICursorDataForInput cursorDataInp = cursProv.GetCursorDataForInput(cursorData.Type);
				cursorDataInp.gameObject.SetActive(false);
				cursorDataInp.SetCapability(CursorCapabilityType.None);
			}

			pinL.gameObject.SetActive(true);
			midR.gameObject.SetActive(true);
			look.gameObject.SetActive(true);

			pinL.SetCapability(CursorCapabilityType.TransformOnly);
			midR.SetCapability(CursorCapabilityType.Full);
			look.SetCapability(CursorCapabilityType.TransformOnly);

			cast.transform.GetChild(0).localPosition = Vector3.zero; //moves "TransformAdjuster"

			castFollow.CursorType = CursorType.LeftPinky;

			castCustom.StandardFollowCursor = CursorType.LeftPinky;
			castCustom.SwitchedFollowCursor = CursorType.RightPinky;
			castCustom.StandardInteractionCursor = CursorType.RightMiddle;
			castCustom.SwitchedInteractionCursor = CursorType.LeftMiddle;

			//for non-playing editor...

			pinL.SetWorldRotation(Quaternion.Euler(0, 160, 80)); //face the camera (for editor)
			castFollow.Update(); //moves interface to the correct cursor transform
			cast.GetComponent<TreeUpdater>().Update(); //forces entire interface to update
		}

	}

}
