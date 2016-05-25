using System;
using Hover.Common.Renderers.Utils;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Shapes.Rect {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverFillRectButton : HoverFill, ISettingsController {
	
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";
		public const string AlphaName = "Alpha";
		public const string HighlightProgressName = "HighlightProgress";
		public const string SelectionProgressName = "SelectionProgress";

		[DisableWhenControlled]
		public HoverMeshRectCentral Background;

		[DisableWhenControlled]
		public HoverMeshRectCentral Highlight;

		[DisableWhenControlled]
		public HoverMeshRectCentral Selection;

		[DisableWhenControlled]
		public HoverMeshRectCentral Edge;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeX = 10;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeY = 10;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float Alpha = 1;
		
		[DisableWhenControlled(RangeMin=0.001f, RangeMax=0.5f)]
		public float EdgeThickness = 0.02f;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float HighlightProgress = 0.7f;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float SelectionProgress = 0.2f;
		
		[DisableWhenControlled]
		public bool UseUvRelativeToSize = false;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
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
		private HoverMeshRectCentral BuildHollowRect(string pName) {
			var rectGo = new GameObject(pName);
			rectGo.transform.SetParent(gameObject.transform, false);
			return rectGo.AddComponent<HoverMeshRectCentral>();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateGeneralSettings() {
			SelectionProgress = Mathf.Min(HighlightProgress, SelectionProgress);
			
			Highlight.Controllers.Set("GameObject.activeSelf", this);
			Selection.Controllers.Set("GameObject.activeSelf", this);
			UpdateMeshControl(Background);
			UpdateMeshControl(Highlight);
			UpdateMeshControl(Selection);
			UpdateMeshControl(Edge);
			
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

			Background.SortingLayer = SortingLayer;
			Highlight.SortingLayer = SortingLayer;
			Selection.SortingLayer = SortingLayer;
			Edge.SortingLayer = SortingLayer;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMeshControl(HoverMeshRectCentral pMesh) {
			pMesh.Controllers.Set(HoverMeshRectCentral.SizeXName, this);
			pMesh.Controllers.Set(HoverMeshRectCentral.SizeYName, this);
			pMesh.Controllers.Set(HoverMeshRectCentral.AlphaName, this);
			pMesh.Controllers.Set(HoverMeshRectCentral.OuterAmountName, this);
			pMesh.Controllers.Set(HoverMeshRectCentral.InnerAmountName, this);
			pMesh.Controllers.Set(HoverMeshRectCentral.UseUvRelativeToSizeName, this);
			pMesh.Controllers.Set(HoverMesh.SortingLayerName, this);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateActiveStates() {
			RendererHelper.SetActiveWithUpdate(Highlight, (Highlight.OuterAmount > 0));
			RendererHelper.SetActiveWithUpdate(Selection, (Selection.OuterAmount > 0));
		}
				
	}

}
