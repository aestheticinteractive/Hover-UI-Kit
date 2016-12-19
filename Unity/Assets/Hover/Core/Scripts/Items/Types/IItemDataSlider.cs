using System;
using Hover.Core.Utils;

namespace Hover.Core.Items.Types {

	/*================================================================================================*/
	public interface IItemDataSlider : IItemDataSelectable<float> {

		string LabelFormat { get; }
		int Ticks { get; }
		int Snaps { get; }
		float RangeMin { get; }
		float RangeMax { get; }
		Func<IItemDataSlider, string> GetFormattedLabel { get; }
		bool AllowJump { get; }
		SliderFillType FillStartingPoint { get; }
		
		float RangeValue { get; }
		float SnappedValue { get; }
		float SnappedRangeValue { get; }
		float? HoverValue { get; }
		float? SnappedHoverValue { get; }

	}

}
