using Hover.Utils;

namespace Hover.Renderers {

	/*================================================================================================*/
	public interface ICursorRenderer : IGameObjectProvider {

		ISettingsController RendererController { get; set; }

		float HighlightProgress { get; set; }
		float SelectionProgress { get; set; }
		string SortingLayer { get; set; }

	}

}
