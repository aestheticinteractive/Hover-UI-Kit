using Hover.Common.Utils;

namespace Hover.Common.Renderers.Shapes.Rect {

	/*================================================================================================*/
	public abstract class HoverMeshRect : HoverMesh {
	
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";

		[DisableWhenControlled(RangeMin=0)]
		public float SizeX = 0.1f;
		
		[DisableWhenControlled(RangeMin=0)]
		public float SizeY = 0.1f;

		private float vPrevSizeX;
		private float vPrevSizeY;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool ShouldUpdateMesh() {
			bool shouldUpdate = (
				base.ShouldUpdateMesh() ||
				SizeX != vPrevSizeX ||
				SizeY != vPrevSizeY
			);

			vPrevSizeX = SizeX;
			vPrevSizeY = SizeY;

			return shouldUpdate;
		}
		
	}

}
