using Hover.Board.Custom;
using Hover.Board.Input;
using UnityEngine;

namespace Hover.Board.State {

	/*================================================================================================*/
	public class CursorState {

		public CursorType? CursorType { get; private set; }
		public bool IsInputAvailable { get; private set; }
		public Vector3 Position { get; private set; }
		public float Size { get; private set; }

		public Vector3? ProjectedPanelPosition { get; private set; }
		public bool ProjectedFromFront { get; private set; }
		public float ProjectedPanelDistance { get; private set; }
		public float ProjectedPanelProgress { get; private set; }
		public float NearestButtonHighlightProgress { get; set; }

		private readonly InteractionSettings vSettings;
		private readonly Transform vBaseTx;
		private Transform vPanelTx;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorState(InteractionSettings pSettings, Transform pBaseTx) {
			vSettings = pSettings;
			vBaseTx = pBaseTx;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal Vector3 GetWorldPosition() {
			return vBaseTx.TransformPoint(Position);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterInput(IInputCursor pInputCursor) {
			if ( pInputCursor == null ) {
				CursorType = null;
				IsInputAvailable = false;
				Position = Vector3.zero;
				Size = 0;
				return;
			}

			CursorType = pInputCursor.Type;
			IsInputAvailable = pInputCursor.IsAvailable;
			Size = pInputCursor.Size;

			Position = pInputCursor.Position+
				pInputCursor.Rotation*Vector3.back*vSettings.CursorForwardDistance;
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void SetNearestPanelTransform(Transform pPanelTx) {
			vPanelTx = pPanelTx;

			if ( vPanelTx == null ) {
				ProjectedPanelPosition = null;
				ProjectedPanelDistance = 0;
				return;
			}

			Vector3 worldPos = GetWorldPosition();
			Vector3 diff = (pPanelTx.position-worldPos);
			Vector3 norm = pPanelTx.rotation*Vector3.down; //TODO: make this Vector3.forward? up?
			float dist = Vector3.Dot(norm, diff);
			Vector3 projWorldPos = worldPos + norm*dist;
			Vector3 projPos = vBaseTx.InverseTransformPoint(projWorldPos);

			ProjectedPanelPosition = projPos;
			ProjectedFromFront = (dist > 0);
			ProjectedPanelDistance = (projPos-Position).magnitude;
			ProjectedPanelProgress = Mathf.InverseLerp(vSettings.HighlightDistanceMax,
				vSettings.HighlightDistanceMin, (projWorldPos-worldPos).magnitude);
		}

	}

}
