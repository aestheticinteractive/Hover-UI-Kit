using System;
using Hover.Common.Items.Types;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverSliderItem : HoverSelectableItemFloat, ISliderItem {
		
		public int Ticks { get; set; } //TODO: doesn't update visually for runtime changes
		public int Snaps { get; set; }
		public float RangeMin { get; set; }
		public float RangeMax { get; set; }
		public Func<ISliderItem, string> ValueToLabel { get; set; }
		public bool AllowJump { get; set; }
		public SliderItemFillType FillStartingPoint { get; set; }

		private float? vHoverValue;
		private string vPrevLabel;
		private float vPrevSnappedValue;
		private string vPrevValueToLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverSliderItem() {
			ValueToLabel = (s => {
				if ( base.Label == vPrevLabel && s.SnappedRangeValue == vPrevSnappedValue ) {
					return vPrevValueToLabel;
				}
				
				vPrevLabel = base.Label;
				vPrevSnappedValue = s.SnappedRangeValue;
				vPrevValueToLabel = vPrevLabel+": "+Math.Round(vPrevSnappedValue*10)/10f; //GC_ALLOC
				return vPrevValueToLabel;
			});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override string Label {
			get {
				return ValueToLabel(this);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public string BaseLabel {
			get {
				return base.Label;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void DeselectStickySelections() {
			Value = SnappedValue;
			base.DeselectStickySelections();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override float Value {
			get {
				return base.Value;
			}
			set {
				base.Value = Math.Max(0, Math.Min(1, value));
			}
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float RangeValue {
			get {
				return Value*(RangeMax-RangeMin)+RangeMin;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SnappedValue {
			get {
				return CalcSnappedValue(Value);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public float SnappedRangeValue {
			get {
				return SnappedValue*(RangeMax-RangeMin)+RangeMin;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public float? HoverValue {
			get {
				return vHoverValue;
			}
			set {
				if ( value == null ) {
					vHoverValue = null;
					return;
				}

				vHoverValue = Math.Max(0, Math.Min(1, (float)value));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public float? HoverSnappedValue {
			get {
				if ( HoverValue == null ) {
					return null;
				}

				return CalcSnappedValue((float)HoverValue);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool UsesStickySelection() {
			return true;
		}

		/*--------------------------------------------------------------------------------------------*/
		private float CalcSnappedValue(float pValue) {
			if ( Snaps < 2 ) {
				return pValue;
			}

			int s = Snaps-1;
			return (float)Math.Round(pValue*s)/s;
		}

	}

}
