using System;
using Hover.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShape))]
	public class HoverFillSlider : HoverFill {

		public const string SegmentInfoName = "SegmentInfo";
		public const int SegmentCount = 4;

		[DisableWhenControlled(DisplaySpecials=true)]
		public HoverRendererSliderSegments SegmentInfo;

		[FormerlySerializedAs("SegmentMeshA")]
		[DisableWhenControlled]
		public HoverMesh SegmentA;

		[FormerlySerializedAs("SegmentMeshB")]
		[DisableWhenControlled]
		public HoverMesh SegmentB;

		[FormerlySerializedAs("SegmentMeshC")]
		[DisableWhenControlled]
		public HoverMesh SegmentC;

		[FormerlySerializedAs("SegmentMeshD")]
		[DisableWhenControlled]
		public HoverMesh SegmentD;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildMeshCount() {
			return SegmentCount;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverMesh GetChildMesh(int pIndex) {
			switch ( pIndex ) {
				case 0: return SegmentA;
				case 1: return SegmentB;
				case 2: return SegmentC;
				case 3: return SegmentD;
			}

			throw new ArgumentOutOfRangeException();
		}

	}

}
