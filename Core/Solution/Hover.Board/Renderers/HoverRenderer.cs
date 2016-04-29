using Hover.Board.Display.Standard;
using Hover.Board.Renderers.Elements;
using UnityEngine;

namespace Hover.Board.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRenderer : MonoBehaviour {

		public HoverRendererHollowRectangle Background;
		public HoverRendererHollowRectangle Highlight;
		public HoverRendererHollowRectangle Selection;
		public HoverRendererHollowRectangle Edge;
		public HoverRendererCanvas Canvas;
		public HoverRendererLabel Label;
		public HoverRendererIcon Icon;
		
		[Range(0, 100)]
		public float SizeX = 10;
		
		[Range(0, 100)]
		public float SizeY = 10;

		[Range(0.001f, 0.5f)]
		public float EdgeThickness = 0.02f;
		
		[Range(0, 1)]
		public float HighlightProgress = 0.7f;
		
		[Range(0, 1)]
		public float SelectionProgress = 0.2f;

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
			UpdateSettings();

			Background.UpdateAfterRenderer();
			Highlight.UpdateAfterRenderer();
			Selection.UpdateAfterRenderer();
			Edge.UpdateAfterRenderer();
			Canvas.UpdateAfterRenderer();
			Label.UpdateAfterRenderer();
			Icon.UpdateAfterRenderer();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildElements() {
			Background = BuildHollowRect("Background");
			Highlight = BuildHollowRect("Highlight");
			Selection = BuildHollowRect("Selection");
			Edge = BuildHollowRect("Edge");

			Background.FillColor = new Color(0.1f, 0.1f, 0.1f, 0.666f);
			Highlight.FillColor = new Color(0.1f, 0.5f, 0.9f);
			Selection.FillColor = new Color(0.1f, 0.9f, 0.2f);
			Edge.FillColor = new Color(1, 1, 1, 1);

			Canvas = BuildCanvas();
			Label = BuildLabel(Canvas);
			Icon = BuildIcon(Canvas);
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererHollowRectangle BuildHollowRect(string pName) {
			var rectGo = new GameObject(pName);
			rectGo.transform.SetParent(gameObject.transform, false);
			return rectGo.AddComponent<HoverRendererHollowRectangle>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererCanvas BuildCanvas() {
			var canvasGo = new GameObject("Canvas");
			canvasGo.transform.SetParent(gameObject.transform, false);
			return canvasGo.AddComponent<HoverRendererCanvas>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererLabel BuildLabel(HoverRendererCanvas pCanvas) {
			var labelGo = new GameObject("Label");
			labelGo.transform.SetParent(pCanvas.gameObject.transform, false);
			return labelGo.AddComponent<HoverRendererLabel>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererIcon BuildIcon(HoverRendererCanvas pCanvas) {
			var iconGo = new GameObject("Icon");
			iconGo.transform.SetParent(pCanvas.gameObject.transform, false);
			return iconGo.AddComponent<HoverRendererIcon>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSettings() {
			SelectionProgress = Mathf.Min(HighlightProgress, SelectionProgress);

			Background.ControlledByRenderer = true;
			Highlight.ControlledByRenderer = true;
			Selection.ControlledByRenderer = true;
			Edge.ControlledByRenderer = true;
			Canvas.ControlledByRenderer = true;
			Label.ControlledByRenderer = true;
			Icon.ControlledByRenderer = true;
			
			Label.CanvasScale = Canvas.CanvasScale;
			Icon.CanvasScale = Canvas.CanvasScale;

			Background.SizeX = SizeX;
			Background.SizeY = SizeY;
			Highlight.SizeX = SizeX;
			Highlight.SizeY = SizeY;
			Selection.SizeX = SizeX;
			Selection.SizeY = SizeY;
			Edge.SizeX = SizeX;
			Edge.SizeY = SizeY;
			Canvas.SizeX = SizeX;
			Canvas.SizeY = SizeY;
			Label.SizeX = SizeX;
			Label.SizeY = SizeY;
			Icon.SizeX = Label.TextComponent.fontSize*Label.CanvasScale;
			Icon.SizeY = Icon.SizeX;
			
			Background.Inset = EdgeThickness;
			Highlight.Inset = EdgeThickness;
			Selection.Inset = EdgeThickness;

			Background.OuterAmount = 1;
			Background.InnerAmount = HighlightProgress;
			Highlight.OuterAmount = HighlightProgress;
			Highlight.InnerAmount = SelectionProgress;
			Selection.OuterAmount = SelectionProgress;
			Selection.InnerAmount = 0;
			Edge.OuterAmount = 1;
			Edge.InnerAmount = 1-EdgeThickness/Mathf.Min(SizeX, SizeY);

			int canvasRenderQueue = Background.MaterialRenderQueue+1;

			Label.TextComponent.material.renderQueue = canvasRenderQueue;
			Icon.ImageComponent.material.renderQueue = canvasRenderQueue;
		}

	}

}
