using System;
using System.Collections.Generic;

namespace Hover.Common.Styles {
	
	/*================================================================================================*/
	public interface IItemStyle {
	
		ITextStyle PrimaryText { get; set; }
		ITextStyle SecondaryText { get; set; }
		IFillStyle Background { get; set; }
		IIconStyle PrimaryIcon { get; set; }
		IIconStyle SecondaryIcon { get; set; }
		
		IDictionary<string, object> Settings { get; } //for customized settings, materials, etc.
		
	}
	
}
