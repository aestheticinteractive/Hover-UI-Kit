using Hover.Common.Renderers.Contents;

namespace Hover.Common.Renderers.Shapes.Rect {

	/*================================================================================================*/
	public interface IRendererRectButton : IRendererRect {

		HoverIcon.IconOffset IconOuterType { get; set; }
		HoverIcon.IconOffset IconInnerType { get; set; }
		
	}

}
