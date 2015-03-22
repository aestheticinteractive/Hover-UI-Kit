using Hover.Cast.Custom;
using Hover.Cast.State;
using UnityEngine;

namespace Hover.Cast.Display {

	/*================================================================================================*/
	public class UiMenu : MonoBehaviour {

		public const float ScaleArcSize = 1.1f;

		private HovercastState vState;

		private UiPalm vUiPalm;
		private UiArc vUiArc;

		private Quaternion vLeftRot;
		private Quaternion vRightRot;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(HovercastState pState, ICustomItem pCustomSeg, ICustomPalm pCustomPalm) {
			vState = pState;
			vLeftRot = Quaternion.identity;
			vRightRot = Quaternion.AngleAxis(180, Vector3.up);

			var palmObj = new GameObject("Palm");
			palmObj.transform.SetParent(gameObject.transform, false);
			vUiPalm = palmObj.AddComponent<UiPalm>();
			vUiPalm.Build(vState.Arc, pCustomPalm);

			var arcObj = new GameObject("Arc");
			arcObj.transform.SetParent(gameObject.transform, false);
			vUiArc = arcObj.AddComponent<UiArc>();
			vUiArc.Build(vState.Arc, pCustomSeg);

			vState.OnSideChange += HandleSideChange;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			ArcState arc = vState.Arc;
			bool isLeft = arc.IsLeft;
			Vector3 scale = Vector3.one*(arc.Size*ScaleArcSize);

			if ( !isLeft ) {
				scale.z *= -1;
			}

			gameObject.transform.localPosition = arc.Center;
			gameObject.transform.localRotation = arc.Rotation;
			gameObject.transform.localScale = scale;

			vUiArc.gameObject.transform.localRotation = (isLeft ? vLeftRot : vRightRot);
			vUiPalm.gameObject.transform.localRotation = (isLeft ? vLeftRot : vRightRot);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleSideChange() {
			vUiPalm.UpdateAfterSideChange();
			vUiArc.UpdateAfterSideChange();
		}

	}

}
