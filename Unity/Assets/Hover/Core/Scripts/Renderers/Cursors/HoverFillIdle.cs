using System;
using Hover.Core.Renderers.Shapes;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Cursors {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShape))]
	public class HoverFillIdle : HoverFill {

		[DisableWhenControlled(DisplaySpecials=true)]
		public HoverMesh Timer;

		[DisableWhenControlled]
		public HoverMesh ItemPointer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildMeshCount() {
			return 1;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverMesh GetChildMesh(int pIndex) {
			switch ( pIndex ) {
				case 0: return Timer;
				//case 1: return ItemPointer;
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
			if ( Timer != null ) {
				UpdateMesh(Timer);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMesh(HoverMesh pMesh, bool pShowMesh=true) {
			pMesh.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
			RendererUtil.SetActiveWithUpdate(pMesh, (pShowMesh && pMesh.IsMeshVisible));
		}

	}

}
