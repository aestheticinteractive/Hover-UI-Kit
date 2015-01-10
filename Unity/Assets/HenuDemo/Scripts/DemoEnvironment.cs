using System;
using System.Collections.Generic;
using Henu.Navigation;
using UnityEngine;

namespace HenuDemo {

	/*================================================================================================*/
	public class DemoEnvironment : MonoBehaviour {

		private const int Count = 400;

		private GameObject[] vHolds;
		private GameObject[] vCubes;
		private Light vLight;
		private GameObject vEnviro;
		private System.Random vRandom;

		private DemoMotion vOrbitMotion;
		private DemoMotion vSpinMotion;
		private DemoMotion vBobMotion;
		private DemoMotion vGrowMotion;

		private DemoAnimVector3 vLightPosAnim;
		private DemoAnimFloat vLightIntenAnim;
		private DemoAnimVector3 vCameraPosAnim;
		private DemoAnimQuaternion vCameraRotAnim;

		private IDictionary<int, Color> vColorMap;
		private IDictionary<int, DemoMotion> vMotionMap;
		private IDictionary<int, Vector3> vLightPosMap;
		private IDictionary<int, float> vLightIntenMap;
		private IDictionary<int, Vector3> vCameraPosMap;
		private IDictionary<int, Quaternion> vCameraRotMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vHolds = new GameObject[Count];
			vCubes = new GameObject[Count];
			vLight = GameObject.Find("Light").GetComponent<Light>();
			vEnviro = GameObject.Find("DemoEnvironment");
			vRandom = new System.Random();

			for ( int i = 0 ; i < Count ; ++i ) {
				BuildCube(i);
			}

			////

			vOrbitMotion = new DemoMotion(10, 600);
			vSpinMotion = new DemoMotion(45, 600);
			vBobMotion = new DemoMotion(0.5f, 600);
			vGrowMotion = new DemoMotion(0.5f, 600);

			vLightPosAnim = new DemoAnimVector3(2000);
			vLightIntenAnim = new DemoAnimFloat(600);
			vCameraPosAnim = new DemoAnimVector3(6000);
			vCameraRotAnim = new DemoAnimQuaternion(6000);

			////

			DemoNavDelegate navDel = DemoNavComponent.NavDelegate;
			DemoNavItems navItems = navDel.Items;

			vColorMap = new Dictionary<int, Color> {
				{ navItems.ColorWhite.Id,	Color.white },
				{ navItems.ColorRed.Id,		Color.red },
				{ navItems.ColorYellow.Id,	Color.yellow },
				{ navItems.ColorGreen.Id,	Color.green },
				{ navItems.ColorBlue.Id,	Color.blue }
			};

			vMotionMap = new Dictionary<int, DemoMotion> {
				{ navItems.MotionOrbit.Id,	vOrbitMotion },
				{ navItems.MotionSpin.Id,	vSpinMotion },
				{ navItems.MotionBob.Id,	vBobMotion },
				{ navItems.MotionGrow.Id,	vGrowMotion }
			};

			vLightPosMap = new Dictionary<int, Vector3> {
				{ navItems.LightPosHighest.Id,	new Vector3(0,  9, 0) },
				{ navItems.LightPosHigh.Id,		new Vector3(0,  3, 0) },
				{ navItems.LightPosLow.Id,		new Vector3(0, -3, 0) },
				{ navItems.LightPosLowest.Id,	new Vector3(0, -9, 0) }
			};

			vLightIntenMap = new Dictionary<int, float> {
				{ navItems.LightIntenHigh.Id,	1.4f },
				{ navItems.LightIntenMed.Id,	0.8f },
				{ navItems.LightIntenLow.Id,	0.2f }
			};

			vCameraPosMap = new Dictionary<int, Vector3> {
				{ navItems.CameraPosCenter.Id,	Vector3.zero },
				{ navItems.CameraPosBack.Id,	new Vector3(0, 0, 20) },
				{ navItems.CameraPosTop.Id,		new Vector3(0, 0, 20) }
			};

			vCameraRotMap = new Dictionary<int, Quaternion> {
				{ navItems.CameraPosCenter.Id, Quaternion.identity },
				{ navItems.CameraPosBack.Id, Quaternion.identity },
				{ navItems.CameraPosTop.Id,	Quaternion.FromToRotation(Vector3.forward, Vector3.up) }
			};

			navDel.OnColorChange += HandleColorChange;
			navDel.OnMotionChange += HandleMotionChange;
			navDel.OnLightPosChange += HandleLightPosChange;
			navDel.OnLightIntenChange += HandleLightIntenChange;
			navDel.OnCameraPosChange += HandleCameraPosChange;

			vLight.transform.localPosition = Vector3.zero;
			vLight.intensity = 0;

			HandleColorChange(DemoNavItems.GetFirstSelectedChildItem(navItems.Colors));
			HandleLightPosChange(DemoNavItems.GetFirstSelectedChildItem(navItems.LightPos));
			HandleLightIntenChange(DemoNavItems.GetFirstSelectedChildItem(navItems.LightInten));
			HandleCameraPosChange(DemoNavItems.GetFirstSelectedChildItem(navItems.CameraPos));
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( Input.GetKey(KeyCode.R) ) {
				OVRManager.display.RecenterPose();
			}

			vOrbitMotion.Update();
			vSpinMotion.Update();
			vBobMotion.Update();
			vGrowMotion.Update();

			for ( int i = 0 ; i < Count ; ++i ) {
				UpdateCube(i);
			}

			vLight.gameObject.transform.localPosition = vLightPosAnim.GetValue();
			vLight.intensity = vLightIntenAnim.GetValue();
			vEnviro.transform.localPosition = vCameraPosAnim.GetValue();
			vEnviro.transform.localRotation = vCameraRotAnim.GetValue();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildCube(int pIndex) {
			float radius = RandomFloat(4, 10);
			float radiusPercent = (radius-4)/6f;
			float orbitSpeed = (float)Math.Pow(1-radiusPercent, 2)*0.2f + 0.8f;

			var hold = new GameObject("Hold"+pIndex);
			hold.transform.parent = gameObject.transform;
			vHolds[pIndex] = hold;

			DemoCubeHold holdData = hold.AddComponent<DemoCubeHold>();
			holdData.OrbitAxis = RandomUnitVector();
			holdData.OrbitSpeed = RandomFloat(0.7f, 1, 2)*orbitSpeed;
			holdData.OrbitInitRot = UnityEngine.Random.rotationUniform;

			////

			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.parent = hold.transform;
			cube.name = "Cube"+pIndex;
			cube.renderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			vCubes[pIndex] = cube;

			DemoCube cubeData = cube.AddComponent<DemoCube>();
			cubeData.ColorLight = RandomUnitColor(0.2f, 1);
			cubeData.ColorDark = RandomUnitColor(0.1f, 0.5f);
			cubeData.SpinAxis = RandomUnitVector();
			cubeData.SpinSpeed = RandomFloat(0.5f, 1, 2);
			cubeData.SpinInitRot = UnityEngine.Random.rotationUniform;
			cubeData.BobSpeed = RandomFloat(0.5f, 1, 2);
			cubeData.BobInitPos = RandomFloat(-1, 1);
			cubeData.BobRadiusMin = radius;
			cubeData.BobRadiusMax = cubeData.BobRadiusMin+3;
			cubeData.GrowSpeed = RandomFloat(0.5f, 1, 2);
			cubeData.GrowInitPos = RandomFloat(-1, 1);
			cubeData.GrowScaleMin = RandomUnitVector(0.4f)*0.6f;
			cubeData.GrowScaleMax = RandomUnitVector(0.4f)*1.2f;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCube(int pIndex) {
			GameObject hold = vHolds[pIndex];
			GameObject cube = vCubes[pIndex];
			DemoCubeHold holdData = hold.GetComponent<DemoCubeHold>();
			DemoCube cubeData = cube.GetComponent<DemoCube>();

			float orbitAngle = vOrbitMotion.Position*holdData.OrbitSpeed;
			hold.transform.localRotation = holdData.OrbitInitRot*
				Quaternion.AngleAxis(orbitAngle, holdData.OrbitAxis);

			float spinAngle = vSpinMotion.Position*cubeData.SpinSpeed;
			cube.transform.localRotation = cubeData.SpinInitRot*
				Quaternion.AngleAxis(spinAngle, cubeData.SpinAxis);

			float bobPos = cubeData.BobInitPos+vBobMotion.Position*cubeData.BobSpeed;
			bobPos = (float)Math.Sin(bobPos*Math.PI)/2f + 0.5f;
			bobPos = LerpFloat(cubeData.BobRadiusMin, cubeData.BobRadiusMax, bobPos);
			cube.transform.localPosition = new Vector3(0, 0, bobPos);

			float growPos = cubeData.GrowInitPos+vGrowMotion.Position*cubeData.GrowSpeed;
			growPos = (float)Math.Sin(growPos*Math.PI)/2f + 0.5f;
			cube.transform.localScale = 
				Vector3.Lerp(cubeData.GrowScaleMin, cubeData.GrowScaleMax, growPos);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleColorChange(NavItem pItem) {
			Color color = Color.white;
			DemoNavItems items = DemoNavComponent.NavDelegate.Items;
			bool isRandLt = (pItem == items.ColorRandLt);
			bool isRandDk = (pItem == items.ColorRandDk);

			if ( vColorMap.ContainsKey(pItem.Id) ) {
				color = vColorMap[pItem.Id];
			}

			for ( int i = 0 ; i < Count ; ++i ) {
				GameObject cube = vCubes[i];
				DemoCube cubeData = cube.GetComponent<DemoCube>();

				if ( isRandLt ) {
					color = cubeData.ColorLight;
				}
				else if ( isRandDk ) {
					color = cubeData.ColorDark;
				}

				cube.renderer.sharedMaterial.color = color;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleMotionChange(NavItem pItem) {
			vMotionMap[pItem.Id].Enable(pItem.Selected);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleLightPosChange(NavItem pItem) {
			vLightPosAnim.Start(vLight.gameObject.transform.localPosition, vLightPosMap[pItem.Id]);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleLightIntenChange(NavItem pItem) {
			vLightIntenAnim.Start(vLight.intensity, vLightIntenMap[pItem.Id]);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleCameraPosChange(NavItem pItem) {
			vCameraPosAnim.Start(vEnviro.transform.localPosition, vCameraPosMap[pItem.Id]);
			vCameraRotAnim.Start(vEnviro.transform.localRotation, vCameraRotMap[pItem.Id]);
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
		private Color RandomUnitColor(float pMin, float pMax) {
			int major = vRandom.Next()%3;
			int minor = (major+(vRandom.Next()%2)+1)%3;

			Func<int, float> getVal = (i => {
				if ( i == major ) {
					return pMax;
				}

				if ( i == minor ) {
					return RandomFloat(pMin, pMax);
				}

				return RandomFloat(0, pMin);
			});

			return new Color(getVal(0), getVal(1), getVal(2));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private Vector3 RandomUnitVector(float pMinDimension) {
			var v = RandomUnitVector();
			v.x = Math.Max(v.x, pMinDimension);
			v.y = Math.Max(v.y, pMinDimension);
			v.z = Math.Max(v.z, pMinDimension);
			return v.normalized;
		}

		/*--------------------------------------------------------------------------------------------* /
		private Quaternion RandomQuaternion() {
			return Quaternion.AngleAxis((float)vRandom.NextDouble()*360, RandomUnitVector());
		}

		/*--------------------------------------------------------------------------------------------*/
		private float RandomFloat(float pMin, float pMax) {
			return (float)vRandom.NextDouble()*(pMax-pMin) + pMin;
		}

		/*--------------------------------------------------------------------------------------------*/
		private float RandomFloat(float pMin, float pMax, float pPow) {
			return (float)Math.Pow(RandomFloat(pMin, pMax), pPow);
		}

		/*--------------------------------------------------------------------------------------------* /
		public static float ClampFloat(float pValue, float pMin, float pMax) {
			return Math.Min(pMax, Math.Max(pMin, pValue));
		}

		/*--------------------------------------------------------------------------------------------*/
		public static float LerpFloat(float pMin, float pMax, float pAmount) {
			return (pMax-pMin)*pAmount + pMin;
		}

	}

}
