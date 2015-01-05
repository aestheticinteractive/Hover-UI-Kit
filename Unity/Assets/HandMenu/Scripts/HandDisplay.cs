using System;
using Leap;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class HandDisplay : MonoBehaviour {

		public Func<Hand> GetCurrentHand;

		private Controller vControl;
		private FingerDisplay[] vFingerDisplays;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vFingerDisplays = new FingerDisplay[5];

			for ( int i = 0 ; i < 5 ; ++i ) {
				var fingerObj = new GameObject("Finger"+i);
				fingerObj.transform.parent = gameObject.transform;

				FingerDisplay fingerDisp = fingerObj.AddComponent<FingerDisplay>();
				vFingerDisplays[i] = fingerDisp;

				int fi = i;
				fingerDisp.GetCurrentFinger = (() => GetFingerAt(fi));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			Hand hand = GetCurrentHand();
			bool isActive = (hand != null);

			foreach ( FingerDisplay fingerDisp in vFingerDisplays ) {
				fingerDisp.gameObject.SetActive(isActive);
			}
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Finger GetFingerAt(int pIndex) {
			Hand hand = GetCurrentHand();

			if ( hand == null || pIndex >= hand.Fingers.Count ) {
				return null;
			}

			Finger finger = hand.Fingers[pIndex];
			return (finger != null && finger.IsValid ? finger : null);
		}

	}

}
