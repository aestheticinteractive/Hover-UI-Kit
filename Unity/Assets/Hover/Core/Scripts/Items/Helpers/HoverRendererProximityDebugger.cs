using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Items.Helpers {

#if UNITY_EDITOR
	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverItemRendererUpdater))]
	[RequireComponent(typeof(HoverItemHighlightState))]
	public class HoverRendererProximityDebugger : TreeUpdateableBehavior {

		public Color ZeroColor = new Color(1, 0, 0, 0.2f);
		public Color NearColor = new Color(1, 1, 0, 0.2f);
		public Color FarColor = new Color(1, 1, 0, 0.8f);
		public Color FullColor = new Color(0.3f, 1, 0.4f, 1);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			DrawProximityDebugLines();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void DrawProximityDebugLines() {
			if ( !Application.isPlaying ) {
				return;
			}

			HoverItemHighlightState.Highlight? nearHigh = 
				GetComponent<HoverItemHighlightState>().NearestHighlight;

			if ( nearHigh == null ) {
				return;
			}

			Vector3 cursorPos = nearHigh.Value.Cursor.WorldPosition;
			Vector3 nearPos = nearHigh.Value.NearestWorldPos;
			float prog = nearHigh.Value.Progress;
			Color color = ZeroColor;

			if ( prog >= 1 ) {
				color = FullColor;
			}
			else if ( prog > 0 ) {
				color = Color.Lerp(NearColor, FarColor, prog);
			}

			Debug.DrawLine(nearPos, cursorPos, color);
		}

	}
#else
	/*================================================================================================*/
	public class HoverRendererProximityDebugger : MonoBehaviour {}
#endif

}
