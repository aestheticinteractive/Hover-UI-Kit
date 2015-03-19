using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hover.Common.Util {

	/*================================================================================================*/
	public static class UnityUtil {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static TBase FindComponentOrFail<TBase>(TBase pProvided, string pLogPrefix) 
																			where TBase : Component {
			if ( pProvided != null ) {
				return pProvided;
			}

			pProvided = Object.FindObjectOfType<TBase>();

			if ( pProvided != null ) {
				Debug.Log(pLogPrefix+" | Using the "+typeof(TBase).Name+" found in the '"+
					GetPath(pProvided.gameObject)+"' GameObject.");
				return pProvided;
			}

			throw new Exception(pLogPrefix+" | Could not find "+typeof(TBase).Name);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TBase[] FindComponentsOrFail<TBase>(TBase[] pProvided, string pLogPrefix) 
																			where TBase : Component {
			if ( pProvided != null && pProvided.Length > 0 ) {
				return pProvided;
			}

			pProvided = Object.FindObjectsOfType<TBase>();

			if ( pProvided != null && pProvided.Length > 0 ) {
				foreach ( TBase comp in pProvided ) {
					Debug.Log(pLogPrefix+" | Using the "+typeof(TBase).Name+" found in the '"+
						GetPath(comp.gameObject)+"' GameObject.");	
				}
				
				return pProvided;
			}

			throw new Exception(pLogPrefix+" | Could not find "+typeof(TBase).Name);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static TBase FindComponentOrCreate<TBase, TDefault>(TBase pProvided, GameObject pGameObj, 
									string pLogPrefix) where TBase : Component where TDefault : TBase {
			if ( pProvided != null ) {
				return pProvided;
			}

			pProvided = Object.FindObjectOfType<TBase>();

			if ( pProvided != null ) {
				Debug.Log(pLogPrefix+" | Using the "+typeof(TBase).Name+" found in the '"+
					GetPath(pProvided.gameObject)+"' GameObject.");
				return pProvided;
			}

			pProvided = pGameObj.AddComponent<TDefault>();
			Debug.Log(pLogPrefix+" | Using the default "+typeof(TBase).Name);
			return pProvided;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TBase CreateComponent<TBase, TDefault>(TBase pProvided, GameObject pGameObj,
									string pLogPrefix) where TBase : Component where TDefault : TBase {
			if ( pProvided != null ) {
				return pProvided;
			}

			pProvided = pGameObj.AddComponent<TDefault>();
			Debug.Log(pLogPrefix+" | Using the default "+typeof(TBase).Name);
			return pProvided;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string GetPath(GameObject pObj) {
			if ( pObj.transform.parent == null ) {
				return "/" + pObj.name;
			}

			return GetPath(pObj.transform.parent.gameObject)+"/"+pObj.name;
		}

	}

}
