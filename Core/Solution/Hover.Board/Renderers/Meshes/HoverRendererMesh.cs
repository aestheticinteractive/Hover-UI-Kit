using Hover.Common.Display;
using Hover.Common.Util;
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
		public virtual void UpdateAfterRenderer() {
			CreateMeshBuilderIfNeeded(false);
			UpdateMesh();
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

			if ( pNewMesh ) {
				meshFilt.sharedMesh = new Mesh();
			}
			
			vMeshBuild = new MeshBuilder(meshFilt.sharedMesh);
			vMeshBuild.Mesh.name = gameObject.name+"Mesh:"+GetInstanceID();
			vMeshBuild.Mesh.MarkDynamic();
			return true;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected abstract void UpdateMesh();
		
	}

}
