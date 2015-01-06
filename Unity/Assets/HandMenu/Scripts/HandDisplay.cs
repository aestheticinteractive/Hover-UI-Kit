using System;
using System.Collections.Generic;
using System.Linq;
using Leap;
using UnityEngine;

namespace HandMenu {

	/*================================================================================================*/
	public class HandDisplay : MonoBehaviour {

		public bool IsLeft;
		public Func<Hand> GetCurrentHand;

		private Controller vControl;
		private IList<FingerDisplay> vFingerDisplays;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			vFingerDisplays = new List<FingerDisplay>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
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
			float alpha = 0;

			if ( hand != null ) {
				alpha = Vector3.Dot(hand.PalmNormal.ToUnity(), Vector3.down);
				alpha = Math.Max(0, (alpha-0.7f)/0.3f);
				alpha = (float)Math.Pow(alpha, 2);
			}

			foreach ( FingerDisplay fingerDisp in vFingerDisplays ) {
				fingerDisp.HandAlpha = alpha;
				fingerDisp.gameObject.SetActive(isActive);
			}
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void AddFingerDisplay(Finger.FingerType pType0, 
													Finger.FingerType? pType1=null, float pAmount=0) {
			string objName = "Finger-"+(int)pType0+"";

			if ( pType1 != null ) {
				objName += "-"+(int)pType1+"-"+pAmount;
			}

			var fingerObj = new GameObject(objName);
			fingerObj.transform.parent = gameObject.transform;

			FingerDisplay fingerDisp = fingerObj.AddComponent<FingerDisplay>();
			fingerDisp.IsLeft = IsLeft;
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
		private FingerData GetFingerData(Finger.FingerType pType) {
			Hand hand = GetCurrentHand();

			if ( hand == null ) {
				return null;
			}

			Finger finger = hand.Fingers.FingerType(pType).FirstOrDefault(f => f.IsValid);
			return (finger == null ? null : new FingerData(finger, hand));
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
			data.Position = Vector3.Slerp(data0.Position, data1.Position, pAmount);
			data.Direction = Vector3.Slerp(data0.Direction, data1.Direction, pAmount);
			data.Rotation = Quaternion.Slerp(data0.Rotation, data1.Rotation, pAmount);
			data.Extension = data0.Extension*(1-pAmount) + data1.Extension*pAmount;
			return data;
		}

	}

}
