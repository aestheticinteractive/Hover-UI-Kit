using System;

namespace Hover.Items.Types {

	/*================================================================================================*/
	public interface ISliderItem : ISelectableItem<float> {

		string LabelFormat { get; }
		int Ticks { get; }
		int Snaps { get; }
		float RangeMin { get; }
		float RangeMax { get; }
		Func<ISliderItem, string> GetFormattedLabel { get; }
		bool AllowJump { get; }
		SliderFillType FillStartingPoint { get; }
		
		float RangeValue { get; }
		float SnappedValue { get; }
		float SnappedRangeValue { get; }
		float? HoverValue { get; }
		float? SnappedHoverValue { get; }

	}

}
