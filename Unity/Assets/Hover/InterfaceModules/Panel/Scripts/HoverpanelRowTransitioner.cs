using System.Diagnostics;
using Hover.Core.Layouts.Rect;
using Hover.Core.Renderers.Shapes.Rect;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.InterfaceModules.Panel {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverShapeRect))]
	[RequireComponent(typeof(HoverpanelInterface))]
	public class HoverpanelRowTransitioner : TreeUpdateableBehavior, ISettingsController {

		private static readonly Quaternion RotatePosX = Quaternion.Euler( 180, 0, 0);
		private static readonly Quaternion RotateNegX = Quaternion.Euler(-180, 0, 0);
		private static readonly Quaternion RotatePosY = Quaternion.Euler(0,  180, 0);
		private static readonly Quaternion RotateNegY = Quaternion.Euler(0, -180, 0);

		public bool IsTransitionActive { get; private set; }
		public float TransitionProgressCurved { get; private set; }

		[SerializeField]
		[FormerlySerializedAs("DepthDistance")]
		private float _DepthDistance = 0.1f;

		[SerializeField]
		[Range(0, 1)]
		[FormerlySerializedAs("TransitionProgress")]
		private float _TransitionProgress = 1;

		[SerializeField]
		[Range(0.1f, 10)]
		[FormerlySerializedAs("TransitionExponent")]
		private float _TransitionExponent = 2;

		[SerializeField]
		[Range(1, 10000)]
		[FormerlySerializedAs("TransitionMilliseconds")]
		private float _TransitionMilliseconds = 1000;

		[SerializeField]
		[FormerlySerializedAs("RowEntryTransition")]
		private HoverpanelRowSwitchingInfo.RowEntryType _RowEntryTransition;

		private Stopwatch vTimer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float DepthDistance {
			get => _DepthDistance;
			set => this.UpdateValueWithTreeMessage(ref _DepthDistance, value, "DepthDistance");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float TransitionProgress {
			get => _TransitionProgress;
			set => this.UpdateValueWithTreeMessage(ref _TransitionProgress, value, "TransProgress");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float TransitionExponent {
			get => _TransitionExponent;
			set => this.UpdateValueWithTreeMessage(ref _TransitionExponent, value, "TransExponent");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float TransitionMilliseconds {
			get => _TransitionMilliseconds;
			set => this.UpdateValueWithTreeMessage(ref _TransitionMilliseconds, value, "TransMs");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverpanelRowSwitchingInfo.RowEntryType RowEntryTransition {
			get => _RowEntryTransition;
			set => this.UpdateValueWithTreeMessage(ref _RowEntryTransition, value, "RowEntryTrans");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Start() {
			base.Start();

			HoverpanelInterface panel = GetComponent<HoverpanelInterface>();

			foreach ( Transform childTx in panel.transform ) {
				HoverLayoutRectRow row = childTx.GetComponent<HoverLayoutRectRow>();

				if ( row != null && row != panel.ActiveRow ) {
					childTx.gameObject.SetActive(false);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
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
