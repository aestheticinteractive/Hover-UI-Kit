using System.Collections.Generic;
using UnityEngine;

namespace Hover.Common.Display {

	/*================================================================================================*/
	public static class Materials {

		public static readonly Material VertColorTex =
			new Material(Shader.Find("HoverVR/VertColorTex"));

		public static readonly Material VertColorTexTwoSided =
			new Material(Shader.Find("HoverVR/VertColorTexTwoSided"));

		public static readonly Material StandardIcons = InitStandardIcons();

		private static readonly IDictionary<RenderQueueLayer, Material> LayerMap = 
			new Dictionary<RenderQueueLayer, Material>();

		public enum RenderQueueLayer {
			Background = 3000+100,
			Ticks,
			Highlight,
			SelectAndEdge,
			Icon,
			Text,
			AboveText,
			Cursor
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
		private static Material InitStandardIcons() {
			Material mat = (Material)Object.Instantiate(VertColorTexTwoSided);
			mat.mainTexture = Resources.Load<Texture2D>("Textures/StandardIcons");
			mat.renderQueue = (int)RenderQueueLayer.Icon;
			return mat;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Material GetLayer(RenderQueueLayer pLayer, bool pTwoSided=true) {
			if ( !LayerMap.ContainsKey(pLayer) ) {
				Material mat = (Material)Object.Instantiate(pTwoSided ? 
					VertColorTexTwoSided : VertColorTex);
				mat.renderQueue = (int)pLayer;
				LayerMap.Add(pLayer, mat);
			}

			return LayerMap[pLayer];
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Material Copy(Material pSource, int pRenderQueue) {
			var matObj = new GameObject("CopyMaterial");
			MeshRenderer rend = matObj.AddComponent<MeshRenderer>();
			rend.material = pSource;

			Material matCopy = rend.material;
			matCopy.name = matObj.name;
			matCopy.renderQueue = pRenderQueue;

			Object.Destroy(matObj);
			return matCopy;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static void SetMeshColor(Mesh pMesh, Color32 pColor) {
			int count = pMesh.vertexCount;

			//TODO: BUG: the Hovercast arcs show strange-colored artifacts during mesh changes

			if ( pMesh.colors32.Length == count && count > 0 && pMesh.colors32[0].Equals(pColor) ) {
				return;
			}

			var colors32 = new Color32[count];

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
