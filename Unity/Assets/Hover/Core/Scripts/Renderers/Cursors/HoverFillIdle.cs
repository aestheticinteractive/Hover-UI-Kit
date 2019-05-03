using System;
using Hover.Core.Renderers.Shapes;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers.Cursors {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShape))]
	public class HoverFillIdle : HoverFill {

		[SerializeField]
		[DisableWhenControlled(DisplaySpecials=true)]
		[FormerlySerializedAs("Timer")]
		private HoverMesh _Timer;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("ItemPointer")]
		private HoverMesh _ItemPointer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverMesh Timer {
			get => _Timer;
			set => this.UpdateValueWithTreeMessage(ref _Timer, value, "Timer");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverMesh ItemPointer {
			get => _ItemPointer;
			set => this.UpdateValueWithTreeMessage(ref _ItemPointer, value, "ItemPointer");
		}


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
