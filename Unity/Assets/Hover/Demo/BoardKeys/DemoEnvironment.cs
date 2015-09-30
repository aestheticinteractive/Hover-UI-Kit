using System.Collections.Generic;
using System.Linq;
using Hover.Common.Display;
using Hover.Demo.Common;
using UnityEngine;
using UnityEngine.VR;

namespace Hover.Demo.BoardKeys {

	/*================================================================================================*/
	public class DemoEnvironment : MonoBehaviour {

		public int RandomSeed = 0;
		public Transform CameraTransform;
		public float LightRange = 16;

		private readonly IList<DemoLetter> vLetters;
		private readonly IList<DemoLetter> vLetterCache;

		private Light vLight;
		private Light vLight2;
		private DemoTextPixels vPixels;
		//private DemoBoxes vBoxes;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoEnvironment() {
			vLetters = new List<DemoLetter>();
			vLetterCache = new List<DemoLetter>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			RandomUtil.Init(RandomSeed);

			////

			var lightObj = new GameObject("Light");
			lightObj.transform.SetParent(gameObject.transform, false);
			lightObj.transform.localPosition = new Vector3(0, 1.5f, 0);

			vLight = lightObj.AddComponent<Light>();
			vLight.type = LightType.Point;
			vLight.range = LightRange;
			vLight.intensity = 1;

			////

			var pixObj = new GameObject("PixelLabel");
			pixObj.transform.SetParent(gameObject.transform, false);

			UiLabel pix = pixObj.AddComponent<UiLabel>();
			pixObj.SetActive(false);

			vPixels = new DemoTextPixels(pix);

			////

			/*var boxesObj = new GameObject("Boxes");
			boxesObj.transform.SetParent(gameObject.transform, false);
			boxesObj.transform.localPosition = new Vector3(0, 0, 3.5f);
			boxesObj.transform.localRotation = 
				Quaternion.FromToRotation(Vector3.up, new Vector3(0, 1, -1.5f).normalized);
			boxesObj.transform.localScale = Vector3.one*0.2f;

			vBoxes = boxesObj.AddComponent<DemoBoxes>();*/

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
				ring.Radius = RandomUtil.Float(LightRange*0.4f)+3;
			}

			////
			
			GameObject ovrObj = GameObject.Find("LeapOVRPlayerController");

			if ( ovrObj != null ) {
				OVRPlayerController ovrPlayer = ovrObj.GetComponent<OVRPlayerController>();
				ovrPlayer.SetSkipMouseRotation(true);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( Input.GetKey(KeyCode.Escape) ) {
				Application.Quit();
				return;
			}

			InputForTests.UpdateOculus();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void FixedUpdate() {
			bool removeFirst = false;

			foreach ( DemoLetter lett in vLetters ) {
				Transform tx = lett.gameObject.transform;
				tx.localPosition += new Vector3(0, 0, 0.03f);
				tx.localRotation *= Quaternion.AngleAxis(0.12f, lett.RandomAxis);

				if ( tx.localPosition.z > LightRange ) {
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
			lettObj.SetActive(true);

			lettObj.transform.parent.rotation = CameraTransform.rotation;

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


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void RemoveFirstLetter() {
			DemoLetter lett = vLetters.First();
			lett.gameObject.SetActive(false);

			vLetters.RemoveAt(0);
			vLetterCache.Add(lett);
		}

	}

}
