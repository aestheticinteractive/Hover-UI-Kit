using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers.Cursors {

	/*================================================================================================*/
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(LineRenderer))]
	public class HoverRaycastLine : TreeUpdateableBehavior {

		public const string RaycastWorldOriginName = "RaycastWorldOrigin";

		public ISettingsControllerMap Controllers { get; private set; }

		[SerializeField]
		[DisableWhenControlled(DisplaySpecials=true)]
		[FormerlySerializedAs("RaycastWorldOrigin")]
		private Vector3 _RaycastWorldOrigin;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverRaycastLine() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Vector3 RaycastWorldOrigin {
			get => _RaycastWorldOrigin;
			set => this.UpdateValueWithTreeMessage(ref _RaycastWorldOrigin, value, "RaycastWorldOrig");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			LineRenderer line = GetComponent<LineRenderer>();
			line.useWorldSpace = true;
			line.SetPosition(0, transform.position);
			line.SetPosition(1, RaycastWorldOrigin);

			Controllers.TryExpireControllers();
		}

	}

}
