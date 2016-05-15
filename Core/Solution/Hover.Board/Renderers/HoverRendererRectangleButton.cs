using Hover.Board.Renderers.Fills;
using Hover.Board.Renderers.Contents;
using Hover.Common;
using Hover.Common.Renderers;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Board.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererRectangleButton : MonoBehaviour, IProximityProvider, ISettingsController {
	
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";
		public const string AlphaName = "Alpha";

		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(DisplayMessage=true)]
		public HoverRendererFillRectangleFromCenter Fill;

		[DisableWhenControlled]
		public HoverRendererCanvas Canvas;
		
		[Range(0, 100)]
		[DisableWhenControlled]
		public float SizeX = 10;
		
		[Range(0, 100)]
		[DisableWhenControlled]
		public float SizeY = 10;

		[Range(0, 1)]
		[DisableWhenControlled]
		public float Alpha = 1;
		
		[DisableWhenControlled]
		public AnchorType Anchor = AnchorType.MiddleCenter;
		
		[HideInInspector]
		[SerializeField]
		private bool vIsBuilt;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererRectangleButton() {
			Controllers = new SettingsControllerMap();
		}


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
			UpdateGeneralSettings();
			UpdateAnchorSettings();
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
			Fill.Controllers.Set(HoverRendererFillRectangleFromCenter.SizeXName, this);
			Fill.Controllers.Set(HoverRendererFillRectangleFromCenter.SizeYName, this);
			Fill.Controllers.Set(HoverRendererFillRectangleFromCenter.AlphaName, this);

			Canvas.Controllers.Set(HoverRendererCanvas.SizeXName, this);
			Canvas.Controllers.Set(HoverRendererCanvas.SizeYName, this);
			Canvas.Controllers.Set(HoverRendererCanvas.AlphaName, this);
			Canvas.Controllers.Set(HoverRendererCanvas.RenderQueueName, this);

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
