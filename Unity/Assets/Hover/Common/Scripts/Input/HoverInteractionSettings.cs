using UnityEngine;

namespace Hover.Common.Input {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverInteractionSettings : MonoBehaviour, IInteractionSettings {

		[SerializeField]
		[Range(0.001f, 100)]
		private float _HighlightDistanceMin;

		[SerializeField]
		[Range(0.001f, 100)]
		private float _HighlightDistanceMax;

		[SerializeField]
		[Range(0.001f, 100)]
		private float _StickyReleaseDistance;

		[SerializeField]
		[Range(1, 2000)]
		private float _SelectionMilliseconds;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverInteractionSettings() {
			HighlightDistanceMin = 3;
			HighlightDistanceMax = 7;
			StickyReleaseDistance = 5;
			SelectionMilliseconds = 400;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float HighlightDistanceMin {
			get { return _HighlightDistanceMin; }
			set { _HighlightDistanceMin = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float HighlightDistanceMax {
			get { return _HighlightDistanceMax; }
			set { _HighlightDistanceMax = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float StickyReleaseDistance {
			get { return _StickyReleaseDistance; }
			set { _StickyReleaseDistance = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SelectionMilliseconds {
			get { return _SelectionMilliseconds; }
			set { _SelectionMilliseconds = value; }
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			HighlightDistanceMax = Mathf.Max(HighlightDistanceMax, HighlightDistanceMin+0.1f);
		}

	}

}
