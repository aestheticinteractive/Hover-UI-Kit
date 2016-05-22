using Hover.Common.Renderers.Shared.Contents;

namespace Hover.Common.Renderers.Rect.Button {

	/*================================================================================================*/
	public interface IHoverRendererRectButton : IHoverRendererRect {

		HoverRendererIcon.IconOffset IconOuterType { get; set; }
		HoverRendererIcon.IconOffset IconInnerType { get; set; }
		
	}

}
