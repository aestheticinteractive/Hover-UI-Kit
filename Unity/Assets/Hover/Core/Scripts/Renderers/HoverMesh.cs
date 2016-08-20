using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	public abstract class HoverMesh : MonoBehaviour, ITreeUpdateable, ISettingsController {

		public enum DisplayModeType {
			Standard,
			SliderFill
		}

		public const string DisplayModeName = "DisplayMode";

		public ISettingsControllerMap Controllers { get; private set; }
		public MeshBuilder Builder { get { return vMeshBuild; } }
		public bool DidRebuildMesh { get; private set; }
		public abstract bool IsMeshVisible { get; }

		[DisableWhenControlled(DisplaySpecials=true)]
		public DisplayModeType DisplayMode = DisplayModeType.Standard;

		protected MeshBuilder vMeshBuild;
		protected bool vForceUpdates;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverMesh() {
			Controllers = new SettingsControllerMap();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			CreateMaterial();
			CreateMesh();
			CreateMeshBuilder();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
			vForceUpdates = UpdateNullScenarios();
			DidRebuildMesh = false;

			if ( ShouldUpdateMesh() ) {
				DidRebuildMesh = true;
				UpdateMesh();
			}

			Controllers.TryExpireControllers();
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void OnDestroy() {
			DestroyImmediate(gameObject.GetComponent<MeshFilter>().sharedMesh);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void CreateMaterial() {
			MeshRenderer meshRend = gameObject.GetComponent<MeshRenderer>();
			
			if ( meshRend.sharedMaterial == null ) {
				meshRend.sharedMaterial = Resources.Load<Material>(
					"Materials/HoverVertexColorMaterial");
				meshRend.sortingOrder = 0;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void CreateMesh() {
			Mesh mesh = new Mesh();
			mesh.name = gameObject.name+"Mesh:"+GetInstanceID();
			mesh.hideFlags = HideFlags.HideAndDontSave;
			mesh.MarkDynamic();
			
			gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void CreateMeshBuilder() {
			vMeshBuild = new MeshBuilder(gameObject.GetComponent<MeshFilter>().sharedMesh);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual bool UpdateNullScenarios() {
			MeshFilter meshFilt = gameObject.GetComponent<MeshFilter>();

			if ( meshFilt.sharedMesh == null ) { //can occur upon "undo" action in the editor
				if ( vMeshBuild == null ) { //just in case; not sure if this scenario can occur
					CreateMesh();
					CreateMeshBuilder();
				}
				else {
					meshFilt.sharedMesh = vMeshBuild.Mesh;
				}

				return true;
			}

			if ( vMeshBuild == null ) { //can occur when recompiled DLLs cause a scene "refresh"
				CreateMeshBuilder();
				return true;
			}

			return false;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected virtual bool ShouldUpdateMesh() {
			return vForceUpdates;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected abstract void UpdateMesh();
		
	}

}
