using System.Collections.Generic;
using Hover.Renderers;
using Hover.Utils;
using UnityEngine;

namespace Hover.RendererModules.Alpha {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverFill))]
	public class HoverAlphaFillColorer : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public const string SortingLayerName = "SortingLayer";
		public const string AlphaName = "Alpha";

		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(DisplayMessage=true)]
		public string SortingLayer = "Default";

		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;

		private readonly List<HoverAlphaMeshColorer> vAlphaMeshColorers;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverAlphaFillColorer() {
			vAlphaMeshColorers = new List<HoverAlphaMeshColorer>();
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
				UpdateChildMeshSettings(hoverFill.GetChildMesh(i));
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateChildMeshSettings(HoverMesh pChildMesh) {
			HoverAlphaMeshColorer meshCol = pChildMesh.GetComponent<HoverAlphaMeshColorer>();

			if ( meshCol == null ) {
				return;
			}

			meshCol.Controllers.Set(HoverAlphaMeshColorer.SortingLayerName, this);
			meshCol.Controllers.Set(HoverAlphaMeshColorer.AlphaName, this);

			meshCol.SortingLayer = SortingLayer;
			meshCol.Alpha = Alpha;
		}

	}

}
