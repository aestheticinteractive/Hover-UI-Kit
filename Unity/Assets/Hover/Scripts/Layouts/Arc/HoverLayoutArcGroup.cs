using System.Collections.Generic;
using Hover.Utils;
using UnityEngine;

namespace Hover.Layouts.Arc {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverLayoutArcGroup : MonoBehaviour, ISettingsController, ITreeUpdateable {
		
		public ISettingsControllerMap Controllers { get; private set; }
		
		protected readonly List<HoverLayoutArcGroupChild> vChildItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverLayoutArcGroup() {
			Controllers = new SettingsControllerMap();
			vChildItems = new List<HoverLayoutArcGroupChild>();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
			FillChildItemsList();
			Controllers.TryExpireControllers();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void FillChildItemsList() {
			vChildItems.Clear();

			foreach ( Transform childTx in gameObject.transform ) {
				IArcLayoutable elem = childTx.GetComponent<IArcLayoutable>();

				if ( elem == null ) {
					//Debug.LogWarning("Item '"+childTx.name+"' does not have a renderer "+
					//	"that implements '"+typeof(IArcLayoutable).Name+"'.");
					continue;
				}

				var item = new HoverLayoutArcGroupChild(elem,
					childTx.GetComponent<HoverLayoutArcRelativeSizer>());

				vChildItems.Add(item);
			}
		}

	}

}
