using System;
using Hover.Common.Renderers.Helpers;
using Hover.Common.Renderers.Meshes;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Fills {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererFillRectangleFromCenter : HoverRendererFill, ISettingsController {
	
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";
		public const string AlphaName = "Alpha";
		public const string HighlightProgressName = "HighlightProgress";
		public const string SelectionProgressName = "SelectionProgress";

		[DisableWhenControlled(DisplayMessage=true)]
		public HoverRendererMeshHollowRectangle Background;

		[DisableWhenControlled]
		public HoverRendererMeshHollowRectangle Highlight;

		[DisableWhenControlled]
		public HoverRendererMeshHollowRectangle Selection;

		[DisableWhenControlled]
		public HoverRendererMeshHollowRectangle Edge;
		
		[Range(0, 100)]
		[DisableWhenControlled]
		public float SizeX = 10;
		
		[Range(0, 100)]
		[DisableWhenControlled]
		public float SizeY = 10;
		
		[Range(0, 1)]
		[DisableWhenControlled]
		public float Alpha = 1;
		
		[Range(0.001f, 0.5f)]
		[DisableWhenControlled]
		public float EdgeThickness = 0.02f;
		
		[Range(0, 1)]
		[DisableWhenControlled]
		public float HighlightProgress = 0.7f;
		
		[Range(0, 1)]
		[DisableWhenControlled]
		public float SelectionProgress = 0.2f;
		
		[DisableWhenControlled]
		public bool UseUvRelativeToSize = false;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int MaterialRenderQueue {
			get {
				return Background.MaterialRenderQueue;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			UpdateGeneralSettings();
			UpdateActiveStates();
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
			
			Highlight.Controllers.Set("GameObject.activeSelf", this);
			Selection.Controllers.Set("GameObject.activeSelf", this);
			SetControllers(Background);
			SetControllers(Highlight);
			SetControllers(Selection);
			SetControllers(Edge);
			
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
		private void SetControllers(HoverRendererMeshHollowRectangle pMesh) {
			pMesh.Controllers.Set(HoverRendererMeshHollowRectangle.SizeXName, this);
			pMesh.Controllers.Set(HoverRendererMeshHollowRectangle.SizeYName, this);
			pMesh.Controllers.Set(HoverRendererMeshHollowRectangle.AlphaName, this);
			pMesh.Controllers.Set(HoverRendererMeshHollowRectangle.OuterAmountName, this);
			pMesh.Controllers.Set(HoverRendererMeshHollowRectangle.InnerAmountName, this);
			pMesh.Controllers.Set(HoverRendererMeshHollowRectangle.UseUvRelativeToSizeName, this);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateActiveStates() {
			RendererHelper.SetActiveWithUpdate(Highlight, (Highlight.OuterAmount > 0));
			RendererHelper.SetActiveWithUpdate(Selection, (Selection.OuterAmount > 0));
		}
				
	}

}
