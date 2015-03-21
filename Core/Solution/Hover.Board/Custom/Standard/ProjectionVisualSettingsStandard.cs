using System;
using UnityEngine;

namespace Hover.Board.Custom.Standard {

	/*================================================================================================*/
	public class ProjectionVisualSettingsStandard : IProjectionVisualSettings {

		public Type Renderer { get; set; }

		public Color SpotlightColor { get; set; }
		public Color LineColor { get; set; }

	}

}
