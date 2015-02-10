using System;
using Hovercast.Core.Custom;
using Hovercast.Core.Navigation;
using Hovercast.Core.State;
using UnityEngine;

namespace Hovercast.Core.Display {

	/*================================================================================================*/
	public class UiPalm : MonoBehaviour {

		private ArcState vArcState;
		private ICustomPalm vCustom;

		private GameObject vRendererHold;
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

			vArcState.OnLevelChange += HandleLevelChange;
			UpdateAfterSideChange();
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterSideChange() {
			if ( vRendererObj != null ) {
				vRendererObj.SetActive(false);
				Destroy(vRendererObj);
			}

			const float halfAngle = UiLevel.AngleFull/2f;
			NavItem navItem = vArcState.GetLevelParentItem();
			Type rendType = vCustom.GetPalmRenderer(navItem);
			SegmentSettings sett = vCustom.GetPalmSettings(navItem);

			vRendererHold.transform.localRotation = Quaternion.AngleAxis(170, Vector3.up);

			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(vRendererHold.transform, false);

			vRenderer = (IUiPalmRenderer)vRendererObj.AddComponent(rendType);
			vRenderer.Build(vArcState, sett, -halfAngle, halfAngle);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleLevelChange(int pDirection) {
			UpdateAfterSideChange();
		}

	}

}
