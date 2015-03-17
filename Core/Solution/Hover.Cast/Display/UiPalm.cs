using System;
using Hover.Cast.Custom;
using Hover.Cast.State;
using Hover.Common.Items;
using UnityEngine;

namespace Hover.Cast.Display {

	/*================================================================================================*/
	public class UiPalm : MonoBehaviour {

		private ArcState vArcState;
		private ICustomPalm vCustom;
		private bool vRebuildOnUpdate;

		private GameObject vRendererHold;
		private GameObject vPrevRendererObj;
		private GameObject vRendererObj;
		private IUiPalmRenderer vRenderer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArc, ICustomPalm pCustom) {
			vArcState = pArc;
			vCustom = pCustom;

			vRendererHold = new GameObject("RendererHold");
			vRendererHold.transform.SetParent(gameObject.transform, false);
			vRendererHold.transform.localPosition = UiLevel.PushFromHand;
			vRendererHold.transform.localRotation = Quaternion.AngleAxis(170, Vector3.up);

			vArcState.OnLevelChange += HandleLevelChange;
			Rebuild();
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterSideChange() {
			vRebuildOnUpdate = true;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( vPrevRendererObj != null ) {
				vPrevRendererObj.SetActive(false);
				Destroy(vPrevRendererObj);
				vPrevRendererObj = null;
			}

			if ( vRebuildOnUpdate ) {
				vRebuildOnUpdate = false;
				Rebuild();
			}

			vRendererHold.SetActive(vArcState.DisplayStrength > 0);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleLevelChange(int pDirection) {
			vRebuildOnUpdate = true;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void Rebuild() {
			vPrevRendererObj = vRendererObj;

			const float halfAngle = UiLevel.AngleFull/2f;
			IBaseItem navItem = vArcState.GetLevelParentItem();
			Type rendType = vCustom.GetPalmRenderer(navItem);
			SegmentSettings sett = vCustom.GetPalmSettings(navItem);

			vRendererHold.SetActive(true); //ensures that Awake() is called in the renderers

			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(vRendererHold.transform, false);

			vRenderer = (IUiPalmRenderer)vRendererObj.AddComponent(rendType);
			vRenderer.Build(vArcState, sett, -halfAngle, halfAngle);
		}

	}

}
