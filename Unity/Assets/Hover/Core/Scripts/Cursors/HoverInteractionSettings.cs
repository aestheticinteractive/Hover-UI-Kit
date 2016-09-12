using UnityEngine;

namespace Hover.Core.Cursors {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverInteractionSettings : MonoBehaviour, IInteractionSettings {

		[SerializeField]
		private float _HighlightDistanceMin = 0.03f;

		[SerializeField]
		private float _HighlightDistanceMax = 0.07f;

		[SerializeField]
		private float _StickyReleaseDistance = 0.05f;

		[SerializeField]
		[Range(1, 10000)]
		private float _SelectionMilliseconds = 400;

		[SerializeField]
		private float _IdleDistanceThreshold = 0.004f;

		[SerializeField]
		[Range(1, 10000)]
		private float _IdleMilliseconds = 1000;


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

		/*--------------------------------------------------------------------------------------------*/
		public float IdleDistanceThreshold {
			get { return _IdleDistanceThreshold; }
			set { _IdleDistanceThreshold = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float IdleMilliseconds {
			get { return _IdleMilliseconds; }
			set { _IdleMilliseconds = value; }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			_HighlightDistanceMin = Mathf.Max(_HighlightDistanceMin, 0.0001f);
			_HighlightDistanceMax = Mathf.Max(_HighlightDistanceMax, _HighlightDistanceMin*1.01f);
			_StickyReleaseDistance = Mathf.Max(_StickyReleaseDistance, 0.0001f);
			_IdleDistanceThreshold = Mathf.Max(_IdleDistanceThreshold, 0);
		}

	}

}
