using System.Diagnostics;
using Hover.Core.Cursors;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.InterfaceModules.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HovercastInterface))]
	[RequireComponent(typeof(HoverCursorFollower))]
	public class HovercastOpenTransitioner : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public bool IsTransitionActive { get; private set; }
		public float TransitionProgressCurved { get; private set; }

		[Range(0, 1)]
		public float TransitionProgress = 1;
		
		[Range(0.1f, 10)]
		public float TransitionExponent = 4;

		[Range(1, 10000)]
		public float TransitionMilliseconds = 500;

		private Stopwatch vTimer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			HovercastInterface cast = GetComponent<HovercastInterface>();
			SetScale(cast.IsOpen ? 1 : 0);
			UpdateIcons(cast);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			HovercastInterface cast = GetComponent<HovercastInterface>();

			UpdateTimedProgress();
			UpdateTransition(cast);
			UpdateIcons(cast);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void OnOpenToggled() { //via HovercastInterface event
			IsTransitionActive = true;
			TransitionProgress = 0;
			vTimer = Stopwatch.StartNew();
			TreeUpdate();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateTimedProgress() {
			if ( vTimer == null ) {
				IsTransitionActive = false;
				return;
			}

			TransitionProgress = (float)vTimer.Elapsed.TotalMilliseconds/TransitionMilliseconds;

			if ( TransitionProgress >= 1 ) {
				TransitionProgress = 1;
				vTimer = null;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateTransition(HovercastInterface pCast) {
			TransitionProgressCurved = 1-Mathf.Pow(1-TransitionProgress, TransitionExponent);
			SetScale(pCast.IsOpen ? TransitionProgressCurved : 1-TransitionProgressCurved);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void SetScale(float pScaleFactor) {
			HoverCursorFollower follower = GetComponent<HoverCursorFollower>();
			Transform openTx = GetComponent<HovercastInterface>().OpenItem.transform;

			for ( int i = 0 ; i < follower.ObjectsToActivate.Length ; i++ ) {
				Transform tx = follower.ObjectsToActivate[i].transform;

				if ( tx == openTx ) {
					continue;
				}

				tx.localScale = Vector3.one*pScaleFactor;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateIcons(HovercastInterface pCast) {
			HovercastOpenIcons icons = pCast.OpenItem.GetComponentInChildren<HovercastOpenIcons>();

			if ( icons.OpenIcon != null ) {
				icons.OpenIcon.SetActive(!pCast.IsOpen);
			}

			if ( icons.CloseIcon != null ) {
				icons.CloseIcon.SetActive(pCast.IsOpen);
			}
		}

	}

}
