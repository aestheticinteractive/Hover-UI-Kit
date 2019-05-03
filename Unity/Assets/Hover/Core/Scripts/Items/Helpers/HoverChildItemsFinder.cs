using System.Collections.Generic;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Items.Helpers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverChildItemsFinder : TreeUpdateableBehavior {

		public List<HoverItemData> ChildItems { get; private set; }

		[SerializeField]
		[FormerlySerializedAs("FindOnlyImmediateChildren")]
		private bool _FindOnlyImmediateChildren = false;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool FindOnlyImmediateChildren {
			get => _FindOnlyImmediateChildren;
			set => this.UpdateValueWithTreeMessage(ref _FindOnlyImmediateChildren, value, "FindImmed");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			if ( ChildItems == null ) {
				ChildItems = new List<HoverItemData>();
			}

			ChildItems.Clear();

			TreeUpdater treeUp = GetComponent<TreeUpdater>();

			AddChildren(treeUp, FindOnlyImmediateChildren);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void AddChildren(TreeUpdater pTreeUp, bool pStopAtThisLevel) {
			List<TreeUpdater> treeUpChildren = pTreeUp.TreeChildrenThisFrame;

			for ( int i = 0 ; i < treeUpChildren.Count ; i++ ) {
				TreeUpdater treeUpChild = treeUpChildren[i];

				if ( treeUpChild == null ) {
					continue;
				}

				HoverItemData item = treeUpChild.GetComponent<HoverItemData>();

				if ( item != null ) {
					ChildItems.Add(item);
					continue; //should never be an item nested within another
				}

				if ( pStopAtThisLevel ) {
					continue;
				}

				AddChildren(treeUpChild, false);
			}
		}
	}

}
