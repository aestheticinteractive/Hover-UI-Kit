using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverIndicator : MonoBehaviour, ITreeUpdateable {

		public ISettingsControllerMap Controllers { get; private set; }
		public bool DidSettingsChange { get; protected set; }

		[DisableWhenControlled(RangeMin=0, RangeMax=1, DisplayMessage=true)]
		public float HighlightProgress = 0.1f;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float SelectionProgress = 0.04f;

		private float vPrevHigh;
		private float vPrevSel;


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
