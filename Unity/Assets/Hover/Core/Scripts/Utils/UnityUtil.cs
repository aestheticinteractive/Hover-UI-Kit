using Hover.Core.Items.Managers;
using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	public static class UnityUtil {


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


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T FindNearbyComponent<T>(GameObject pGameObj) where T : Component {
			T sibling = pGameObj.GetComponent<T>();

			if ( sibling != null ) {
				return sibling;
			}

			T child = pGameObj.GetComponentInChildren<T>();

			if ( child != null ) {
				return child;
			}

			T parent = pGameObj.GetComponentInParent<T>();

			if ( parent != null ) {
				return parent;
			}
			return Object.FindObjectOfType<T>();
		}

	}

}
