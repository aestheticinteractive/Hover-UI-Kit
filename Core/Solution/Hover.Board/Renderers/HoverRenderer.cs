using System;
using Hover.Board.Display.Standard;
using Hover.Board.Renderers.Elements;
using UnityEngine;

namespace Hover.Board.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRenderer : MonoBehaviour {
	
		public enum AnchorType {
			UpperLeft,
			UpperCenter,
			UpperRight,
			MiddleLeft,
			MiddleCenter,
			MiddleRight,
			LowerLeft,
			LowerCenter,
			LowerRight,
			Custom
		}
		
		public enum CanvasAlignmentType {
			Left,
			Center,
			Right,
			Custom
		}
		
		public enum IconSizeType {
			FontSize,
			ThreeQuartersFontSize,
			OneAndHalfFontSize,
			DoubleFontSize,
			Custom
		}

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
		
		[Range(0, 50)]
		public float CanvasPaddingX = 0.5f;
		
		[Range(0, 50)]
		public float CanvasPaddingY = 0.5f;
		
		public AnchorType Anchor = AnchorType.MiddleCenter;
		public CanvasAlignmentType CanvasAlignment = CanvasAlignmentType.Left;
		public IconSizeType IconSize = IconSizeType.FontSize;
		
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
			
			Label.CanvasScale = Canvas.Scale;
			Icon.CanvasScale = Canvas.Scale;

			Background.SizeX = SizeX;
			Background.SizeY = SizeY;
			Highlight.SizeX = SizeX;
			Highlight.SizeY = SizeY;
			Selection.SizeX = SizeX;
			Selection.SizeY = SizeY;
			Edge.SizeX = SizeX;
			Edge.SizeY = SizeY;
			Canvas.SizeX = SizeX-(CanvasPaddingX+EdgeThickness)*2;
			Canvas.SizeY = SizeY-(CanvasPaddingY+EdgeThickness)*2;
			
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
			
			////

			int canvasRenderQueue = Background.MaterialRenderQueue+1;

			Label.TextComponent.material.renderQueue = canvasRenderQueue;
			Icon.ImageComponent.material.renderQueue = canvasRenderQueue;
			
			UpdateAnchorSettings();
			UpdateIconSizeSettings();
			UpdateCanvasAlignmentSettings();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateAnchorSettings() {
			if ( Anchor == AnchorType.Custom ) {
				return;
			}
			
			int ai = (int)Anchor;
			float x = (ai%3)/2f - 0.5f;
			float y = (ai/3)/2f - 0.5f;
			var localPos = new Vector3(-SizeX*x, SizeY*y, 0);
			
			Background.transform.localPosition = localPos;
			Highlight.transform.localPosition = localPos;
			Selection.transform.localPosition = localPos;
			Edge.transform.localPosition = localPos;
			Canvas.transform.localPosition = localPos;
		}
				
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateIconSizeSettings() {
			if ( IconSize == IconSizeType.Custom ) {
				return;
			}
			
			float fontSize = Label.TextComponent.fontSize*Label.CanvasScale;
			
			switch ( IconSize ) {
				case IconSizeType.FontSize:
					Icon.SizeX = fontSize;
					break;
					
				case IconSizeType.ThreeQuartersFontSize:
					Icon.SizeX = fontSize*0.75f;
					break;
					
				case IconSizeType.OneAndHalfFontSize:
					Icon.SizeX = fontSize*1.5f;
					break;
					
				case IconSizeType.DoubleFontSize:
					Icon.SizeX = fontSize*2;
					break;
			}
			
			Icon.SizeY = Icon.SizeX;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCanvasAlignmentSettings() {
			if ( CanvasAlignment == CanvasAlignmentType.Custom ) {
				return;
			}
		
			const float iconVertShiftMult = -0.35f;
			const float labelHorizInsetMult = 1.2f;
			
			float fontSize = Label.TextComponent.fontSize*Label.CanvasScale/2;
			float iconAvailW = Canvas.SizeX-Icon.SizeX;
			float iconShiftX = 0;
			float iconShiftY = 0;
			float labelInsetL = 0;
			float labelInsetR = 0;
			float labelInsetT = 0;
			TextAnchor labelAlign;
			
			switch ( CanvasAlignment ) {
				case CanvasAlignmentType.Left:
					iconShiftX = -0.5f*iconAvailW;
					iconShiftY = iconVertShiftMult*fontSize;
					labelInsetL = Icon.SizeX*labelHorizInsetMult;
					labelAlign = TextAnchor.MiddleLeft;
					break;
					
				case CanvasAlignmentType.Center:
					iconShiftY = fontSize/2;
					labelInsetT = Icon.SizeY/2;
					labelAlign = TextAnchor.MiddleCenter;
					break;
					
				case CanvasAlignmentType.Right:
					iconShiftX = 0.5f*iconAvailW;
					iconShiftY = iconVertShiftMult*fontSize;
					labelInsetR = Icon.SizeX*labelHorizInsetMult;
					labelAlign = TextAnchor.MiddleRight;
					break;
				
				default:
					throw new Exception("Unhandled alignment: "+CanvasAlignment);
			}
			
			var labelLocalPos = new Vector3((labelInsetL-labelInsetR)/2, -labelInsetT, 0);
			var iconLocalPos = new Vector3(iconShiftX, iconShiftY, 0);
			
			Label.SizeX = Canvas.SizeX-labelInsetL-labelInsetR;
			Label.SizeY = Canvas.SizeY-labelInsetT;
			
			Label.TextComponent.alignment = labelAlign;
			
			Label.transform.localPosition = labelLocalPos/Canvas.Scale;
			Icon.transform.localPosition = iconLocalPos/Canvas.Scale;
		}
		
	}

}
