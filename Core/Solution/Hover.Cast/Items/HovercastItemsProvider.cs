using System.Collections.Generic;
using Hover.Common.Items;
using Hover.Common.Items.Groups;
using UnityEngine;

namespace Hover.Cast.Items {

	/*================================================================================================*/
	public class HovercastItemsProvider : MonoBehaviour { 

		public string Title = "Hovercast VR";

		private readonly IItemHierarchy vRoot;
		private IItemGroup vRootLevel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastItemsProvider() {
			vRoot = new ItemHierarchy();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			vRoot.Title = Title;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IItemHierarchy GetRoot() {
			if ( vRootLevel == null ) {
				vRootLevel = new ItemGroup(GetChildItems);
				vRoot.Build(vRootLevel);
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
