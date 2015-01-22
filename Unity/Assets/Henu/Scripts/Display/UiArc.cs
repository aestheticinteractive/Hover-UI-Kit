using System;
using Henu.Settings;
using Henu.State;
using UnityEngine;

namespace Henu.Display {

	/*================================================================================================*/
	public class UiArc : MonoBehaviour {

		private const float LevelChangeMilliseconds = 1000;
		private const float LevelChangeDistance = 0.5f;
		public static readonly float ScaleArcSize = 1.1f;

		private ArcState vArcState;
		private ISettings vSettings;

		private GameObject vPrevLevelObj;
		private GameObject vCurrLevelObj;
		private DateTime? vChangeTime;
		private int vChangeDir;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal void Build(ArcState pArc, ISettings pSettings) {
			vArcState = pArc;
			vSettings = pSettings;

			vArcState.OnLevelChange += HandleLevelChange;
			vArcState.OnIsLeftChange += HandleIsLeftChange;
			HandleIsLeftChange();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			gameObject.transform.localPosition = vArcState.Center;
			gameObject.transform.localRotation = vArcState.Rotation;
			gameObject.transform.localScale = Vector3.one*(vArcState.Size*ScaleArcSize);

			UpdateItemChangeAnim();
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

			UiArcLevel arcLevel = vCurrLevelObj.AddComponent<UiArcLevel>();
			arcLevel.Build(vArcState, vSettings);

			////

			vChangeTime = DateTime.UtcNow;
			vChangeDir = pDirection;
			UpdateItemChangeAnim();
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleIsLeftChange() {
			vPrevLevelObj = vCurrLevelObj;
			DestroyPrevLevel();

			vCurrLevelObj = new GameObject("CurrLevel");
			vCurrLevelObj.transform.SetParent(gameObject.transform, false);
			vCurrLevelObj.transform.localScale = Vector3.one;

			UiArcLevel arcLevel = vCurrLevelObj.AddComponent<UiArcLevel>();
			arcLevel.Build(vArcState, vSettings);
			arcLevel.HandleChangeAnimation(true, 0, 1);
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

				vPrevLevelObj.GetComponent<UiArcLevel>()
					.HandleChangeAnimation(false, vChangeDir, prog);
				vPrevLevelObj.transform.localScale = Vector3.one*(1+prevScale);
			}

			if ( vCurrLevelObj != null ) {
				float currScale = dist*(1-push);

				if ( vChangeDir < 0 ) {
					currScale *= 0.666f;
				}

				vCurrLevelObj.GetComponent<UiArcLevel>()
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
