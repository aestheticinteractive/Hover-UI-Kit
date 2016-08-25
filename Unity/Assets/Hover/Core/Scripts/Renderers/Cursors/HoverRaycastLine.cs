using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Cursors {

	/*================================================================================================*/
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(LineRenderer))]
	public class HoverRaycastLine : MonoBehaviour, ITreeUpdateable {

		public const string RaycastWorldOriginName = "RaycastWorldOrigin";

		public ISettingsControllerMap Controllers { get; private set; }

		[DisableWhenControlled(DisplaySpecials=true)]
		public Vector3 RaycastWorldOrigin;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverRaycastLine() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Start() {
			//do nothing...
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void TreeUpdate() {
			LineRenderer line = GetComponent<LineRenderer>();
			line.useWorldSpace = true;
			line.SetPosition(0, transform.position);
			line.SetPosition(1, RaycastWorldOrigin);

			Controllers.TryExpireControllers();
		}

	}

}
