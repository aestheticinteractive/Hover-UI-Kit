using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Shared.Bases {

	/*================================================================================================*/
	public interface IHoverRenderer {

		GameObject gameObject { get; }
		
		ISettingsController RendererController { get; set; }

		float Alpha { get; set; }
		string LabelText { get; set; }
		float HighlightProgress { get; set; }
		float SelectionProgress { get; set; }
		bool ShowEdge { get; set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition);

	}

}
