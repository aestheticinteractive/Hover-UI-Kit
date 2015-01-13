using System;
using System.Linq;

namespace Henu.Display {

	/*================================================================================================*/
	public struct Renderers {

		public Type ArcSegmentParent;
		public Type ArcSegmentSelection;
		public Type ArcSegmentCheckbox;
		public Type ArcSegmentRadio;
		public Type Cursor;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Verify() {
			VerifyArcSegType(ArcSegmentParent, "ArcSegmentParent");
			VerifyArcSegType(ArcSegmentSelection, "ArcSegmentSelection");
			VerifyArcSegType(ArcSegmentCheckbox, "ArcSegmentCheckbox");
			VerifyArcSegType(ArcSegmentRadio, "ArcSegmentRadio");
			VerifyCursorType(Cursor, "Cursor");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static void VerifyType(Type pType, string pName) {
			if ( pType == null ) {
				throw new Exception("The ''"+pName+"' Renderer is not set.");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private static void VerifyArcSegType(Type pType, string pName) {
			VerifyType(pType, pName);

			if ( !pType.GetInterfaces().Contains(typeof(IUiArcSegmentRenderer)) ) {
				throw new Exception("The ''"+pName+"' Renderer must be an IUiArcSegmentRenderer.");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private static void VerifyCursorType(Type pType, string pName) {
			VerifyType(pType, pName);

			if ( !pType.GetInterfaces().Contains(typeof(IUiCursorRenderer)) ) {
				throw new Exception("The ''"+pName+"' Renderer must be an IUiCursorRenderer.");
			}
		}

	}

}
