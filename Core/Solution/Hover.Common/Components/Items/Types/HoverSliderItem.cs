using System;
using Hover.Common.Items.Types;
using UnityEngine;

namespace Hover.Common.Components.Items.Types {

	/*================================================================================================*/
	public class HoverSliderItem : HoverSelectableItemFloat, ISliderItem {

		public Func<ISliderItem, string> ValueToLabel { get; set; }

		[SerializeField]
		private int vTicks;

		[SerializeField]
		private int vSnaps;

		[SerializeField]
		private float vRangeMin;

		[SerializeField]
		private float vRangeMax;

		[SerializeField]
		private bool vAllowJump;

		[SerializeField]
		private SliderItemFillType vFillStartingPoint;
		
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
		public int Ticks { //TODO: doesn't update visually for runtime changes
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
		public SliderItemFillType FillStartingPoint {
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
