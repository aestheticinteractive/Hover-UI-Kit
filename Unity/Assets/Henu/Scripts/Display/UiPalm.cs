using System;
using Henu.Navigation;
using Henu.Settings;
using Henu.State;
using UnityEngine;

namespace Henu.Display {

	/*================================================================================================*/
	public class UiPalm : MonoBehaviour {

		private ArcState vArcState;
		private ISettings vSettings;

		private GameObject vRendererHold;
		private GameObject vRendererObj;
		private IUiPalmRenderer vRenderer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArc, ISettings pSettings) {
			vArcState = pArc;
			vSettings = pSettings;

			const float halfAngle = UiArcLevel.AngleFull/2f;
			Type rendType = pSettings.GetUiPalmRendererType();

			vRendererHold = new GameObject("RendererHold");
			vRendererHold.transform.SetParent(gameObject.transform, false);
			vRendererHold.transform.localPosition = UiArcLevel.PushFromHand;
			vRendererHold.transform.localRotation = 
				Quaternion.AngleAxis(180+10*(vArcState.IsLeft ? -1 : 1), Vector3.up);

			vRendererObj = new GameObject("Renderer");
			vRendererObj.transform.SetParent(vRendererHold.transform, false);

			vRenderer = (IUiPalmRenderer)vRendererObj.AddComponent(rendType);
			vRenderer.Build(vArcState, -halfAngle, halfAngle);

			////

			vArcState.OnLevelChange += HandleLevelChange;
			HandleLevelChange(0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			gameObject.transform.localPosition = vArcState.Center;
			gameObject.transform.localRotation = vArcState.Rotation;
			gameObject.transform.localScale = Vector3.one*(vArcState.Size*UiArc.ScaleArcSize);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleLevelChange(int pDirection) {
			NavItem parNavItem = vArcState.GetLevelParentItem();
			var arcSegSett = vSettings.GetArcSegmentSettings(parNavItem);
			vRenderer.SetSettings(arcSegSett);
		}

	}

}
