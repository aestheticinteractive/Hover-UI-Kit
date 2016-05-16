using System.Collections.Generic;

namespace Hover.Common.Util {

	/*================================================================================================*/
	public class SettingsControllerMap : ISettingsControllerMap {

		private readonly Dictionary<string, ISettingsController> vMap;
		private readonly List<string> vKeys;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public SettingsControllerMap() {
			vMap = new Dictionary<string, ISettingsController>();
			vKeys = new List<string>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Set(string pValueName, ISettingsController pController) {
			if ( vMap.ContainsKey(pValueName) ) {
				vMap[pValueName] = pController;
			}
			else {
				vMap.Add(pValueName, pController);
				vKeys.Add(pValueName);
			}

		}

		/*--------------------------------------------------------------------------------------------*/
		public ISettingsController Get(string pValueName) {
			return (vMap.ContainsKey(pValueName) ? vMap[pValueName] : null);
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

	}

}
