using System;

namespace Hover.Common.Items.Types {

	/*================================================================================================*/
	public interface ISliderItem : ISelectableItem<float> {

		int Ticks { get; set; }
		int Snaps { get; set; }
		float RangeMin { get; set; }
		float RangeMax { get; set; }
		string BaseLabel { get; }
		Func<ISliderItem, string> ValueToLabel { get; set; }
		bool AllowJump { get; set; }
		SliderItemFillType FillStartingPoint { get; set; }

		float RangeValue { get; }
		float SnappedValue { get; }
		float SnappedRangeValue { get; }
		float? HoverValue { get; set; }
		float? HoverSnappedValue { get; }

	}

}
