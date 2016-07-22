using Hover.Renderers;
using UnityEngine;

namespace Hover.RendererModules.Alpha {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	public class HoverAlphaStationaryRendererUpdater : HoverAlphaRendererUpdater {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();

			HoverIndicator indic = GetComponent<HoverIndicator>();

			Controllers.Set(MasterAlphaName, this);
			MasterAlpha = Mathf.Pow(indic.HighlightProgress, 2);
		}

	}

}
