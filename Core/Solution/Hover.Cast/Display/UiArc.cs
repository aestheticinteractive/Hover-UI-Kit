using System;
using Hover.Cast.Custom;
using Hover.Cast.State;
using UnityEngine;

namespace Hover.Cast.Display {

	/*================================================================================================*/
	public class UiArc : MonoBehaviour {

		private const float LevelChangeMilliseconds = 1000;
		private const float LevelChangeDistance = 0.5f;

		private ArcState vArcState;
		private IItemVisualSettingsProvider vVisualSettingsProv;

		private GameObject vPrevLevelObj;
		private GameObject vCurrLevelObj;
		private DateTime? vChangeTime;
		private int vChangeDir;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArc, IItemVisualSettingsProvider pVisualSettingsProv) {
			vArcState = pArc;
			vVisualSettingsProv = pVisualSettingsProv;

			vArcState.OnLevelChange += HandleLevelChange;
			UpdateAfterSideChange();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			UpdateItemChangeAnim();
			vCurrLevelObj.SetActive(vArcState.DisplayStrength > 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterSideChange() {
			vPrevLevelObj = vCurrLevelObj;
			DestroyPrevLevel();

			vCurrLevelObj = new GameObject("CurrLevel");
			vCurrLevelObj.transform.SetParent(gameObject.transform, false);
			vCurrLevelObj.transform.localScale = Vector3.one;

			UiLevel level = vCurrLevelObj.AddComponent<UiLevel>();
			level.Build(vArcState, vVisualSettingsProv);
			level.HandleChangeAnimation(true, 0, 1);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleLevelChange(int pDirection) {
			DestroyPrevLevel();
			vPrevLevelObj = vCurrLevelObj;

			if ( vPrevLevelObj != null ) {
				vPrevLevelObj.name = "PrevLevel";
			}

			////

			vCurrLevelObj = new GameObject("CurrLevel");
			vCurrLevelObj.transform.SetParent(gameObject.transform, false);

			UiLevel level = vCurrLevelObj.AddComponent<UiLevel>();
			level.Build(vArcState, vVisualSettingsProv);

			////

			vChangeTime = DateTime.UtcNow;
			vChangeDir = pDirection;
			UpdateItemChangeAnim();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void DestroyPrevLevel() {
			if ( vPrevLevelObj == null ) {
				return;
			}

			vPrevLevelObj.SetActive(false);
			Destroy(vPrevLevelObj);
			vPrevLevelObj = null;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateItemChangeAnim() {
			if ( vChangeTime == null ) {
				return;
			}

			float ms = (float)(DateTime.UtcNow-(DateTime)vChangeTime).TotalMilliseconds;
			float prog = Math.Min(1, ms/LevelChangeMilliseconds);
			float push = 1-(float)Math.Pow(1-prog, 3);
			float dist = LevelChangeDistance*vChangeDir;

			if ( vPrevLevelObj != null ) {
				float prevScale = -dist*push;

				if ( vChangeDir > 0 ) {
					prevScale *= 0.666f;
				}

				vPrevLevelObj.GetComponent<UiLevel>()
					.HandleChangeAnimation(false, vChangeDir, prog);
				vPrevLevelObj.transform.localScale = Vector3.one*(1+prevScale);
			}

			if ( vCurrLevelObj != null ) {
				float currScale = dist*(1-push);

				if ( vChangeDir < 0 ) {
					currScale *= 0.666f;
				}

				vCurrLevelObj.GetComponent<UiLevel>()
					.HandleChangeAnimation(true, vChangeDir, prog);
				vCurrLevelObj.transform.localScale = Vector3.one*(1+currScale);
			}

			if ( prog < 1 ) {
				return;
			}

			vChangeTime = null;
			DestroyPrevLevel();
		}

	}

}
