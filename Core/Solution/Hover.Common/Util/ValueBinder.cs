using System;

namespace Hover.Common.Util {
	
	/*================================================================================================*/
	public class ValueBinder {

		public static Func<string, string, bool> AreStringsEqual = ((a, b) => (a == b));
		public static Func<bool, bool, bool> AreBoolsEqual = ((a, b) => (a == b));
		public static Func<int, int, bool> AreIntsEqual = ((a, b) => (a == b));
		public static Func<float, float, bool> AreFloatsEqual = ((a, b) => (a == b));

	}

	/*================================================================================================*/
	public class ValueBinder<T> : ValueBinder{

		private readonly Action<T> vSetCore;
		private readonly Action<T> vSetEditor;
		private readonly Func<T, T, bool> vAreEqual;

		private T vValue;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ValueBinder(Action<T> pSetCore, Action<T> pSetEditor, Func<T,T,bool> pAreEqual) {
			vSetCore = pSetCore;
			vSetEditor = pSetEditor;
			vAreEqual = pAreEqual;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool UpdateValuesIfChanged(T pValue, bool pForce=false) {
			if ( !pForce && pValue == vValue ) {
				return false;
			}

			vValue = pValue;
			vSetCore(vValue);
			vSetEditor(vValue);
			return true;
		}

	}

}
