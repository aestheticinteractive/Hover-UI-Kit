using Hover.Core.Renderers.Items;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverRendererItemUpdater))]
	public class HoverRendererIndicatorOverrider : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public const string MinHightlightProgressName = "MinHightlightProgress";
		public const string MinSelectionProgressName = "MinSelectionProgress";

		public ISettingsControllerMap Controllers { get; private set; }
		
		[DisableWhenControlled(DisplaySpecials=true, RangeMin=0, RangeMax=1)]
		public float MinHightlightProgress = 0;

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float MinSelectionProgress = 0;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverRendererIndicatorOverrider() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
			HoverRendererItemUpdater rendUp = GetComponent<HoverRendererItemUpdater>();
			HoverIndicator rendInd = rendUp.ActiveRenderer.GetIndicator();

			rendInd.Controllers.Set(HoverIndicator.HighlightProgressName, this);
			rendInd.Controllers.Set(HoverIndicator.SelectionProgressName, this);

			rendInd.HighlightProgress = Mathf.Max(rendInd.HighlightProgress, MinHightlightProgress);
			rendInd.SelectionProgress = Mathf.Max(rendInd.SelectionProgress, MinSelectionProgress);

			Controllers.TryExpireControllers();
		}

	}

}
