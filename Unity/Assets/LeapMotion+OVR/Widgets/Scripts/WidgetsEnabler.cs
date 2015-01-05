using UnityEngine;
using System.Collections;

public class WidgetsEnabler : MonoBehaviour {
  private bool isActive = true;
	// Update is called once per frame
	void Update () {
    if (Input.GetKeyDown(KeyCode.W))
    {
      isActive = !isActive;
      foreach (Transform child_transform in transform)
      {
        child_transform.gameObject.SetActive(isActive);
      }
    }
	}
}
