using System;
using UnityEngine;

namespace Hover.Common.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class SliderItem : SelectableItemFloat, ISliderItem {

		public enum FillType {
			MinimumValue,
			Zero,
			MaximumValue
		}

		public Func<ISliderItem, string> GetFormattedLabel { get; set; }

		[SerializeField]
		private string vLabelFormat = "{0}: {1:N1}";

		[SerializeField]
		private int vTicks = 0;

		[SerializeField]
		private int vSnaps = 0;

		[SerializeField]
		private float vRangeMin = -100;

		[SerializeField]
		private float vRangeMax = 100;

		[SerializeField]
		private bool vAllowJump = false;

		[SerializeField]
		private FillType vFillStartingPoint = FillType.Zero;
		
		private float? vHoverValue;
		private string vPrevLabel;
		private string vPrevLabelFormat;
		private float vPrevSnappedRangeValue;
		private string vPrevValueToLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public SliderItem() {
			vValue = 0.5f;

			GetFormattedLabel = (s => {
				if ( s.Label == vPrevLabel && s.LabelFormat == vPrevLabelFormat &&
						s.SnappedRangeValue == vPrevSnappedRangeValue ) {
					return vPrevValueToLabel;
				}
				
				vPrevLabel = s.Label;
				vPrevLabelFormat = s.LabelFormat;
				vPrevSnappedRangeValue = s.SnappedRangeValue;
				vPrevValueToLabel = string.Format(vPrevLabelFormat,
					vPrevLabel, vPrevSnappedRangeValue); //GC_ALLOC
				return vPrevValueToLabel;
			});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string LabelFormat {
			get { return vLabelFormat; }
			set { vLabelFormat = value; }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public int Ticks {
			get { return vTicks; }
			set { vTicks = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public int Snaps {
			get { return vSnaps; }
			set { vSnaps = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RangeMin {
			get { return vRangeMin; }
			set { vRangeMin = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RangeMax {
			get { return vRangeMax; }
			set { vRangeMax = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool AllowJump {
			get { return vAllowJump; }
			set { vAllowJump = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public FillType FillStartingPoint {
			get { return vFillStartingPoint; }
			set { vFillStartingPoint = value; }
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
		public float? SnappedHoverValue {
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
