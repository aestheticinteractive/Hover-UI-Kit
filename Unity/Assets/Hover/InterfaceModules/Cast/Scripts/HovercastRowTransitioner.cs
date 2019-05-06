using System.Diagnostics;
using Hover.Core.Layouts.Arc;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.InterfaceModules.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HovercastInterface))]
	public class HovercastRowTransitioner : TreeUpdateableBehavior, ISettingsController {

		public bool IsTransitionActive { get; private set; }
		public float TransitionProgressCurved { get; private set; }

		[SerializeField]
		[FormerlySerializedAs("RowThickness")]
		private float _RowThickness = 0.06f;

		[SerializeField]
		[FormerlySerializedAs("InnerRadius")]
		private float _InnerRadius = 0.12f;

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
		private float _TransitionMilliseconds = 500;

		[SerializeField]
		[FormerlySerializedAs("RowEntryTransition")]
		private HovercastRowSwitchingInfo.RowEntryType _RowEntryTransition;

		private Stopwatch vTimer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float RowThickness {
			get => _RowThickness;
			set => this.UpdateValueWithTreeMessage(ref _RowThickness, value, "RowThickness");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float InnerRadius {
			get => _InnerRadius;
			set => this.UpdateValueWithTreeMessage(ref _InnerRadius, value, "InnerRadius");
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
		public HovercastRowSwitchingInfo.RowEntryType RowEntryTransition {
			get => _RowEntryTransition;
			set => this.UpdateValueWithTreeMessage(ref _RowEntryTransition, value, "RowEntryTrans");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Start() {
			base.Start();

			HovercastInterface cast = GetComponent<HovercastInterface>();

			foreach ( Transform childTx in cast.RowContainer ) {
				HoverLayoutArcRow row = childTx.GetComponent<HoverLayoutArcRow>();

				if ( row != null && row != cast.ActiveRow ) {
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
		public void OnRowSwitched(HovercastRowSwitchingInfo.RowEntryType pEntryType) {
			IsTransitionActive = true;
			RowEntryTransition = pEntryType;

			if ( pEntryType == HovercastRowSwitchingInfo.RowEntryType.Immediate ) {
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
			RowThickness = Mathf.Max(0, RowThickness);
			InnerRadius = Mathf.Max(0, InnerRadius);
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
			HovercastInterface cast = GetComponent<HovercastInterface>();
			bool hasPrevRow = (cast.PreviousRow != null);
			bool isTransitionDone = (TransitionProgress >= 1);
			float outerRadius = InnerRadius+RowThickness;
			float scaleFactor = outerRadius/InnerRadius;
			float activeScale = 1;
			float prevScale = 1;

			TransitionProgressCurved = 1-Mathf.Pow(1-TransitionProgress, TransitionExponent);

			cast.ActiveRow.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
			cast.ActiveRow.Controllers.Set(SettingsControllerMap.TransformLocalScale, this);
			cast.ActiveRow.Controllers.Set(HoverLayoutArcRow.OuterRadiusName, this);
			cast.ActiveRow.Controllers.Set(HoverLayoutArcRow.InnerRadiusName, this);

			cast.ActiveRow.InnerRadius = InnerRadius;
			cast.ActiveRow.OuterRadius = outerRadius;
			cast.ActiveRow.gameObject.SetActive(true);

			if ( hasPrevRow ) {
				cast.PreviousRow.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
				cast.PreviousRow.Controllers.Set(SettingsControllerMap.TransformLocalScale, this);
				cast.PreviousRow.Controllers.Set(HoverLayoutArcRow.OuterRadiusName, this);
				cast.PreviousRow.Controllers.Set(HoverLayoutArcRow.InnerRadiusName, this);

				cast.PreviousRow.InnerRadius = InnerRadius;
				cast.PreviousRow.OuterRadius = outerRadius;
				cast.PreviousRow.gameObject.SetActive(!isTransitionDone);
			}

			if ( !isTransitionDone ) {
				switch ( RowEntryTransition ) {
					case HovercastRowSwitchingInfo.RowEntryType.FromInside:
						activeScale = Mathf.Lerp(1/scaleFactor, 1, TransitionProgressCurved);
						prevScale = Mathf.Lerp(1, scaleFactor, TransitionProgressCurved);
						break;
						
					case HovercastRowSwitchingInfo.RowEntryType.FromOutside:
						activeScale = Mathf.Lerp(scaleFactor, 1, TransitionProgressCurved);
						prevScale = Mathf.Lerp(1, 1/scaleFactor, TransitionProgressCurved);
						break;
				}
			}

			cast.ActiveRow.transform.localScale = Vector3.one*activeScale;

			if ( hasPrevRow ) {
				cast.PreviousRow.transform.localScale = Vector3.one*prevScale;
			}
		}

	}

}
