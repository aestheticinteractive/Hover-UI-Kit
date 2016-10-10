namespace HoverDemos.BoardKeys.CastItems {

	/*================================================================================================* /
	public class DemoFastSelectListener : DemoBaseListener<ICheckboxItem> {

		private InteractionSettings vInteractSett;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		protected override void Setup() {
			base.Setup();
			vInteractSett = HoverboardSetup.InteractionSettings.GetSettings();
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------* /
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		private void HandleValueChanged(ISelectableItem<bool> pItem) {
			vInteractSett.SelectionMilliseconds = (pItem.Value ? 200 : 400);
		}

	}*/

}
