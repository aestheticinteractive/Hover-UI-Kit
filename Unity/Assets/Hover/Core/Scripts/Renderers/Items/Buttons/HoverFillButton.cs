using System;
using Hover.Core.Renderers.Shapes;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Items.Buttons {

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

	}

}
