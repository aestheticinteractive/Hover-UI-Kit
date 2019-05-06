using System.Diagnostics;
using Hover.Core.Cursors;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.InterfaceModules.Cast {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HovercastInterface))]
	[RequireComponent(typeof(HoverCursorFollower))]
	public class HovercastOpenTransitioner : TreeUpdateableBehavior, ISettingsController {

		public bool IsTransitionActive { get; private set; }
		public float TransitionProgressCurved { get; private set; }

		[SerializeField]
		[Range(0, 1)]
		[FormerlySerializedAs("TransitionProgress")]
		private float _TransitionProgress = 1;

		[SerializeField]
		[Range(0.1f, 10)]
		[FormerlySerializedAs("TransitionExponent")]
		private float _TransitionExponent = 4;

		[SerializeField]
		[Range(1, 10000)]
		[FormerlySerializedAs("TransitionMilliseconds")]
		private float _TransitionMilliseconds = 500;

		private Stopwatch vTimer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float TransitionProgress {
			get => _TransitionProgress;
			set => this.UpdateValueWithTreeMessage(ref _TransitionProgress, value, "TransProgress");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float TransitionExponent {
			get => _TransitionExponent;
			set => this.UpdateValueWithTreeMessage(ref _TransitionExponent, value, "TransExponent");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float TransitionMilliseconds {
			get => _TransitionMilliseconds;
			set => this.UpdateValueWithTreeMessage(ref _TransitionMilliseconds, value, "TransMs");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void Start() {
			base.Start();

			HovercastInterface cast = GetComponent<HovercastInterface>();
			SetScale(cast.IsOpen ? 1 : 0);
			UpdateIcons(cast);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
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
