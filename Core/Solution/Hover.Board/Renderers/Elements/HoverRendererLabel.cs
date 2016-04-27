using UnityEngine;
using UnityEngine.UI;

namespace Hover.Board.Renderers.Elements {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererLabel : MonoBehaviour {

		public bool ControlledByRenderer { get; set; }
		public float CanvasSizeX { get; private set; }
		public float CanvasSizeY { get; private set; }
		
		public Canvas Canvas;
		public Text Text;
		
		[Range(0.01f, 1)]
		public float CanvasScale = 0.02f;
		
		[Range(0, 100)]
		public float SizeX = 10;

		[Range(0, 100)]
		public float SizeY = 10;
		
		[Range(0, 20)]
		public float PaddingX = 0.5f;
		
		[Range(0, 50)]
		public float InsetL = 0;
		
		[Range(0, 50)]
		public float InsetR = 0;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( Canvas == null ) {
				BuildElements();
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
			Canvas.transform.localScale = Vector3.one*CanvasScale;
			CanvasSizeX = SizeX/CanvasScale;
			CanvasSizeY = SizeY/CanvasScale;

			RectTransform canvasRect = Canvas.GetComponent<RectTransform>();
			canvasRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CanvasSizeX);
			canvasRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CanvasSizeY);
			
			float textX = (PaddingX+InsetL)/CanvasScale;
			float textSizeX = (SizeX-PaddingX*2-InsetL-InsetR)/CanvasScale;

			RectTransform textRect = Text.GetComponent<RectTransform>();
			textRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, textX, textSizeX);
			textRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, CanvasSizeY);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildElements() {
			var canvasGo = new GameObject("Canvas");
			canvasGo.transform.SetParent(gameObject.transform, false);

			Canvas = canvasGo.AddComponent<Canvas>();
			Canvas.renderMode = RenderMode.WorldSpace;

			var textGo = new GameObject("Text");
			textGo.transform.SetParent(canvasGo.transform, false);
			
			Text = textGo.AddComponent<Text>();
			Text.font = Resources.Load<Font>("Fonts/Tahoma");
			Text.fontSize = 40;
			Text.lineSpacing = 0.75f;
			Text.color = Color.white;
			Text.alignment = TextAnchor.MiddleCenter;
			Text.text = "Label";
			Text.raycastTarget = false;
		}

	}

}
