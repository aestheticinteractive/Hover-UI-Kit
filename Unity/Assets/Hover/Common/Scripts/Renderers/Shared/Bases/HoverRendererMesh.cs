using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Shared.Bases {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	public abstract class HoverRendererMesh : MonoBehaviour, ITreeUpdateable {

		public const string SortingLayerName = "SortingLayer";

		public ISettingsControllerMap Controllers { get; private set; }
		
		[DisableWhenControlled(DisplayMessage=true)]
		public string SortingLayer = "Default";

		protected MeshBuilder vMeshBuild;
		private string vPrevSortLayer;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererMesh() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual int MaterialRenderQueue {
			get {
				return GetComponent<MeshRenderer>().sharedMaterial.renderQueue;
			}
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			CreateMesh();
			CreateMeshBuilder();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
			if ( vMeshBuild == null ) { //this can occur when recompiled DLLs cause a scene "refresh"
				CreateMeshBuilder();
			}

			UpdateMesh();
			UpdateSortingLayer();
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void OnDestroy() {
			DestroyImmediate(gameObject.GetComponent<MeshFilter>().sharedMesh);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void CreateMesh() {
			MeshRenderer meshRend = gameObject.GetComponent<MeshRenderer>();
			MeshFilter meshFilt = gameObject.GetComponent<MeshFilter>();
			
			if ( meshRend.sharedMaterial == null ) {
				meshRend.sharedMaterial = Resources.Load<Material>(
					"Materials/HoverRendererVertexColorMaterial");
				meshRend.sortingOrder = 0;
			}

			Mesh mesh = new Mesh();
			mesh.name = gameObject.name+"Mesh:"+GetInstanceID();
			mesh.hideFlags = HideFlags.HideAndDontSave;
			mesh.MarkDynamic();
			
			meshFilt.sharedMesh = mesh;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void CreateMeshBuilder() {
			vMeshBuild = new MeshBuilder(gameObject.GetComponent<MeshFilter>().sharedMesh);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected abstract void UpdateMesh();
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateSortingLayer() {
			if ( SortingLayer == vPrevSortLayer ) {
				return;
			}

			gameObject.GetComponent<MeshRenderer>().sortingLayerName = SortingLayer;
			vPrevSortLayer = SortingLayer;
		}
		
	}

}
