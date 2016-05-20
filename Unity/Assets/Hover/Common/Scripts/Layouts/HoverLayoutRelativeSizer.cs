using Hover.Common.Utils;
using UnityEngine;

namespace Hover.Common.Layouts {

	/*================================================================================================*/
	public class HoverLayoutRelativeSizer : MonoBehaviour, ISettingsController {

		public const string RelativeSizeXName = "_RelativeSizeX";
		public const string RelativeSizeYName = "_RelativeSizeY";

		public ISettingsControllerMap Controllers { get; private set; }

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		private float _RelativeSizeX = 1;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=100)]
		private float _RelativeSizeY = 1;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverLayoutRelativeSizer() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public float RelativeSizeX {
			get { return _RelativeSizeX; }
			set { _RelativeSizeX = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RelativeSizeY {
			get { return _RelativeSizeY; }
			set { _RelativeSizeY = value; }
		}

	}

}
