using System.Collections.Generic;
using Hover.Utils;
using UnityEngine;

namespace Hover.Layouts.Rect {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverLayoutRectGroup : MonoBehaviour, ISettingsController, ITreeUpdateable {

		public ISettingsControllerMap Controllers { get; private set; }
		
		protected readonly List<HoverLayoutRectGroupChild> vChildItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverLayoutRectGroup() {
			Controllers = new SettingsControllerMap();
			vChildItems = new List<HoverLayoutRectGroupChild>();
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
				IRectLayoutable elem = childTx.GetComponent<IRectLayoutable>();

				if ( elem == null ) {
					//Debug.LogWarning("Item '"+childTx.name+"' does not have a renderer "+
					//	"that implements '"+typeof(IRectangleLayoutElement).Name+"'.");
					continue;
				}

				var item = new HoverLayoutRectGroupChild(elem,
					childTx.GetComponent<HoverLayoutRectRelativeSizer>());

				vChildItems.Add(item);
			}
		}

	}

}
