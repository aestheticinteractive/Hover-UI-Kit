using Hover.Common.Items;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Common.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverItemData))]
	[RequireComponent(typeof(HoverItemCursorActivity))]
	[RequireComponent(typeof(HoverItemSelectionActivity))]
	public abstract class HoverRendererController : MonoBehaviour, IProximityProvider {

		public bool ShowProximityDebugLines = true;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			HoverItemCursorActivity hoverItemCursorAct = GetComponent<HoverItemCursorActivity>();
			
			if ( hoverItemCursorAct.ProximityProvider == null ) {
				hoverItemCursorAct.ProximityProvider = this;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			if ( ShowProximityDebugLines ) {
				DrawProximityDebugLines();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public abstract Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected void DrawProximityDebugLines() {
			HoverItemCursorActivity.Highlight? nearHigh = 
				GetComponent<HoverItemCursorActivity>().NearestHighlight;

			if ( nearHigh == null ) {
				return;
			}

			Vector3 cursorPos = nearHigh.Value.Data.transform.position;
			Vector3 nearPos = nearHigh.Value.NearestWorldPos;
			float prog = nearHigh.Value.Progress;
			Color color = (prog >= 1 ? new Color(0.3f, 1, 0.4f, 1) : 
				(prog <= 0 ? new Color(1, 0, 0, 0.25f) : new Color(1, 1, 0, prog*0.5f+0.25f)));

			Debug.DrawLine(nearPos, cursorPos, color);
		}
		
	}

}
