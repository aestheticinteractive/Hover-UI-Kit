using Hover.Board.Display.Standard;
using UnityEngine;

namespace Hover.Board.Custom.Standard {

	/*================================================================================================*/
	public class HoverboardProjectionVisualSettingsStandard : HoverboardProjectionVisualSettings {

		public Color SpotlightColor = new Color(1, 1, 1);
		public Color LineColor = new Color(1, 1, 1);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override IProjectionVisualSettings GetSettingsInner() {
			var sett = new ProjectionVisualSettingsStandard();
			sett.Renderer = typeof(UiProjectionRenderer);
			sett.SpotlightColor = SpotlightColor;
			sett.LineColor = LineColor;
			return sett;
		}

	}

}
