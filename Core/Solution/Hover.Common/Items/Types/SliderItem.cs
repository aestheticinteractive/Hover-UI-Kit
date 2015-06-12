using System;

namespace Hover.Common.Items.Types {

	/*================================================================================================*/
	public class SliderItem : SelectableItem<float>, ISliderItem {

		public enum FillType {
			MinimumValue,
			Zero,
			MaximumValue
		}

		public int Ticks { get; set; }
		public int Snaps { get; set; }
		public float RangeMin { get; set; }
		public float RangeMax { get; set; }
		public Func<ISliderItem, string> ValueToLabel { get; set; }
		public bool AllowJump { get; set; }
		public FillType FillStartingPoint { get; set; }

		private float? vHoverValue;
		private string vPrevLabel;
		private float vPrevSnappedValue;
		private string vPrevValueToLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public SliderItem() {
			ValueToLabel = (s => {
				if ( base.Label == vPrevLabel && s.RangeSnappedValue == vPrevSnappedValue ) {
					return vPrevValueToLabel;
				}

				vPrevLabel = base.Label;
				vPrevSnappedValue = s.RangeSnappedValue;
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

		/*--------------------------------------------------------------------------------------------*/
		public float SnappedValue {
			get {
				return CalcSnappedValue(Value);
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

		/*--------------------------------------------------------------------------------------------*/
		public float RangeValue {
			get {
				return Value*(RangeMax-RangeMin)+RangeMin;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RangeSnappedValue {
			get {
				return SnappedValue*(RangeMax-RangeMin)+RangeMin;
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


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool AreValuesEqual(float pValueA, float pValueB) {
			return (pValueA == pValueB);
		}

	}

}
