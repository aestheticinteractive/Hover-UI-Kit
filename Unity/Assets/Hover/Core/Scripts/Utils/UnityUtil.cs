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


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static GameObject FindOrAddHoverKitPrefab() {
			GameObject hoverKitGo = GameObject.Find("HoverKit");

			if ( hoverKitGo != null ) {
				return hoverKitGo;
			}

			GameObject managerPrefab = Resources.Load<GameObject>("Prefabs/HoverKit");
			BuildPrefab(managerPrefab);
			Debug.Log("Added the 'HoverKit' prefab to the scene.", managerPrefab);
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

		/*--------------------------------------------------------------------------------------------* /
		public static T FindComponentAll<T>(Func<T, bool> pMatchFunc) where T : Object {
			T[] comps = Resources.FindObjectsOfTypeAll<T>();

			foreach ( T comp in comps ) {
				if ( pMatchFunc(comp) ) {
					return comp;
				}
			}

			Debug.LogError("Could not find a matching "+typeof(T).Name+".");
			return default(T);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T FindComponentAll<T>(string pName) where T : Object {
			T[] comps = Resources.FindObjectsOfTypeAll<T>();

			foreach ( T comp in comps ) {
				if ( comp.name == pName ) {
					return comp;
				}
			}

			Debug.LogError("Could not find a "+typeof(T).Name+" with name '"+pName+"'.");
			return default(T);
		}

	}

}
