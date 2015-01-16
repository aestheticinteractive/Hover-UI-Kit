using System;

namespace Henu.Navigation {

	/*================================================================================================*/
	public class NavItemSlider : NavItem {

		public int Ticks { get; set; }
		public int Snaps { get; set; }
		public Func<float, string> ValueToLabel { get; set; }

		private float vCurrValue;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItemSlider(string pLabel, float pRelativeSize=1) : 
														base(ItemType.Slider, pLabel, pRelativeSize) {
			ValueToLabel = (v => 
				(string.IsNullOrEmpty(Label) ? "" : Label+": ")+(v*100).ToString("0.0")+"%"
			);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float CurrentValue {
			get {
				return vCurrValue;
			}
			set {
				vCurrValue = Math.Max(0, Math.Min(1, value));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override bool IsSelected {
			set {
				if ( base.IsSelected && !value ) {
					CurrentValue = SnappedValue;
				}

				base.IsSelected = value;
			}
		}


		/*--------------------------------------------------------------------------------------------*/
		public float SnappedValue {
			get {
				if ( Snaps < 2 ) {
					return CurrentValue;
				}

				int s = Snaps-1;
				return (float)Math.Round(CurrentValue*s)/s;
			}
		}

	}

}
