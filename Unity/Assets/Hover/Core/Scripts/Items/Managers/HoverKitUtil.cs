using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Items.Managers {

	/*================================================================================================*/
	public static class HoverKitUtil {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static GameObject FindOrAddHoverKitPrefab() {
			HoverItemsManager itemsMan = Object.FindObjectOfType<HoverItemsManager>();

			if ( itemsMan != null ) {
				return itemsMan.gameObject;
			}

			GameObject managerPrefab = Resources.Load<GameObject>("Prefabs/HoverKit");
			UnityUtil.BuildPrefab(managerPrefab);
			Debug.Log("Added the 'HoverKit' prefab to the scene.", managerPrefab);
			return managerPrefab;
		}

	}

}
