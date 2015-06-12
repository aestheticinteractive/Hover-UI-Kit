using Hover.Board.Custom;
using Hover.Common.Input;
using Hover.Common.Items;
using Hover.Common.Items.Types;

namespace Hover.Demo.BoardKeys.CastItems {

	/*================================================================================================*/
	public class DemoAllowThumbListener : DemoBaseListener<ICheckboxItem> {

		private InteractionSettings vInteractSett;
		private CursorType[] vNoThumbCursors;
		private CursorType[] vThumbCursors;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void Setup() {
			base.Setup();
			vInteractSett = HoverboardSetup.InteractionSettings.GetSettings();

			vNoThumbCursors = new[] {
				CursorType.LeftIndex,
				CursorType.RightIndex
			};

			vThumbCursors = new[] {
				CursorType.LeftIndex,
				CursorType.LeftThumb,
				CursorType.RightIndex,
				CursorType.RightThumb
			};

			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleValueChanged(ISelectableItem<bool> pItem) {
			CursorType[] cursorTypes = (pItem.Value ? vThumbCursors : vNoThumbCursors);

			vInteractSett.Cursors.Clear();

			foreach ( CursorType cursorType in cursorTypes ) {
				vInteractSett.Cursors.Add(cursorType);
			}
		}
	}

}
