using System.Collections.Generic;
using Hover.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Hover.Interfaces.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HovercastInterface))]
	public class HovercastMirrorSwitcher : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public bool IsLeftHanded = true;

		private readonly List<Text> vLabelTexts;
		private bool vWasLeftHanded;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HovercastMirrorSwitcher() {
			vLabelTexts = new List<Text>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vWasLeftHanded = IsLeftHanded;
		}

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
			if ( IsLeftHanded == vWasLeftHanded ) {
				return;
			}

			PerformSwitch();
			vWasLeftHanded = IsLeftHanded;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void PerformSwitch() {
			HovercastInterface cast = GetComponent<HovercastInterface>();
			Transform adjustTx = cast.transform.GetChild(0);
			Vector3 flipX = new Vector3(-1,  1,  1);
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

			cast.GetComponentsInChildren(true, vLabelTexts);

			for ( int i = 0 ; i < vLabelTexts.Count ; i++ ) {
				Transform labelTx = vLabelTexts[i].transform;
				labelTx.localScale = Vector3.Scale(labelTx.localScale, flipX);
			}
		}

	}

}
