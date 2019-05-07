using UnityEngine;

namespace Hover.Core.Utils {

	/*================================================================================================*/
	public abstract class TreeUpdateableBehavior : MonoBehaviour, ITreeUpdateable {
	
		public string TypeName { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void OnEnable() {
			TreeUpdater.SendTreeUpdatableChanged(this, "OnEnabled");
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Start() {
			TypeName = GetType().Name;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void OnDisable() {
			TreeUpdater.SendTreeUpdatableChanged(this, "OnDisabled");
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void OnValidate() { //editor only
			TreeUpdater.SendTreeUpdatableChanged(this, "OnValidate");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract void TreeUpdate();

	}

}
