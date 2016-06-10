using System.Collections.Generic;
using Hover.Utils;
using UnityEngine;

namespace Hover.Layouts.Arc {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public abstract class HoverLayoutArcGroup : MonoBehaviour, ISettingsController, ITreeUpdateable {
		
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
		public int GetChildOrder(IArcLayoutable pFirst, IArcLayoutable pSecond) {
			int firstIndex = -1;
			int secondIndex = -1;

			for ( int i = 0 ; i < vChildItems.Count ; i++ ) {
				HoverLayoutArcGroupChild group = vChildItems[i];

				if ( group.Elem == pFirst ) {
					firstIndex = i;
				}

				if ( group.Elem == pSecond ) {
					secondIndex = i;
				}

				if ( firstIndex != -1 && secondIndex != -1 ) {
					return firstIndex.CompareTo(secondIndex);
				}
			}

			return 0;
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
