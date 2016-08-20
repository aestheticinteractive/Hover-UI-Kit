namespace Hover.Core.Layouts.Arc {

	/*================================================================================================*/
	public struct HoverLayoutArcGroupChild {

		public ILayoutableArc Elem { get; private set; }
		public HoverLayoutArcRelativeSizer RelSizer { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverLayoutArcGroupChild(ILayoutableArc pElem, HoverLayoutArcRelativeSizer pSizer) {
			Elem = pElem;
			RelSizer = pSizer;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float RelativeThickness {
			get { return (RelSizer == null ? 1 : RelSizer.RelativeThickness); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativeArcDegrees {
			get { return (RelSizer == null ? 1 : RelSizer.RelativeArcDegrees); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativeRadiusOffset {
			get { return (RelSizer == null ? 0 : RelSizer.RelativeRadiusOffset); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativeStartDegreeOffset {
			get { return (RelSizer == null ? 0 : RelSizer.RelativeStartDegreeOffset); }
		}

	}

}
