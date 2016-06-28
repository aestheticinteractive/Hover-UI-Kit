using Hover.Utils;

namespace Hover.Renderers.Elements {

	/*================================================================================================*/
	public class HoverShapeRect : HoverShape {
		
		[DisableWhenControlled(RangeMin=0, DisplayMessage=true)]
		public float SizeX = 0.1f;
		
		[DisableWhenControlled(RangeMin=0)]
		public float SizeY = 0.1f;

		private float vPrevSizeX;
		private float vPrevSizeY;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();

			DidSettingsChange = (
				DidSettingsChange ||
				SizeX != vPrevSizeX ||
				SizeY != vPrevSizeY
			);

			vPrevSizeX = SizeX;
			vPrevSizeY = SizeY;
		}

	}

}
