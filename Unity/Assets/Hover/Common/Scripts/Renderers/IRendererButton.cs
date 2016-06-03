using Hover.Common.Renderers.Contents;

namespace Hover.Common.Renderers {

	/*================================================================================================*/
	public interface IRendererButton : IRenderer {

		HoverIcon.IconOffset IconOuterType { get; set; }
		HoverIcon.IconOffset IconInnerType { get; set; }
		
	}

}
