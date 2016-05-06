using System;
using Hover.Board.Renderers.Fills;
using Hover.Board.Renderers.Contents;
using Hover.Board.Renderers.Helpers;
using UnityEngine;

namespace Hover.Board.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererButton : MonoBehaviour {
	
		public bool ControlledByRenderer { get; set; }
	
		public HoverRendererFillRectangleFromCenter Fill;
		public HoverRendererCanvas Canvas;
		
		[Range(0, 100)]
		public float SizeX = 10;
		
		[Range(0, 100)]
		public float SizeY = 10;
		
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
			if ( !ControlledByRenderer ) {
				UpdateAfterRenderer();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void UpdateAfterRenderer() {
			UpdateGeneralSettings();
			UpdateAnchorSettings();

			Fill.UpdateAfterRenderer();
			Canvas.UpdateAfterRenderer();
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
			Canvas.ControlledByRenderer = true;

			Fill.SizeX = SizeX;
			Fill.SizeY = SizeY;
			Canvas.SizeX = SizeX-Fill.EdgeThickness*2;
			Canvas.SizeY = SizeY-Fill.EdgeThickness*2;
			
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
