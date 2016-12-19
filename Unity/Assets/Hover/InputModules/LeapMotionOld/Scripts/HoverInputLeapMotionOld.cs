#if HOVER_INPUT_LEAPMOTIONOLD

using System;
using Hover.Core.Cursors;
using Hover.InputModules.Follow;
using Leap;
using UnityEngine;

namespace Hover.InputModules.LeapMotionOld {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverInputLeapMotionOld : MonoBehaviour {

		private static readonly Quaternion RotationFix = Quaternion.Euler(90, 90, 90);

		public HoverCursorDataProvider CursorDataProvider;
		public HandController LeapControl;
		public bool UseStabilizedPositions = false;

		[Range(0, 0.04f)]
		public float ExtendFingertipDistance = 0;

		[Space(12)]

		public FollowCursor Look = new FollowCursor(CursorType.Look);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			InputModuleUtil.FindCursorReference(this, ref CursorDataProvider, false);

			if ( LeapControl == null ) {
				LeapControl = FindObjectOfType<HandController>();
			}

			if ( Look.FollowTransform == null ) {
				GameObject lookGo = GameObject.Find("CenterEyeAnchor");
				Look.FollowTransform = (lookGo == null ? null : lookGo.transform);
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
			UpdateCursorsWithHands(LeapControl.GetFrame().Hands);
			Look.UpdateData(CursorDataProvider);
			CursorDataProvider.ActivateAllCursorsBasedOnUsage();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorsWithHands(HandList pLeapHands) {
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

			Transform leapTx = LeapControl.transform;
			Vector palmPos = (UseStabilizedPositions ? 
				pLeapHand.StabilizedPalmPosition : pLeapHand.PalmPosition);

			ICursorDataForInput data = CursorDataProvider.GetCursorDataForInput(cursorType);
			data.SetWorldPosition(leapTx.TransformPoint(palmPos.ToUnityScaled()));
			data.SetWorldRotation(leapTx.rotation*pLeapHand.Basis.Rotation()*RotationFix);
			data.SetSize(pLeapHand.PalmWidth*UnityVectorExtension.INPUT_SCALE);
			data.SetTriggerStrength(pLeapHand.GrabStrength);
			data.SetUsedByInput(true);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCursorsWithFinger(Hand pLeapHand, Finger pLeapFinger) {
			CursorType cursorType = GetFingerCursorType(pLeapHand.IsLeft, pLeapFinger.Type);

			if ( !CursorDataProvider.HasCursorData(cursorType) ) {
				return;
			}

			Transform leapTx = LeapControl.transform;
			Vector tipPos = (UseStabilizedPositions ? 
				pLeapFinger.StabilizedTipPosition: pLeapFinger.TipPosition);
			Bone distalBone = pLeapFinger.Bone(Bone.BoneType.TYPE_DISTAL);
			Vector3 tipWorldPos = leapTx.TransformPoint(tipPos.ToUnityScaled());
			Vector3 boneWorldPos = leapTx.TransformPoint(distalBone.Center.ToUnityScaled());
			Vector3 extendedWorldPos = tipWorldPos;

			if ( ExtendFingertipDistance != 0 ) {
				extendedWorldPos += (tipWorldPos-boneWorldPos).normalized*ExtendFingertipDistance;
			}

			ICursorDataForInput data = CursorDataProvider.GetCursorDataForInput(cursorType);
			data.SetWorldPosition(extendedWorldPos);
			data.SetWorldRotation(leapTx.rotation*distalBone.Basis.Rotation()*RotationFix);
			data.SetSize(pLeapFinger.Width*UnityVectorExtension.INPUT_SCALE);
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

namespace Hover.InputModules.LeapMotionOld {

	/*================================================================================================*/
	public class HoverInputLeapMotionOld : HoverInputMissing {

		public override string ModuleName { get { return "LeapMotionOld"; } }
		public override string RequiredSymbol { get { return "HOVER_INPUT_LEAPMOTIONOLD"; } }

	}

}

#endif //HOVER_INPUT_LEAPMOTIONOLD
