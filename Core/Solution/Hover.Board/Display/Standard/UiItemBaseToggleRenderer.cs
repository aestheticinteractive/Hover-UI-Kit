using Hover.Board.State;
using Hover.Common.Custom;
using Hover.Common.Display;
using Hover.Common.State;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Board.Display.Standard {

	/*================================================================================================*/
	public abstract class UiItemBaseToggleRenderer : UiItemSelectRenderer {

		protected GameObject vOuter;
		protected GameObject vInner;
		protected MeshBuilder vOuterMeshBuilder;
		protected MeshBuilder vInnerMeshBuilder;

		private int vPrevTextSize;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract Materials.IconOffset GetOuterIconOffset();

		/*--------------------------------------------------------------------------------------------*/
		protected abstract Materials.IconOffset GetInnerIconOffset();

		/*--------------------------------------------------------------------------------------------*/
		protected abstract bool IsToggled();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Build(IHoverboardPanelState pPanelState,
										IHoverboardLayoutState pLayoutState, IBaseItemState pItemState,
										IItemVisualSettings pSettings) {
			base.Build(pPanelState, pLayoutState, pItemState, pSettings);

			vLabel.AlignLeft = true;
			vLabel.transform.localPosition = new Vector3(-vItemState.Item.Width/2, 0, 0);

			////

			vOuter = new GameObject("ToggleOuter");
			vOuter.transform.SetParent(gameObject.transform, false);
			vOuter.transform.localRotation = vLabel.CanvasLocalRotation;
			vOuter.AddComponent<MeshRenderer>();

			MeshFilter outerFilt = vOuter.AddComponent<MeshFilter>();
			vOuterMeshBuilder = new MeshBuilder();
			MeshUtil.BuildQuadMesh(vOuterMeshBuilder);
			Materials.SetMeshIconCoords(vOuterMeshBuilder, GetOuterIconOffset());
			vOuterMeshBuilder.Commit();
			vOuterMeshBuilder.CommitColors(Color.clear);
			outerFilt.sharedMesh = vOuterMeshBuilder.Mesh;

			////

			vInner = new GameObject("ToggleInner");
			vInner.transform.SetParent(gameObject.transform, false);
			vInner.transform.localRotation = vLabel.CanvasLocalRotation;
			vInner.AddComponent<MeshRenderer>();

			MeshFilter innerFilt = vInner.AddComponent<MeshFilter>();
			vInnerMeshBuilder = new MeshBuilder();
			MeshUtil.BuildQuadMesh(vInnerMeshBuilder);
			Materials.SetMeshIconCoords(vInnerMeshBuilder, GetInnerIconOffset());
			vInnerMeshBuilder.Commit();
			vInnerMeshBuilder.CommitColors(Color.clear);
			innerFilt.sharedMesh = vInnerMeshBuilder.Mesh;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void SetDepthHint(int pDepthHint) {
			base.SetDepthHint(pDepthHint);

			vOuter.GetComponent<MeshRenderer>().sharedMaterial = 
				Materials.GetLayer(Materials.Layer.Icon, pDepthHint, "StandardIcons");
			vInner.GetComponent<MeshRenderer>().sharedMaterial = 
				Materials.GetLayer(Materials.Layer.Icon, pDepthHint, "StandardIcons");
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();

			Color color = vSettings.ToggleIconColor;
			color.a *= (vItemState.MaxHighlightProgress*0.25f + 0.75f)*vMainAlpha;

			vOuterMeshBuilder.CommitColors(color);
			vInnerMeshBuilder.CommitColors(color);
			vInner.SetActive(IsToggled());

			if ( vSettings.TextSize != vPrevTextSize ) {
				vPrevTextSize = vSettings.TextSize;

				float inset = vSettings.TextSize;
				Vector3 pos = vLabel.transform.localPosition+
					new Vector3(inset*0.666f*LabelCanvasScale, 0, 0);
				Vector3 scale = Vector3.one*(vSettings.TextSize*0.75f*LabelCanvasScale);

				vLabel.SetInset(true, inset);

				vOuter.transform.localPosition = pos;
				vOuter.transform.localScale = scale;

				vInner.transform.localPosition = pos;
				vInner.transform.localScale = scale;
			}
		}

	}

}
