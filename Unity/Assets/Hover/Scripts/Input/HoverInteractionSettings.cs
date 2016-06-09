using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Input {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverInteractionSettings : MonoBehaviour, IInteractionSettings {

		public ISettingsControllerMap Controllers { get; private set; }

		[SerializeField]
		[DisableWhenControlled(RangeMin=0.0001f)]
		private float _HighlightDistanceMin = 0.03f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0.0001f)]
		private float _HighlightDistanceMax = 0.07f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0.0001f)]
		private float _StickyReleaseDistance = 0.05f;

		[SerializeField]
		[Range(1, 10000)]
		private float _SelectionMilliseconds = 400;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverInteractionSettings() {
			Controllers = new SettingsControllerMap();
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
			HighlightDistanceMax = Mathf.Max(HighlightDistanceMax, HighlightDistanceMin*1.01f);
		}

	}

}
