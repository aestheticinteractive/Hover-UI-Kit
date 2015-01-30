using System;
using Hovercast.Core.Navigation;
using Hovercast.Core.Settings;
using Hovercast.Core.State;
using UnityEngine;

namespace Hovercast.Core.Display {

	/*================================================================================================*/
	public class UiPalm : MonoBehaviour {

		private ArcState vArcState;
		private ISettings vSettings;

		private GameObject vRendererHold;
		private GameObject vRendererObj;
		private IUiPalmRenderer vRenderer;

		private bool? vPrevIsLeft;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArc, ISettings pSettings) {
			vArcState = pArc;
			vSettings = pSettings;

			vRendererHold = new GameObject("RendererHold");
			vRendererHold.transform.SetParent(gameObject.transform, false);
			vRendererHold.transform.localPosition = UiArcLevel.PushFromHand;

			vArcState.OnIsLeftChange += HandleIsLeftChange;
			vArcState.OnLevelChange += HandleLevelChange;

			HandleIsLeftChange();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleLevelChange(int pDirection) {
			NavItem parNavItem = vArcState.GetLevelParentItem();
			var arcSegSett = vSettings.GetArcSegmentSettings(parNavItem);
			vRenderer.SetSettings(arcSegSett);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleIsLeftChange() {
			if ( vRendererObj != null ) {
				vRendererObj.SetActive(false);
				Destroy(vRendererObj);
			}

			const float halfAngle = UiArcLevel.AngleFull/2f;
			Type rendType = vSettings.GetUiPalmRendererType();

			vRendererHold.transform.localRotation = Quaternion.AngleAxis(170, Vector3.up);

			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(vRendererHold.transform, false);

			vRenderer = (IUiPalmRenderer)vRendererObj.AddComponent(rendType);
			vRenderer.Build(vArcState, -halfAngle, halfAngle);

			HandleLevelChange(0);
		}

	}

}
