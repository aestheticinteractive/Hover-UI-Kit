using System;
using System.Linq;
using Hovercast.Core.Display;
using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;
using UnityEngine;

namespace Hovercast.Core {

	/*================================================================================================*/
	public abstract class HovercastSettingsComponent : MonoBehaviour, ISettings {


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
		public Type GetUiPalmRendererType() {
			Type type = GetUiPalmRendererTypeInner();
			VerifyPalmType(type, "Palm");
			return type;
		}


		/*--------------------------------------------------------------------------------------------*/
		public abstract ArcSegmentSettings GetArcSegmentSettings(NavItem pNavItem);

		/*--------------------------------------------------------------------------------------------*/
		public abstract CursorSettings GetCursorSettings();

		/*--------------------------------------------------------------------------------------------*/
		public abstract InteractionSettings GetInteractionSettings();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract Type GetUiArcSegmentRendererTypeInner(NavItem pNavItem);

		/*--------------------------------------------------------------------------------------------*/
		protected abstract Type GetUiCursorRendererTypeInner();

		/*--------------------------------------------------------------------------------------------*/
		protected abstract Type GetUiPalmRendererTypeInner();


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

		/*--------------------------------------------------------------------------------------------*/
		private static void VerifyPalmType(Type pType, string pName) {
			VerifyType(pType, pName);

			if ( !pType.GetInterfaces().Contains(typeof(IUiPalmRenderer)) ) {
				throw new Exception("The ''"+pName+"' Renderer must implement IUiPalmRenderer.");
			}
		}

	}

}
