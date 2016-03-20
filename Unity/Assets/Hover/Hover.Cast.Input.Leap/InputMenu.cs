using System;
using System.Collections.Generic;
using Leap;
using UnityEngine;

namespace Hover.Cast.Input.Leap {

	/*================================================================================================*/
	public class InputMenu : IInputMenu {

		public bool IsLeft { get; private set; }
		public bool IsAvailable { get; private set; }

		public Vector3 Position { get; private set; }
		public Quaternion Rotation { get; private set; }
		public float Radius { get; private set; }

		public float NavigateBackStrength { get; private set; }
		public float DisplayStrength { get; private set; }

		private readonly InputSettings vSettings;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public InputMenu(bool pIsLeft, InputSettings pSettings) {
			IsLeft = pIsLeft;
			vSettings = pSettings;
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void Rebuild(Hand pLeapHand) {
			if ( pLeapHand == null ) {
				IsAvailable = false;
				Position = Vector3.zero;
				Rotation = Quaternion.identity;
				Radius = 0;
				NavigateBackStrength = 0;
				DisplayStrength = 0;
				return;
			}

			IsAvailable = true;
			Position = pLeapHand.PalmPosition.ToVector3();
			Rotation = pLeapHand.Basis.Rotation(); //GC_ALLOC
			Radius = 0.01f;

			List<Finger> leapFingers = pLeapHand.Fingers; //GC_ALLOC

			for ( int i = 0 ; i < leapFingers.Count ; i++ ) {
				Finger leapFinger = leapFingers[i]; //GC_ALLOC

				if ( leapFinger == null ) {
					continue;
				}

				Vector3 palmToFinger = leapFinger.TipPosition.ToVector3()-Position; //GC_ALLOC
				//Bone bone = leapFinger.Bone(Bone.BoneType.TYPE_DISTAL); //GC_ALLOC
				//Quaternion boneRot = bone.Basis.Rotation(); //GC_ALLOC

				Radius = Math.Max(Radius, palmToFinger.sqrMagnitude);
				//TODO: fix for Orion (causes menu to "jump" at a point during finger moving to palm)
				//Rotation = Quaternion.Slerp(Rotation, boneRot, 0.1f);
			}

			Vector3 palmNormal = pLeapHand.PalmNormal.ToVector3();

			Radius = (float)Math.Sqrt(Radius);
			Position += palmNormal*(vSettings.DistanceFromPalm*Radius);

			NavigateBackStrength = pLeapHand.GrabStrength/vSettings.NavBackGrabThreshold;
			NavigateBackStrength = Mathf.Clamp(NavigateBackStrength, 0, 1);

			Vector3 palmToEyeDir = (vSettings.CameraTransform.position-Position).normalized;
			float palmNormalDotDir = Vector3.Dot(palmNormal, palmToEyeDir);

			DisplayStrength = Mathf.Clamp((palmNormalDotDir-0.7f)/0.25f, 0, 1);
		}

	}

}
