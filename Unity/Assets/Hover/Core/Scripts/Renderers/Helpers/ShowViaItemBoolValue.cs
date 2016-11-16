using Hover.Core.Items.Types;
using UnityEngine;

namespace Hover.Core.Renderers.Helpers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshRenderer))]
	public class ShowViaItemBoolValue : MonoBehaviour {

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
