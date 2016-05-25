using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public abstract class HoverFill : MonoBehaviour, ITreeUpdateable {

		public const string SortingLayerName = "SortingLayer";

		public ISettingsControllerMap Controllers { get; private set; }
		
		[DisableWhenControlled(DisplayMessage=true)]
		public string SortingLayer = "Default";

		[HideInInspector]
		[SerializeField]
		private bool _IsBuilt;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverFill() {
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
