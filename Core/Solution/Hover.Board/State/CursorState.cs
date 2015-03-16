using Hover.Board.Custom;
using Hover.Board.Input;
using Hover.Engines;

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

		private readonly IEngine vEngine;
		private readonly InteractionSettings vSettings;
		private readonly ITransform vBaseTx;
		private ITransform vPanelTx;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CursorState(IEngine pEngine, InteractionSettings pSettings, ITransform pBaseTx) {
			vEngine = pEngine;
			vSettings = pSettings;
			vBaseTx = pBaseTx;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal Vector3 GetWorldPosition() {
			return vBaseTx.GetWorldPoint(Position);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterInput(IInputCursor pInputCursor) {
			if ( pInputCursor == null ) {
				CursorType = null;
				IsInputAvailable = false;
				Position = Vector3.Zero;
				Size = 0;
				return;
			}

			CursorType = pInputCursor.Type;
			IsInputAvailable = pInputCursor.IsAvailable;
			Size = pInputCursor.Size;

			Position = pInputCursor.Position+
				vEngine.Math.RotateVector(Engines.Vector3.Back, pInputCursor.Rotation)*
				vSettings.CursorForwardDistance;
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void SetNearestPanelTransform(ITransform pPanelTx) {
			vPanelTx = pPanelTx;

			if ( vPanelTx == null ) {
				ProjectedPanelPosition = null;
				ProjectedPanelDistance = 0;
				return;
			}

			Vector3 worldPos = GetWorldPosition();
			Vector3 diff = (pPanelTx.WorldPosition-worldPos);
			Vector3 norm = pPanelTx.WorldRotation*Vector3.Down; //TODO: make this Vector3.forward? up?
			float dist = norm.DotWith(diff);
			Vector3 projWorldPos = worldPos + norm*dist;
			Vector3 projPos = vBaseTx.InverseTransformPoint(projWorldPos);

			ProjectedPanelPosition = projPos;
			ProjectedFromFront = (dist > 0);
			ProjectedPanelDistance = (projPos-Position).Magnitude;
			ProjectedPanelProgress = Mathf.InverseLerp(vSettings.HighlightDistanceMax,
				vSettings.HighlightDistanceMin, (projWorldPos-worldPos).Magnitude);
		}

	}

}
