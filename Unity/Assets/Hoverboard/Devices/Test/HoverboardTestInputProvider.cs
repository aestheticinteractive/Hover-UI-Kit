using System.Collections.Generic;
using Hoverboard.Core.Input;
using UnityEngine;

namespace Hoverboard.Devices.Test {

	/*================================================================================================*/
	public class HoverboardTestInputProvider : HoverboardInputProvider {

		private IDictionary<CursorType, TestInputCursor> vCursorMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput() {
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override IInputCursor GetCursor(CursorType pType) {
			if ( vCursorMap == null ) {
				Init();
			}

			return vCursorMap[pType];
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void Init() {
			vCursorMap = new Dictionary<CursorType, TestInputCursor>();
			vCursorMap[CursorType.PrimaryLeft] = 
				GetChild<TestInputCursor>(gameObject, "CursorPrimaryLeft");
			vCursorMap[CursorType.SecondaryLeft] = 
				GetChild<TestInputCursor>(gameObject, "CursorSecondaryLeft");
			vCursorMap[CursorType.PrimaryRight] = 
				GetChild<TestInputCursor>(gameObject, "CursorPrimaryRight");
			vCursorMap[CursorType.SecondaryRight] =
				GetChild<TestInputCursor>(gameObject, "CursorSecondaryRight");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static T GetChild<T>(GameObject pParentObj, string pName) where T : Component {
			return pParentObj.transform.FindChild(pName).gameObject.GetComponent<T>();
		}

	}

}
