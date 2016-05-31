using Hover.Common.Renderers.Contents;

namespace Hover.Common.Renderers.Shapes.Arc {

	/*================================================================================================*/
	public interface IRendererArcButton : IRendererArc {

		HoverIcon.IconOffset IconOuterType { get; set; }
		HoverIcon.IconOffset IconInnerType { get; set; }
		
	}

}
