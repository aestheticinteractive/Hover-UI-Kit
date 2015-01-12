using System;
using System.Collections.Generic;
using Henu.Input;
using Henu.State;
using UnityEngine;

namespace Henu.Display {

	/*================================================================================================*/
	public class UiMenuHand : MonoBehaviour {

		private MenuHandState vMenuHand;
		private GameObject vFan;
		private IList<UiMenuPoint> vUiPoints;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Build(MenuHandState pMenuHand, Renderers pRenderers) {
			vMenuHand = pMenuHand;

			vFan = new GameObject("Fan");
			vFan.transform.SetParent(gameObject.transform, false);
			vFan.AddComponent<MeshFilter>();
			vFan.AddComponent<MeshRenderer>();
			vFan.renderer.sharedMaterial = new Material(Shader.Find("Unlit/AlphaSelfIllum"));
			vFan.renderer.sharedMaterial.color = Color.clear;
			BuildFanMesh(vFan.GetComponent<MeshFilter>().mesh, 1, 1.5f);

			vUiPoints = new List<UiMenuPoint>();

			foreach ( InputPointZone zone in MenuHandState.PointZones ) {
				var pointObj = new GameObject("Point-"+zone);
				pointObj.transform.parent = gameObject.transform;

				UiMenuPoint uiPoint = pointObj.AddComponent<UiMenuPoint>();
				uiPoint.Build(vMenuHand, vMenuHand.GetPointState(zone), pRenderers);
				vUiPoints.Add(uiPoint);
			}

			vMenuHand.OnLevelChange += HandleLevelChange;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			foreach ( UiMenuPoint uiPoint in vUiPoints ) {
				uiPoint.gameObject.SetActive(vMenuHand.IsActive && uiPoint.IsActive());
			}

			////

			Quaternion rot = vMenuHand.Rotation;
			MenuPointState ind = vMenuHand.GetPointState(InputPointZone.Index);
			MenuPointState mid = vMenuHand.GetPointState(InputPointZone.Middle);
			MenuPointState rin = vMenuHand.GetPointState(InputPointZone.Ring);
			MenuPointState pin = vMenuHand.GetPointState(InputPointZone.Pinky);

			rot = Quaternion.Slerp(rot, ind.Rotation, 0.1f);
			rot = Quaternion.Slerp(rot, mid.Rotation, 0.1f);
			rot = Quaternion.Slerp(rot, rin.Rotation, 0.1f);
			rot = Quaternion.Slerp(rot, pin.Rotation, 0.1f);

			float scale = 0;
			scale = Math.Max(scale, (ind.Position-vMenuHand.Center).sqrMagnitude);
			scale = Math.Max(scale, (mid.Position-vMenuHand.Center).sqrMagnitude);
			scale = Math.Max(scale, (rin.Position-vMenuHand.Center).sqrMagnitude);
			scale = Math.Max(scale, (pin.Position-vMenuHand.Center).sqrMagnitude);
			scale = (float)Math.Sqrt(scale)*1.1f;

			float alpha = 1-(float)Math.Pow(1-vMenuHand.Strength, 2);
			alpha -= (float)Math.Pow(vMenuHand.GrabStrength, 2);

			vFan.SetActive(vMenuHand.IsActive);
			vFan.transform.localPosition = vMenuHand.Center;
			vFan.transform.localRotation = rot;
			vFan.transform.localScale = Vector3.one*scale;
			vFan.renderer.sharedMaterial.color = new Color(0, 1, 0, Math.Max(0, alpha));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void HandleLevelChange(int pDirection) {
			Update(); //reset point visibility
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static void BuildFanMesh(Mesh pMesh, float pInnerRadius, float pOuterRadius) {
			const int steps = 32;
			const float angle = (float)Math.PI/2f;
			const float angleInc = angle/steps;

			var verts = new List<Vector3>();
			var uvs = new List<Vector2>();
			var tris = new List<int>();

			float a = (float)Math.PI-angle*0.6f;

			for ( int i = 0 ; i <= steps ; ++i ) {
				float x = (float)Math.Sin(a);
				float y = (float)Math.Cos(a);
				int vi = verts.Count;

				verts.Add(new Vector3(x*pInnerRadius, 0, y*pInnerRadius));
				verts.Add(new Vector3(x*pOuterRadius, 0, y*pOuterRadius));

				uvs.Add(new Vector2(0, 0));
				uvs.Add(new Vector2(0, 0));

				if ( i > 0 ) {
					tris.Add(vi-1);
					tris.Add(vi-2);
					tris.Add(vi);

					tris.Add(vi+1);
					tris.Add(vi-1);
					tris.Add(vi);
				}

				a += angleInc;
			}

			pMesh.Clear();
			pMesh.vertices = verts.ToArray();
			pMesh.uv = uvs.ToArray();
			pMesh.triangles = tris.ToArray();
			pMesh.RecalculateNormals();
			pMesh.RecalculateBounds();
			pMesh.Optimize();
		}
		
	}

}
