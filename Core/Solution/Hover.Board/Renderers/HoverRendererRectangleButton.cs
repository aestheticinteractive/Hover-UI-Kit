using Hover.Board.Renderers.Fills;
using Hover.Board.Renderers.Contents;
using Hover.Board.Renderers.Helpers;
using Hover.Common.Renderers;
using UnityEngine;

namespace Hover.Board.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererRectangleButton : MonoBehaviour, IProximityProvider, ISettingsController {
	
		public ISettingsController ParentController { get; set; }
		public ISettingsController ParentRenderer { get; set; }
	
		public HoverRendererFillRectangleFromCenter Fill;
		public HoverRendererCanvas Canvas;
		
		[Range(0, 100)]
		public float SizeX = 10;
		
		[Range(0, 100)]
		public float SizeY = 10;

		[Range(0, 1)]
		public float Alpha = 1;
		
		public AnchorType Anchor = AnchorType.MiddleCenter;
		
		[HideInInspector]
		[SerializeField]
		private bool vIsBuilt;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !vIsBuilt ) {
				BuildElements();
				vIsBuilt = true;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( RendererHelper.IsUpdatePreventedBy(ParentRenderer) ||
					RendererHelper.IsUpdatePreventedBy(ParentController) ) {
				return;
			}

			UpdateAfterParent();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterParent() {
			if ( RendererHelper.IsUpdatePreventedBySelf(this) ) {
				return;
			}

			UpdateGeneralSettings();
			UpdateAnchorSettings();

			Fill.UpdateAfterRenderer();
			Canvas.UpdateAfterParent();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			return RendererHelper.GetNearestWorldPositionOnRectangle(
				pFromWorldPosition, Fill.transform, SizeX, SizeY);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildElements() {
			Fill = BuildFill();
			Canvas = BuildCanvas();
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererFillRectangleFromCenter BuildFill() {
			var rectGo = new GameObject("Fill");
			rectGo.transform.SetParent(gameObject.transform, false);
			return rectGo.AddComponent<HoverRendererFillRectangleFromCenter>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererCanvas BuildCanvas() {
			var canvasGo = new GameObject("Canvas");
			canvasGo.transform.SetParent(gameObject.transform, false);
			return canvasGo.AddComponent<HoverRendererCanvas>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateGeneralSettings() {
			Fill.ControlledByRenderer = true;
			Canvas.ParentRenderer = this;

			Fill.SizeX = SizeX;
			Fill.SizeY = SizeY;
			Canvas.SizeX = SizeX-Fill.EdgeThickness*2;
			Canvas.SizeY = SizeY-Fill.EdgeThickness*2;
			
			Fill.Alpha = Alpha;
			Canvas.Alpha = Alpha;

			Canvas.RenderQueue = Fill.MaterialRenderQueue+1;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateAnchorSettings() {
			if ( Anchor == AnchorType.Custom ) {
				return;
			}
			
			Vector2 anchorPos = RendererHelper.GetRelativeAnchorPosition(Anchor);
			var localPos = new Vector3(SizeX*anchorPos.x, SizeY*anchorPos.y, 0);
			
			Fill.transform.localPosition = localPos;
			Canvas.transform.localPosition = localPos;
		}
		
	}

}
