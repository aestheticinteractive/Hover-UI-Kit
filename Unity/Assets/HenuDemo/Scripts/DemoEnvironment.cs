using System;
using System.Collections.Generic;
using Henu.Navigation;
using UnityEngine;

namespace HenuDemo {

	/*================================================================================================*/
	public class DemoEnvironment : MonoBehaviour {

		private const int Count = 200;

		private Material vCubeMat;
		private GameObject[] vHolds;
		private GameObject[] vCubes;
		private IDictionary<int, Color> vColorMap;
		private System.Random vRandom;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vCubeMat = new Material(Shader.Find("Diffuse"));
			vHolds = new GameObject[Count];
			vCubes = new GameObject[Count];
			vRandom = new System.Random();

			for ( int i = 0 ; i < Count ; ++i ) {
				float scaleAmt = (float)Math.Pow(RandomFloat(0.6f, 1.2f), 3);

				var hold = new GameObject("Hold"+i);
				hold.transform.parent = gameObject.transform;
				hold.transform.localRotation = RandomQuaternion();
				vHolds[i] = hold;

				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.name = "Cube"+i;
				cube.renderer.sharedMaterial = vCubeMat;
				cube.transform.parent = hold.transform;
				cube.transform.localPosition = Vector3.up*3;
				cube.transform.localRotation = RandomQuaternion();
				cube.transform.localScale = RandomUnitVector(0.3f)*scaleAmt;
				vCubes[i] = cube;
			}

			////

			DemoNavDelegate navDel = DemoNavComponent.NavDelegate;
			DemoNavItems navItems = navDel.Items;

			vColorMap = new Dictionary<int, Color> {
				{ navItems.ColorWhite.Id,	Color.white },
				{ navItems.ColorRed.Id,		Color.red },
				{ navItems.ColorOrange.Id,	new Color(1, 0.6f, 0) },
				{ navItems.ColorYellow.Id,	Color.yellow },
				{ navItems.ColorGreen.Id,	Color.green },
				{ navItems.ColorBlue.Id,	Color.blue },
				{ navItems.ColorPurple.Id,	new Color(0.8f, 0, 1f) }
			};

			navDel.OnColorChange += HandleColorChange;
			HandleColorChange(navItems.ColorWhite);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleColorChange(NavItem pItem) {
			vCubeMat.color = vColorMap[pItem.Id];
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private Vector3 RandomUnitVector() {
			var v = new Vector3(
				RandomFloat(-1, 1),
				RandomFloat(-1, 1),
				RandomFloat(-1, 1)
			);

			return v.normalized;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private Vector3 RandomUnitVector(float pMinDimension) {
			var v = RandomUnitVector();
			v.x = Math.Max(v.x, pMinDimension);
			v.y = Math.Max(v.y, pMinDimension);
			v.z = Math.Max(v.z, pMinDimension);
			return v.normalized;
		}

		/*--------------------------------------------------------------------------------------------*/
		private Quaternion RandomQuaternion() {
			return Quaternion.AngleAxis((float)vRandom.NextDouble()*360, RandomUnitVector());
		}

		/*--------------------------------------------------------------------------------------------*/
		private float RandomFloat(float pMin, float pMax) {
			return (float)vRandom.NextDouble()*(pMax-pMin) + pMin;
		}

	}

}
