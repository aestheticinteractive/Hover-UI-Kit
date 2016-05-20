using Hover.Common.Renderers.Contents;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers {

	/*================================================================================================*/
	public interface IHoverRenderer {

		GameObject gameObject { get; }
		
		ISettingsController RendererController { get; set; }

		float Alpha { get; set; }
		string LabelText { get; set; }
		HoverRendererIcon.IconOffset IconOuterType { get; set; }
		HoverRendererIcon.IconOffset IconInnerType { get; set; }
		float HighlightProgress { get; set; }
		float SelectionProgress { get; set; }
		bool ShowEdge { get; set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition);

	}

}
