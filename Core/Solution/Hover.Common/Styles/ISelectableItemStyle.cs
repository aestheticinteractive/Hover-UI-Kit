using System;

namespace Hover.Common.Styles {
	
	/*================================================================================================*/
	public interface ISelectableItemStyle : IItemStyle {
		
		IFillStyle ProximityIndicator { get; set; }
		IFillStyle SelectionIndicator { get; set; }
		IFillStyle NearestIndicator { get; set; }
		
	}
	
}
