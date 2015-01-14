using System;
using System.Linq;
using Henu.Display;
using Henu.Navigation;
using Henu.Settings;
using UnityEngine;

namespace Henu {

	/*================================================================================================*/
	public abstract class HenuSettingsComponent : MonoBehaviour, ISettings {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Type GetUiArcSegmentRendererType(NavItem pNavItem) {
			Type type = GetUiArcSegmentRendererTypeInner(pNavItem);
			VerifyArcSegType(type, pNavItem.Label);
			return type;
		}

		/*--------------------------------------------------------------------------------------------*/
		public Type GetUiCursorRendererType() {
			Type type = GetUiCursorRendererTypeInner();
			VerifyCursorType(type, "Cursor");
			return type;
		}

		/*--------------------------------------------------------------------------------------------*/
		public abstract ArcSegmentSettings GetArcSegmentSettings(NavItem pNavItem);

		/*--------------------------------------------------------------------------------------------*/
		public abstract InteractionSettings GetInteractionSettings();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract Type GetUiArcSegmentRendererTypeInner(NavItem pNavItem);

		/*--------------------------------------------------------------------------------------------*/
		protected abstract Type GetUiCursorRendererTypeInner();


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
				throw new Exception("The ''"+pName+"' Renderer must implement IUiArcSegmentRenderer.");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private static void VerifyCursorType(Type pType, string pName) {
			VerifyType(pType, pName);

			if ( !pType.GetInterfaces().Contains(typeof(IUiCursorRenderer)) ) {
				throw new Exception("The ''"+pName+"' Renderer must implement IUiCursorRenderer.");
			}
		}

	}

}
