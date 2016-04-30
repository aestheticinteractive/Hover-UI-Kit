using System;
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
		
		public HoverRendererHollowRectangle Background;
		public HoverRendererHollowRectangle Highlight;
		public HoverRendererHollowRectangle Selection;
		public HoverRendererHollowRectangle Edge;
		public HoverRendererCanvas Canvas;
		
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
			UpdateGeneralSettings();
			UpdateActiveStates();
			UpdateAnchorSettings();

			Background.UpdateAfterRenderer();
			Highlight.UpdateAfterRenderer();
			Selection.UpdateAfterRenderer();
			Edge.UpdateAfterRenderer();
			Canvas.UpdateAfterRenderer();
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


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateGeneralSettings() {
			SelectionProgress = Mathf.Min(HighlightProgress, SelectionProgress);

			Background.ControlledByRenderer = true;
			Highlight.ControlledByRenderer = true;
			Selection.ControlledByRenderer = true;
			Edge.ControlledByRenderer = true;
			Canvas.ControlledByRenderer = true;

			Background.SizeX = SizeX;
			Background.SizeY = SizeY;
			Highlight.SizeX = SizeX;
			Highlight.SizeY = SizeY;
			Selection.SizeX = SizeX;
			Selection.SizeY = SizeY;
			Edge.SizeX = SizeX;
			Edge.SizeY = SizeY;
			Canvas.SizeX = SizeX-EdgeThickness*2;
			Canvas.SizeY = SizeY-EdgeThickness*2;
			
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
			
			Canvas.RenderQueue = Background.MaterialRenderQueue+1;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateActiveStates() {
			Highlight.gameObject.SetActive(Highlight.OuterAmount > 0);
			Selection.gameObject.SetActive(Selection.OuterAmount > 0);
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
		
	}

}
