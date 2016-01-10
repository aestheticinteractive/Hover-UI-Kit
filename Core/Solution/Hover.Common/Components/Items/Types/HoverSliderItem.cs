using Hover.Common.Items;
using Hover.Common.Items.Types;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverSliderItem : HoverSelectableItem<float> {

		public new ISliderItem Item { get; private set; }

		public int Ticks;
		public int Snaps;
		public float RangeMin;
		public float RangeMax;
		public bool AllowJump;
		public SliderItem.FillType FillStartingPoint;

		private float SnappedValue;
		private float? HoverValue;
		private float? HoverSnappedValue;
		private float RangeValue;
		private float RangeSnappedValue;

		private readonly ValueBinder<int> vBindTicks;
		private readonly ValueBinder<int> vBindSnaps;
		private readonly ValueBinder<float> vBindMin;
		private readonly ValueBinder<float> vBindMax;
		private readonly ValueBinder<bool> vBindJump;
		private readonly ValueBinder<SliderItem.FillType> vBindFill;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverSliderItem() {
			Item = new SliderItem();
			Init((SelectableItem<float>)Item);

			vBindTicks = new ValueBinder<int>(
				(x => { Item.Ticks = x; }),
				(x => { Ticks = x; }),
				ValueBinder.AreIntsEqual
			);

			vBindSnaps = new ValueBinder<int>(
				(x => { Item.Snaps = x; }),
				(x => { Snaps = x; }),
				ValueBinder.AreIntsEqual
			);

			vBindMin = new ValueBinder<float>(
				(x => { Item.RangeMin = x; }),
				(x => { RangeMin = x; }),
				ValueBinder.AreFloatsEqual
			);

			vBindMax = new ValueBinder<float>(
				(x => { Item.RangeMax = x; }),
				(x => { RangeMax = x; }),
				ValueBinder.AreFloatsEqual
			);

			vBindJump = new ValueBinder<bool>(
				(x => { Item.AllowJump = x; }),
				(x => { AllowJump = x; }),
				ValueBinder.AreBoolsEqual
			);

			vBindFill = new ValueBinder<SliderItem.FillType>(
				(x => { Item.FillStartingPoint = x; }),
				(x => { FillStartingPoint = x; }),
				((a,b) => (a == b))
			);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateAllValues(bool pForceUpdate=false) {
			base.UpdateAllValues(pForceUpdate);

			//Allow developers to set the RangeValue here, then normalized it (from 0 to 1)
			float valueFromRange = Mathf.InverseLerp(RangeMin, RangeMax, Value);
			vBindValue.UpdateValuesIfChanged(Item.Value, valueFromRange, pForceUpdate);

			vBindTicks.UpdateValuesIfChanged(Item.Ticks, Ticks, pForceUpdate);
			vBindSnaps.UpdateValuesIfChanged(Item.Snaps, Snaps, pForceUpdate);
			vBindMin.UpdateValuesIfChanged(Item.RangeMin, RangeMin, pForceUpdate);
			vBindMax.UpdateValuesIfChanged(Item.RangeMax, RangeMax, pForceUpdate);
			vBindJump.UpdateValuesIfChanged(Item.AllowJump, AllowJump, pForceUpdate);
			vBindFill.UpdateValuesIfChanged(Item.FillStartingPoint, FillStartingPoint, pForceUpdate);

			SnappedValue = Item.SnappedValue;
			HoverValue = Item.HoverValue;
			HoverSnappedValue = Item.HoverSnappedValue;
			RangeValue = Item.RangeValue;
			RangeSnappedValue = Item.RangeSnappedValue;
		}

	}

}
