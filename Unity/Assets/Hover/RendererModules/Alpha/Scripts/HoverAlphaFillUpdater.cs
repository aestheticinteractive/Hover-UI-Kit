using Hover.Core.Renderers;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.RendererModules.Alpha {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverFill))]
	public class HoverAlphaFillUpdater : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public const string SortingLayerName = "SortingLayer";
		public const string AlphaName = "Alpha";

		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(DisplaySpecials=true)]
		public string SortingLayer = "Default";

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverAlphaFillUpdater() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			HoverFill hoverFill = GetComponent<HoverFill>();
			int meshCount = hoverFill.GetChildMeshCount();

			for ( int i = 0 ; i < meshCount ; i++ ) {
				UpdateChildMesh(hoverFill.GetChildMesh(i));
			}

			Controllers.TryExpireControllers();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateChildMesh(HoverMesh pChildMesh) {
			HoverAlphaMeshUpdater meshUp = pChildMesh.GetComponent<HoverAlphaMeshUpdater>();

			if ( meshUp == null ) {
				return;
			}

			meshUp.Controllers.Set(HoverAlphaMeshUpdater.SortingLayerName, this);
			meshUp.Controllers.Set(HoverAlphaMeshUpdater.AlphaName, this);

			meshUp.SortingLayer = SortingLayer;
			meshUp.Alpha = Alpha;
		}

	}

}
