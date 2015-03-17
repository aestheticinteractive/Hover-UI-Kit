using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hover.Common.Util {

	/*================================================================================================*/
	public static class UnityUtil {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static TBase FindComponentOrFail<TBase>(TBase pProvided, GameObject pGameObj,
									string pLogPrefix) where TBase : Component {
			if ( pProvided != null ) {
				return pProvided;
			}

			pProvided = Object.FindObjectOfType<TBase>();

			if ( pProvided != null ) {
				Debug.Log(pLogPrefix+" | Found a "+typeof(TBase).Name+" in the '"+
					GetPath(pProvided.gameObject)+"' GameObject.");
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
				Debug.Log(pLogPrefix+" | Found a "+typeof(TBase).Name+" in the '"+
					GetPath(pProvided.gameObject)+"' GameObject.");
				return pProvided;
			}

			pProvided = pGameObj.AddComponent<TDefault>();
			Debug.Log(pLogPrefix+" | Using the default "+typeof(TBase).Name);
			return pProvided;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static string GetPath(GameObject pObj) {
			if ( pObj.transform.parent == null ) {
				return "/" + pObj.name;
			}

			return GetPath(pObj.transform.parent.gameObject)+"/"+pObj.name;
		}

	}

}
