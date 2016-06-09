using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers {

	/*================================================================================================*/
	public interface IRenderer {

		GameObject gameObject { get; }
		
		ISettingsController RendererController { get; set; }

		bool IsEnabled { get; set; }
		string LabelText { get; set; }
		float HighlightProgress { get; set; }
		float SelectionProgress { get; set; }
		bool ShowEdge { get; set; }
		string SortingLayer { get; set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition);

	}

}
