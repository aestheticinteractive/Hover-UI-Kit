using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Layouts.Rect {

	/*================================================================================================*/
	public class HoverLayoutRectRelativeSizer : MonoBehaviour, ISettingsController {

		[SerializeField]
		[Range(0, 10)]
		[FormerlySerializedAs("RelativeSizeX")]
		private float _RelativeSizeX = 1;

		[SerializeField]
		[Range(0, 10)]
		[FormerlySerializedAs("RelativeSizeY")]
		private float _RelativeSizeY = 1;

		[SerializeField]
		[Range(-2, 2)]
		[FormerlySerializedAs("RelativePositionOffsetX")]
		private float _RelativePositionOffsetX = 0;

		[SerializeField]
		[Range(-2, 2)]
		[FormerlySerializedAs("RelativePositionOffsetY")]
		private float _RelativePositionOffsetY = 0;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float RelativeSizeX {
			get => _RelativeSizeX;
			set => this.UpdateValueWithTreeMessage(ref _RelativeSizeX, value, "RelSizeX");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativeSizeY {
			get => _RelativeSizeY;
			set => this.UpdateValueWithTreeMessage(ref _RelativeSizeY, value, "RelSizeX");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativePositionOffsetX {
			get => _RelativePositionOffsetX;
			set => this.UpdateValueWithTreeMessage(ref _RelativePositionOffsetX, value, "RelOffsetX");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativePositionOffsetY {
			get => _RelativePositionOffsetY;
			set => this.UpdateValueWithTreeMessage(ref _RelativePositionOffsetY, value, "RelOffsetY");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void OnValidate() { //editor only
			TreeUpdater.SendTreeUpdatableChanged(this, "OnValidate");
		}

	}

}
