using System;
using Hover.Renderers.Shapes;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Buttons {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShape))]
	public class HoverFillButton : HoverFill {

		public const string ShowEdgeName = "ShowEdge";

		[DisableWhenControlled(DisplaySpecials=true)]
		public HoverMesh Background;

		[DisableWhenControlled]
		public HoverMesh Highlight;

		[DisableWhenControlled]
		public HoverMesh Selection;

		[DisableWhenControlled]
		public HoverMesh Edge;
		
		[DisableWhenControlled]
		public bool ShowEdge = true;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildMeshCount() {
			return 4;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverMesh GetChildMesh(int pIndex) {
			switch ( pIndex ) {
				case 0: return Background;
				case 1: return Highlight;
				case 2: return Selection;
				case 3: return Edge;
			}

			throw new ArgumentOutOfRangeException();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();
			UpdateMeshes();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMeshes() {
			if ( Background != null ) {
				UpdateMesh(Background, true);
			}
			
			if ( Highlight != null ) {
				UpdateMesh(Highlight, true);
			}
			
			if ( Selection != null ) {
				UpdateMesh(Selection, true);
			}
			
			if ( Edge != null ) {
				UpdateMesh(Edge, ShowEdge);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMesh(HoverMesh pMesh, bool pShowMesh) {
			pMesh.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);

			//TODO: determine when mesh can be disabled (without needing a mesh update to occur first)
			RendererUtil.SetActiveWithUpdate(pMesh, (pShowMesh /*&& pMesh.IsMeshVisible*/));
		}

	}

}
