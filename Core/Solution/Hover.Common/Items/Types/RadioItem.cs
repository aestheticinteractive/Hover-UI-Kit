namespace Hover.Common.Items.Types {

	/*================================================================================================*/
	public class RadioItem : SelectableItem<bool>, IRadioItem {

		public string GroupId { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Select() {
			Value = true;
			base.Select();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override bool AllowSelection {
			get {
				return (!Value && base.AllowSelection);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override bool AreValuesEqual(bool pValueA, bool pValueB) {
			return (pValueA == pValueB);
		}

	}

}
