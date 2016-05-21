using Hover.Common.Renderers.Contents;

namespace Hover.Common.Renderers {

	/*================================================================================================*/
	public interface IHoverRendererRectangleButton : IHoverRendererRectangle {

		HoverRendererIcon.IconOffset IconOuterType { get; set; }
		HoverRendererIcon.IconOffset IconInnerType { get; set; }
		
	}

}
