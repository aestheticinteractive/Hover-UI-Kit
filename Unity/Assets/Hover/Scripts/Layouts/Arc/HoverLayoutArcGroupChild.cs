namespace Hover.Layouts.Arc {

	/*================================================================================================*/
	public struct HoverLayoutArcGroupChild {

		public IArcLayoutable Elem { get; private set; }
		public HoverLayoutArcRelativeSizer RelSizer { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverLayoutArcGroupChild(IArcLayoutable pElem, HoverLayoutArcRelativeSizer pSizer) {
			Elem = pElem;
			RelSizer = pSizer;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float RelativeThickness {
			get { return (RelSizer == null ? 1 : RelSizer.RelativeThickness); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativeArcAngle {
			get { return (RelSizer == null ? 1 : RelSizer.RelativeArcAngle); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativeRadiusOffset {
			get { return (RelSizer == null ? 0 : RelSizer.RelativeRadiusOffset); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativeStartAngleOffset {
			get { return (RelSizer == null ? 0 : RelSizer.RelativeStartAngleOffset); }
		}

	}

}
