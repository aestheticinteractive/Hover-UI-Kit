using System;
using System.Collections.Generic;

namespace Hover.Common.Styles {
	
	/*================================================================================================*/
	public interface IIconStyle {
		
		GraphicType Graphic { get; set; }
		string GraphicPath { get; set; }
		float Width { get; set; }
		float Height { get; set; }
		Color4 Color { get; set; }
		AlignmentType Alignment { get; set; }
		
		IDictionary<string, object> Settings { get; } //for customized settings
		
	}
	
}
