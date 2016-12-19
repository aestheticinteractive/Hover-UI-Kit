using Hover.Core.Items.Managers;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Items.Helpers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverItemRendererUpdater))]
	[RequireComponent(typeof(HoverItemHighlightState))]
	public class HoverRendererProximityDebugger : MonoBehaviour, ITreeUpdateable {

		public Color ZeroColor = new Color(1, 0, 0, 0.2f);
		public Color NearColor = new Color(1, 1, 0, 0.2f);
		public Color FarColor = new Color(1, 1, 0, 0.8f);
		public Color FullColor = new Color(0.3f, 1, 0.4f, 1);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
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

}
