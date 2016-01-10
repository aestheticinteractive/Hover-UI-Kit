using Hover.Common.Components.Items;
using Hover.Common.Items;
using Hover.Common.Items.Groups;
using UnityEngine;

namespace Hover.Cast.Items {

	/*================================================================================================*/
	public class HovercastItemHierarchy : MonoBehaviour { 

		public string Title = "Hovercast VR";

		private IItemHierarchy vRoot;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IItemHierarchy GetRoot() {
			if ( vRoot == null ) {
				var rootLevel = new ItemGroup(GetChildItems);

				vRoot = new ItemHierarchy();
				vRoot.Title = Title;
				vRoot.Build(rootLevel);
			}

			return vRoot;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private IBaseItem[] GetChildItems() {
			return HoverBaseItem.GetChildItems(gameObject);
		}

	}

}
