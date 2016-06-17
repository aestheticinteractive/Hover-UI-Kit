namespace Hover.Renderers.Shapes.Arc {

	/*================================================================================================*/
	public interface ICursorRendererArc : ICursorRenderer {
		
		float OuterRadius { get; set; }
		float InnerRadius { get; set; }
		float ArcAngle { get; set; }

	}

}
