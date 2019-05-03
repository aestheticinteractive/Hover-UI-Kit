using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Layouts.Arc {

	/*================================================================================================*/
	public class HoverLayoutArcRelativeSizer : MonoBehaviour, ISettingsController {

		[SerializeField]
		[Range(0, 10)]
		[FormerlySerializedAs("RelativeThickness")]
		private float _RelativeThickness = 1;

		[SerializeField]
		[Range(0, 10)]
		[FormerlySerializedAs("RelativeArcDegrees")]
		private float _RelativeArcDegrees = 1;

		[SerializeField]
		[Range(-2, 2)]
		[FormerlySerializedAs("RelativeRadiusOffset")]
		private float _RelativeRadiusOffset = 0;

		[SerializeField]
		[Range(-2, 2)]
		[FormerlySerializedAs("RelativeStartDegreeOffset")]
		private float _RelativeStartDegreeOffset = 0;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float RelativeThickness {
			get => _RelativeThickness;
			set => this.UpdateValueWithTreeMessage(ref _RelativeThickness, value, "RelThickness");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativeArcDegrees {
			get => _RelativeArcDegrees;
			set => this.UpdateValueWithTreeMessage(ref _RelativeArcDegrees, value, "RelArcDegrees");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativeRadiusOffset {
			get => _RelativeRadiusOffset;
			set => this.UpdateValueWithTreeMessage(ref _RelativeRadiusOffset, value, "RelRadOffset");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativeStartDegreeOffset {
			get => _RelativeStartDegreeOffset;
			set => this.UpdateValueWithTreeMessage(ref _RelativeStartDegreeOffset, value, "RelStrtDeg");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void OnValidate() { //editor only
			TreeUpdater.SendTreeUpdatableChanged(this, "OnValidate");
		}

	}

}
