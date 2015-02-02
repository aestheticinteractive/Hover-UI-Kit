using Hovercast.Core;
using Hovercast.Core.Input;
using UnityEngine;

namespace Hovercast.Devices.Test {

	/*================================================================================================*/
	public class HovercastTestInputProvider : HovercastInputProvider {

		private TestInputSide vSideL;
		private TestInputSide vSideR;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateInput() {
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override IInputSide GetSide(bool pIsLeft) {
			if ( vSideL == null ) {
				Init();
			}

			return (pIsLeft ? vSideL : vSideR);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void Init() {
			vSideL = GetChild<TestInputSide>(gameObject, "SideLeft");
			vSideR = GetChild<TestInputSide>(gameObject, "SideRight");

			vSideL.Init(true);
			vSideR.Init(false);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal static T GetChild<T>(GameObject pParentObj, string pName) where T : Component {
			return pParentObj.transform.FindChild(pName).gameObject.GetComponent<T>();
		}

	}

}
