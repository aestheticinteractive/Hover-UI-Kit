using System.Collections.Generic;
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


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static bool UpdateValueWithTreeMessage<TB, TV>(this TB pSource, ref TV pOld, TV pNew,
																string pNote) where TB : MonoBehaviour {
			if ( pNew == null && pOld == null ) {
				return false;
			}

			if ( EqualityComparer<TV>.Default.Equals(pNew, pOld) ) {
				return false;
			}

			pOld = pNew;
			TreeUpdater.SendTreeUpdatableChanged(pSource, pNote);
			return true;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static GameObject FindOrAddHoverKitPrefab() {
			HoverItemsManager itemsMan = HoverItemsManager.Instance;

			if ( itemsMan != null ) {
				return itemsMan.gameObject;
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


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string ToDebugPath(this Transform pTx) {
			string path = pTx.name;
			Transform pathTx = pTx;

			while ( pathTx.parent != null ) {
				pathTx = pathTx.parent;
				path = pathTx.name+"/"+path;
			}

			return path;
		}

	}

}
