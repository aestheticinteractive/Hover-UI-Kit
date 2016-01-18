using System;
using System.Diagnostics;
using Hover.Cast.State;
using Hover.Common.Custom;
using UnityEngine;

namespace Hover.Cast.Display {

	/*================================================================================================*/
	public class UiArc : MonoBehaviour {

		public const float LevelChangeMilliseconds = 1000;
		private const float LevelChangeDistance = 0.5f;

		private MenuState vMenuState;
		private IItemVisualSettingsProvider vVisualSettingsProv;

		private GameObject vPrevLevelObj;
		private GameObject vCurrLevelObj;
		private Stopwatch vChangeAnim;
		private int vChangeDir;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(MenuState pMenu, IItemVisualSettingsProvider pVisualSettingsProv) {
			vMenuState = pMenu;
			vVisualSettingsProv = pVisualSettingsProv;

			vMenuState.OnLevelChanged += HandleLevelChange;
			UpdateAfterSideChange();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			UpdateItemChangeAnim();
			vCurrLevelObj.SetActive(vMenuState.DisplayStrength > 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		internal void UpdateAfterSideChange() {
			vPrevLevelObj = vCurrLevelObj;
			DestroyPrevLevel();

			vCurrLevelObj = new GameObject("CurrLevel");
			vCurrLevelObj.transform.SetParent(gameObject.transform, false);
			vCurrLevelObj.transform.localScale = Vector3.one;

			UiLevel level = vCurrLevelObj.AddComponent<UiLevel>();
			level.Build(vMenuState, vVisualSettingsProv);
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
			level.Build(vMenuState, vVisualSettingsProv);

			////

			vChangeAnim = Stopwatch.StartNew();
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
			if ( vChangeAnim == null ) {
				return;
			}

			float ms = (float)vChangeAnim.Elapsed.TotalMilliseconds;
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

			vChangeAnim = null;
			DestroyPrevLevel();
		}

	}

}
