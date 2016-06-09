using Hover.Renderers.Shapes.Rect;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Packs.Alpha.Rect {

	/*================================================================================================*/
	public class HoverAlphaFillRectButton : HoverFillRectButton {
	
		public const string AlphaName = "Alpha";

		[DisableWhenControlled]
		public HoverAlphaMeshRectCentral _Background;

		[DisableWhenControlled]
		public HoverAlphaMeshRectCentral _Highlight;

		[DisableWhenControlled]
		public HoverAlphaMeshRectCentral _Selection;

		[DisableWhenControlled]
		public HoverAlphaMeshRectCentral _Edge;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override HoverMeshRectButton Background {
			get { return _Background; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverMeshRectButton Highlight {
			get { return _Highlight; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverMeshRectButton Selection {
			get { return _Selection; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverMeshRectButton Edge {
			get { return _Edge; }
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void BuildElements() {
			_Background = BuildHollowRect("Background");
			_Highlight = BuildHollowRect("Highlight");
			_Selection = BuildHollowRect("Selection");
			_Edge = BuildHollowRect("Edge");

			_Background.FillColor = new Color(0.1f, 0.1f, 0.1f, 0.666f);
			_Highlight.FillColor = new Color(0.1f, 0.5f, 0.9f);
			_Selection.FillColor = new Color(0.1f, 0.9f, 0.2f);
			_Edge.FillColor = new Color(1, 1, 1, 1);
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverAlphaMeshRectCentral BuildHollowRect(string pName) {
			var rectGo = new GameObject(pName);
			rectGo.transform.SetParent(gameObject.transform, false);
			return rectGo.AddComponent<HoverAlphaMeshRectCentral>();
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
