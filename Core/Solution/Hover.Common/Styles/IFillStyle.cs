using System;
using System.Collections.Generic;

namespace Hover.Common.Styles {
	
	/*================================================================================================*/
	public interface IFillStyle {
		
		GraphicType Graphic { get; set; }
		string GraphicPath { get; set; }
		Color4 Color { get; set; }
		
		IDictionary<string, object> Settings { get; } //for customized settings, materials, etc.
		
	}
	
}
