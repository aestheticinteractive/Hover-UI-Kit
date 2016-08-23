using System.Diagnostics;
using Hover.Core.Layouts.Rect;
using Hover.Core.Renderers.Shapes.Rect;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.InterfaceModules.Panel {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverShapeRect))]
	[RequireComponent(typeof(HoverpanelInterface))]
	public class HoverpanelRowTransitioner : MonoBehaviour, ITreeUpdateable, ISettingsController {

		private static readonly Quaternion RotatePosX = Quaternion.Euler( 180, 0, 0);
		private static readonly Quaternion RotateNegX = Quaternion.Euler(-180, 0, 0);
		private static readonly Quaternion RotatePosY = Quaternion.Euler(0,  180, 0);
		private static readonly Quaternion RotateNegY = Quaternion.Euler(0, -180, 0);

		public bool IsTransitionActive { get; private set; }
		public float TransitionProgressCurved { get; private set; }

		public float DepthDistance = 0.1f;

		[Range(0, 1)]
		public float TransitionProgress = 1;
		
		[Range(0.1f, 10)]
		public float TransitionExponent = 2;

		[Range(1, 10000)]
		public float TransitionMilliseconds = 1000;

		public HoverpanelRowSwitchingInfo.RowEntryType RowEntryTransition;

		private Stopwatch vTimer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			HoverpanelInterface panel = GetComponent<HoverpanelInterface>();

			foreach ( Transform childTx in panel.transform ) {
				HoverLayoutRectRow row = childTx.GetComponent<HoverLayoutRectRow>();

				if ( row != null && row != panel.ActiveRow ) {
					childTx.gameObject.SetActive(false);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			UpdateSettings();
			UpdateTimedProgress();
			UpdateRows();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnRowSwitched(HoverpanelRowSwitchingInfo.RowEntryType pEntryType) {
			IsTransitionActive = true;
			RowEntryTransition = pEntryType;

			if ( pEntryType == HoverpanelRowSwitchingInfo.RowEntryType.Immediate ) {
				TransitionProgress = 1;
				vTimer = null;
			}
			else {
				TransitionProgress = 0;
				vTimer = Stopwatch.StartNew();
			}

			TreeUpdate();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSettings() {
			DepthDistance = Mathf.Max(0, DepthDistance);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateTimedProgress() {
			if ( vTimer == null ) {
				IsTransitionActive = false;
				return;
			}

			TransitionProgress = (float)vTimer.Elapsed.TotalMilliseconds/TransitionMilliseconds;

			if ( TransitionProgress >= 1 ) {
				TransitionProgress = 1;
				vTimer = null;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateRows() {
			HoverpanelInterface panel = GetComponent<HoverpanelInterface>();
			HoverShapeRect shape = GetComponent<HoverShapeRect>();
			bool isTransitionDone = (TransitionProgress >= 1);
			var posScale = new Vector3(shape.SizeX, shape.SizeY, DepthDistance);
			Vector3 activeFromLocalPos = Vector3.zero;
			Vector3 prevToLocalPos = Vector3.zero;
			Quaternion activeFromLocalRot = Quaternion.identity;
			Quaternion prevToLocalRot = Quaternion.identity;

			TransitionProgressCurved = 1-Mathf.Pow(1-TransitionProgress, TransitionExponent);

			if ( !isTransitionDone ) {
				switch ( RowEntryTransition ) {
					case HoverpanelRowSwitchingInfo.RowEntryType.SlideFromTop:
						activeFromLocalPos = Vector3.up;
						prevToLocalPos = Vector3.down;
						break;

					case HoverpanelRowSwitchingInfo.RowEntryType.SlideFromBottom:
						activeFromLocalPos = Vector3.down;
						prevToLocalPos = Vector3.up;
						break;

					case HoverpanelRowSwitchingInfo.RowEntryType.SlideFromLeft:
						activeFromLocalPos = Vector3.left;
						prevToLocalPos = Vector3.right;
						break;

					case HoverpanelRowSwitchingInfo.RowEntryType.SlideFromRight:
						activeFromLocalPos = Vector3.right;
						prevToLocalPos = Vector3.left;
						break;

					case HoverpanelRowSwitchingInfo.RowEntryType.SlideFromFront:
						activeFromLocalPos = Vector3.back;
						prevToLocalPos = Vector3.forward;
						break;

					case HoverpanelRowSwitchingInfo.RowEntryType.SlideFromBack:
						activeFromLocalPos = Vector3.forward;
						prevToLocalPos = Vector3.back;
						break;

					case HoverpanelRowSwitchingInfo.RowEntryType.RotateFromTop:
						activeFromLocalRot = RotateNegX;
						prevToLocalRot = RotatePosX;
						break;

					case HoverpanelRowSwitchingInfo.RowEntryType.RotateFromBottom:
						activeFromLocalRot = RotatePosX;
						prevToLocalRot = RotateNegX;
						break;

					case HoverpanelRowSwitchingInfo.RowEntryType.RotateFromLeft:
						activeFromLocalRot = RotateNegY;
						prevToLocalRot = RotatePosY;
						break;

					case HoverpanelRowSwitchingInfo.RowEntryType.RotateFromRight:
						activeFromLocalRot = RotatePosY;
						prevToLocalRot = RotateNegY;
						break;
				}
			}

			panel.ActiveRow.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
			panel.ActiveRow.Controllers.Set(SettingsControllerMap.TransformLocalPosition, this);
			panel.ActiveRow.Controllers.Set(SettingsControllerMap.TransformLocalRotation, this);

			panel.ActiveRow.gameObject.SetActive(true);
			activeFromLocalPos = Vector3.Scale(activeFromLocalPos, posScale);

			panel.ActiveRow.transform.localPosition = 
				Vector3.Lerp(activeFromLocalPos, Vector3.zero, TransitionProgressCurved);
			panel.ActiveRow.transform.localRotation =
				Quaternion.Slerp(activeFromLocalRot, Quaternion.identity, TransitionProgressCurved);

			if ( panel.PreviousRow == null ) {
				return;
			}

			panel.PreviousRow.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
			panel.PreviousRow.Controllers.Set(SettingsControllerMap.TransformLocalPosition, this);
			panel.PreviousRow.Controllers.Set(SettingsControllerMap.TransformLocalRotation, this);

			panel.PreviousRow.gameObject.SetActive(!isTransitionDone);
			prevToLocalPos = Vector3.Scale(prevToLocalPos, posScale);

			panel.PreviousRow.transform.localPosition =
				Vector3.Lerp(Vector3.zero, prevToLocalPos, TransitionProgressCurved);
			panel.PreviousRow.transform.localRotation =
				Quaternion.Slerp(Quaternion.identity, prevToLocalRot, TransitionProgressCurved);
		}

	}

}
