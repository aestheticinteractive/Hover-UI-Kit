#if HOVER_INPUT_OCULUSTOUCH

using System;
using Hover.Core.Cursors;
using UnityEngine;

namespace Hover.InputModules.OculusTouch {

	/*================================================================================================*/
	[Serializable]
	public class OculusTouchCursor {

		public static float IndexTriggerMax = 0.999f;
		public static float HandTriggerMax = 0.998f;
		public static float ThumbstickMax = 0.95f;

		private static readonly Vector3 LocalPosFix = new Vector3(0, -0.008f, 0);
		private static readonly Quaternion LocalLeftRotFix = Quaternion.Euler(90, 0, 90);
		private static readonly Quaternion LocalRightRotFix = Quaternion.Euler(-90, 0, -90);

		public enum InputSourceType {
			None,
			IndexTrigger,
			HandTrigger,
			Button1Press,
			Button2Press,
			StartPress,
			ThumbstickPress,
			ThumbstickY,
			ThumbstickUp,
			ThumbstickDown,
			ThumbstickX,
			ThumbstickLeft,
			ThumbstickRight
		}

		public CursorType Type { get; private set; }
		public Transform OriginTransform { get; internal set; }
		public Transform AvatarElementTransform { get; private set; }

		public Vector3 LocalPosition = Vector3.zero;
		public Vector3 LocalRotation = Vector3.zero;
		public bool ShouldFollowAvatarElement = false;
		public string AvatarElementName = null;
		public InputSourceType TriggerStrengthInput = InputSourceType.IndexTrigger;
		public InputSourceType CursorSizeInput = InputSourceType.None;

		[Range(0.01f, 0.1f)]
		public float MinSize = 0.01f;

		[Range(0.02f, 0.2f)]
		public float MaxSize = 0.03f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public OculusTouchCursor(CursorType pType) {
			Type = pType;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateData(HoverCursorDataProvider pCursorDataProv,
										HoverInputOculusTouch.ControlState pState, OvrAvatar pAvatar) {
			ICursorDataForInput data = GetData(pCursorDataProv);

			if ( data == null ) {
				return;
			}

			data.SetUsedByInput(pState.IsValid);

			if ( !pState.IsValid ) {
				return;
			}

			if ( !TryUpdateDataWithAvatarElement(data, pAvatar) ) {
				UpdateDataWithLocalOffsets(data, pState);
			}

			UpdateDataForTrigger(data, pState);
			UpdateDataForSize(data, pState);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private bool TryUpdateDataWithAvatarElement(ICursorDataForInput pData, OvrAvatar pAvatar) {
			if ( !ShouldFollowAvatarElement ) {
				AvatarElementTransform = null;
				return false;
			}

			if ( AvatarElementTransform == null ) {
				AvatarElementTransform = HoverInputOculusTouch.FindAvatarTransform(
					pAvatar.transform, AvatarElementName);
			}

			if ( AvatarElementTransform == null ) {
				return false;
			}

			Quaternion sidedLocalRotFix = (Type < CursorType.RightPalm ?
				LocalLeftRotFix : LocalRightRotFix);

			pData.SetWorldRotation(AvatarElementTransform.rotation*sidedLocalRotFix);
			pData.SetWorldPosition(AvatarElementTransform.position+pData.WorldRotation*LocalPosFix);
			return true;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateDataWithLocalOffsets(ICursorDataForInput pData, 
															HoverInputOculusTouch.ControlState pState) {
			pData.SetWorldPosition(
				OriginTransform.TransformPoint(pState.LocalPos)+pState.LocalRot*LocalPosition);
			pData.SetWorldRotation(
				OriginTransform.rotation*pState.LocalRot*Quaternion.Euler(LocalRotation));
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateDataForTrigger(ICursorDataForInput pData,
															HoverInputOculusTouch.ControlState pState) {
			float prog = GetInputSourceProgress(TriggerStrengthInput, pState, 0);
			pData.SetTriggerStrength(prog);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateDataForSize(ICursorDataForInput pData,
															HoverInputOculusTouch.ControlState pState) {
			float prog = GetInputSourceProgress(CursorSizeInput, pState, 0.5f);
			pData.SetSize(Mathf.Lerp(MinSize, MaxSize, prog));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private ICursorDataForInput GetData(HoverCursorDataProvider pCursorDataProv) {
			if ( !pCursorDataProv.HasCursorData(Type) ) {
				return null;
			}

			return pCursorDataProv.GetCursorDataForInput(Type);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private float GetInputSourceProgress(InputSourceType pInputSourceType,
											HoverInputOculusTouch.ControlState pState, float pDefault) {
			switch ( pInputSourceType ) {
				case InputSourceType.IndexTrigger:
					return Mathf.InverseLerp(0, IndexTriggerMax, pState.IndexTrigger);

				case InputSourceType.HandTrigger:
					return Mathf.InverseLerp(0, HandTriggerMax, pState.HandTrigger);

				case InputSourceType.Button1Press:
					return (pState.Button1Press ? 1 : 0);

				case InputSourceType.Button2Press:
					return (pState.Button2Press ? 1 : 0);

				case InputSourceType.StartPress:
					return (pState.StartPress ? 1 : 0);

				case InputSourceType.ThumbstickPress:
					return (pState.ThumbstickPress ? 1 : 0);

				case InputSourceType.ThumbstickY:
					return Mathf.InverseLerp(-ThumbstickMax, ThumbstickMax, pState.ThumbstickAxis.y);

				case InputSourceType.ThumbstickUp:
					return Mathf.InverseLerp(0, ThumbstickMax, pState.ThumbstickAxis.y);

				case InputSourceType.ThumbstickDown:
					return Mathf.InverseLerp(0, -ThumbstickMax, pState.ThumbstickAxis.y);

				case InputSourceType.ThumbstickX:
					return Mathf.InverseLerp(-ThumbstickMax, ThumbstickMax, pState.ThumbstickAxis.x);

				case InputSourceType.ThumbstickLeft:
					return Mathf.InverseLerp(0, -ThumbstickMax, pState.ThumbstickAxis.x);

				case InputSourceType.ThumbstickRight:
					return Mathf.InverseLerp(0, ThumbstickMax, pState.ThumbstickAxis.x);
			}

			return pDefault;
		}

	}

}

#endif //HOVER_INPUT_OCULUSTOUCH
