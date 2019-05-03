using System;
using Hover.Core.Renderers;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.RendererModules.Opaque {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverMesh))]
	public class HoverOpaqueMeshUpdater : TreeUpdateableBehavior, ISettingsController {

		public ISettingsControllerMap Controllers { get; private set; }

		[SerializeField]
		[DisableWhenControlled]
		[ColorUsage(false, true)]
		[FormerlySerializedAs("StandardColor")]
		private Color _StandardColor = Color.gray;

		[SerializeField]
		[DisableWhenControlled]
		[ColorUsage(false, true)]
		[FormerlySerializedAs("SliderFillColor")]
		private Color _SliderFillColor = Color.white;

		private Color vPrevColor;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverOpaqueMeshUpdater() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Color StandardColor {
			get => _StandardColor;
			set => this.UpdateValueWithTreeMessage(ref _StandardColor, value, "StandardColor");
		}

		/*--------------------------------------------------------------------------------------------*/
		public Color SliderFillColor {
			get => _SliderFillColor;
			set => this.UpdateValueWithTreeMessage(ref _SliderFillColor, value, "SliderFillColor");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
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
