using System.Collections.Generic;
using UnityEngine;

namespace Hovercast.Core.Navigation {

	/*================================================================================================*/
	public class HovercastNavProvider : MonoBehaviour { 

		public string Title = "Hovercast VR";

		private readonly NavRoot vRoot;
		private NavLevel vRootLevel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HovercastNavProvider() {
			vRoot = new NavRoot();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			vRoot.Title = Title;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public NavRoot GetRoot() {
			if ( vRootLevel == null ) {
				vRootLevel = new NavLevel(GetChildItems);
				vRoot.Build(vRootLevel);
			}

			return vRoot;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private NavItem[] GetChildItems() {
			return GetChildItems(gameObject);
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal static NavItem[] GetChildItems(GameObject pParentObj) {
			Transform tx = pParentObj.transform;
			int childCount = tx.childCount;
			var items = new List<NavItem>();
			
			for ( int i = 0 ; i < childCount ; ++i ) {
				HovercastNavItem hni = tx.GetChild(i).GetComponent<HovercastNavItem>();
				NavItem item = hni.GetItem();

				if ( !item.IsVisible ) {
					continue;
				}

				items.Add(item);
			}

			return items.ToArray();
		}

	}

}
