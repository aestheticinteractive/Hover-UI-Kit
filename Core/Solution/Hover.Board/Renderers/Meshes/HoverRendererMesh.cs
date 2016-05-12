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
			CreateMeshBuilderIfNeeded(true);
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

			CreateMeshBuilderIfNeeded(false);
			UpdateMesh();
			return true;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void OnDestroy() {
			DestroyImmediate(gameObject.GetComponent<MeshFilter>().sharedMesh);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual bool CreateMeshBuilderIfNeeded(bool pNewMesh) {
			if ( vMeshBuild != null ) {
				return false;
			}
			
			MeshRenderer meshRend = gameObject.GetComponent<MeshRenderer>();
			MeshFilter meshFilt = gameObject.GetComponent<MeshFilter>();
			
			if ( meshRend.sharedMaterial == null ) {
				meshRend.sharedMaterial = Resources.Load<Material>(
					"Materials/HoverRendererVertexColorMaterial");
			}

			if ( pNewMesh || meshFilt.sharedMesh == null ) {
				meshFilt.sharedMesh = new Mesh();
			}

			Mesh mesh = meshFilt.sharedMesh;
			mesh.name = gameObject.name+"Mesh:"+GetInstanceID();
			mesh.hideFlags = HideFlags.HideAndDontSave;
			mesh.MarkDynamic();
			
			vMeshBuild = new MeshBuilder(mesh);
			return true;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected abstract void UpdateMesh();
		
	}

}
