using Hover.Core.Items.Types;
using UnityEngine;

namespace Hover.Core.Items.Helpers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshRenderer))]
	public class ShowViaHoverItemBoolValue : MonoBehaviour {

		public HoverItemDataSelectableBool Data;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( Data == null ) {
				Data = GetComponentInParent<HoverItemDataSelectableBool>();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			GetComponent<MeshRenderer>().enabled = Data.Value;
		}

	}

}
