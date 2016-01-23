using System;
using System.Collections.Generic;

namespace Hover.Common.Styles {
	
	/*================================================================================================*/
	public interface ITextStyle {
		
		string Font { get; set; }
		int Size { get; set; }
		Color4 Color { get; set; }
		AlignmentType Alignment { get; set; }
		
		IDictionary<string, object> Settings { get; } //for customized settings
		
	}
	
}
