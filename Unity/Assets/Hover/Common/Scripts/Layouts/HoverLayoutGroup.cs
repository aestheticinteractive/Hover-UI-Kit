using System.Collections.Generic;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Items {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverLayoutGroup : MonoBehaviour, ISettingsController {

		public ISettingsControllerMap Controllers { get; private set; }
		
		protected readonly List<HoverItem> vChildItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverLayoutGroup() {
			Controllers = new SettingsControllerMap();
			vChildItems = new List<HoverItem>();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
			FillChildItemsList();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void FillChildItemsList() {
			vChildItems.Clear();

			foreach ( Transform childTx in gameObject.transform ) {
				HoverItem item = childTx.GetComponent<HoverItem>();

				if ( item == null ) {
					continue;
				}

				vChildItems.Add(item);
			}
		}

	}

}
