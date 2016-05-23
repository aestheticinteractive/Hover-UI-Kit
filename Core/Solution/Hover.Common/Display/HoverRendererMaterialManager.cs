using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hover.Common.Renderers.Shared {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public class HoverRendererMaterialManager : MonoBehaviour {

		[Serializable]
		public struct NamedSharedMat {
			public string Key;
			public Material SharedMaterial;
			public int BaseRenderQueue;
		}

		public NamedSharedMat[] NamedMaterials;
		public int RenderQueueHintMultiplier = 4;
		
		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;

		private readonly Dictionary<string, NamedSharedMat> vNamedMap;
		private readonly Dictionary<string, Dictionary<int, Material>> vMatMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererMaterialManager() {
			vNamedMap = new Dictionary<string, NamedSharedMat>();
			vMatMap = new Dictionary<string, Dictionary<int, Material>>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( !_IsBuilt ) {
				NamedMaterials = new NamedSharedMat[3];
				NamedMaterials[0] = BuildStandardFill();
				NamedMaterials[1] = BuildStandardIcon();
				NamedMaterials[2] = BuildStandardLabel();
			}

			foreach ( NamedSharedMat namedMat in NamedMaterials ) {
				var hintMap = new Dictionary<int, Material>();
				hintMap.Add(0, namedMat.SharedMaterial);

				vMatMap.Add(namedMat.Key, hintMap);
				vNamedMap.Add(namedMat.Key, namedMat);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private NamedSharedMat BuildStandardFill() {
			return new NamedSharedMat {
				Key = "StandardFill",
				SharedMaterial = Resources.Load<Material>(
					"Materials/HoverRendererVertexColorMaterial"),
				BaseRenderQueue = 3200
			};
		}

		/*--------------------------------------------------------------------------------------------*/
		private NamedSharedMat BuildStandardIcon() {
			return new NamedSharedMat {
				Key = "StandardIcon",
				SharedMaterial = Resources.Load<Material>(
					"Materials/HoverRendererStandardIconsMaterial"),
				BaseRenderQueue = 3201
			};
		}

		/*--------------------------------------------------------------------------------------------*/
		private NamedSharedMat BuildStandardLabel() {
			return new NamedSharedMat {
				Key = "StandardLabel",
				SharedMaterial = Resources.Load<Font>("Fonts/Tahoma").material,
				BaseRenderQueue = 3201
			};
		}

			
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Material GetMaterial(string pMaterialKey, int pRenderQueueHint=0) {
			if ( !vNamedMap.ContainsKey(pMaterialKey) ) {
				throw new Exception("Unknown material key: '"+pMaterialKey+"'.");
			}

			NamedSharedMat namedMat = vNamedMap[pMaterialKey];
			Dictionary<int, Material> hintMap = vMatMap[pMaterialKey];

			if ( !hintMap.ContainsKey(pRenderQueueHint) ) {
				Material mat = Instantiate(namedMat.SharedMaterial);
				mat.renderQueue = namedMat.BaseRenderQueue + pRenderQueueHint*RenderQueueHintMultiplier;
				hintMap.Add(pRenderQueueHint, mat);
			}

			return hintMap[pRenderQueueHint];
		}

	}

}
