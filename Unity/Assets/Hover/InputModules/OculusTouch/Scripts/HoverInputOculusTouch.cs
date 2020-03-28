#if HOVER_INPUT_OCULUSTOUCH

using System;
using Hover.Core.Cursors;
using OculusSampleFramework;
using UnityEngine;

namespace Hover.InputModules.OculusTouch {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverInputOculusTouch : MonoBehaviour {
		
		private static readonly Quaternion PalmRotationFixL = Quaternion.Euler( 90, 180,  90);
		private static readonly Quaternion PalmRotationFixR = Quaternion.Euler(-90, 180, -90);
		private static readonly Quaternion FingerRotationFixL = Quaternion.Euler( 90, 0,  90);
		private static readonly Quaternion FingerRotationFixR = Quaternion.Euler(-90, 0, -90);

		public struct ControlState {
			public OVRInput.Controller ControllerType;
			public bool IsValid;
			public Vector3 LocalPos;
			public Quaternion LocalRot;
			public Vector2 ThumbstickAxis;
			public float IndexTrigger;
			public float HandTrigger;
			public bool Button1Press;
			public bool Button2Press;
			public bool StartPress;
			public bool ThumbstickPress;
		}

		public ControlState StateLeft { get; private set; }
		public ControlState StateRight { get; private set; }

		public HoverCursorDataProvider CursorDataProvider;
		public OvrAvatar Avatar;
		public HandsManager HandsManager;

		[Space(12)]

		public FollowCursor Look = new FollowCursor(CursorType.Look);

		[Header("Left Controller")]

		public OculusTouchCursor LeftPalm = new OculusTouchCursor(CursorType.LeftPalm) {
			LocalPosition = new Vector3(0, 0.01f, 0),
			LocalRotation = new Vector3(-90, 0, 180),
			CursorSizeInput = OculusTouchCursor.InputSourceType.ThumbstickX,
			MinSize = 0.04f,
			MaxSize = 0.06f
		};

		public OculusTouchCursor LeftThumb = new OculusTouchCursor(CursorType.LeftThumb) {
			LocalPosition = new Vector3(0, 0, -0.09f),
			LocalRotation = new Vector3(-90, 0, 0),
			ShouldFollowAvatarElement = true,
			AvatarElementName = "hands:b_l_thumb_ignore"
		};

		public OculusTouchCursor LeftIndex = new OculusTouchCursor(CursorType.LeftIndex) {
			LocalPosition = new Vector3(-0.06f, 0, 0.05f),
			LocalRotation = new Vector3(-90, -40, 180),
			ShouldFollowAvatarElement = true,
			AvatarElementName = "hands:b_l_index_ignore"
		};

		public OculusTouchCursor LeftMiddle = new OculusTouchCursor(CursorType.LeftMiddle) {
			LocalPosition = new Vector3(0, 0, 0.08f),
			LocalRotation = new Vector3(-90, 0, 180)
		};

		public OculusTouchCursor LeftRing = new OculusTouchCursor(CursorType.LeftRing) {
			LocalPosition = new Vector3(0.06f, 0, 0.02f),
			LocalRotation = new Vector3(-90, 40, 180)
		};

		public OculusTouchCursor LeftPinky = new OculusTouchCursor(CursorType.LeftPinky) {
			LocalPosition = new Vector3(0.05f, 0, -0.05f),
			LocalRotation = new Vector3(-90, -180, 80),
			TriggerStrengthInput = OculusTouchCursor.InputSourceType.ThumbstickLeft //for Hovercast
		};

		[Header("Right Controller")]

		public OculusTouchCursor RightPalm = new OculusTouchCursor(CursorType.RightPalm) {
			LocalPosition = new Vector3(0, 0.01f, 0),
			LocalRotation = new Vector3(-90, 0, 180),
			CursorSizeInput = OculusTouchCursor.InputSourceType.ThumbstickX,
			MinSize = 0.04f,
			MaxSize = 0.06f
		};

		public OculusTouchCursor RightThumb = new OculusTouchCursor(CursorType.RightThumb) {
			LocalPosition = new Vector3(0, 0, -0.09f),
			LocalRotation = new Vector3(-90, 0, 0),
			ShouldFollowAvatarElement = true,
			AvatarElementName = "hands:b_r_thumb_ignore"
		};

		public OculusTouchCursor RightIndex = new OculusTouchCursor(CursorType.RightIndex) {
			LocalPosition = new Vector3(0.06f, 0, 0.05f),
			LocalRotation = new Vector3(-90, 40, 180),
			ShouldFollowAvatarElement = true,
			AvatarElementName = "hands:b_r_index_ignore"
		};

		public OculusTouchCursor RightMiddle = new OculusTouchCursor(CursorType.RightMiddle) {
			LocalPosition = new Vector3(0, 0, 0.08f),
			LocalRotation = new Vector3(-90, 0, 180)
		};

		public OculusTouchCursor RightRing = new OculusTouchCursor(CursorType.RightRing) {
			LocalPosition = new Vector3(-0.06f, 0, 0.02f),
			LocalRotation = new Vector3(-90, -40, 180)
		};

		public OculusTouchCursor RightPinky = new OculusTouchCursor(CursorType.RightPinky) {
			LocalPosition = new Vector3(-0.05f, 0, -0.05f),
			LocalRotation = new Vector3(-90, 180, -80),
			TriggerStrengthInput = OculusTouchCursor.InputSourceType.ThumbstickRight //for Hovercast
		};

		[Header("Hands")]

		public bool RequireHighHandConfidence = true;

		[Range(0, 0.04f)]
		public float ExtendFingertipDistance = 0;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			CursorUtil.FindCursorReference(this, ref CursorDataProvider, false);

			if ( Avatar == null ) {
				Avatar = FindObjectOfType<OvrAvatar>();
			}

			if ( HandsManager == null ) {
				HandsManager = FindObjectOfType<HandsManager>();
			}

			if ( Look.FollowTransform == null ) {
				Look.FollowTransform = Camera.main.transform;
			}

			LeftPalm.OriginTransform = transform;
			LeftThumb.OriginTransform = transform;
			LeftIndex.OriginTransform = transform;
			LeftMiddle.OriginTransform = transform;
			LeftRing.OriginTransform = transform;
			LeftPinky.OriginTransform = transform;

			RightPalm.OriginTransform = transform;
			RightThumb.OriginTransform = transform;
			RightIndex.OriginTransform = transform;
			RightMiddle.OriginTransform = transform;
			RightRing.OriginTransform = transform;
			RightPinky.OriginTransform = transform;
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

			if ( HandsManager == null || 
					(!HandsManager.LeftHand.IsTracked && !HandsManager.RightHand.IsTracked) ) {
				UpdateCursorsWithControllers();
			}
			else {
				UpdateCursorsWithHand(HandsManager.LeftHand, HandsManager.LeftHandSkeleton);
				UpdateCursorsWithHand(HandsManager.RightHand, HandsManager.RightHandSkeleton);
			}

			Look.UpdateData(CursorDataProvider);
			CursorDataProvider.ActivateAllCursorsBasedOnUsage();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateCursorsWithHand(OVRHand pHand, OVRSkeleton pSkeleton) {
			if ( !pHand.IsTracked || !pHand.IsDataValid ) {
				return;
			}

			if ( RequireHighHandConfidence && !pHand.IsDataHighConfidence ) {
				return;
			}

			UpdateCursorsWithPalm(pHand, pSkeleton);

			UpdateCursorsWithFinger(pHand, pSkeleton,
				OVRHand.HandFinger.Thumb, OVRSkeleton.BoneId.Hand_ThumbTip);
			UpdateCursorsWithFinger(pHand, pSkeleton,
				OVRHand.HandFinger.Index, OVRSkeleton.BoneId.Hand_IndexTip);
			UpdateCursorsWithFinger(pHand, pSkeleton,
				OVRHand.HandFinger.Middle, OVRSkeleton.BoneId.Hand_MiddleTip);
			UpdateCursorsWithFinger(pHand, pSkeleton,
				OVRHand.HandFinger.Ring, OVRSkeleton.BoneId.Hand_RingTip);
			UpdateCursorsWithFinger(pHand, pSkeleton,
				OVRHand.HandFinger.Pinky, OVRSkeleton.BoneId.Hand_PinkyTip);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateCursorsWithPalm(OVRHand pHand, OVRSkeleton pSkeleton) {
			bool isLeft = (pSkeleton.GetSkeletonType() == OVRSkeleton.SkeletonType.HandLeft);
			CursorType cursorType = (isLeft ? CursorType.LeftPalm : CursorType.RightPalm);

			if ( !CursorDataProvider.HasCursorData(cursorType) ) {
				return;
			}

			const int boneI = (int)OVRSkeleton.BoneId.Hand_Middle1;

			if ( boneI >= pSkeleton.Bones.Count ) {
				return;
			}

			OVRBone bone = pSkeleton.Bones[boneI];
			Vector3 palmPos = Vector3.Lerp(pSkeleton.transform.position, bone.Transform.position, 0.7f);

			ICursorDataForInput data = CursorDataProvider.GetCursorDataForInput(cursorType);
			data.SetWorldPosition(palmPos);
			data.SetWorldRotation(pSkeleton.transform.rotation*
				(isLeft ? PalmRotationFixL : PalmRotationFixR));
			data.SetSize(pHand.HandScale*0.025f);
			data.SetTriggerStrength(pHand.GetFingerPinchStrength(OVRHand.HandFinger.Index));
			data.SetUsedByInput(true);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateCursorsWithFinger(OVRHand pHand, OVRSkeleton pSkeleton, 
										OVRHand.HandFinger pFingerType, OVRSkeleton.BoneId pBoneId) {
			bool isLeft = (pSkeleton.GetSkeletonType() == OVRSkeleton.SkeletonType.HandLeft);
			CursorType cursorType = GetFingerCursorType(isLeft, pFingerType);

			if ( !CursorDataProvider.HasCursorData(cursorType) ) {
				return;
			}

			int boneI = (int)pBoneId;

			if ( boneI >= pSkeleton.Bones.Count ) {
				return;
			}

			OVRBone bone = pSkeleton.Bones[boneI];
			Vector3 extendedWorldPos;

			if ( ExtendFingertipDistance == 0 ) {
				extendedWorldPos = bone.Transform.position;
			}
			else {
				extendedWorldPos = bone.Transform.TransformPoint(
					new Vector3(ExtendFingertipDistance*(isLeft ? -1 : 1), 0, 0));
			}

			ICursorDataForInput data = CursorDataProvider.GetCursorDataForInput(cursorType);
			data.SetWorldPosition(extendedWorldPos);
			data.SetWorldRotation(bone.Transform.rotation*
				(isLeft ? FingerRotationFixL : FingerRotationFixR));
			data.SetSize(pHand.HandScale*GetFingerCursorSize(pFingerType));
			data.SetTriggerStrength(pHand.GetFingerPinchStrength(pFingerType));
			data.SetUsedByInput(true);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static CursorType GetFingerCursorType(bool pIsLeft, OVRHand.HandFinger pFingerType) {
			switch ( pFingerType ) {
				case OVRHand.HandFinger.Thumb:
					return (pIsLeft ? CursorType.LeftThumb : CursorType.RightThumb);
					
				case OVRHand.HandFinger.Index:
					return (pIsLeft ? CursorType.LeftIndex : CursorType.RightIndex);
					
				case OVRHand.HandFinger.Middle:
					return (pIsLeft ? CursorType.LeftMiddle : CursorType.RightMiddle);
					
				case OVRHand.HandFinger.Ring:
					return (pIsLeft ? CursorType.LeftRing : CursorType.RightRing);
					
				case OVRHand.HandFinger.Pinky:
					return (pIsLeft ? CursorType.LeftPinky : CursorType.RightPinky);
			}

			throw new Exception("Unhandled cursor combination: "+pIsLeft+" / "+pFingerType);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static float GetFingerCursorSize(OVRHand.HandFinger pFingerType) {
			switch ( pFingerType ) {
				case OVRHand.HandFinger.Thumb:
					return 0.012f;
					
				case OVRHand.HandFinger.Index:
				case OVRHand.HandFinger.Middle:
				case OVRHand.HandFinger.Ring:
					return 0.01f;
					
				case OVRHand.HandFinger.Pinky:
					return 0.008f;
			}

			throw new Exception("Unhandled finger type: "+pFingerType);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Transform FindAvatarTransform(Transform pParentTx, string pName) {
			//TODO: find a better way to obtain the "hand bones"

			foreach ( Transform childTx in pParentTx ) {
				if ( childTx.name == pName ) {
					return childTx;
				}

				Transform foundTx = FindAvatarTransform(childTx, pName);

				if ( foundTx != null ) {
					return foundTx;
				}
			}

			return null;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateCursorsWithControllers() {
			StateLeft = GetControllerState(OVRInput.Controller.LTouch);
			StateRight = GetControllerState(OVRInput.Controller.RTouch);

			LeftPalm.UpdateData(CursorDataProvider, StateLeft, Avatar);
			LeftThumb.UpdateData(CursorDataProvider, StateLeft, Avatar);
			LeftIndex.UpdateData(CursorDataProvider, StateLeft, Avatar);
			LeftMiddle.UpdateData(CursorDataProvider, StateLeft, Avatar);
			LeftRing.UpdateData(CursorDataProvider, StateLeft, Avatar);
			LeftPinky.UpdateData(CursorDataProvider, StateLeft, Avatar);

			RightPalm.UpdateData(CursorDataProvider, StateRight, Avatar);
			RightThumb.UpdateData(CursorDataProvider, StateRight, Avatar);
			RightIndex.UpdateData(CursorDataProvider, StateRight, Avatar);
			RightMiddle.UpdateData(CursorDataProvider, StateRight, Avatar);
			RightRing.UpdateData(CursorDataProvider, StateRight, Avatar);
			RightPinky.UpdateData(CursorDataProvider, StateRight, Avatar);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual ControlState GetControllerState(OVRInput.Controller pType) {
			OVRInput.Controller contTypes = OVRInput.GetConnectedControllers();
			bool isValid = ((contTypes & pType) != 0);

			var state = new ControlState();
			state.ControllerType = pType;
			state.IsValid = isValid;

			if ( !state.IsValid ) {
				return state;
			}

			state.LocalPos = OVRInput.GetLocalControllerPosition(pType);
			state.LocalRot = OVRInput.GetLocalControllerRotation(pType);
			state.ThumbstickAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, pType);
			state.IndexTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, pType);
			state.HandTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, pType);
			state.Button1Press = OVRInput.Get(OVRInput.Button.One, pType);
			state.Button2Press = OVRInput.Get(OVRInput.Button.Two, pType);
			state.StartPress = OVRInput.Get(OVRInput.Button.Start, pType);
			state.ThumbstickPress = OVRInput.Get(OVRInput.Button.PrimaryThumbstick, pType);

			return state;
		}

	}

}

#else

using Hover.Core.Utils;

namespace Hover.InputModules.OculusTouch {

	/*================================================================================================*/
	public class HoverInputOculusTouch : HoverInputMissing {

		public override string ModuleName { get { return "OculusTouch"; } }
		public override string RequiredSymbol { get { return "HOVER_INPUT_OCULUSTOUCH"; } }

	}

}

#endif //HOVER_INPUT_OCULUSTOUCH
