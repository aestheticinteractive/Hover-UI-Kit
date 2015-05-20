using Hover.Cast.State;
using Hover.Common.Custom;
using Hover.Common.Display;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Cast.Display.Standard {

	/*================================================================================================*/
	public abstract class UiItemBaseToggleRenderer : UiItemSelectRenderer {

		protected GameObject vOuter;
		protected GameObject vInner;
		protected Mesh vOuterMesh;
		protected Mesh vInnerMesh;

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
		public override void Build(IHovercastMenuState pMenuState, IBaseItemState pItemState,
													float pArcAngle, IItemVisualSettings pSettings) {
			base.Build(pMenuState, pItemState, pArcAngle, pSettings);

			////

			vOuter = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vOuter.name = "ToggleOuter";
			vOuter.transform.SetParent(gameObject.transform, false);
			vOuter.transform.localRotation = vLabel.CanvasLocalRotation;

			vOuterMesh = vOuter.GetComponent<MeshFilter>().mesh;
			Materials.SetMeshColor(vOuterMesh, Color.clear);
			Materials.SetMeshIconCoords(vOuterMesh, GetOuterIconOffset());

			////

			vInner = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vInner.name = "ToggleInner";
			vInner.transform.SetParent(gameObject.transform, false);
			vInner.transform.localRotation = vLabel.CanvasLocalRotation;

			vInnerMesh = vInner.GetComponent<MeshFilter>().mesh;
			Materials.SetMeshColor(vInnerMesh, Color.clear);
			Materials.SetMeshIconCoords(vInnerMesh, GetInnerIconOffset());
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

			Materials.SetMeshColor(vOuterMesh, color);
			Materials.SetMeshColor(vInnerMesh, color);
			vInner.SetActive(IsToggled());

			if ( vSettings.TextSize != vPrevTextSize ) {
				vPrevTextSize = vSettings.TextSize;

				float inset = vLabel.TextH*0.85f;
				Vector3 pos = new Vector3(0, 0, 1+inset*0.75f*ArcCanvasScale);
				Vector3 scale = Vector3.one*(vSettings.TextSize*0.75f*ArcCanvasScale);

				vLabel.SetInset(vMenuState.IsOnLeftSide, inset);

				vOuter.transform.localPosition = pos;
				vOuter.transform.localScale = scale;

				vInner.transform.localPosition = pos;
				vInner.transform.localScale = scale;
			}
		}

	}

}
