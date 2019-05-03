using Hover.Core.Renderers;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Items.Helpers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverItemRendererUpdater))]
	public class HoverIndicatorOverrider : TreeUpdateableBehavior, ISettingsController {

		public const string MinHightlightProgressName = "MinHightlightProgress";
		public const string MinSelectionProgressName = "MinSelectionProgress";

		public ISettingsControllerMap Controllers { get; private set; }
		
		[SerializeField]
		[DisableWhenControlled(DisplaySpecials=true, RangeMin=0, RangeMax=1)]
		[FormerlySerializedAs("MinHightlightProgress")]
		private float _MinHightlightProgress = 0;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		[FormerlySerializedAs("MinSelectionProgress")]
		private float _MinSelectionProgress = 0;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverIndicatorOverrider() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float MinHightlightProgress {
			get => _MinHightlightProgress;
			set => this.UpdateValueWithTreeMessage(ref _MinHightlightProgress, value, "MinHighProg");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float MinSelectionProgress {
			get => _MinSelectionProgress;
			set => this.UpdateValueWithTreeMessage(ref _MinSelectionProgress, value, "MinSelProg");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
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
