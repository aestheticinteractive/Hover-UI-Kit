using Hover.Cast.State;
using Hover.Common.Custom;
using Hover.Common.State;
using UnityEngine;

namespace Hover.Cast.Display.Standard {

	/*================================================================================================*/
	public abstract class UiItemBaseToggleRenderer : UiItemSelectRenderer {

		protected GameObject vOuter;
		protected GameObject vInner;

		private int vPrevTextSize;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract Texture2D GetOuterTexture();

		/*--------------------------------------------------------------------------------------------*/
		protected abstract Texture2D GetInnerTexture();

		/*--------------------------------------------------------------------------------------------*/
		protected abstract bool IsToggled();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Build(IHovercastMenuState pMenuState, IBaseItemState pItemState,
													float pArcAngle, IItemVisualSettings pSettings) {
			base.Build(pMenuState, pItemState, pArcAngle, pSettings);

			vOuter = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vOuter.name = "ToggleOuter";
			vOuter.transform.SetParent(gameObject.transform, false);
			vOuter.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vOuter.renderer.sharedMaterial.color = Color.clear;
			vOuter.renderer.sharedMaterial.mainTexture = GetOuterTexture();
			vOuter.transform.localRotation = vLabel.CanvasLocalRotation;

			vInner = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vInner.name = "ToggleInner";
			vInner.transform.SetParent(gameObject.transform, false);
			vInner.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vInner.renderer.sharedMaterial.color = Color.clear;
			vInner.renderer.sharedMaterial.mainTexture = GetInnerTexture();
			vInner.transform.localRotation = vLabel.CanvasLocalRotation;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();

			Color color = vSettings.ToggleIconColor;
			color.a *= (vItemState.MaxHighlightProgress*0.25f + 0.75f)*vMainAlpha;

			vOuter.renderer.sharedMaterial.color = color;
			vInner.renderer.sharedMaterial.color = color;
			vInner.renderer.enabled = IsToggled();

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
