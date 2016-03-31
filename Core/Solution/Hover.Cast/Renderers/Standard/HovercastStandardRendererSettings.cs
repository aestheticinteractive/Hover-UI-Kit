using UnityEngine;

namespace Hover.Cast.Renderers.Standard {

	/*================================================================================================*/
	public class HovercastStandardRendererSettings {

		[Header("Text")]
		[Range(1, 100)]
		public int TextSize = 30;
		public Color TextColor = new Color(1, 1, 1);
		public Font TextFont = Resources.Load<Font>("Fonts/Tahoma");

		[Header("Icons")]
		public Color ArrowIconColor = new Color(1, 1, 1);
		public Color ToggleIconColor = new Color(1, 1, 1);

		[Header("Fills")]
		public Color BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.666f);
		public Color EdgeColor = new Color(1, 1, 1, 1);
		public Color HighlightColor = new Color(0.1f, 0.5f, 0.9f);
		public Color SelectionColor = new Color(0.1f, 0.9f, 0.2f);

		[Header("Sliders")]
		public Color SliderTrackColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
		public Color SliderFillColor = new Color(0.1f, 0.9f, 0.2f, 0.5f);
		public Color SliderTickColor = new Color(1, 1, 1, 0.2f);

	}

}
