using System;
using System.Linq;
using UnityEngine;

namespace HandMenu.Display {

	/*================================================================================================*/
	public struct Renderers {

		public Type PointParent;
		public Type PointSelection;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Type GetType(Component pComp, Type pDefault) {
			return (pComp == null ? pDefault : pComp.GetType());
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Verify() {
			VerifyType(PointParent, "PointParent");
			VerifyType(PointSelection, "PointSelection");
		}

		/*--------------------------------------------------------------------------------------------*/
		private static void VerifyType(Type pType, string pName) {
			if ( pType == null ) {
				throw new Exception("The ''"+pName+"' Renderer is not set.");
			}

			if ( !pType.GetInterfaces().Contains(typeof(IMenuPointRenderer)) ) {
				throw new Exception("The ''"+pName+"' Renderer must be an IMenuPointRenderer.");
			}
		}

	}

}
