using Henu.State;
using UnityEngine;

namespace Henu.Display.Default {

	/*================================================================================================*/
	public abstract class UiArcSegmentToggleRenderer : UiArcSegmentRenderer {

		public static float IconSize = 20;

		private Texture2D ToggleOuterTexture;
		private Texture2D ToggleInnerTexture;
		private GameObject vOuter;
		private GameObject vInner;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract string GetIconName();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Build(ArcState pArcState, ArcSegmentState pSegState,
																		float pAngle0, float pAngle1) {
			base.Build(pArcState, pSegState, pAngle0, pAngle1);

			RectTransform rect = vTextObj.GetComponent<RectTransform>();
			rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, Height, Width-Height-8);

			////
			
			string iconName = GetIconName();
			
			vOuter = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vOuter.transform.SetParent(gameObject.transform, false);
			vOuter.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vOuter.renderer.sharedMaterial.color = Color.clear;
			vOuter.renderer.sharedMaterial.mainTexture = Resources.Load<Texture2D>(iconName+"Outer");

			vInner = GameObject.CreatePrimitive(PrimitiveType.Quad);
			vInner.transform.SetParent(gameObject.transform, false);
			vInner.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vInner.renderer.sharedMaterial.color = Color.clear;
			vInner.renderer.sharedMaterial.mainTexture = Resources.Load<Texture2D>(iconName+"Inner");

			////
			
			int mult = (vArcState.IsLeft ? 1 : -1);

			vOuter.transform.localPosition = new Vector3(0, 0, 1+(Height/2f)*Scale*mult);
			vOuter.transform.localRotation = vCanvasGroupObj.transform.localRotation;
			vOuter.transform.localScale = new Vector3(-IconSize*Scale*mult, IconSize*Scale, 1);

			vInner.transform.localPosition = vOuter.transform.localPosition;
			vInner.transform.localRotation = vOuter.transform.localRotation;
			vInner.transform.localScale = vOuter.transform.localScale;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void Update() {
			base.Update();

			float alpha = vSegState.HighlightProgress*0.25f + 0.75f;

			vOuter.renderer.sharedMaterial.color = new Color(1, 1, 1, alpha*vMainAlpha);
			vInner.renderer.sharedMaterial.color = new Color(1, 1, 1, alpha*vMainAlpha);
			vInner.renderer.enabled = vSegState.NavItem.Selected;
		}

	}

}
