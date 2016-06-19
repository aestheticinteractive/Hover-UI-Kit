using System.Diagnostics;
using Hover.Layouts.Arc;
using Hover.Utils;
using UnityEngine;

namespace Hover.Interfaces.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HovercastInterface))]
	public class HovercastRowTransitioner : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public float TransitionProgressCurved { get; private set; }

		public float RowThickness = 0.06f;
		public float InnerRadius = 0.12f;

		[Range(0, 1)]
		public float TransitionProgress = 1;
		
		[Range(0.1f, 10)]
		public float TransitionExponent = 2;

		[Range(1, 10000)]
		public float TransitionMilliseconds = 1000;

		public HovercastRowSwitcher.RowEntryType RowEntryTransition;

		private Stopwatch vTimer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			HovercastInterface cast = GetComponent<HovercastInterface>();

			foreach ( Transform childTx in cast.RowContainer ) {
				HoverLayoutArcRow row = childTx.GetComponent<HoverLayoutArcRow>();

				if ( row != null && row != cast.ActiveRow ) {
					childTx.gameObject.SetActive(false);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			gameObject.GetComponent<HovercastInterface>()
				.OnRowTransitionEvent.AddListener(HandleTransitionEvent);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			RowThickness = Mathf.Max(0, RowThickness);
			InnerRadius = Mathf.Max(0, InnerRadius);

			UpdateTimedProgress();
			UpdateRows();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleTransitionEvent(HovercastRowSwitcher.RowEntryType pEntryType) {
			RowEntryTransition = pEntryType;

			if ( pEntryType == HovercastRowSwitcher.RowEntryType.Immediate ) {
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
		private void UpdateTimedProgress() {
			if ( vTimer == null ) {
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
			bool isTransitionDone = (TransitionProgress >= 1 || !hasPrevRow);
			float outerRadius = InnerRadius+RowThickness;
			float scaleFactor = outerRadius/InnerRadius;
			float activeScale = 1;
			float prevScale = 1;

			TransitionProgressCurved = 1-Mathf.Pow(1-TransitionProgress, TransitionExponent);

			cast.ActiveRow.Controllers.Set("gameObject.activeSelf", this);
			cast.ActiveRow.Controllers.Set("transform.localScale", this);
			cast.ActiveRow.Controllers.Set(HoverLayoutArcRow.OuterRadiusName, this);
			cast.ActiveRow.Controllers.Set(HoverLayoutArcRow.InnerRadiusName, this);

			cast.ActiveRow.InnerRadius = InnerRadius;
			cast.ActiveRow.OuterRadius = outerRadius;
			cast.ActiveRow.gameObject.SetActive(true);

			if ( hasPrevRow ) {
				cast.PreviousRow.Controllers.Set("gameObject.activeSelf", this);
				cast.PreviousRow.Controllers.Set("transform.localScale", this);
				cast.PreviousRow.Controllers.Set(HoverLayoutArcRow.OuterRadiusName, this);
				cast.PreviousRow.Controllers.Set(HoverLayoutArcRow.InnerRadiusName, this);

				cast.PreviousRow.InnerRadius = InnerRadius;
				cast.PreviousRow.OuterRadius = outerRadius;
				cast.PreviousRow.gameObject.SetActive(!isTransitionDone);
			}

			if ( !isTransitionDone ) {
				switch ( RowEntryTransition ) {
					case HovercastRowSwitcher.RowEntryType.FromInside:
						activeScale = Mathf.Lerp(1/scaleFactor, 1, TransitionProgressCurved);
						prevScale = Mathf.Lerp(1, scaleFactor, TransitionProgressCurved);
						break;
						
				case HovercastRowSwitcher.RowEntryType.FromOutside:
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
