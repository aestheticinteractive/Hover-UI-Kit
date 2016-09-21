using System.Collections.Generic;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Layouts.Rect {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public abstract class HoverLayoutRectGroup : MonoBehaviour, ISettingsController, ITreeUpdateable {

		public ISettingsControllerMap Controllers { get; private set; }
		
		protected readonly List<HoverLayoutRectGroupChild> vChildItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverLayoutRectGroup() {
			Controllers = new SettingsControllerMap();
			vChildItems = new List<HoverLayoutRectGroupChild>();
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
		public virtual void TreeUpdate() {
			FillChildItemsList();
			Controllers.TryExpireControllers();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void FillChildItemsList() {
			vChildItems.Clear();

			int childCount = transform.childCount;

			for ( int i = 0 ; i < childCount ; i++ ) {
				Transform childTx = transform.GetChild(i);
				ILayoutableRect elem = childTx.GetComponent<ILayoutableRect>();

				if ( elem == null ) {
					//Debug.LogWarning("Item '"+childTx.name+"' does not have a renderer "+
					//	"that implements '"+typeof(IRectangleLayoutElement).Name+"'.");
					continue;
				}

				if ( !elem.isActiveAndEnabled ) {
					continue;
				}

				var item = new HoverLayoutRectGroupChild(elem,
					childTx.GetComponent<HoverLayoutRectRelativeSizer>());

				vChildItems.Add(item);
			}
		}

	}

}
