using System;
using UnityEngine;

namespace Hover.Cursor.Custom.Standard {

	/*================================================================================================*/
	public class CursorSettingsStandard : ICursorSettings {

		public Type Renderer { get; set; }

		public Color ColorNorm { get; set; }
		public Color ColorHigh { get; set; }
		public float RadiusNorm { get; set; }
		public float RadiusHigh { get; set; }
		public float ThickNorm { get; set; }
		public float ThickHigh { get; set; }
		public float CursorForwardDistance { get; set; }

	}

}
