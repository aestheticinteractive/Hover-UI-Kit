using System;

namespace HandMenu.Navigation {

	/*================================================================================================*/
	public class ItemData {

		public enum ItemType {
			Parent,
			Selection,
			Checkbox,
			Slider
		}

		public ItemType Type { get; private set; }
		public string Label { get; private set; }
		public ItemData[] Children { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ItemData(ItemType pType, string pLabel) {
			Type = pType;
			Label = pLabel;
			Children = null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetChildren(ItemData[] pChildren) {
			if ( Children == null ) {
				throw new Exception("Children already set.");
			}

			Children = pChildren;
		}

	}

}
