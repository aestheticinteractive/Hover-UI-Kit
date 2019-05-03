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
	public class HoverFillCursor : HoverFill {

		[SerializeField]
		[DisableWhenControlled(DisplaySpecials=true)]
		[FormerlySerializedAs("Background")]
		private HoverMesh _Background;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("Highlight")]
		private HoverMesh _Highlight;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("Selection")]
		private HoverMesh _Selection;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverMesh Background {
			get => _Background;
			set => this.UpdateValueWithTreeMessage(ref _Background, value, "Background");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverMesh Highlight {
			get => _Highlight;
			set => this.UpdateValueWithTreeMessage(ref _Highlight, value, "Highlight");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverMesh Selection {
			get => _Selection;
			set => this.UpdateValueWithTreeMessage(ref _Selection, value, "Selection");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildMeshCount() {
			return 3;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverMesh GetChildMesh(int pIndex) {
			switch ( pIndex ) {
				case 0: return Background;
				case 1: return Highlight;
				case 2: return Selection;
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
				UpdateMesh(Background);
			}

			if ( Highlight != null ) {
				UpdateMesh(Highlight);
			}

			if ( Selection != null ) {
				UpdateMesh(Selection);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMesh(HoverMesh pMesh) {
			pMesh.Controllers.Set(SettingsControllerMap.GameObjectActiveSelf, this);
			RendererUtil.SetActiveWithUpdate(pMesh, pMesh.IsMeshVisible);
		}

	}

}
