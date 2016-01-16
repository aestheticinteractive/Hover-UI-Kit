namespace Hover.Common.Items.Types {

	/*================================================================================================*/
	public class RadioItem : SelectableItem<bool>, IRadioItem {

		public string GroupId { get; set; }  //TODO: doesn't update for runtime changes


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
