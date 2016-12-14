using System;
using Hover.Core.Renderers;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.RendererModules.Opaque {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverMesh))]
	public class HoverOpaqueMeshUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {
	
		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled]
		[ColorUsage(false, true, 0, 1000, 0, 1000)]
		public Color StandardColor = Color.gray;
		
		[DisableWhenControlled]
		[ColorUsage(false, true, 0, 1000, 0, 1000)]
		public Color SliderFillColor = Color.white;

		private Color vPrevColor;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverOpaqueMeshUpdater() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			TryUpdateColor(GetComponent<HoverMesh>());
			vPrevColor = StandardColor;
			Controllers.TryExpireControllers();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void TryUpdateColor(HoverMesh pHoverMesh) {
			Controllers.Set(SettingsControllerMap.MeshColors, this);

			Color useColor;

			switch ( pHoverMesh.DisplayMode ) {
				case HoverMesh.DisplayModeType.Standard:
					useColor = StandardColor;
					break;

				case HoverMesh.DisplayModeType.SliderFill:
					useColor = SliderFillColor;
					break;

				default:
					throw new Exception("Unhandled display mode: "+pHoverMesh.DisplayMode);
			}

			if ( !pHoverMesh.DidRebuildMesh && useColor == vPrevColor ) {
				return;
			}

			pHoverMesh.Builder.CommitColors(useColor);
		}
		
	}

}
