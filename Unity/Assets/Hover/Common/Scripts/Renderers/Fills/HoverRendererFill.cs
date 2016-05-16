using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Fills {

	/*================================================================================================*/
	[ExecuteInEditMode]
	public abstract class HoverRendererFill : MonoBehaviour {
	
		public ISettingsControllerMap Controllers { get; private set; }
		
		[HideInInspector]
		[SerializeField]
		private bool vIsBuilt;
		
		
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
			if ( !vIsBuilt ) {
				BuildElements();
				vIsBuilt = true;
			}
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected abstract void BuildElements();
				
	}

}
