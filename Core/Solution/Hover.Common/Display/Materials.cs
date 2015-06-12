using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hover.Common.Display {

	/*================================================================================================*/
	public static class Materials {

		private static readonly Material VertColorTop =
			new Material(Shader.Find("HoverVR/VertColorTop"));

		private static readonly Material VertColorTex =
			new Material(Shader.Find("HoverVR/VertColorTex"));

		private static readonly Material VertColorTexTwoSided =
			new Material(Shader.Find("HoverVR/VertColorTexTwoSided"));

		public const int BaseRenderQueue = 3200;
		public const int DepthHintMin = -4;
		public const int DepthHintMax = 4;

		private static readonly IDictionary<string, Material> LayerMap = 
			new Dictionary<string, Material>();
		private static readonly IDictionary<int, Material> TextMap = new Dictionary<int, Material>();
		private static readonly int LayerCount = Enum.GetNames(typeof(Layer)).Length;
		private static Material CursorMat;

		public enum Layer {
			Background,
			Ticks,
			Highlight,
			SelectAndEdge,
			Icon,
			Text,
			AboveText
		}

		public enum IconOffset {
			None,
			CheckOuter,
			CheckInner,
			RadioOuter,
			RadioInner,
			Parent,
			Slider,
			Sticky
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static int GetRenderQueue(Layer pLayer, int pDepthHint) {
			if ( pDepthHint > DepthHintMax || pDepthHint < DepthHintMin ) {
				throw new Exception("GroupQueue ("+pDepthHint+") is out of bounds: ["+
					DepthHintMin+", "+DepthHintMax+"]");
			}

			return BaseRenderQueue + LayerCount*pDepthHint + (int)pLayer;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Material GetLayer(Layer pLayer, int pDepthHint, string pTexture=null) {
			string key = pLayer+"|"+pDepthHint+(pTexture == null ? "" : "|"+pTexture);

			if ( !LayerMap.ContainsKey(key) ) {
				Material mat = (Material)Object.Instantiate(VertColorTexTwoSided);
				mat.renderQueue = GetRenderQueue(pLayer, pDepthHint);

				if ( pTexture != null ) {
					mat.mainTexture = Resources.Load<Texture2D>("Textures/"+pTexture);
				}

				LayerMap.Add(key, mat);
			}

			return LayerMap[key];
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Material GetTextLayer(Material pTextMat, int pDepthHint) {
			if ( !TextMap.ContainsKey(pDepthHint) ) {
				Material mat = (Material)Object.Instantiate(pTextMat);
				mat.renderQueue = GetRenderQueue(Layer.Text, pDepthHint);
				TextMap.Add(pDepthHint, mat);
			}

			return TextMap[pDepthHint];
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Material GetCursorLayer() {
			if ( CursorMat == null ) {
				CursorMat = (Material)Object.Instantiate(VertColorTop);
				CursorMat.renderQueue = BaseRenderQueue + LayerCount*(DepthHintMax+1);
			}

			return CursorMat;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void SetMeshColor(Mesh pMesh, Color32 pColor) {
			int count = pMesh.vertexCount;
			Color32[] colors32 = pMesh.colors32;
			
			if ( colors32 == null || colors32.Length != count ) {
				colors32 = new Color32[count];
			}

			for ( int i = 0 ; i < count ; i++ ) {
				colors32[i] = pColor;
			}

			pMesh.colors32 = colors32;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void SetMeshIconCoords(Mesh pMesh, IconOffset pOffset) {
			Vector2[] uvList = pMesh.uv;
			const float step = 1/8f;
			float offset = step*(int)pOffset;
			var uvCenter = new Vector2(0.5f, 0.5f);
			const float cheatToCenterAmount = 0.015f;

			for ( int i = 0 ; i < uvList.Length ; i++ ) {
				Vector2 uv = uvList[i];
				uv = Vector2.Lerp(uv, uvCenter, cheatToCenterAmount);
				uv.x *= step;
				uv.x += offset;
				uvList[i] = uv;
			}

			pMesh.uv = uvList;
		}

	}

}
