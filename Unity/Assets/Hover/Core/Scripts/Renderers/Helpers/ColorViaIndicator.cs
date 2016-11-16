using UnityEngine;

namespace Hover.Core.Renderers.Helpers {

	/*================================================================================================*/
	[RequireComponent(typeof(MeshRenderer))]
	public class ColorViaIndicator : MonoBehaviour {

		public HoverIndicator Indicator;
		public Color StartColor = new Color(1, 1, 1);
		public Color HighlightColor = new Color(0, 0.5f, 1);
		public Color SelectionColor = new Color(0, 1, 0);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			Material mat = GetComponent<MeshRenderer>().material;

			if ( Indicator.SelectionProgress > 0 ) {
				mat.color = Color.Lerp(HighlightColor, SelectionColor, Indicator.SelectionProgress);
			}
			else {
				mat.color = Color.Lerp(StartColor, HighlightColor, Indicator.HighlightProgress);
			}
		}

	}

}
