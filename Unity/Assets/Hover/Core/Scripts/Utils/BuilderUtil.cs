using Hover.Items.Managers;
using UnityEngine;

namespace Hover.Utils {

	/*================================================================================================*/
	public static class BuilderUtil {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static GameObject BuildPrefab(GameObject pPrefab) {
#if UNITY_EDITOR
			return (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(pPrefab);
#else
			return UnityEngine.Object.Instantiate(pPrefab);
#endif
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static GameObject FindOrAddHoverManagerPrefab() {
			HoverItemsManager itemsMan = Object.FindObjectOfType<HoverItemsManager>();

			if ( itemsMan != null ) {
				return itemsMan.gameObject;
			}

			GameObject managerPrefab = Resources.Load<GameObject>("Prefabs/HoverManagers");
			BuildPrefab(managerPrefab);
			Debug.Log("Added the 'HoverManagers' prefab to the scene.", managerPrefab);
			return managerPrefab;
		}

	}

}
