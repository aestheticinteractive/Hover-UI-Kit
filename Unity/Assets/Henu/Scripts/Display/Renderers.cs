using System;
using System.Linq;

namespace Henu.Display {

	/*================================================================================================*/
	public struct Renderers {

		public Type PointParent;
		public Type PointSelection;
		public Type PointCheckbox;
		public Type PointRadio;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Verify() {
			VerifyType(PointParent, "PointParent");
			VerifyType(PointSelection, "PointSelection");
			VerifyType(PointCheckbox, "PointCheckbox");
			VerifyType(PointRadio, "PointRadio");
		}

		/*--------------------------------------------------------------------------------------------*/
		private static void VerifyType(Type pType, string pName) {
			if ( pType == null ) {
				throw new Exception("The ''"+pName+"' Renderer is not set.");
			}

			if ( !pType.GetInterfaces().Contains(typeof(IUiMenuPointRenderer)) ) {
				throw new Exception("The ''"+pName+"' Renderer must be an IMenuPointRenderer.");
			}
		}

	}

}
