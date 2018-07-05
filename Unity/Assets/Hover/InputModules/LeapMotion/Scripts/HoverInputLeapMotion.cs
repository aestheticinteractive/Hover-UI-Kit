#if HOVER_INPUT_LEAPMOTION

using System;
using System.Collections.Generic;
using Hover.Core.Cursors;
using Leap;
using Leap.Unity;
using UnityEngine;

namespace Hover.InputModules.LeapMotion {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverInputLeapMotion : MonoBehaviour {

		private static readonly Quaternion RotationFix = Quaternion.Euler(90, 90, -90);

		public HoverCursorDataProvider CursorDataProvider;
		public LeapServiceProvider LeapServiceProvider;
		public bool UseStabilizedPositions = false;

		[Range(0, 0.04f)]
		public float ExtendFingertipDistance = 0;

		[Space(12)]

		public FollowCursor Look = new FollowCursor(CursorType.Look);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			CursorUtil.FindCursorReference(this, ref CursorDataProvider, false);

			if ( LeapServiceProvider == null ) {
				LeapServiceProvider = FindObjectOfType<LeapServiceProvider>();
			}

			if ( Look.FollowTransform == null ) {
				GameObject lookGo = GameObject.Find("CenterEyeAnchor");
				Look.FollowTransform = (lookGo == null ? null : lookGo.transform);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !CursorUtil.FindCursorReference(this, ref CursorDataProvider, true) ) {
				return;
			}

			if ( !Application.isPlaying ) {
				return;
			}

			CursorDataProvider.MarkAllCursorsUnused();
			UpdateCursorsWithHands(LeapServiceProvider.CurrentFrame.Hands);
			Look.UpdateData(CursorDataProvider);
			CursorDataProvider.ActivateAllCursorsBasedOnUsage();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorsWithHands(List<Hand> pLeapHands) {
			for ( int i = 0 ; i < pLeapHands.Count ; i++ ) {
				UpdateCursorsWithHand(pLeapHands[i]);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorsWithHand(Hand pLeapHand) {
			UpdateCursorsWithPalm(pLeapHand);

			for ( int i = 0 ; i < pLeapHand.Fingers.Count ; i++ ) {
				UpdateCursorsWithFinger(pLeapHand, pLeapHand.Fingers[i]);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorsWithPalm(Hand pLeapHand) {
			CursorType cursorType = (pLeapHand.IsLeft ? CursorType.LeftPalm : CursorType.RightPalm);

			if ( !CursorDataProvider.HasCursorData(cursorType) ) {
				return;
			}

			Vector palmPos = (UseStabilizedPositions ? 
				pLeapHand.StabilizedPalmPosition : pLeapHand.PalmPosition);

			ICursorDataForInput data = CursorDataProvider.GetCursorDataForInput(cursorType);
			data.SetWorldPosition(palmPos.ToVector3());
			data.SetWorldRotation(pLeapHand.Basis.CalculateRotation()*RotationFix);
			data.SetSize(pLeapHand.PalmWidth);
			data.SetTriggerStrength(pLeapHand.GrabStrength);
			data.SetUsedByInput(true);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorsWithFinger(Hand pLeapHand, Finger pLeapFinger) {
			CursorType cursorType = GetFingerCursorType(pLeapHand.IsLeft, pLeapFinger.Type);

			if ( !CursorDataProvider.HasCursorData(cursorType) ) {
				return;
			}

			Bone distalBone = pLeapFinger.Bone(Bone.BoneType.TYPE_DISTAL);
			Vector3 tipWorldPos = pLeapFinger.TipPosition.ToVector3();
			Vector3 boneWorldPos = distalBone.Center.ToVector3();
			Vector3 extendedWorldPos = tipWorldPos;

			if ( ExtendFingertipDistance != 0 ) {
				extendedWorldPos += (tipWorldPos-boneWorldPos).normalized*ExtendFingertipDistance;
			}

			ICursorDataForInput data = CursorDataProvider.GetCursorDataForInput(cursorType);
			data.SetWorldPosition(extendedWorldPos);
			data.SetWorldRotation(distalBone.Basis.CalculateRotation()*RotationFix);
			data.SetSize(pLeapFinger.Width);
			data.SetUsedByInput(true);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private CursorType GetFingerCursorType(bool pIsLeft, Finger.FingerType pLeapFingerType) {
			switch ( pLeapFingerType ) {
				case Finger.FingerType.TYPE_THUMB:
					return (pIsLeft ? CursorType.LeftThumb : CursorType.RightThumb);
					
				case Finger.FingerType.TYPE_INDEX:
					return (pIsLeft ? CursorType.LeftIndex : CursorType.RightIndex);
					
				case Finger.FingerType.TYPE_MIDDLE:
					return (pIsLeft ? CursorType.LeftMiddle : CursorType.RightMiddle);
					
				case Finger.FingerType.TYPE_RING:
					return (pIsLeft ? CursorType.LeftRing : CursorType.RightRing);
					
				case Finger.FingerType.TYPE_PINKY:
					return (pIsLeft ? CursorType.LeftPinky : CursorType.RightPinky);
			}
			
			throw new Exception("Unhandled cursor combination: "+pIsLeft+" / "+pLeapFingerType);
		}

	}

}

#else

using Hover.Core.Utils;

namespace Hover.InputModules.LeapMotion {

	/*================================================================================================*/
	public class HoverInputLeapMotion : HoverInputMissing {

		public override string ModuleName { get { return "LeapMotion"; } }
		public override string RequiredSymbol { get { return "HOVER_INPUT_LEAPMOTION"; } }

	}

}

#endif //HOVER_INPUT_LEAPMOTION
