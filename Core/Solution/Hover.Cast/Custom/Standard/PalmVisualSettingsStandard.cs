using System;
using UnityEngine;

namespace Hover.Cast.Custom.Standard {

	/*================================================================================================*/
	public class PalmVisualSettingsStandard : IPalmVisualSettings {

		public Type Renderer { get; set; }

		public int TextSize { get; set; }
		public Color TextColor { get; set; }
		public string TextFont { get; set; }
		public Color BackgroundColor { get; set; }

	}

}
