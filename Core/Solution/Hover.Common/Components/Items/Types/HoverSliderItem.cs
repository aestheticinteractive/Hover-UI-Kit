using Hover.Common.Items;
using Hover.Common.Items.Types;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverSliderItem : HoverSelectableItemFloat {

		public new ISliderItem Item { get; private set; }

		//TODO: Disable the "Value" property in the inspector

		public int Ticks = 3;
		public int Snaps = 0;
		public float RangeValue = 0;
		public float RangeMin = 0;
		public float RangeMax = 100;
		public bool AllowJump = false;
		public SliderItem.FillType FillStartingPoint = SliderItem.FillType.MinimumValue;

		private float SnappedValue;
		private string HoverValue;
		private string HoverSnappedValue;
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
			
			vBlockBaseLabelBinding = true;
			vBlockBaseValueBinding = true;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateAllValues(bool pForceUpdate=false) {
			base.UpdateAllValues(pForceUpdate);

			float value = Mathf.InverseLerp(RangeMin, RangeMax, RangeValue);
			vBindValue.UpdateValuesIfChanged(Item.Value, value, pForceUpdate);
			
			//Reset label using "BaseLabel" due to the slider's dynamic "Label" string
			vBindLabel.UpdateValuesIfChanged(Item.BaseLabel, Label, pForceUpdate);

			//TODO: the "Ticks" don't update visually for runtime changes
			vBindTicks.UpdateValuesIfChanged(Item.Ticks, Ticks, pForceUpdate);
			vBindSnaps.UpdateValuesIfChanged(Item.Snaps, Snaps, pForceUpdate);
			vBindMin.UpdateValuesIfChanged(Item.RangeMin, RangeMin, pForceUpdate);
			vBindMax.UpdateValuesIfChanged(Item.RangeMax, RangeMax, pForceUpdate);
			vBindJump.UpdateValuesIfChanged(Item.AllowJump, AllowJump, pForceUpdate);
			vBindFill.UpdateValuesIfChanged(Item.FillStartingPoint, FillStartingPoint, pForceUpdate);

			SnappedValue = Item.SnappedValue;
			HoverValue = Item.HoverValue+"";
			HoverSnappedValue = Item.HoverSnappedValue+"";
			RangeValue = Item.RangeValue;
			RangeSnappedValue = Item.RangeSnappedValue;
		}

	}

}
