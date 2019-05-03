using System;
using Hover.Core.Renderers.Shapes;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers.Items.Buttons {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShape))]
	public class HoverFillButton : HoverFill {

		public const string ShowEdgeName = "ShowEdge";

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

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("Edge")]
		private HoverMesh _Edge;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("ShowEdge")]
		private bool _ShowEdge = true;


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

		/*--------------------------------------------------------------------------------------------*/
		public HoverMesh Edge {
			get => _Edge;
			set => this.UpdateValueWithTreeMessage(ref _Edge, value, "Edge");
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool ShowEdge {
			get => _ShowEdge;
			set => this.UpdateValueWithTreeMessage(ref _ShowEdge, value, "ShowEdge");
		}


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

	}

}
