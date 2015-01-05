using UnityEngine;
using System.Collections;

public class PassthroughEnabler : MonoBehaviour {

  public GameObject passthroughLeft;
  public GameObject passthroughRight;

  private bool show_passthrough_ = true;

  private Vector3 prev_scale_ = Vector3.zero;
  private Vector3 prev_position_ = Vector3.zero;
	
	// Update is called once per frame
	void Update () {
    if (Input.GetKeyDown(KeyCode.P))
    {
      show_passthrough_ = !show_passthrough_;
      if (show_passthrough_)
      {
        passthroughLeft.SetActive(true);
        passthroughRight.SetActive(true);
        transform.localScale = prev_scale_;
        transform.localPosition = prev_position_;
      }
      else
      {
        prev_scale_ = transform.localScale;
        prev_position_ = transform.localPosition;
        passthroughLeft.SetActive(false);
        passthroughRight.SetActive(false); 
        transform.localScale = Vector3.one;
        transform.localPosition = new Vector3(0.0f, 0.0f, 0.08f);
      }
    }
	}
}
