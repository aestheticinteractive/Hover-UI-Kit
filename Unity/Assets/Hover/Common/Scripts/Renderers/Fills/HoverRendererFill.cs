using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Fills {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public abstract class HoverRendererFill : MonoBehaviour, ITreeUpdateable {
	
		public ISettingsControllerMap Controllers { get; private set; }
		
		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverRendererFill() {
			Controllers = new SettingsControllerMap();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract int MaterialRenderQueue { get; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Awake() {
			if ( !_IsBuilt ) {
				BuildElements();
				_IsBuilt = true;
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public abstract void TreeUpdate();
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract void BuildElements();
				
	}

}
