using Hover.Core.Renderers;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Items.Helpers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverItemRendererUpdater))]
	public class HoverIndicatorOverrider : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public const string MinHightlightProgressName = "MinHightlightProgress";
		public const string MinSelectionProgressName = "MinSelectionProgress";

		public ISettingsControllerMap Controllers { get; private set; }
		
		[DisableWhenControlled(DisplaySpecials=true, RangeMin=0, RangeMax=1)]
		public float MinHightlightProgress = 0;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float MinSelectionProgress = 0;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverIndicatorOverrider() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
			HoverItemRendererUpdater rendUp = GetComponent<HoverItemRendererUpdater>();
			HoverIndicator rendInd = rendUp.ActiveRenderer.GetIndicator();

			rendInd.Controllers.Set(HoverIndicator.HighlightProgressName, this);
			rendInd.Controllers.Set(HoverIndicator.SelectionProgressName, this);

			rendInd.HighlightProgress = Mathf.Max(rendInd.HighlightProgress, MinHightlightProgress);
			rendInd.SelectionProgress = Mathf.Max(rendInd.SelectionProgress, MinSelectionProgress);

			Controllers.TryExpireControllers();
		}

	}

}
