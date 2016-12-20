using UnityEngine;

namespace Hover.Core.Renderers.Helpers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshRenderer))]
	public class ColorViaHoverIndicator : MonoBehaviour {

		public HoverIndicator Indicator;
		public Color StartColor = new Color(1, 1, 1);
		public Color HighlightColor = new Color(0, 0.5f, 1);
		public Color SelectionColor = new Color(0, 1, 0);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( Indicator == null ) {
				Indicator = GetComponentInParent<HoverIndicator>();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( !Application.isPlaying ) {
				return;
			}

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
