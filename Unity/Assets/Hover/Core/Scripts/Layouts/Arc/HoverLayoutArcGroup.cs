using System.Collections.Generic;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Layouts.Arc {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public abstract class HoverLayoutArcGroup : TreeUpdateableBehavior, ISettingsController {
		
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
		public void OnTransformChildrenChanged() {
			vShouldRefreshChildren = true;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
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

				if ( elem == null || !elem.isActiveAndEnabled ) {
					continue;
				}

				var item = new HoverLayoutArcGroupChild(elem,
					childTx.GetComponent<HoverLayoutArcRelativeSizer>());

				vChildItems.Add(item);
			}
		}

	}

}
