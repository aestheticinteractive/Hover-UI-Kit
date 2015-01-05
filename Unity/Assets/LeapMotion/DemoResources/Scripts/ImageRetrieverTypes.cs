using UnityEngine;
using System.Collections;

namespace Leap
{
  public class ImageRetrieverTypes : MonoBehaviour
  {

    void Update()
    {
      LeapImageRetriever images = GetComponent<LeapImageRetriever>();
      if (Input.GetKeyDown("1"))
      {
        images.imageColor = Color.white;
        images.blackIsTransparent = false;
        images.undistortImage = false;
      }
      if (Input.GetKeyDown("2"))
      {
        images.imageColor = Color.white;
        images.blackIsTransparent = false;
        images.undistortImage = true;
      }
      if (Input.GetKeyDown("3"))
      {
        images.imageColor = Color.black;
        images.blackIsTransparent = true;
        images.undistortImage = true;
      }
    }
  }
}

