using System;
using Hover.Common.Renderers.Utils;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Shapes.Arc {

	/*================================================================================================*/
	public abstract class HoverFillArcButton : HoverFill {
	
		public const string OuterRadiusName = "OuterRadius";
		public const string InnerRadiusName = "InnerRadius";
		public const string ArcAngleName = "ArcAngle";
		public const string HighlightProgressName = "HighlightProgress";
		public const string SelectionProgressName = "SelectionProgress";
		
		public abstract HoverMeshArcButton Background { get; }
		public abstract HoverMeshArcButton Highlight { get; }
		public abstract HoverMeshArcButton Selection { get; }
		public abstract HoverMeshArcButton Edge { get; }
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float OuterRadius = 10;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		public float InnerRadius = 4;

		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		public float ArcAngle = 60;
		
		[DisableWhenControlled(RangeMin=0.01f, RangeMax=0.5f)]
		public float EdgeThickness = 0.05f;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float HighlightProgress = 0.7f;
		
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		public float SelectionProgress = 0.2f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
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
			
			float insetInnerRadius = InnerRadius + EdgeThickness*Mathf.Sign(OuterRadius-InnerRadius);

			Background.OuterRadius = OuterRadius;
			Highlight.OuterRadius = OuterRadius;
			Selection.OuterRadius = OuterRadius;
			Edge.OuterRadius = insetInnerRadius;

			Background.InnerRadius = insetInnerRadius;
			Highlight.InnerRadius = insetInnerRadius;
			Selection.InnerRadius = insetInnerRadius;
			Edge.InnerRadius = InnerRadius;

			Background.ArcAngle = ArcAngle;
			Highlight.ArcAngle = ArcAngle;
			Selection.ArcAngle = ArcAngle;
			Edge.ArcAngle = ArcAngle;
			
			Background.OuterAmount = 1;
			Background.InnerAmount = HighlightProgress;
			Highlight.OuterAmount = HighlightProgress;
			Highlight.InnerAmount = SelectionProgress;
			Selection.OuterAmount = SelectionProgress;
			Selection.InnerAmount = 0;
			Edge.OuterAmount = Mathf.Abs(OuterRadius-InnerRadius)/EdgeThickness;
			Edge.InnerAmount = 0;

			Background.SortingLayer = SortingLayer;
			Highlight.SortingLayer = SortingLayer;
			Selection.SortingLayer = SortingLayer;
			Edge.SortingLayer = SortingLayer;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateMeshControl(HoverMeshArcButton pMesh) {
			pMesh.Controllers.Set(HoverMeshArcButton.OuterRadiusName, this);
			pMesh.Controllers.Set(HoverMeshArcButton.InnerRadiusName, this);
			pMesh.Controllers.Set(HoverMeshArcButton.ArcAngleName, this);
			pMesh.Controllers.Set(HoverMeshArcButton.OuterAmountName, this);
			pMesh.Controllers.Set(HoverMeshArcButton.InnerAmountName, this);
			pMesh.Controllers.Set(HoverMesh.SortingLayerName, this);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateActiveStates() {
			RendererUtil.SetActiveWithUpdate(Highlight, (Highlight.OuterAmount > 0));
			RendererUtil.SetActiveWithUpdate(Selection, (Selection.OuterAmount > 0));
		}
				
	}

}
