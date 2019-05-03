using Hover.Core.Renderers;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.RendererModules.Alpha {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverFill))]
	public class HoverAlphaFillUpdater : TreeUpdateableBehavior, ISettingsController {

		public const string SortingLayerName = "SortingLayer";
		public const string AlphaName = "Alpha";

		public ISettingsControllerMap Controllers { get; private set; }

		[SerializeField]
		[DisableWhenControlled(DisplaySpecials=true)]
		[FormerlySerializedAs("SortingLayer")]
		private string _SortingLayer = "Default";

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		[FormerlySerializedAs("Alpha")]
		private float _Alpha = 1;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverAlphaFillUpdater() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string SortingLayer {
			get => _SortingLayer;
			set => this.UpdateValueWithTreeMessage(ref _SortingLayer, value, "SortingLayer");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float Alpha {
			get => _Alpha;
			set => this.UpdateValueWithTreeMessage(ref _Alpha, value, "Alpha");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
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
