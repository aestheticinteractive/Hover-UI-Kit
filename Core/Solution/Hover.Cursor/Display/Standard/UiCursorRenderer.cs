using System;
using Hover.Common.Display;
using Hover.Common.Util;
using Hover.Cursor.Custom;
using Hover.Cursor.Custom.Standard;
using Hover.Cursor.State;
using UnityEngine;

namespace Hover.Cursor.Display.Standard {

	/*================================================================================================*/
	public class UiCursorRenderer : MonoBehaviour, IUiCursorRenderer {

		protected ICursorState vCursorState;
		protected CursorSettingsStandard vSettings;
		protected Mesh vRingMesh;
		protected GameObject vRingObj;

		protected float vCurrThickness;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Build(ICursorState pCursorState, ICursorSettings pSettings) {
			vCursorState = pCursorState;
			vSettings = (CursorSettingsStandard)pSettings;

			vRingObj = new GameObject("Ring");
			vRingObj.transform.SetParent(gameObject.transform, false);
			MeshRenderer meshRend = vRingObj.AddComponent<MeshRenderer>();
			meshRend.sharedMaterial = Materials.GetCursorLayer();
			vRingObj.AddComponent<MeshFilter>();

			vRingMesh = vRingObj.GetComponent<MeshFilter>().mesh;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			float maxProg = vCursorState.GetMaxHighlightProgress();
			bool high = (maxProg >= 1);
			float thick = Mathf.Lerp(vSettings.ThickNorm, vSettings.ThickHigh, maxProg);
			float scale = Mathf.Lerp(vSettings.RadiusNorm, vSettings.RadiusHigh, maxProg);

			Color col = (high ? vSettings.ColorHigh : vSettings.ColorNorm);
			col.a *= vCursorState.DisplayStrength;

			BuildMesh(thick);

			vRingObj.transform.localScale = Vector3.one*scale;
			Materials.SetMeshColor(vRingMesh, col);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void BuildMesh(float pThickness) {
			if ( pThickness == vCurrThickness ) {
				return;
			}

			vCurrThickness = pThickness;
			MeshUtil.BuildRingMesh(vRingMesh, (1-pThickness)/2f, 0.5f, 0, (float)Math.PI*2, 24);
		}

	}

}
