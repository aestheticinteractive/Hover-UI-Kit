using System;
using System.Collections.Generic;
using Hovercast.Core.Navigation;
using UnityEngine;

namespace Hovercast.Demo {

	/*================================================================================================*/
	public class DemoEnvironment : MonoBehaviour {

		private const int Count = 400;
		private const float SpeedMax = 4;
		private const float SpeedMin = 0.4f;
		private const float SpeedRange = SpeedMax-SpeedMin;

		private GameObject[] vHolds;
		private GameObject[] vCubes;
		private Light vLight;
		private Light vSpotlight;
		private GameObject vEnviro;
		private System.Random vRandom;

		private DemoMotion vOrbitMotion;
		private DemoMotion vSpinMotion;
		private DemoMotion vBobMotion;
		private DemoMotion vGrowMotion;

		private DemoAnimFloat vLightSpotAnim;
		private DemoAnimVector3 vCameraAnim;
		private DemoAnimQuaternion vCameraRotAnim;

		private DemoNavItems vNavItems;
		private IDictionary<int, DemoMotion> vMotionMap;
		private IDictionary<int, Vector3> vCameraMap;
		private IDictionary<int, Quaternion> vCameraRotMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			vHolds = new GameObject[Count];
			vCubes = new GameObject[Count];
			vLight = GameObject.Find("Light").GetComponent<Light>();
			vSpotlight = GameObject.Find("Spotlight").GetComponent<Light>();
			vEnviro = GameObject.Find("DemoEnvironment");
			vRandom = new System.Random();

			for ( int i = 0 ; i < Count ; ++i ) {
				BuildCube(i);
			}

			////

			GameObject ovrPlayerObj = GameObject.Find("LeapOVRPlayerController");

			if ( ovrPlayerObj != null ) {
				OVRPlayerController ovrPlayer = ovrPlayerObj.GetComponent<OVRPlayerController>();
				ovrPlayer.SetSkipMouseRotation(true);
			}

			////

			vOrbitMotion = new DemoMotion(10, 600);
			vSpinMotion = new DemoMotion(45, 600);
			vBobMotion = new DemoMotion(0.5f, 600);
			vGrowMotion = new DemoMotion(0.5f, 600);

			vLightSpotAnim = new DemoAnimFloat(600);
			vCameraAnim = new DemoAnimVector3(6000);
			vCameraRotAnim = new DemoAnimQuaternion(6000);

			////

			DemoNavDelegate navDel = DemoNavComponent.NavDelegate;
			vNavItems = navDel.Items;

			vMotionMap = new Dictionary<int, DemoMotion> {
				{ vNavItems.MotionOrbit.Id,	vOrbitMotion },
				{ vNavItems.MotionSpin.Id,	vSpinMotion },
				{ vNavItems.MotionBob.Id,	vBobMotion },
				{ vNavItems.MotionGrow.Id,	vGrowMotion }
			};

			vCameraMap = new Dictionary<int, Vector3> {
				{ vNavItems.CameraCenter.Id,	Vector3.zero },
				{ vNavItems.CameraBack.Id,	new Vector3(0, 0, 20) },
				{ vNavItems.CameraTop.Id,	new Vector3(0, 0, 20) }
			};

			vCameraRotMap = new Dictionary<int, Quaternion> {
				{ vNavItems.CameraCenter.Id, Quaternion.identity },
				{ vNavItems.CameraBack.Id,	Quaternion.identity },
				{ vNavItems.CameraTop.Id,	Quaternion.FromToRotation(Vector3.forward, Vector3.up) }
			};

			navDel.OnMotionChange += HandleMotionChange;
			navDel.OnCameraChange += HandleCameraChange;

			vNavItems.ColorWhite.OnValueChanged += HandleColorWhiteToggle;
			vNavItems.ColorRandom.OnValueChanged += HandleColorRandomToggle;
			vNavItems.ColorCustom.OnValueChanged += HandleColorCustomToggle;
			vNavItems.LightSpot.OnSelected += HandleLightSpotSelected;
			vNavItems.LightSpot.OnDeselected += HandleLightSpotSelected;
			vNavItems.CameraReorient.OnSelected += HandleCameraReorient;

			////

			vNavItems.ColorWhite.Value = true;
			vNavItems.ColorHue.IsEnabled = false;
			vNavItems.ColorHue.ValueToLabel = ((v,sv) => "Hue: "+Math.Round(sv*360));
			vNavItems.ColorHue.Value = 0.333f;

			vNavItems.LightPos.Snaps = 4;
			vNavItems.LightPos.Ticks = 4;
			vNavItems.LightPos.Value = 2/3f;
			vNavItems.LightPos.ValueToLabel = ((v, sv) => {
				string lbl = "";

				switch ( (int)Math.Round(sv*3) ) {
					case 0: lbl = "Lowest"; break;
					case 1: lbl = "Low"; break;
					case 2: lbl = "High"; break;
					case 3: lbl = "Highest"; break;
				}

				return "Pos: "+lbl;
			});

			vNavItems.LightInten.Value = 0.5f;
			vNavItems.LightInten.ValueToLabel = ((v, sv) => "Power: "+Math.Round((sv*120)+20));

			vNavItems.CameraCenter.Value = true;

			vNavItems.MotionSpeed.Value = (1-SpeedMin)/SpeedRange;
			vNavItems.MotionSpeed.ValueToLabel = 
				((v, sv) => "Speed: "+((sv*SpeedRange)+SpeedMin).ToString("0.0")+"x");

			UpdateLightPos();
			UpdateLightInten();
			UpdateMotionSpeed();
			vSpotlight.enabled = false;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			if ( Input.GetKey(KeyCode.Escape) ) {
				Application.Quit();
				return;
			}

			UpdateOculus();

			vOrbitMotion.Update();
			vSpinMotion.Update();
			vBobMotion.Update();
			vGrowMotion.Update();

			for ( int i = 0 ; i < Count ; ++i ) {
				UpdateCube(i);
			}

			if ( vNavItems.LightPos.IsStickySelected ) {
				UpdateLightPos();
			}

			if ( vNavItems.LightInten.IsStickySelected ) {
				UpdateLightInten();
			}

			if ( vNavItems.MotionSpeed.IsStickySelected ) {
				UpdateMotionSpeed();
			}

			vSpotlight.intensity = vLightSpotAnim.GetValue();
			vSpotlight.enabled = (vSpotlight.intensity > 0);

			vEnviro.transform.localPosition = vCameraAnim.GetValue();
			vEnviro.transform.localRotation = vCameraRotAnim.GetValue();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static void UpdateOculus() {
			if ( OVRManager.capiHmd == null ) {
				return;
			}

			if ( Input.GetKey(KeyCode.R) ) {
				OVRManager.display.RecenterPose();
			}

			if ( !OVRManager.capiHmd.GetHSWDisplayState().Displayed ) {
				return;
			}

			OVRManager.capiHmd.DismissHSWDisplay();
			OVRManager.display.RecenterPose();
		}

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
			cubeData.ColorRandom = RandomUnitColor(0.1f, 1);
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
			bobPos = Mathf.Lerp(cubeData.BobRadiusMin, cubeData.BobRadiusMax, bobPos);
			cube.transform.localPosition = new Vector3(0, 0, bobPos);

			float growPos = cubeData.GrowInitPos+vGrowMotion.Position*cubeData.GrowSpeed;
			growPos = (float)Math.Sin(growPos*Math.PI)/2f + 0.5f;
			cube.transform.localScale = 
				Vector3.Lerp(cubeData.GrowScaleMin, cubeData.GrowScaleMax, growPos);

			if ( vNavItems.ColorHue.IsStickySelected ) {
				UpdateCubeHue(cube);
				float value = vNavItems.ColorHue.Value;
				cube.renderer.sharedMaterial.color = HsvToColor(value*360, 1, 1);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateCubeHue(GameObject pCube) {
			float sv = vNavItems.ColorHue.SnappedValue;
			pCube.renderer.sharedMaterial.color = HsvToColor(sv*360, 1, 1);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateLightPos() {
			float sv = vNavItems.LightPos.SnappedValue;
			vLight.gameObject.transform.localPosition = new Vector3(0, (sv*2-1)*9, 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateLightInten() {
			float sv = vNavItems.LightInten.SnappedValue;
			vLight.intensity = sv*1.2f+0.2f;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateMotionSpeed() {
			float speed = vNavItems.MotionSpeed.SnappedValue*SpeedRange + SpeedMin;

			vOrbitMotion.GlobalSpeed = speed;
			vSpinMotion.GlobalSpeed = speed;
			vBobMotion.GlobalSpeed = speed;
			vGrowMotion.GlobalSpeed = speed;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleColorWhiteToggle(NavItem<bool> pItem) {
			if ( !pItem.Value ) {
				return;
			}

			foreach ( GameObject cube in vCubes ) {
				cube.renderer.sharedMaterial.color = Color.white;
			}

			vNavItems.ColorHue.IsEnabled = false;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void HandleColorRandomToggle(NavItem<bool> pItem) {
			if ( !pItem.Value ) {
				return;
			}

			for ( int i = 0 ; i < Count ; ++i ) {
				GameObject cube = vCubes[i];
				DemoCube cubeData = cube.GetComponent<DemoCube>();
				cube.renderer.sharedMaterial.color = cubeData.ColorRandom;
			}

			vNavItems.ColorHue.IsEnabled = false;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleColorCustomToggle(NavItem<bool> pItem) {
			if ( !pItem.Value ) {
				return;
			}

			foreach ( GameObject cube in vCubes ) {
				UpdateCubeHue(cube);
			}

			vNavItems.ColorHue.IsEnabled = true;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleLightSpotSelected(NavItem pItem) {
			vLightSpotAnim.Start(vSpotlight.intensity, (pItem.IsStickySelected ? 3 : 0));
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleMotionChange(NavItem pItem) {
			vMotionMap[pItem.Id].Enable(((NavItemCheckbox)pItem).Value);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void HandleCameraChange(NavItem pItem) {
			vCameraAnim.Start(vEnviro.transform.localPosition, vCameraMap[pItem.Id]);
			vCameraRotAnim.Start(vEnviro.transform.localRotation, vCameraRotMap[pItem.Id]);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void HandleCameraReorient(NavItem pItem) {
			if ( OVRManager.display != null ) {
				OVRManager.display.RecenterPose();
			}
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

		/*--------------------------------------------------------------------------------------------*/
		private float RandomFloat(float pMin, float pMax) {
			return (float)vRandom.NextDouble()*(pMax-pMin) + pMin;
		}

		/*--------------------------------------------------------------------------------------------*/
		private float RandomFloat(float pMin, float pMax, float pPow) {
			return (float)Math.Pow(RandomFloat(pMin, pMax), pPow);
		}

		/*--------------------------------------------------------------------------------------------*/
		//based on: http://stackoverflow.com/questions/1335426
		public static Color HsvToColor(float pHue, float pSat, float pVal) {
			float hue60 = pHue/60f;
			int i = (int)Math.Floor(hue60)%6;
			float f = hue60 - (int)Math.Floor(hue60);

			float v = pVal;
			float p = pVal * (1-pSat);
			float q = pVal * (1-f*pSat);
			float t = pVal * (1-(1-f)*pSat);

			switch ( i ) {
				case 0: return new Color(v, t, p);
				case 1: return new Color(q, v, p);
				case 2: return new Color(p, v, t);
				case 3: return new Color(p, q, v);
				case 4: return new Color(t, p, v);
				case 5: return new Color(v, p, q);
			}

			return Color.black;
		}

	}

}
