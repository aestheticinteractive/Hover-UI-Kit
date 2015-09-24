using System;

namespace Hover.Common.Styles {
	
	/*================================================================================================*/
	public interface ISliderItemStyle : ISelectableItemStyle {
		
		IFillStyle SliderTrack { get; set; }
		IFillStyle SliderFill { get; set; }
		IFillStyle SliderTick { get; set; }
		
	}
	
}
