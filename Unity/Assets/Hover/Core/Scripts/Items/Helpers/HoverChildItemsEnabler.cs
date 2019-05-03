using System.Collections.Generic;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Items.Helpers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverChildItemsFinder))]
	public class HoverChildItemsEnabler : TreeUpdateableBehavior {

		[SerializeField]
		[FormerlySerializedAs("AreItemsEnabled")]
		private bool _AreItemsEnabled = true;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public bool AreItemsEnabled {
			get => _AreItemsEnabled;
			set => this.UpdateValueWithTreeMessage(ref _AreItemsEnabled, value, "AreItemsEnabled");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			List<HoverItemData> items = GetComponent<HoverChildItemsFinder>().ChildItems;

			for ( int i = 0 ; i < items.Count ; i++ ) {
				HoverItemData item = items[i];
				//TODO: item.Controllers.Set(HoverItemData.IsEnabledName, this);
				item.IsEnabled = AreItemsEnabled;
			}
		}

	}

}
