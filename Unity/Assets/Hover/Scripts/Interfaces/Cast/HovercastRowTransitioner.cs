using System.Diagnostics;
using Hover.Layouts.Arc;
using Hover.Utils;
using UnityEngine;

namespace Hover.Interfaces.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HovercastInterface))]
	public class HovercastRowTransitioner : MonoBehaviour, ITreeUpdateable {

		public float TransitionProgressCurved { get; private set; }

		public float RowThickness = 0.06f;
		public float InnerRadius = 0.12f;

		[Range(0, 1)]
		public float TransitionProgress = 1;
		
		[Range(0.1f, 10)]
		public float TransitionExponent = 3;

		[Range(1, 10000)]
		public float TransitionMilliseconds = 1000;

		public HovercastRowSwitcher.RowEntryType RowEntryTransition;

		private Stopwatch vTimer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			GetComponent<HovercastInterface>().OnRowTransitionEvent.AddListener(HandleTransitionEvent);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
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
			float radOffset = 0;
			int childOrder = 0;

			TransitionProgressCurved = 1-Mathf.Pow(1-TransitionProgress, TransitionExponent);

			cast.ArcStack.InnerRadius = InnerRadius;
			cast.ArcStack.OuterRadius = InnerRadius + RowThickness*(isTransitionDone ? 1 : 2);
			cast.ActiveRow.gameObject.SetActive(true);

			if ( hasPrevRow ) {
				cast.PreviousRow.gameObject.SetActive(!isTransitionDone); //before "childOrder"
			}

			cast.ArcStack.DisableAllChildrenExcept(cast.ActiveRow, cast.PreviousRow);

			if ( !isTransitionDone ) {
				childOrder = cast.ArcStack.GetChildOrder(cast.PreviousRow, cast.ActiveRow);
			}

			switch ( RowEntryTransition ) {
				case HovercastRowSwitcher.RowEntryType.Immediate:
					break;

				case HovercastRowSwitcher.RowEntryType.FromInside:
					radOffset = (isTransitionDone ? 0 : TransitionProgressCurved-1);
					cast.ArcStack.Arrangement = (childOrder > 0 ?
						HoverLayoutArcStack.ArrangementType.InnerToOuter :
						HoverLayoutArcStack.ArrangementType.OuterToInner);
					break;
					
				case HovercastRowSwitcher.RowEntryType.FromOutside:
					radOffset = (isTransitionDone ? 0 : -TransitionProgressCurved);
					cast.ArcStack.Arrangement = (childOrder > 0 ?
						HoverLayoutArcStack.ArrangementType.OuterToInner :
						HoverLayoutArcStack.ArrangementType.InnerToOuter);
					break;
			}

			if ( hasPrevRow ) {
				HoverLayoutArcRelativeSizer prevSizer = GetRelativeSizer(cast.PreviousRow);
				prevSizer.RelativeRadiusOffset = radOffset;
				//prevSizer.RelativeArcAngle = Mathf.Lerp(1, 0, TransitionProgressCurved);
			}

			HoverLayoutArcRelativeSizer activeSizer = GetRelativeSizer(cast.ActiveRow);
			activeSizer.RelativeRadiusOffset = radOffset;
			//activeSizer.RelativeArcAngle = Mathf.Lerp(0, 1, TransitionProgressCurved);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private HoverLayoutArcRelativeSizer GetRelativeSizer(HoverLayoutArcRow pRow) {
			HoverLayoutArcRelativeSizer sizer = 				
				pRow.gameObject.GetComponent<HoverLayoutArcRelativeSizer>();
			return (sizer ? sizer : pRow.gameObject.AddComponent<HoverLayoutArcRelativeSizer>());
		}

	}

}
