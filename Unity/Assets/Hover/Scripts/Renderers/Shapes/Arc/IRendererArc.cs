namespace Hover.Renderers.Shapes.Arc {

	/*================================================================================================*/
	public interface IRendererArc : IRenderer {
		
		float OuterRadius { get; set; }
		float InnerRadius { get; set; }
		float ArcAngle { get; set; }

	}

}
