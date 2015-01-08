using System;

namespace HandMenu.Navigation {

	/*================================================================================================*/
	public class NavItemData {

		public enum ItemType {
			Parent,
			Selection,
			Checkbox,
			Radio
		}

		public string Label { get; private set; }
		public ItemType Type { get; private set; }
		public NavItemData[] Children { get; private set; }

		public bool Selected { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItemData(ItemType pType, string pLabel) {
			Type = pType;
			Label = pLabel;
			Children = null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetChildren(NavItemData[] pChildren) {
			if ( Children != null ) {
				throw new Exception("Children already set.");
			}

			Children = pChildren;
		}

	}

}
