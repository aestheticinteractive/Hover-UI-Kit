using Hover.Renderers.Shapes.Arc;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Packs.Alpha.Arc {

	/*================================================================================================*/
	public class HoverAlphaFillArcButton : HoverFillArcButton {
	
		public const string AlphaName = "Alpha";

		[DisableWhenControlled]
		public HoverAlphaMeshArcLateral _Background;

		[DisableWhenControlled]
		public HoverAlphaMeshArcLateral _Highlight;

		[DisableWhenControlled]
		public HoverAlphaMeshArcLateral _Selection;

		[DisableWhenControlled]
		public HoverAlphaMeshArcLateral _Edge;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override HoverMeshArcButton Background {
			get { return _Background; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverMeshArcButton Highlight {
			get { return _Highlight; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverMeshArcButton Selection {
			get { return _Selection; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverMeshArcButton Edge {
			get { return _Edge; }
		}
		
		public override int GetChildMeshCount() { return 0; }
		public override HoverMesh GetChildMesh(int pIndex) { return null; }

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected void BuildElements() {
			_Background = BuildArcMesh("Background");
			_Highlight = BuildArcMesh("Highlight");
			_Selection = BuildArcMesh("Selection");
			_Edge = BuildArcMesh("Edge");

			_Background.FillColor = new Color(0.1f, 0.1f, 0.1f, 0.666f);
			_Highlight.FillColor = new Color(0.1f, 0.5f, 0.9f);
			_Selection.FillColor = new Color(0.1f, 0.9f, 0.2f);
			_Edge.FillColor = new Color(1, 1, 1, 1);
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverAlphaMeshArcLateral BuildArcMesh(string pName) {
			var rectGo = new GameObject(pName);
			rectGo.transform.SetParent(gameObject.transform, false);
			return rectGo.AddComponent<HoverAlphaMeshArcLateral>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateGeneralSettings() {
			base.UpdateGeneralSettings();

			_Background.Controllers.Set(HoverAlphaMeshArcLateral.AlphaName, this);
			_Highlight.Controllers.Set(HoverAlphaMeshArcLateral.AlphaName, this);
			_Selection.Controllers.Set(HoverAlphaMeshArcLateral.AlphaName, this);
			_Edge.Controllers.Set(HoverAlphaMeshArcLateral.AlphaName, this);

			_Background.Alpha = Alpha;
			_Highlight.Alpha = Alpha;
			_Selection.Alpha = Alpha;
			_Edge.Alpha = Alpha;
		}

	}

}
