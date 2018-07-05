using System.Collections.Generic;
using System.Linq;
using HoverDemos.Common;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace HoverDemos.KeyboardPixels {

	/*================================================================================================*/
	public class DemoEnvironment : MonoBehaviour {

		public int RandomSeed = 0;
		public Text PixelText;

		private DemoTextPixels vPixels;
		private IList<DemoLetter> vLetters;
		private IList<DemoLetter> vLetterCache;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			RandomUtil.Init(RandomSeed);

			vPixels = new DemoTextPixels(PixelText);
			vLetters = new List<DemoLetter>();
			vLetterCache = new List<DemoLetter>();

			////

			for ( int i = 0 ; i < 6 ; ++i ) {
				var lettHoldObj = new GameObject("LetterHold"+i);
				lettHoldObj.transform.SetParent(gameObject.transform, false);

				var lettObj = new GameObject("Letter");
				lettObj.transform.SetParent(lettHoldObj.transform, false);
				lettObj.transform.localScale = Vector3.one*0.3f;

				var lett = lettObj.AddComponent<DemoLetter>();
				lett.RandomAxis = Random.onUnitSphere;
				vLetterCache.Add(lett);

				lettObj.SetActive(false);
			}

			////

			for ( int i = 0 ; i < 40 ; ++i ) {
				var ringObj = new GameObject("Ring"+i);
				ringObj.transform.SetParent(gameObject.transform, false);
				ringObj.transform.localPosition = Random.onUnitSphere*2;
				ringObj.transform.localRotation = Random.rotation;

				DemoRing ring = ringObj.AddComponent<DemoRing>();
				ring.Radius = RandomUtil.Float(6)+3;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( Input.GetKey(KeyCode.Escape) ) {
				Application.Quit();
				return;
			}

			if ( Input.GetKey(KeyCode.R) ) {
				InputTracking.Recenter();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void FixedUpdate() {
			bool removeFirst = false;

			foreach ( DemoLetter lett in vLetters ) {
				Transform tx = lett.gameObject.transform;
				tx.localPosition += new Vector3(0, 0, 0.03f);
				tx.localRotation *= Quaternion.AngleAxis(0.12f, lett.RandomAxis);

				if ( tx.localPosition.z > 16 ) {
					removeFirst = true;
				}
			}

			if ( removeFirst ) {
				RemoveFirstLetter();
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddLetter(char pLetter) {
			int w;
			int h;
			float[,] pixels = vPixels.GetPixels(pLetter, out w, out h);
			DemoLetter lett;
			
			if ( vLetterCache.Count > 0 ) {
				lett = vLetterCache.First();
				vLetterCache.RemoveAt(0);
			}
			else {
				lett = vLetters.First();
				vLetters.RemoveAt(0);
			}

			GameObject lettObj = lett.gameObject;
			lettObj.transform.localPosition = new Vector3(0, 0, 0);
			lettObj.transform.localRotation = Quaternion.FromToRotation(Vector3.up, Vector3.back);
			lettObj.transform.parent.rotation = Camera.main.transform.rotation;
			lettObj.SetActive(true);

			lett.SetPixels(pixels, w, h);
			vLetters.Add(lett);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void RemoveLatestLetter() {
			if ( vLetters.Count == 0 ) {
				return;
			}

			DemoLetter lett = vLetters.Last();
			lett.gameObject.SetActive(false);

			vLetters.RemoveAt(vLetters.Count-1);
			vLetterCache.Add(lett);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void RemoveFirstLetter() {
			DemoLetter lett = vLetters.First();
			lett.gameObject.SetActive(false);

			vLetters.RemoveAt(0);
			vLetterCache.Add(lett);
		}

	}

}
