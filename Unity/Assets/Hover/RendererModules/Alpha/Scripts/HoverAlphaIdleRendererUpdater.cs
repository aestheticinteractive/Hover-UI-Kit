using Hover.Core.Renderers;
using UnityEngine;

namespace Hover.RendererModules.Alpha {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	public class HoverAlphaIdleRendererUpdater : HoverAlphaRendererUpdater {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			HoverIndicator indic = GetComponent<HoverIndicator>();

			Controllers.Set(MasterAlphaName, this);
			MasterAlpha = Mathf.Pow(indic.HighlightProgress, 2);

			base.TreeUpdate();
		}

	}

}
