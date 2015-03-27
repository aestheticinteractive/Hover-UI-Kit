using Hover.Board.Custom;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class ProjectionState {

		public bool IsActive { get; set; }
		public ICursorState Cursor { get; private set; }

		public Vector3? ProjectedPanelPosition { get; private set; }
		public bool ProjectedFromFront { get; private set; }
		public float ProjectedPanelDistance { get; private set; }
		public float ProjectedPanelProgress { get; private set; }
		public float NearestItemHighlightProgress { get; set; }

		private readonly InteractionSettings vSettings;
		private readonly Transform vBaseTx;

		private Transform vPanelTx;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ProjectionState(ICursorState pCursor, InteractionSettings pSettings, Transform pBaseTx) {
			Cursor = pCursor;
			vSettings = pSettings;
			vBaseTx = pBaseTx;
			IsActive = true;
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

			Vector3 cursorWorldPos = Cursor.GetWorldPosition();
			Vector3 diff = pPanelTx.position-cursorWorldPos;
			Vector3 norm = pPanelTx.rotation*Vector3.down; //TODO: make this Vector3.forward? up?
			float normLength = Vector3.Dot(norm, diff);
			Vector3 projWorldPos = cursorWorldPos + norm*normLength;
			Vector3 projPos = vBaseTx.InverseTransformPoint(projWorldPos);

			ProjectedPanelPosition = projPos;
			ProjectedFromFront = (normLength > 0);
			ProjectedPanelDistance = (projPos-Cursor.Position).magnitude;
			ProjectedPanelProgress = Mathf.InverseLerp(vSettings.HighlightDistanceMax,
				vSettings.HighlightDistanceMin, ProjectedPanelDistance);
		}

	}

}
