using System;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShape))]
	public class HoverFillSlider : HoverFill {

		public const string SegmentInfoName = "SegmentInfo";
		public const int SegmentCount = 4;

		[DisableWhenControlled(DisplaySpecials=true)]
		public HoverRendererSliderSegments SegmentInfo;

		[DisableWhenControlled]
		public HoverMesh SegmentMeshA;

		[DisableWhenControlled]
		public HoverMesh SegmentMeshB;

		[DisableWhenControlled]
		public HoverMesh SegmentMeshC;

		[DisableWhenControlled]
		public HoverMesh SegmentMeshD;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildMeshCount() {
			return SegmentCount;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverMesh GetChildMesh(int pIndex) {
			switch ( pIndex ) {
				case 0: return SegmentMeshA;
				case 1: return SegmentMeshB;
				case 2: return SegmentMeshC;
				case 3: return SegmentMeshD;
			}

			throw new ArgumentOutOfRangeException();
		}

	}

}
