using System;

namespace Henu.Navigation {

	/*================================================================================================*/
	public class NavItemData {

		public enum ItemType {
			Parent,
			Selection,
			Checkbox,
			Radio
		}

		private static int ItemCount;

		public int Id { get; private set; }
		public string Label { get; private set; }
		public ItemType Type { get; private set; }
		public NavItemData[] Children { get; private set; }

		public bool Selected { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavItemData(ItemType pType, string pLabel) {
			Id = (++ItemCount);
			Type = pType;
			Label = pLabel;
			Children = null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetChildren(NavItemData[] pChildren) {
			if ( Children != null ) {
				throw new Exception("Children already set.");
			}

			if ( Type != ItemType.Parent ) {
				throw new Exception("Only items of type 'Parent' can have children.");
			}

			Children = pChildren;
		}

	}

}
