namespace Hover.Common.Layouts.Arc {

	/*================================================================================================*/
	public struct HoverLayoutArcGroupChild {

		public IArcLayoutable Elem { get; private set; }
		public HoverLayoutArcRelativeSizer RelSizer { get; private set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverLayoutArcGroupChild(IArcLayoutable pElem, HoverLayoutArcRelativeSizer pRelSizer) {
			Elem = pElem;
			RelSizer = pRelSizer;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float RelativeOuterRadius {
			get { return (RelSizer == null ? 1 : RelSizer.RelativeOuterRadius); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativeInnerRadius {
			get { return (RelSizer == null ? 1 : RelSizer.RelativeInnerRadius); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativeArcAngle {
			get { return (RelSizer == null ? 1 : RelSizer.RelativeArcAngle); }
		}

	}

}
