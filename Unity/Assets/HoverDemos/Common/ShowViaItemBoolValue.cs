using Hover.Core.Items.Types;
using UnityEngine;

namespace HoverDemos.Common {

	/*================================================================================================*/
	[RequireComponent(typeof(MeshRenderer))]
	public class ShowViaItemBoolValue : MonoBehaviour {

		public HoverItemDataSelectableBool Data;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			GetComponent<MeshRenderer>().enabled = Data.Value;
		}

	}

}
