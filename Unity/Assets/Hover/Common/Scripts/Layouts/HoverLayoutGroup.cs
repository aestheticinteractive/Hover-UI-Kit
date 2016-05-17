using System.Collections.Generic;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Items {

	/*================================================================================================*/
	public class HoverLayoutGroup : MonoBehaviour, ISettingsController {

		public ISettingsControllerMap Controllers { get; private set; }
		
		protected readonly List<HoverItemData> vChildItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverLayoutGroup() {
			Controllers = new SettingsControllerMap();
			vChildItems = new List<HoverItemData>();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Update() {
			FillChildItemsList();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void FillChildItemsList() {
			vChildItems.Clear();

			foreach ( Transform childTx in gameObject.transform ) {
				HoverItemData itemData = childTx.GetComponent<HoverItemData>();

				if ( itemData == null ) {
					continue;
				}

				vChildItems.Add(itemData);
			}
		}

	}

}
