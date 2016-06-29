using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverIndicator : MonoBehaviour, ITreeUpdateable {
		
		public const string HighlightProgressName = "HighlightProgress";
		public const string SelectionProgressName = "SelectionProgress";

		public ISettingsControllerMap Controllers { get; private set; }
		public bool DidSettingsChange { get; protected set; }

		[DisableWhenControlled(RangeMin=0, RangeMax=1, DisplayMessage=true)]
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
			DidSettingsChange = (
				HighlightProgress != vPrevHigh ||
				SelectionProgress != vPrevSel
			);

			vPrevHigh = HighlightProgress;
			vPrevSel = SelectionProgress;
		}

	}

}
