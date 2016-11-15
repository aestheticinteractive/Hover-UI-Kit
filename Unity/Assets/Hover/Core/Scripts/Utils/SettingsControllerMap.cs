using System.Collections.Generic;
using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	public class SettingsControllerMap : ISettingsControllerMap {

		public const char SpecialPrefixChar = '*';
		public const string SpecialPrefix = "*";
		public const string GameObjectActiveSelf = SpecialPrefix+"gameObject.activeSelf";
		public const string TransformPosition = SpecialPrefix+"transform.position";
		public const string TransformRotation = SpecialPrefix+"transform.rotation";
		public const string TransformLocalPosition = SpecialPrefix+"transform.localPosition";
		public const string TransformLocalRotation = SpecialPrefix+"transform.localRotation";
		public const string TransformLocalScale = SpecialPrefix+"transform.localScale";
		public const string CanvasSortingLayer = SpecialPrefix+"canvas.sortingLayer";
		public const string CanvasGroupAlpha = SpecialPrefix+"canvasGroup.alpha";
		public const string TextText = SpecialPrefix+"Text.text"; 
		public const string TextAlignment = SpecialPrefix+"Text.alignment";
		public const string MeshRendererSortingLayer = SpecialPrefix+"MeshRenderer.sortingLayer";
		public const string MeshRendererSortingOrder = SpecialPrefix+"MeshRenderer.sortingOrder";
		public const string MeshColors = SpecialPrefix+"Mesh.colors";

		private class ExpirableController {
			public ISettingsController Controller;
			public int ExpireCount;
		}

		private readonly Dictionary<string, ExpirableController> vMap;
		private readonly List<string> vKeys;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public SettingsControllerMap() {
			vMap = new Dictionary<string, ExpirableController>();
			vKeys = new List<string>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Set(string pValueName, ISettingsController pController, int pExpirationCount=1) {
			if ( !Application.isEditor ) {
				return;
			}

			ExpirableController expCont;

			if ( vMap.ContainsKey(pValueName) ) {
				expCont = vMap[pValueName];
			}
			else {
				expCont = new ExpirableController();
				vMap.Add(pValueName, expCont);
				vKeys.Add(pValueName);
			}

			expCont.Controller = pController;
			expCont.ExpireCount = pExpirationCount;
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool Unset(string pValueName, ISettingsController pController) {
			if ( !Application.isEditor ) {
				return false;
			}

			if ( vMap.ContainsKey(pValueName) && vMap[pValueName].Controller == pController ) {
				vMap.Remove(pValueName);
				vKeys.Remove(pValueName);
				return true;
			}

			return false;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TryExpireControllers() {
			if ( !Application.isEditor ) {
				return;
			}

			for ( int i = 0 ; i < vKeys.Count ; i++ ) {
				ExpirableController expCont = vMap[vKeys[i]];

				if ( --expCont.ExpireCount >= 0 ) {
					continue;
				}

				vMap.Remove(vKeys[i]);
				vKeys.RemoveAt(i);
				i--;
			}
		}


#if UNITY_EDITOR
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ISettingsController Get(string pValueName) {
			return (vMap.ContainsKey(pValueName) ? vMap[pValueName].Controller : null);
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsControlled(string pValueName) {
			ISettingsController cont = Get(pValueName);
			//Debug.Log(" ============ "+pValueName+" = "+
			//	cont+" / "+(cont == null ? "---" : cont.isActiveAndEnabled+""));
			return (cont != null && cont.isActiveAndEnabled);
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool AreAnyControlled() {
			for ( int i = 0 ; i < vKeys.Count ; i++ ) {
				if ( IsControlled(vKeys[i]) ) {
					return true;
				}
			}

			return false;
		}

		/*--------------------------------------------------------------------------------------------*/
		public int GetControlledCount(bool pSpecialsOnly=false) {
			int count = 0;

			for ( int i = 0 ; i < vKeys.Count ; i++ ) {
				if ( pSpecialsOnly && vKeys[i][0] != SpecialPrefixChar ) {
					continue;
				}

				if ( IsControlled(vKeys[i]) ) {
					count++;
				}
			}

			return count;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public List<string> GetNewListOfControlledValueNames(bool pSpecialsOnly=false) {
			var list = new List<string>();
			FillListWithControlledValueNames(list, pSpecialsOnly);
			return list;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void FillListWithControlledValueNames(List<string> pList, bool pSpecialsOnly=false) {
			pList.Clear();

			for ( int i = 0 ; i < vKeys.Count ; i++ ) {
				string valueName = vKeys[i];

				if ( pSpecialsOnly && valueName[0] != SpecialPrefixChar ) {
					continue;
				}

				if ( !IsControlled(valueName) ) {
					continue;
				}

				pList.Add(valueName);
			}
		}
#endif

	}

}
