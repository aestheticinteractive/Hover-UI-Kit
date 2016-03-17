using System.Collections.Generic;
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
			return GetChildItemsFromGameObject(gameObject);
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal static IBaseItem[] GetChildItemsFromGameObject(GameObject pParentObj) {
			Transform tx = pParentObj.transform;
			int childCount = tx.childCount;
			var items = new List<IBaseItem>();
			
			for ( int i = 0 ; i < childCount ; ++i ) {
				HovercastItem hni = tx.GetChild(i).GetComponent<HovercastItem>();
				IBaseItem item = hni.GetItem();

				if ( !item.IsVisible ) {
					continue;
				}

				items.Add(item);
			}

			return items.ToArray();
		}

	}

}
