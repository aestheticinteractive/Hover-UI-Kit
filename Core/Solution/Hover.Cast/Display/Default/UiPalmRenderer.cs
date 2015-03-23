using System;
using Hover.Cast.Custom;
using Hover.Cast.Custom.Standard;
using Hover.Cast.State;
using Hover.Common.Display;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Cast.Display.Default {

	/*================================================================================================*/
	public class UiPalmRenderer : MonoBehaviour, IUiPalmRenderer {

		protected ArcState vArcState;
		protected float vAngle0;
		protected float vAngle1;
		protected int vMeshSteps;

		protected float vInnerRadius;
		protected float vMainAlpha;
		private PalmVisualSettingsStandard vSettings;

		protected GameObject vBackground;
		protected UiLabel vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(ArcState pArcState, IPalmVisualSettings pSettings, 
																		float pAngle0, float pAngle1) {
			vArcState = pArcState;
			vSettings = (PalmVisualSettingsStandard)pSettings;
			vAngle0 = pAngle0;
			vAngle1 = pAngle1;
			vMeshSteps = (int)Math.Round(Math.Max(2, (vAngle1-vAngle0)/Math.PI*60));
			vInnerRadius = 0.17f;

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
			labelObj.transform.localRotation = Quaternion.FromToRotation(Vector3.back, Vector3.right);
			labelObj.transform.localScale = new Vector3(1, 1, (vArcState.IsLeft ? 1 : -1));

			vLabel = labelObj.AddComponent<UiLabel>();
			vLabel.AlignLeft = vArcState.IsLeft;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			if ( vLabel != null ) {
				vLabel.Alpha = 0;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = UiSelectRenderer.GetArcAlpha(vArcState);

			Color colBg = vSettings.BackgroundColor;
			colBg.a *= vMainAlpha;

			vBackground.renderer.sharedMaterial.color = colBg;

			if ( vSettings.TextSize != vLabel.FontSize ) {
				const float scale = UiSelectRenderer.ArcCanvasScale;

				vLabel.SetSize(UiSelectRenderer.ArcCanvasThickness*scale, 
					vSettings.TextSize*1.5f*scale, scale);
			}

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
