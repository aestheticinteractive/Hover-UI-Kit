using Hover.Board.Custom;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class ProjectionState {

		public ICursorState CursorState { get; private set; }

		public Vector3? ProjectedPanelPosition { get; private set; }
		public bool ProjectedFromFront { get; private set; }
		public float ProjectedPanelDistance { get; private set; }
		public float ProjectedPanelProgress { get; private set; }

		private readonly InteractionSettings vSettings;
		private readonly Transform vBaseTx;
		private readonly ICursorInteractState vCursorInteractState;

		private Transform vPanelTx;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ProjectionState(ICursorState pCursorState, InteractionSettings pSettings, 
																					Transform pBaseTx) {
			CursorState = pCursorState;
			vSettings = pSettings;
			vBaseTx = pBaseTx;

			vCursorInteractState = pCursorState.AddOrGetInteractionState(CursorDomain.Hoverboard, "");
			vCursorInteractState.DisplayStrength = 1;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void SetNearestPanelTransform(Transform pPanelTx) {
			vPanelTx = pPanelTx;

			if ( vPanelTx == null ) {
				ProjectedPanelPosition = null;
				ProjectedPanelDistance = 0;
				return;
			}

			Vector3 worldPos = CursorState.GetWorldPosition();
			Vector3 diff = (pPanelTx.position-worldPos);
			Vector3 norm = pPanelTx.rotation*Vector3.down; //TODO: make this Vector3.forward? up?
			float dist = Vector3.Dot(norm, diff);
			Vector3 projWorldPos = worldPos + norm*dist;
			Vector3 projPos = vBaseTx.InverseTransformPoint(projWorldPos);

			ProjectedPanelPosition = projPos;
			ProjectedFromFront = (dist > 0);
			ProjectedPanelDistance = (projPos-CursorState.Position).magnitude;
			ProjectedPanelProgress = Mathf.InverseLerp(vSettings.HighlightDistanceMax,
				vSettings.HighlightDistanceMin, (projWorldPos-worldPos).magnitude);
		}

		/*--------------------------------------------------------------------------------------------*/
		public float NearestButtonHighlightProgress {
			get {
				return vCursorInteractState.HighlightProgress;
			}
			set {
				vCursorInteractState.HighlightProgress = value;
			}
		}

	}

}
