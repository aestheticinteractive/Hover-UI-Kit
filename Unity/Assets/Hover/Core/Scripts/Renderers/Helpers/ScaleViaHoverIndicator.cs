using UnityEngine;

namespace Hover.Core.Renderers.Helpers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class ScaleViaHoverIndicator : MonoBehaviour {

		public HoverIndicator Indicator;
		public Vector3 StartLocalScale = new Vector3(1, 1, 1);
		public Vector3 HighlightLocalScale = new Vector3(2, 2, 2);
		public Vector3 SelectionLocalScale = new Vector3(2, 2, 2);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( Indicator == null ) {
				Indicator = GetComponentInParent<HoverIndicator>();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( Indicator.SelectionProgress > 0 ) {
				transform.localScale = Vector3.Lerp(
					HighlightLocalScale, SelectionLocalScale, Indicator.SelectionProgress);
			}
			else {
				transform.localScale = Vector3.Lerp(
					StartLocalScale, HighlightLocalScale, Indicator.HighlightProgress);
			}
		}

	}

}
