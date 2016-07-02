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
		public HoverMesh SegmentA;

		[DisableWhenControlled]
		public HoverMesh SegmentB;

		[DisableWhenControlled]
		public HoverMesh SegmentC;

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
