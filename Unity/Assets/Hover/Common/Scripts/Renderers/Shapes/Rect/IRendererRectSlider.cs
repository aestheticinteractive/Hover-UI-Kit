using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Common.Renderers.Shapes.Rect {

	/*================================================================================================*/
	public interface IRendererRectSlider : IRendererRect {

		float ZeroValue { get; set; }
		float HandleValue { get; set; }
		float JumpValue { get; set; }
		bool AllowJump { get; set; }
		int TickCount { get; set; }
		SliderItem.FillType FillStartingPoint { get; set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		float GetValueViaNearestWorldPosition(Vector3 pNearestWorldPosition);
		
	}

}
