using Hover.Core.Layouts.Rect;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.InterfaceModules.Panel {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverpanelInterface))]
	[RequireComponent(typeof(HoverpanelRowTransitioner))]
	public class HoverpanelActiveDirection : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public Transform ActiveWhenFacing;
		public bool OnlyDuringTransitions = true;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( ActiveWhenFacing == null ) {
				ActiveWhenFacing = Camera.main.transform;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			HoverpanelInterface panel = GetComponent<HoverpanelInterface>();
			HoverpanelRowTransitioner trans = GetComponent<HoverpanelRowTransitioner>();

			if ( OnlyDuringTransitions && !trans.IsTransitionActive ) {
				return;
			}

			UpdateRow(panel.ActiveRow);
			UpdateRow(panel.PreviousRow);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateRow(HoverLayoutRectRow pRow) {
			if ( pRow == null || !pRow.gameObject.activeSelf ) {
				return;
			}

			Vector3 panelWorldNorm = pRow.transform.TransformDirection(Vector3.back);
			Vector3 panelToTxWorldDir = (ActiveWhenFacing.position-pRow.transform.position).normalized;
			float dotBetweenDirs = Vector3.Dot(panelWorldNorm, panelToTxWorldDir);

			if ( dotBetweenDirs > 0 ) {
				return;
			}

			pRow.gameObject.SetActive(false);

			//Vector3 panelPos = pRow.transform.position;
			//Debug.DrawLine(panelPos, panelPos+panelWorldNorm, Color.red);
			//Debug.DrawLine(panelPos, panelPos+panelToTxWorldDir, Color.cyan);
		}

	}

}
