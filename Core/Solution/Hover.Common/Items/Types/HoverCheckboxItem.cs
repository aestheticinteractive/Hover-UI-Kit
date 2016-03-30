namespace Hover.Common.Items.Types {

	/*================================================================================================*/
	public class HoverCheckboxItem : HoverSelectableItemBool, ICheckboxItem {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Select() {
			Value = !Value;
			base.Select();
		}

	}

}
