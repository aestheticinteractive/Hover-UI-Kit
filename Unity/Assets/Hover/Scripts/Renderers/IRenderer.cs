using Hover.Utils;

namespace Hover.Renderers {

	/*================================================================================================*/
	public interface IRenderer : IGameObjectProvider, IProximityProvider {

		ISettingsController RendererController { get; set; }

		bool IsEnabled { get; set; }
		string LabelText { get; set; }
		float HighlightProgress { get; set; }
		float SelectionProgress { get; set; }
		bool ShowEdge { get; set; }
		string SortingLayer { get; set; }

	}

}
