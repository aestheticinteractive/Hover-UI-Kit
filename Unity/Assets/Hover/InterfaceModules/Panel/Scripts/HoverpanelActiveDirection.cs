using Hover.Core.Layouts.Rect;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.InterfaceModules.Panel {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverpanelInterface))]
	[RequireComponent(typeof(HoverpanelRowTransitioner))]
	public class HoverpanelActiveDirection : TreeUpdateableBehavior, ISettingsController {

		public const string ActiveWhenFacingTransformName = "ActiveWhenFacingTransform";

		public ISettingsControllerMap Controllers { get; private set; }

		[SerializeField]
		[FormerlySerializedAs("ActiveWhenFacingMainCamera")]
		private bool _ActiveWhenFacingMainCamera = true;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("ActiveWhenFacingTransform")]
		private Transform _ActiveWhenFacingTransform;

		[SerializeField]
		[FormerlySerializedAs("OnlyDuringTransitions")]
		private bool _OnlyDuringTransitions = true;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverpanelActiveDirection() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool ActiveWhenFacingMainCamera {
			get => _ActiveWhenFacingMainCamera;
			set => this.UpdateValueWithTreeMessage(ref _ActiveWhenFacingMainCamera, value, "ActFaceMc");
		}

		/*--------------------------------------------------------------------------------------------*/
		public Transform ActiveWhenFacingTransform {
			get => _ActiveWhenFacingTransform;
			set => this.UpdateValueWithTreeMessage(ref _ActiveWhenFacingTransform, value, "ActFaceTx");
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool OnlyDuringTransitions {
			get => _OnlyDuringTransitions;
			set => this.UpdateValueWithTreeMessage(ref _OnlyDuringTransitions, value, "OnlyDurTrans");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
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
			}

			if ( ActiveWhenFacingMainCamera || ActiveWhenFacingTransform == null ) {
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
