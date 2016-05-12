using Hover.Common.Display;
using UnityEngine;

namespace Hover.Board.Renderers.Meshes {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	public abstract class HoverRendererMesh : MonoBehaviour {
	
		public bool ControlledByRenderer { get; set; }

		protected MeshBuilder vMeshBuild;

		
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
		public virtual void Update() {
			if ( !ControlledByRenderer ) {
				UpdateAfterRenderer();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual bool UpdateAfterRenderer() {
			if ( !gameObject.activeInHierarchy ) {
				return false; //avoid create/update mesh while inactive, wait til Awake() to create
			}

			if ( vMeshBuild == null ) { //this can occur when recompiled DLLs cause a scene "refresh"
				CreateMeshBuilder();
			}

			UpdateMesh();
			return true;
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
		
	}

}
