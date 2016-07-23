using System;
using Hover.Renderers.Shapes;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Renderers.Cursors {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShape))]
	public class HoverFillIdle : HoverFill {

		[DisableWhenControlled(DisplaySpecials=true)]
		public HoverMesh BackgroundTop;

		[DisableWhenControlled]
		public HoverMesh BackgroundBottom;

		[DisableWhenControlled]
		public HoverMesh TimerLeft;

		[DisableWhenControlled]
		public HoverMesh TimerRight;

		[DisableWhenControlled]
		public HoverMesh ItemPointer;

		[DisableWhenControlled]
		public HoverMesh CursorPointer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildMeshCount() {
			return 6;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverMesh GetChildMesh(int pIndex) {
			switch ( pIndex ) {
				case 0: return BackgroundTop;
				case 1: return BackgroundBottom;
				case 2: return TimerLeft;
				case 3: return TimerRight;
				case 4: return ItemPointer;
				case 5: return CursorPointer;
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
			if ( BackgroundTop != null ) {
				UpdateMesh(BackgroundTop);
			}

			if ( BackgroundBottom != null ) {
				UpdateMesh(BackgroundBottom);
			}

			if ( TimerLeft != null ) {
				UpdateMesh(TimerLeft);
			}

			if ( TimerRight != null ) {
				UpdateMesh(TimerRight);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMesh(HoverMesh pMesh, bool pShowMesh=true) {
			pMesh.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
			RendererUtil.SetActiveWithUpdate(pMesh, (pShowMesh && pMesh.IsMeshVisible));
		}

	}

}
