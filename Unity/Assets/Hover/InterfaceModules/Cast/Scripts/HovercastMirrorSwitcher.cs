using System.Collections.Generic;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.InterfaceModules.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HovercastInterface))]
	public class HovercastMirrorSwitcher : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public bool UseMirrorLayout = false;

		private readonly List<HoverCanvas> vHoverCanvases;
		private bool vWasMirror;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HovercastMirrorSwitcher() {
			vHoverCanvases = new List<HoverCanvas>();
			vWasMirror = false;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			TrySwitch();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void TrySwitch() { 
			if ( UseMirrorLayout == vWasMirror ) {
				return;
			}

			PerformSwitch();
			vWasMirror = UseMirrorLayout;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void PerformSwitch() {
			HovercastInterface cast = GetComponent<HovercastInterface>();
			Transform adjustTx = cast.transform.GetChild(0);
			Vector3 flipY = new Vector3( 1, -1,  1);
			Vector3 flipZ = new Vector3( 1,  1, -1);
			Quaternion spinRotZ = Quaternion.Euler(0, 0, 180);

			////

			adjustTx.localRotation = Quaternion.Euler(
				Vector3.Scale(adjustTx.localRotation.eulerAngles, flipZ));
			adjustTx.localScale = Vector3.Scale(adjustTx.localScale, flipY);

			////

			cast.BackItem.transform.localRotation *= spinRotZ;
			cast.TitleItem.transform.localRotation *= spinRotZ;
			cast.OpenItem.transform.localRotation *= spinRotZ;
			cast.RowContainer.transform.localRotation *= spinRotZ;

			cast.OpenItem.transform.localPosition = 
				Vector3.Scale(cast.OpenItem.transform.localPosition, flipY);

			////

			cast.GetComponentsInChildren(true, vHoverCanvases);

			for ( int i = 0 ; i < vHoverCanvases.Count ; i++ ) {
				HoverCanvas can = vHoverCanvases[i];
				can.UseMirrorLayout = !can.UseMirrorLayout;

#if UNITY_EDITOR
				//force serialization of changes made outside of the renderer prefab
				UnityEditor.EditorUtility.SetDirty(can);
#endif
			}
		}

	}

}
