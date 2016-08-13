using System.Diagnostics;
using Hover.Cursors;
using Hover.Utils;
using UnityEngine;

namespace Hover.Interfaces.Cast {

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
		public float TransitionExponent = 2;

		[Range(1, 10000)]
		public float TransitionMilliseconds = 200;

		private Stopwatch vTimer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			HovercastInterface cast = GetComponent<HovercastInterface>();
			SetScale(cast.IsOpen ? 1 : 0);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			UpdateTimedProgress();
			UpdateTransition();
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
		private void UpdateTransition() {
			HovercastInterface cast = GetComponent<HovercastInterface>();

			TransitionProgressCurved = 1-Mathf.Pow(1-TransitionProgress, TransitionExponent);
			SetScale(cast.IsOpen ? TransitionProgressCurved : 1-TransitionProgressCurved);
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

	}

}
