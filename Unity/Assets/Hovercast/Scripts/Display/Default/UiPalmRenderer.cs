using System;
using Hovercast.Settings;
using Hovercast.State;
using UnityEngine;

namespace Hovercast.Display.Default {

	/*================================================================================================*/
	public class UiPalmRenderer : MonoBehaviour, IUiPalmRenderer {

		protected ArcState vArcState;
		protected float vAngle0;
		protected float vAngle1;
		protected int vMeshSteps;

		protected float vInnerRadius;
		protected float vDiameter;
		protected float vMainAlpha;
		private ArcSegmentSettings vSettings;

		protected GameObject vBackground;
		protected UiLabel vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(ArcState pArcState, float pAngle0, float pAngle1) {
			vArcState = pArcState;
			vAngle0 = pAngle0;
			vAngle1 = pAngle1;
			vMeshSteps = (int)Math.Round(Math.Max(2, (vAngle1-vAngle0)/Math.PI*60));

			vInnerRadius = 0.17f;
			vDiameter = UiSelectRenderer.ArcCanvasThickness;

			////

			vBackground = new GameObject("Background");
			vBackground.transform.SetParent(gameObject.transform, false);
			vBackground.AddComponent<MeshFilter>();
			vBackground.AddComponent<MeshRenderer>();
			vBackground.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vBackground.renderer.sharedMaterial.renderQueue -= 100;
			vBackground.renderer.sharedMaterial.color = Color.clear;

			BuildMesh(vBackground.GetComponent<MeshFilter>().mesh);

			////

			var labelObj = new GameObject("Label");
			labelObj.transform.SetParent(gameObject.transform, false);
			labelObj.transform.localPosition = new Vector3(0, 0, vInnerRadius);
			labelObj.transform.localScale = new Vector3(1, 1, (vArcState.IsLeft ? 1 : -1));

			vLabel = labelObj.AddComponent<UiLabel>();
			vLabel.IsLeft = vArcState.IsLeft;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void SetSettings(ArcSegmentSettings pSettings) {
			vSettings = pSettings;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = UiSelectRenderer.GetArcAlpha(vArcState);

			Color colBg = vSettings.BackgroundColor;
			colBg.a *= vMainAlpha;

			vBackground.renderer.sharedMaterial.color = colBg;

			vLabel.Alpha = vMainAlpha;
			vLabel.FontName = vSettings.TextFont;
			vLabel.FontSize = vSettings.TextSize;
			vLabel.Color = vSettings.TextColor;
			vLabel.Label = vArcState.GetLevelTitle();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildMesh(Mesh pMesh) {
			MeshUtil.BuildRingMesh(pMesh, vInnerRadius, vInnerRadius+0.5f, vAngle0, vAngle1,vMeshSteps);
		}

	}

}
