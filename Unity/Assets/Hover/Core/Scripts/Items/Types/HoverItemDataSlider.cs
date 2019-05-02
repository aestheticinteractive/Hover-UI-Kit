using System;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class HoverItemDataSlider : HoverItemDataSelectableFloat, IItemDataSlider {

		public Func<IItemDataSlider, string> GetFormattedLabel { get; set; }

		[SerializeField]
		private string _LabelFormat = "{0}: {1:N1}";

		[SerializeField]
		private int _Ticks = 0;

		[SerializeField]
		private int _Snaps = 0;

		[SerializeField]
		private float _RangeMin = -100;

		[SerializeField]
		private float _RangeMax = 100;

		[SerializeField]
		private bool _AllowJump = false;

		[SerializeField]
		private SliderFillType _FillStartingPoint = SliderFillType.MinimumValue;

		[SerializeField]
		private bool _AllowIdleDeselection = false;

		private float? vHoverValue;
		private string vPrevLabel;
		private string vPrevLabelFormat;
		private float vPrevSnappedRangeValue;
		private string vPrevValueToLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverItemDataSlider() {
			_Value = 0.5f;

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
			get => _LabelFormat;
			set => this.UpdateValueWithTreeMessage(ref _LabelFormat, value, "LabelFormat");
		}

		/*--------------------------------------------------------------------------------------------*/
		public int Ticks {
			get => _Ticks;
			set => this.UpdateValueWithTreeMessage(ref _Ticks, value, "Ticks");
		}

		/*--------------------------------------------------------------------------------------------*/
		public int Snaps {
			get => _Snaps;
			set => this.UpdateValueWithTreeMessage(ref _Snaps, value, "Snaps");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RangeMin {
			get => _RangeMin;
			set => this.UpdateValueWithTreeMessage(ref _RangeMin, value, "RangeMin");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RangeMax {
			get => _RangeMax;
			set => this.UpdateValueWithTreeMessage(ref _RangeMax, value, "RangeMax");
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool AllowJump {
			get => _AllowJump;
			set => this.UpdateValueWithTreeMessage(ref _AllowJump, value, "AllowJump");
		}

		/*--------------------------------------------------------------------------------------------*/
		public SliderFillType FillStartingPoint {
			get => _FillStartingPoint;
			set => this.UpdateValueWithTreeMessage(ref _FillStartingPoint, value, "FillStartingPoint");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void DeselectStickySelections() {
			Value = SnappedValue;
			base.DeselectStickySelections();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override bool AllowIdleDeselection {
			get => _AllowIdleDeselection;
			set => this.UpdateValueWithTreeMessage(
				ref _AllowIdleDeselection, value, "AllowIdleDeselection");
		}

		/*--------------------------------------------------------------------------------------------*/
		public override float Value {
			get => base.Value;
			set => base.Value = Mathf.Clamp01(value);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetValueViaRangeValue(float pRangeValue) {
			Value = Mathf.InverseLerp(RangeMin, RangeMax, pRangeValue);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float RangeValue {
			get => Value*(RangeMax-RangeMin)+RangeMin;
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SnappedValue {
			get => CalcSnappedValue(Value);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public float SnappedRangeValue {
			get => SnappedValue*(RangeMax-RangeMin)+RangeMin;
		}

		/*--------------------------------------------------------------------------------------------*/
		public float? HoverValue {
			get => vHoverValue;
			set {
				float? newHoverValue = null;

				if ( value != null ) {
					newHoverValue = Mathf.Clamp01((float)value);
				}

				this.UpdateValueWithTreeMessage(ref vHoverValue, newHoverValue, "HoverValue");
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
