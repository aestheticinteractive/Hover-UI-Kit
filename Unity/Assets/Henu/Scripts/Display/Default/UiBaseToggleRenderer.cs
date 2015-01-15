using Henu.Settings;
using Henu.State;
using UnityEngine;

namespace Henu.Display.Default {

	/*================================================================================================*/
	public abstract class UiBaseToggleRenderer : UiSelectRenderer {

		private GameObject vOuter;
		private GameObject vInner;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract Texture2D GetOuterTexture();

		/*--------------------------------------------------------------------------------------------*/
		protected abstract Texture2D GetInnerTexture();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Build(ArcState pArcState, ArcSegmentState pSegState,
														float pArcAngle, ArcSegmentSettings pSettings) {
			base.Build(pArcState, pSegState, pArcAngle, pSettings);

			float scale = vSettings.TextSize*0.75f*vTextScale;
			int mult = (vArcState.IsLeft ? 1 : -1);
			float push = vTextH+vTextPadW;
			float pos = 1+push/2f*vTextScale;

			RectTransform.Edge edge = (vArcState.IsLeft ? 
				RectTransform.Edge.Left : RectTransform.Edge.Right);

			RectTransform rect = vTextObj.GetComponent<RectTransform>();
			rect.SetInsetAndSizeFromParentEdge(edge, push, vCanvasW-push-vTextPadW);

			vOuter = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vOuter.name = "ToggleOuter";
			vOuter.transform.SetParent(gameObject.transform, false);
			vOuter.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vOuter.renderer.sharedMaterial.color = Color.clear;
			vOuter.renderer.sharedMaterial.mainTexture = GetOuterTexture();
			vOuter.transform.localPosition = new Vector3(0, 0, pos);
			vOuter.transform.localRotation = vCanvasGroupObj.transform.localRotation;
			vOuter.transform.localScale = new Vector3(scale*mult, scale, 1);

			vInner = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vInner.name = "ToggleInner";
			vInner.transform.SetParent(gameObject.transform, false);
			vInner.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vInner.renderer.sharedMaterial.color = Color.clear;
			vInner.renderer.sharedMaterial.mainTexture = GetInnerTexture();
			vInner.transform.localPosition = vOuter.transform.localPosition;
			vInner.transform.localRotation = vOuter.transform.localRotation;
			vInner.transform.localScale = vOuter.transform.localScale;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();

			Color color = vSettings.ToggleIconColor;
			color.a *= (vSegState.HighlightProgress*0.25f + 0.75f)*vMainAlpha;

			vOuter.renderer.sharedMaterial.color = color;
			vInner.renderer.sharedMaterial.color = color;
			vInner.renderer.enabled = vSegState.NavItem.Selected;
		}

	}

}
