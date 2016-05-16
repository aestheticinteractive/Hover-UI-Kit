using System;

namespace Hover.Common.Items.Types {

	/*================================================================================================*/
	public interface ISliderItem : ISelectableItem<float> {

		string LabelFormat { get; set; }
		int Ticks { get; set; }
		int Snaps { get; set; }
		float RangeMin { get; set; }
		float RangeMax { get; set; }
		Func<ISliderItem, string> GetFormattedLabel { get; set; }
		bool AllowJump { get; set; }
		SliderItem.FillType FillStartingPoint { get; set; }
		
		float RangeValue { get; }
		float SnappedValue { get; }
		float SnappedRangeValue { get; }
		float? HoverValue { get; set; }
		float? SnappedHoverValue { get; }

	}

}
