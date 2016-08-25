using System;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverIndicator : MonoBehaviour, ITreeUpdateable, ISettingsController {
		
		public const string HighlightProgressName = "HighlightProgress";
		public const string SelectionProgressName = "SelectionProgress";

		public ISettingsControllerMap Controllers { get; private set; }
		public bool DidSettingsChange { get; protected set; }
		public DateTime LatestSelectionTime { get; set; }

		[DisableWhenControlled(RangeMin=0, RangeMax=1, DisplaySpecials=true)]
		public float HighlightProgress = 0.7f;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float SelectionProgress = 0.2f;

		private float vPrevHigh;
		private float vPrevSel;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverIndicator() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			SelectionProgress = Mathf.Min(SelectionProgress, HighlightProgress);

			DidSettingsChange = (
				HighlightProgress != vPrevHigh ||
				SelectionProgress != vPrevSel
			);

			UpdateIndicatorChildren();

			vPrevHigh = HighlightProgress;
			vPrevSel = SelectionProgress;

			Controllers.TryExpireControllers();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateIndicatorChildren() {
			TreeUpdater tree = GetComponent<TreeUpdater>();

			for ( int i = 0 ; i < tree.TreeChildrenThisFrame.Count ; i++ ) {
				TreeUpdater child = tree.TreeChildrenThisFrame[i];
				HoverIndicator childInd = child.GetComponent<HoverIndicator>();

				if ( childInd == null ) {
					continue;
				}

				childInd.Controllers.Set(HighlightProgressName, this);
				childInd.Controllers.Set(SelectionProgressName, this);

				childInd.HighlightProgress = HighlightProgress;
				childInd.SelectionProgress = SelectionProgress;
				childInd.LatestSelectionTime = LatestSelectionTime;
			}
		}

	}

}
