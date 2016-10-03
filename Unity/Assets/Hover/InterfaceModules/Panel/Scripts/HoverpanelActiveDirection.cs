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

		public const string ActiveWhenFacingTransformName = "ActiveWhenFacingTransform";

		public ISettingsControllerMap Controllers { get; private set; }

		public bool ActiveWhenFacingMainCamera = true;

		[DisableWhenControlled]
		public Transform ActiveWhenFacingTransform;

		public bool OnlyDuringTransitions = true;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverpanelActiveDirection() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			UpdateFacingTransform();

			HoverpanelInterface panel = GetComponent<HoverpanelInterface>();
			HoverpanelRowTransitioner trans = GetComponent<HoverpanelRowTransitioner>();

			if ( OnlyDuringTransitions && !trans.IsTransitionActive ) {
				return;
			}

			UpdateRow(panel.ActiveRow);
			UpdateRow(panel.PreviousRow);
			Controllers.TryExpireControllers();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateFacingTransform() {
			if ( ActiveWhenFacingMainCamera ) {
				Controllers.Set(ActiveWhenFacingTransformName, this);
				ActiveWhenFacingTransform = null;
			}

			if ( ActiveWhenFacingTransform == null ) {
				ActiveWhenFacingTransform = (Camera.main == null ? transform : Camera.main.transform);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateRow(HoverLayoutRectRow pRow) {
			if ( pRow == null || !pRow.gameObject.activeSelf ) {
				return;
			}

			Vector3 panelWorldNorm = pRow.transform.TransformDirection(Vector3.back);
			Vector3 panelToTxWorldVec = (ActiveWhenFacingTransform.position-pRow.transform.position);
			Vector3 panelToTxWorldDir = panelToTxWorldVec.normalized;
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
