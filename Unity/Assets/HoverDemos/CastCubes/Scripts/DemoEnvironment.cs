using System;
using System.Collections.Generic;
using Hover.Core.Utils;
using HoverDemos.Common;
using UnityEngine;
using UnityEngine.XR;

namespace HoverDemos.CastCubes {

	/*================================================================================================*/
	public class DemoEnvironment : MonoBehaviour {

		public int RandomSeed = 0;

		private const int Count = 400;

		public enum ColorMode {
			White,
			Random,
			Custom
		}

		public enum MotionType {
			Orbit,
			Spin,
			Bob,
			Grow
		}

		public enum CameraPlacement {
			Center,
			Back,
			Top
		}

		public Light MainLight;
		public Light Spotlight;

		private readonly GameObject[] vHolds;
		private readonly GameObject[] vCubes;

		private readonly DemoMotion vOrbitMotion;
		private readonly DemoMotion vSpinMotion;
		private readonly DemoMotion vBobMotion;
		private readonly DemoMotion vGrowMotion;

		private readonly DemoAnimFloat vLightSpotAnim;
		private readonly DemoAnimVector3 vCameraAnim;
		private readonly DemoAnimQuaternion vCameraRotAnim;

		private readonly IDictionary<MotionType, DemoMotion> vMotionMap;
		private readonly IDictionary<CameraPlacement, Vector3> vCameraMap;
		private readonly IDictionary<CameraPlacement, Quaternion> vCameraRotMap;

		private GameObject vCubesObj;
		private ColorMode vColorMode;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public DemoEnvironment() {
			vHolds = new GameObject[Count];
			vCubes = new GameObject[Count];

			vOrbitMotion = new DemoMotion(10, 600);
			vSpinMotion = new DemoMotion(45, 600);
			vBobMotion = new DemoMotion(0.5f, 600);
			vGrowMotion = new DemoMotion(0.5f, 600);

			vLightSpotAnim = new DemoAnimFloat(600);
			vCameraAnim = new DemoAnimVector3(6000);
			vCameraRotAnim = new DemoAnimQuaternion(6000);

			vMotionMap = new Dictionary<MotionType, DemoMotion> {
				{ MotionType.Orbit,	vOrbitMotion },
				{ MotionType.Spin,	vSpinMotion },
				{ MotionType.Bob,	vBobMotion },
				{ MotionType.Grow,	vGrowMotion }
			};

			vCameraMap = new Dictionary<CameraPlacement, Vector3> {
				{ CameraPlacement.Center,	Vector3.zero },
				{ CameraPlacement.Back,	new Vector3(0, 0, 20) },
				{ CameraPlacement.Top,	new Vector3(0, 0, 20) }
			};

			vCameraRotMap = new Dictionary<CameraPlacement, Quaternion> {
				{ CameraPlacement.Center, Quaternion.identity },
				{ CameraPlacement.Back,	Quaternion.identity },
				{ CameraPlacement.Top,	Quaternion.FromToRotation(Vector3.forward, Vector3.up) }
			};
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			RandomUtil.Init(RandomSeed);
			XRSettings.eyeTextureResolutionScale = 2;

			vCubesObj = new GameObject("Cubes");
			vCubesObj.transform.SetParent(gameObject.transform, false);
			
			for ( int i = 0 ; i < Count ; ++i ) {
				BuildCube(i);
			}

			Spotlight.enabled = false;
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

			vOrbitMotion.Update();
			vSpinMotion.Update();
			vBobMotion.Update();
			vGrowMotion.Update();

			for ( int i = 0 ; i < Count ; ++i ) {
				UpdateCube(i);
			}
			
			Spotlight.intensity = vLightSpotAnim.GetValue();
			Spotlight.enabled = (Spotlight.intensity > 0);

			gameObject.transform.localPosition = vCameraAnim.GetValue();
			gameObject.transform.localRotation = vCameraRotAnim.GetValue();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ColorMode GetColorMode() {
			return vColorMode;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetColorMode(ColorMode pMode, float pHue=0) {
			vColorMode = pMode;

			Color color = Color.white;

			if ( vColorMode == ColorMode.Custom ) {
				color = DisplayUtil.HsvToColor(pHue, 1, 1);
			}

			for ( int i = 0 ; i < Count ; ++i ) {
				GameObject cube = vCubes[i];

				if ( vColorMode == ColorMode.Random ) {
					color = cube.GetComponent<DemoCube>().ColorRandom;
				}

				cube.GetComponent<Renderer>().sharedMaterial.color = color;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ToggleMotion(MotionType pType, bool pIsEnabled) {
			vMotionMap[pType].Enable(pIsEnabled);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetMotionSpeed(float pSpeed) {
			vOrbitMotion.GlobalSpeed = pSpeed;
			vSpinMotion.GlobalSpeed = pSpeed;
			vBobMotion.GlobalSpeed = pSpeed;
			vGrowMotion.GlobalSpeed = pSpeed;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetLightPos(float pPosition) {
			MainLight.gameObject.transform.localPosition = new Vector3(0, pPosition, 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void SetLightIntensitiy(float pIntensity) {
			MainLight.intensity = pIntensity;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ShowSpotlight(bool pShow) {
			vLightSpotAnim.Start(Spotlight.intensity, (pShow ? 3 : 0));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetCameraPlacement(CameraPlacement pPlace) {
			vCameraAnim.Start(gameObject.transform.localPosition, vCameraMap[pPlace]);
			vCameraRotAnim.Start(gameObject.transform.localRotation, vCameraRotMap[pPlace]);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void ReorientCamera() {
			InputTracking.Recenter();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void BuildCube(int pIndex) {
			float radius = RandomUtil.Float(4, 10);
			float radiusPercent = (radius-4)/6f;
			float orbitSpeed = (float)Math.Pow(1-radiusPercent, 2)*0.2f + 0.8f;

			var hold = new GameObject("Hold"+pIndex);
			hold.transform.parent = vCubesObj.transform;
			vHolds[pIndex] = hold;

			DemoCubeHold holdData = hold.AddComponent<DemoCubeHold>();
			holdData.OrbitAxis = RandomUtil.UnitVector();
			holdData.OrbitSpeed = RandomUtil.Float(0.7f, 1, 2)*orbitSpeed;
			holdData.OrbitInitRot = UnityEngine.Random.rotationUniform;

			////

			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.parent = hold.transform;
			cube.name = "Cube"+pIndex;
			cube.GetComponent<Renderer>().material = new Material(Shader.Find("Diffuse"));
			vCubes[pIndex] = cube;

			DemoCube cubeData = cube.AddComponent<DemoCube>();
			cubeData.ColorRandom = RandomUtil.UnitColor(0.1f, 1);
			cubeData.SpinAxis = RandomUtil.UnitVector();
			cubeData.SpinSpeed = RandomUtil.Float(0.5f, 1, 2);
			cubeData.SpinInitRot = UnityEngine.Random.rotationUniform;
			cubeData.BobSpeed = RandomUtil.Float(0.5f, 1, 2);
			cubeData.BobInitPos = RandomUtil.Float(-1, 1);
			cubeData.BobRadiusMin = radius;
			cubeData.BobRadiusMax = cubeData.BobRadiusMin+3;
			cubeData.GrowSpeed = RandomUtil.Float(0.5f, 1, 2);
			cubeData.GrowInitPos = RandomUtil.Float(-1, 1);
			cubeData.GrowScaleMin = RandomUtil.UnitVector(0.4f)*0.6f;
			cubeData.GrowScaleMax = RandomUtil.UnitVector(0.4f)*1.2f;
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
			bobPos = Mathf.Lerp(cubeData.BobRadiusMin, cubeData.BobRadiusMax, bobPos);
			cube.transform.localPosition = new Vector3(0, 0, bobPos);

			float growPos = cubeData.GrowInitPos+vGrowMotion.Position*cubeData.GrowSpeed;
			growPos = (float)Math.Sin(growPos*Math.PI)/2f + 0.5f;
			cube.transform.localScale = 
				Vector3.Lerp(cubeData.GrowScaleMin, cubeData.GrowScaleMax, growPos);
		}

	}

}
