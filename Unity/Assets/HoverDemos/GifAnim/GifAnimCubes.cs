using System;
using HoverDemos.Common;
using UnityEngine;

namespace HoverDemos.GifAnim {

	/*================================================================================================*/
	public class GifAnimCubes : MonoBehaviour {

		public int RandomSeed = 116; //110, 76, 63, 77, 3, 27
		public int CubeCount = 400;

		private Material vCubeMat;
		private GameObject vCubesObj;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			RandomUtil.Init(RandomSeed);

			vCubeMat = new Material(Shader.Find("Diffuse"));
			vCubeMat.color = new Color(0x66/255f, 0x88/255f, 0x33/255f);

			vCubesObj = new GameObject("Cubes");
			vCubesObj.transform.SetParent(gameObject.transform, false);
			
			for ( int i = 0 ; i < CubeCount ; ++i ) {
				BuildCube(i);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( Input.GetKey(KeyCode.Escape) ) {
				Application.Quit();
				return;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildCube(int pIndex) {
			var hold = new GameObject("Hold"+pIndex);
			hold.transform.SetParent(vCubesObj.transform, false);

			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.name = "Cube"+pIndex;
			cube.transform.SetParent(hold.transform, false);
			cube.GetComponent<Renderer>().sharedMaterial = vCubeMat;

			////

			hold.transform.localRotation = UnityEngine.Random.rotationUniform;
			cube.transform.localRotation = UnityEngine.Random.rotationUniform;

			float radius = RandomUtil.Float(4, 10);

			float bobPos = RandomUtil.Float(-1, 1);
			bobPos = (float)Math.Sin(bobPos*Math.PI)/2f + 0.5f;
			bobPos = Mathf.Lerp(radius, radius+3, bobPos);
			cube.transform.localPosition = new Vector3(0, 0, bobPos);

			float growPos = RandomUtil.Float(-1, 1);
			growPos = (float)Math.Sin(growPos*Math.PI)/2f + 0.5f;
			cube.transform.localScale = Vector3.Lerp(
				RandomUtil.UnitVector(0.4f)*0.6f, RandomUtil.UnitVector(0.4f)*1.2f, growPos);
		}

	}

}
