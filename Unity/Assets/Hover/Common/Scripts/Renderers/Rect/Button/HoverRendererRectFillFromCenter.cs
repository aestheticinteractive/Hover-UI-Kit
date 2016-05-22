using System;
using Hover.Common.Renderers.Shared.Bases;
using Hover.Common.Renderers.Shared.Utils;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Rect.Button {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererRectFillFromCenter : HoverRendererFill, ISettingsController {
	
		public const string SizeXName = "SizeX";
		public const string SizeYName = "SizeY";
		public const string AlphaName = "Alpha";
		public const string HighlightProgressName = "HighlightProgress";
		public const string SelectionProgressName = "SelectionProgress";

		[DisableWhenControlled(DisplayMessage=true)]
		public HoverRendererRectMeshHollow Background;

		[DisableWhenControlled]
		public HoverRendererRectMeshHollow Highlight;

		[DisableWhenControlled]
		public HoverRendererRectMeshHollow Selection;

		[DisableWhenControlled]
		public HoverRendererRectMeshHollow Edge;
		
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
		public override int MaterialRenderQueue {
			get {
				return Background.MaterialRenderQueue;
			}
		}
		
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
		private HoverRendererRectMeshHollow BuildHollowRect(string pName) {
			var rectGo = new GameObject(pName);
			rectGo.transform.SetParent(gameObject.transform, false);
			return rectGo.AddComponent<HoverRendererRectMeshHollow>();
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
		private void SetControllers(HoverRendererRectMeshHollow pMesh) {
			pMesh.Controllers.Set(HoverRendererRectMeshHollow.SizeXName, this);
			pMesh.Controllers.Set(HoverRendererRectMeshHollow.SizeYName, this);
			pMesh.Controllers.Set(HoverRendererRectMeshHollow.AlphaName, this);
			pMesh.Controllers.Set(HoverRendererRectMeshHollow.OuterAmountName, this);
			pMesh.Controllers.Set(HoverRendererRectMeshHollow.InnerAmountName, this);
			pMesh.Controllers.Set(HoverRendererRectMeshHollow.UseUvRelativeToSizeName, this);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateActiveStates() {
			RendererHelper.SetActiveWithUpdate(Highlight, (Highlight.OuterAmount > 0));
			RendererHelper.SetActiveWithUpdate(Selection, (Selection.OuterAmount > 0));
		}
				
	}

}
