using Hover.Common.Renderers.Packs.Alpha.Rect;
using Hover.Common.Renderers.Shapes.Arc;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Packs.Alpha.Arc {

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

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void BuildElements() {
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

			_Background.Controllers.Set(HoverAlphaMeshRectCentral.AlphaName, this);
			_Highlight.Controllers.Set(HoverAlphaMeshRectCentral.AlphaName, this);
			_Selection.Controllers.Set(HoverAlphaMeshRectCentral.AlphaName, this);
			_Edge.Controllers.Set(HoverAlphaMeshRectCentral.AlphaName, this);

			_Background.Alpha = Alpha;
			_Highlight.Alpha = Alpha;
			_Selection.Alpha = Alpha;
			_Edge.Alpha = Alpha;
		}
				
	}

}
