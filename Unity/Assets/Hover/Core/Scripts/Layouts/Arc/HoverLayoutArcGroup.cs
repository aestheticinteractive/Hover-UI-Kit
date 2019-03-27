using System.Collections.Generic;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Layouts.Arc {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public abstract class HoverLayoutArcGroup : MonoBehaviour, ISettingsController, ITreeUpdateable {
		
		public ISettingsControllerMap Controllers { get; private set; }

		protected readonly List<HoverLayoutArcGroupChild> vChildItems;
		private bool vShouldRefreshChildren;
		protected bool vDidRefreshChildren;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverLayoutArcGroup() {
			Controllers = new SettingsControllerMap();
			vChildItems = new List<HoverLayoutArcGroupChild>();
			vShouldRefreshChildren = true;
		}

		/*--------------------------------------------------------------------------------------------*/
		public int ChildCount {
			get {
				return vChildItems.Count;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}

		/*--------------------------------------------------------------------------------------------*/
		public void OnTransformChildrenChanged() {
			vShouldRefreshChildren = true;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
			vDidRefreshChildren = false;
			FillChildItemsList();
			Controllers.TryExpireControllers();
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void FillChildItemsList() {
			if ( !vShouldRefreshChildren ) {
				return;
			}

			vShouldRefreshChildren = false;
			vDidRefreshChildren = true;
			vChildItems.Clear();

			int childCount = transform.childCount;

			for ( int i = 0 ; i < childCount ; i++ ) {
				Transform childTx = transform.GetChild(i);
				ILayoutableArc elem = childTx.GetComponent<ILayoutableArc>();

				if ( elem == null ) {
					//Debug.LogWarning("Item '"+childTx.name+"' does not have a renderer "+
					//	"that implements '"+typeof(IArcLayoutable).Name+"'.");
					continue;
				}

				if ( !elem.isActiveAndEnabled ) {
					continue;
				}

				var item = new HoverLayoutArcGroupChild(elem,
					childTx.GetComponent<HoverLayoutArcRelativeSizer>());

				vChildItems.Add(item);
			}
		}

	}

}
