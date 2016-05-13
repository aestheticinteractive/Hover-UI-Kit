using System;
using Hover.Board.Renderers.Meshes;
using UnityEngine;

namespace Hover.Board.Renderers.Fills {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererFillRectangleFromCenter : HoverRendererFill {
	
		public HoverRendererMeshHollowRectangle Background;
		public HoverRendererMeshHollowRectangle Highlight;
		public HoverRendererMeshHollowRectangle Selection;
		public HoverRendererMeshHollowRectangle Edge;
		
		[Range(0, 100)]
		public float SizeX = 10;
		
		[Range(0, 100)]
		public float SizeY = 10;
		
		[Range(0, 1)]
		public float Alpha = 1;
		
		[Range(0.001f, 0.5f)]
		public float EdgeThickness = 0.02f;
		
		[Range(0, 1)]
		public float HighlightProgress = 0.7f;
		
		[Range(0, 1)]
		public float SelectionProgress = 0.2f;
		
		public bool UseUvRelativeToSize = false;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int MaterialRenderQueue {
			get {
				return Background.MaterialRenderQueue;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void UpdateAfterRenderer() {
			UpdateGeneralSettings();
			UpdateActiveStates();

			Background.UpdateAfterRenderer();
			Highlight.UpdateAfterRenderer();
			Selection.UpdateAfterRenderer();
			Edge.UpdateAfterRenderer();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void BuildElements() {
			Background = BuildHollowRect("Background");
			Highlight = BuildHollowRect("Highlight");
			Selection = BuildHollowRect("Selection");
			Edge = BuildHollowRect("Edge");

			Background.FillColor = new Color(0.1f, 0.1f, 0.1f, 0.666f);
			Highlight.FillColor = new Color(0.1f, 0.5f, 0.9f);
			Selection.FillColor = new Color(0.1f, 0.9f, 0.2f);
			Edge.FillColor = new Color(1, 1, 1, 1);
		}

		/*--------------------------------------------------------------------------------------------*/
		private HoverRendererMeshHollowRectangle BuildHollowRect(string pName) {
			var rectGo = new GameObject(pName);
			rectGo.transform.SetParent(gameObject.transform, false);
			return rectGo.AddComponent<HoverRendererMeshHollowRectangle>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateGeneralSettings() {
			SelectionProgress = Mathf.Min(HighlightProgress, SelectionProgress);

			Background.ControlledByRenderer = true;
			Highlight.ControlledByRenderer = true;
			Selection.ControlledByRenderer = true;
			Edge.ControlledByRenderer = true;
			
			float insetSizeX = Math.Max(0, SizeX-EdgeThickness);
			float insetSizeY = Math.Max(0, SizeY-EdgeThickness);

			Background.SizeX = insetSizeX;
			Background.SizeY = insetSizeY;
			Highlight.SizeX = insetSizeX;
			Highlight.SizeY = insetSizeY;
			Selection.SizeX = insetSizeX;
			Selection.SizeY = insetSizeY;
			Edge.SizeX = SizeX;
			Edge.SizeY = SizeY;

			Background.Alpha = Alpha;
			Highlight.Alpha = Alpha;
			Selection.Alpha = Alpha;
			Edge.Alpha = Alpha;
			
			Background.OuterAmount = 1;
			Background.InnerAmount = HighlightProgress;
			Highlight.OuterAmount = HighlightProgress;
			Highlight.InnerAmount = SelectionProgress;
			Selection.OuterAmount = SelectionProgress;
			Selection.InnerAmount = 0;
			Edge.OuterAmount = 1;
			Edge.InnerAmount = 1-EdgeThickness/Mathf.Min(SizeX, SizeY);
			
			Background.UseUvRelativeToSize = UseUvRelativeToSize;
			Highlight.UseUvRelativeToSize = UseUvRelativeToSize;
			Selection.UseUvRelativeToSize = UseUvRelativeToSize;
			Edge.UseUvRelativeToSize = UseUvRelativeToSize;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateActiveStates() {
			Highlight.gameObject.SetActive(Highlight.OuterAmount > 0);
			Selection.gameObject.SetActive(Selection.OuterAmount > 0);
		}
				
	}

}
