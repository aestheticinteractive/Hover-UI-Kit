namespace Hover.Common.Layouts.Rect {

	/*================================================================================================*/
	public struct HoverLayoutRectGroupChild {

		public IRectLayoutable Elem { get; private set; }
		public HoverLayoutRectRelativeSizer RelSizer { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverLayoutRectGroupChild(IRectLayoutable pElem, HoverLayoutRectRelativeSizer pSizer) {
			Elem = pElem;
			RelSizer = pSizer;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float RelativeSizeX {
			get { return (RelSizer == null ? 1 : RelSizer.RelativeSizeX); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativeSizeY {
			get { return (RelSizer == null ? 1 : RelSizer.RelativeSizeY); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public float RelativePositionOffsetX {
			get { return (RelSizer == null ? 0 : RelSizer.RelativePositionOffsetX); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativePositionOffsetY {
			get { return (RelSizer == null ? 0 : RelSizer.RelativePositionOffsetY); }
		}

	}

}
