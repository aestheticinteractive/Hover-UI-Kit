using System.Collections.Generic;
using UnityEngine;

namespace Hover.Utils {

	/*================================================================================================*/
	public class SettingsControllerMap : ISettingsControllerMap {

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

			var expCont = new ExpirableController();
			expCont.Controller = pController;
			expCont.ExpireCount = pExpirationCount;

			if ( vMap.ContainsKey(pValueName) ) {
				vMap[pValueName] = expCont;
			}
			else {
				vMap.Add(pValueName, expCont);
				vKeys.Add(pValueName);
			}
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
		public int GetControlledCount() {
			int count = 0;

			for ( int i = 0 ; i < vKeys.Count ; i++ ) {
				if ( IsControlled(vKeys[i]) ) {
					count++;
				}
			}

			return count;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public List<string> GetNewListOfControlledValueNames() {
			var list = new List<string>();
			FillListWithControlledValueNames(list);
			return list;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void FillListWithControlledValueNames(List<string> pList) {
			pList.Clear();

			for ( int i = 0 ; i < vKeys.Count ; i++ ) {
				string valueName = vKeys[i];

				if ( !IsControlled(valueName) ) {
					continue;
				}

				pList.Add(valueName);
			}
		}
#endif

	}

}
