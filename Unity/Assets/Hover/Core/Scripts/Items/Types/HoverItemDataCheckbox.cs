using System;
using UnityEngine;

namespace Hover.Core.Items.Types {

	/*================================================================================================*/
	[Serializable]
	public class HoverItemDataCheckbox : HoverItemDataSelectableBool, IItemDataCheckbox {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[ContextMenu("Checkbox Select")]
		public override void Select() {
			Value = !Value;
			base.Select();
		}

	}

}
