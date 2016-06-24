using Hover.Utils;
using UnityEngine;

namespace Hover.Interfaces.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HovercastOpenIcons : MonoBehaviour, ITreeUpdateable {
		
		public HovercastInterface HovercastInterface;
		public GameObject OpenIcon;
		public GameObject CloseIcon;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( HovercastInterface == null ) {
				HovercastInterface = GetComponentInParent<HovercastInterface>();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			if ( HovercastInterface == null ) {
				Debug.LogError("Reference to '"+typeof(HovercastInterface).Name+"' must be set.");
				return;
			}

			if ( OpenIcon != null ) {
				OpenIcon.SetActive(!HovercastInterface.IsOpen);
			}

			if ( CloseIcon != null ) {
				CloseIcon.SetActive(HovercastInterface.IsOpen);
			}
		}

	}

}
