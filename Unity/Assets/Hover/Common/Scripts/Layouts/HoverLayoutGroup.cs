using System.Collections.Generic;
using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Layouts {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	public class HoverLayoutGroup : MonoBehaviour, ISettingsController, ITreeUpdateable {

		protected struct ChildItem {
			public float RelSizeX {
				get { return (RelSizer == null ? 1 : RelSizer.RelativeSizeX); }
			}

			public float RelSizeY {
				get { return (RelSizer == null ? 1 : RelSizer.RelativeSizeY); }
			}

			public IRectangleLayoutElement Elem;
			public HoverLayoutRelativeSizer RelSizer;
		}

		public ISettingsControllerMap Controllers { get; private set; }
		
		protected readonly List<ChildItem> vChildItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverLayoutGroup() {
			Controllers = new SettingsControllerMap();
			vChildItems = new List<ChildItem>();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual void TreeUpdate() {
			FillChildItemsList();
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void FillChildItemsList() {
			vChildItems.Clear();

			foreach ( Transform childTx in gameObject.transform ) {
				IRectangleLayoutElement elem = childTx.GetComponent<IRectangleLayoutElement>();

				if ( elem == null ) {
					//Debug.LogWarning("Item '"+childTx.name+"' does not have a renderer "+
					//	"that implements '"+typeof(IRectangleLayoutElement).Name+"'.");
					continue;
				}

				var item = new ChildItem {
					Elem = elem,
					RelSizer = childTx.GetComponent<HoverLayoutRelativeSizer>()
				};

				vChildItems.Add(item);
			}
		}

	}

}
