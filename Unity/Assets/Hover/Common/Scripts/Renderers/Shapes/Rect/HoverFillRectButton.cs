using System;
using Hover.Common.Renderers.Utils;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Shapes.Rect {

	/*================================================================================================*/
	public abstract class HoverFillRectButton : HoverFill {
	
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";
		public const string HighlightProgressName = "HighlightProgress";
		public const string SelectionProgressName = "SelectionProgress";
		
		public abstract HoverMeshRectButton Background { get; }
		public abstract HoverMeshRectButton Highlight { get; }
		public abstract HoverMeshRectButton Selection { get; }
		public abstract HoverMeshRectButton Edge { get; }

		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeX = 10;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float SizeY = 10;
		
		[DisableWhenControlled(RangeMin=0.01f, RangeMax=0.5f)]
		public float EdgeThickness = 0.05f;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float HighlightProgress = 0.7f;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float SelectionProgress = 0.2f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();
			UpdateGeneralSettings();
			UpdateActiveStates();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateGeneralSettings() {
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
			
			Background.OuterAmount = 1;
			Background.InnerAmount = HighlightProgress;
			Highlight.OuterAmount = HighlightProgress;
			Highlight.InnerAmount = SelectionProgress;
			Selection.OuterAmount = SelectionProgress;
			Selection.InnerAmount = 0;
			Edge.OuterAmount = 1;
			Edge.InnerAmount = 1-EdgeThickness/Mathf.Min(SizeX, SizeY);

			Background.SortingLayer = SortingLayer;
			Highlight.SortingLayer = SortingLayer;
			Selection.SortingLayer = SortingLayer;
			Edge.SortingLayer = SortingLayer;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMeshControl(HoverMeshRectButton pMesh) {
			pMesh.Controllers.Set(HoverMeshRectButton.SizeXName, this);
			pMesh.Controllers.Set(HoverMeshRectButton.SizeYName, this);
			pMesh.Controllers.Set(HoverMeshRectButton.OuterAmountName, this);
			pMesh.Controllers.Set(HoverMeshRectButton.InnerAmountName, this);
			pMesh.Controllers.Set(HoverMesh.SortingLayerName, this);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateActiveStates() {
			RendererUtil.SetActiveWithUpdate(Highlight, (Highlight.OuterAmount > 0));
			RendererUtil.SetActiveWithUpdate(Selection, (Selection.OuterAmount > 0));
		}
				
	}

}
