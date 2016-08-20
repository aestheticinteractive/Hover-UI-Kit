using System.Collections.Generic;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Items.Helpers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverChildItemsFinder : MonoBehaviour, ITreeUpdateable {

		public List<HoverItemData> ChildItems { get; private set; }

		public bool FindOnlyImmediateChildren = false;
		public bool ForceUpdate = false;

		private bool vPrevAffectOnlyImmediateChildren;

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			bool isFirst = false;

			if ( ChildItems == null ) {
				ChildItems = new List<HoverItemData>();
				isFirst = true;
			}

			if ( !isFirst && !ForceUpdate && 
					FindOnlyImmediateChildren == vPrevAffectOnlyImmediateChildren ) {
				return;
			}

			ChildItems.Clear();
			FillChildItemsList(gameObject.transform);

			ForceUpdate = false;
			vPrevAffectOnlyImmediateChildren = FindOnlyImmediateChildren;
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void FillChildItemsList(Transform pParentTx) {
			int childCount = pParentTx.childCount;

			for ( int i = 0 ; i < childCount ; i++ ) {
				Transform childTx = pParentTx.GetChild(i);
				HoverItemData item = childTx.GetComponent<HoverItemData>();

				if ( item != null ) {
					ChildItems.Add(item);
				}

				if ( !FindOnlyImmediateChildren ) {
					FillChildItemsList(childTx);
				}
			}
		}

	}

}
