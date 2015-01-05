using System;
using System.Collections.Generic;
using System.Linq;
using Leap;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class HandDisplay : MonoBehaviour {

		public Func<Hand> GetCurrentHand;

		private Controller vControl;
		private IList<FingerDisplay> vFingerDisplays;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vFingerDisplays = new List<FingerDisplay>();

			AddFingerDisplay(Finger.FingerType.TYPE_INDEX);
			AddFingerDisplay(Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_MIDDLE, 0.5f);
			AddFingerDisplay(Finger.FingerType.TYPE_MIDDLE);
			AddFingerDisplay(Finger.FingerType.TYPE_MIDDLE, Finger.FingerType.TYPE_RING, 0.5f);
			AddFingerDisplay(Finger.FingerType.TYPE_RING);
			AddFingerDisplay(Finger.FingerType.TYPE_RING, Finger.FingerType.TYPE_PINKY, 0.5f);
			AddFingerDisplay(Finger.FingerType.TYPE_PINKY);
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
		private void AddFingerDisplay(Finger.FingerType pType0, 
													Finger.FingerType? pType1=null, float pAmount=0) {
			var fingerObj = new GameObject("FingerDisplay");
			fingerObj.transform.parent = gameObject.transform;

			FingerDisplay fingerDisp = fingerObj.AddComponent<FingerDisplay>();
			vFingerDisplays.Add(fingerDisp);

			if ( pType1 == null ) {
				fingerDisp.GetCurrentData = (() => GetFingerData(pType0));
			}
			else {
				fingerDisp.GetCurrentData = 
					(() => GetFingerData(pType0, (Finger.FingerType)pType1, pAmount));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private Finger GetFinger(Finger.FingerType pType) {
			Hand hand = GetCurrentHand();

			if ( hand == null ) {
				return null;
			}

			return hand.Fingers.FingerType(pType).FirstOrDefault(f => f.IsValid);
		}

		/*--------------------------------------------------------------------------------------------*/
		private FingerData GetFingerData(Finger.FingerType pType) {
			Finger finger = GetFinger(pType);
			return (finger == null ? null : new FingerData(finger));
		}

		/*--------------------------------------------------------------------------------------------*/
		private FingerData GetFingerData(Finger.FingerType pType0, 
															Finger.FingerType pType1, float pAmount) {
			FingerData data0 = GetFingerData(pType0);
			FingerData data1 = GetFingerData(pType1);

			if ( data0 == null || data1 == null ) {
				return null;
			}

			var data = new FingerData();
			data.Position = data0.Position*(1-pAmount) + data1.Position*pAmount;
			data.Direction = data0.Direction*(1-pAmount) + data1.Direction*pAmount;
			return data;
		}

	}

}
