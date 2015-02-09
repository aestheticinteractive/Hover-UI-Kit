using System;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class NavItemSlider : NavItem<float> {

		public int Ticks { get; set; }
		public int Snaps { get; set; }
		public float RangeMin { get; set; }
		public float RangeMax { get; set; }
		public Func<NavItemSlider, string> ValueToLabel { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItemSlider() : base(ItemType.Slider) {
			ValueToLabel = (s => base.Label+": "+(s.SnappedValue*100).ToString("0.0")+"%");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override string Label {
			get {
				return ValueToLabel(this);
			}
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
		public override void DeselectStickySelections() {
			Value = SnappedValue;
			base.DeselectStickySelections();
		}

		/*--------------------------------------------------------------------------------------------*/
		public float SnappedValue {
			get {
				if ( Snaps < 2 ) {
					return Value;
				}

				int s = Snaps-1;
				return (float)Math.Round(Value*s)/s;
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

	}

}
