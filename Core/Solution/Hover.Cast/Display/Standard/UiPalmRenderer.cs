using System;
using Hover.Cast.Custom.Standard;
using Hover.Cast.State;
using Hover.Common.Custom;
using Hover.Common.Display;
using Hover.Common.Util;
using UnityEngine;

namespace Hover.Cast.Display.Standard {

	/*================================================================================================*/
	public class UiPalmRenderer : MonoBehaviour, IUiPalmRenderer {

		public const float InnerRadius = 0.13f;
		public const float OuterRadius = InnerRadius+UiItemSelectRenderer.Thickness;

		protected MenuState vMenuState;
		protected float vAngle0;
		protected float vAngle1;
		protected int vMeshSteps;

		protected float vMainAlpha;
		private ItemVisualSettingsStandard vSettings;

		protected GameObject vBackground;
		protected Mesh vBackgroundMesh;
		protected UiLabel vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(MenuState pMenuState, IItemVisualSettings pSettings, 
																		float pAngle0, float pAngle1) {
			vMenuState = pMenuState;
			vSettings = (ItemVisualSettingsStandard)pSettings;
			vAngle0 = pAngle0;
			vAngle1 = pAngle1;
			vMeshSteps = (int)Math.Round(Math.Max(2, (vAngle1-vAngle0)/Math.PI*60));

			////

			vBackground = new GameObject("Background");
			vBackground.transform.SetParent(gameObject.transform, false);
			vBackground.AddComponent<MeshRenderer>();
			
			MeshFilter bgFilt = vBackground.AddComponent<MeshFilter>();
			vBackgroundMesh = bgFilt.mesh;
			BuildMesh(vBackgroundMesh);
			Materials.SetMeshColor(vBackgroundMesh, Color.clear);

			////

			var labelObj = new GameObject("Label");
			labelObj.transform.SetParent(gameObject.transform, false);
			labelObj.transform.localPosition = new Vector3(0, 0, InnerRadius);
			labelObj.transform.localRotation = Quaternion.FromToRotation(Vector3.back, Vector3.right);
			labelObj.transform.localScale = new Vector3((vMenuState.IsOnLeftSide ? 1 : -1), 1, 1);

			vLabel = labelObj.AddComponent<UiLabel>();
			vLabel.AlignLeft = vMenuState.IsOnLeftSide;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void SetDepthHint(int pDepthHint) {
			vBackground.GetComponent<MeshRenderer>().sharedMaterial = 
				Materials.GetLayer(Materials.Layer.Background, pDepthHint);
			vLabel.SetDepthHint(pDepthHint);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnEnable() {
			if ( vLabel != null ) {
				vLabel.Alpha = 0;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			vMainAlpha = UiItemSelectRenderer.GetArcAlpha(vMenuState);

			Color colBg = vSettings.BackgroundColor;
			colBg.a *= vMainAlpha;

			Materials.SetMeshColor(vBackgroundMesh, colBg);

			if ( vSettings.TextSize != vLabel.FontSize ) {
				const float scale = UiItemSelectRenderer.ArcCanvasScale;

				vLabel.SetSize(UiItemSelectRenderer.ArcCanvasThickness*scale, 
					vSettings.TextSize*1.5f*scale, vSettings.TextSize*0.6f, scale);
			}

			vLabel.Alpha = vMainAlpha;
			vLabel.FontName = vSettings.TextFont;
			vLabel.FontSize = vSettings.TextSize;
			vLabel.Color = vSettings.TextColor;
			vLabel.Label = vMenuState.GetLevelTitle();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildMesh(Mesh pMesh) {
			MeshUtil.BuildRingMesh(pMesh, InnerRadius, OuterRadius, vAngle0, vAngle1, vMeshSteps);
		}

	}

}
