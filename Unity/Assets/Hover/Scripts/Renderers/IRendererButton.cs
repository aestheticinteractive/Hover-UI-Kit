using Hover.Renderers.Contents;

namespace Hover.Renderers {

	/*================================================================================================*/
	public interface IRendererButton : IRenderer {

		HoverIcon.IconOffset IconOuterType { get; set; }
		HoverIcon.IconOffset IconInnerType { get; set; }
		
	}

}
