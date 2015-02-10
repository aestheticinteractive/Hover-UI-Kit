using System;
using System.Linq;
using Hovercast.Core.Display;
using UnityEngine;

namespace Hovercast.Core.Custom {

	/*================================================================================================*/
	public abstract class HovercastCustomPalm : MonoBehaviour {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Type GetRenderer() {
			Type type = GetRendererInner();

			if ( type == null ) {
				throw new Exception(GetErrorPrefix()+"cannot be null.");
			}

			if ( !type.GetInterfaces().Contains(typeof(IUiPalmRenderer)) ) {
				throw new Exception(GetErrorPrefix()+"must implement the "+
					typeof(IUiPalmRenderer).Name+" interface.");
			}

			return type;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract Type GetRendererInner();

		/*--------------------------------------------------------------------------------------------*/
		public abstract SegmentSettings GetSettings();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static string GetErrorPrefix() {
			return "Hovercast | The 'Palm' Renderer ";
		}

	}

}
