using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers.Shared.Bases {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public abstract class HoverRendererFill : MonoBehaviour, ITreeUpdateable {

		public const string SortingLayerName = "SortingLayer";

		public ISettingsControllerMap Controllers { get; private set; }
		
		[DisableWhenControlled(DisplayMessage=true)]
		public string SortingLayer = "Default";

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
