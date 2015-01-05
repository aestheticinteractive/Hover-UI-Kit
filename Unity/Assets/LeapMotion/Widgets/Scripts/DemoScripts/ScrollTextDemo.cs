using UnityEngine;
using System.Collections;
using LMWidgets;

public class ScrollTextDemo : ScrollTextBase 
{
  public float contentLimit;

  public override void ScrollActive()
  {
  }

  public override void ScrollInactive()
  {
  }

  private void UpdateContentPosition()
  {
    Vector3 content_position = content.transform.localPosition;
    content_position.z = Mathf.Min(transform.localPosition.z, contentLimit);
    content.transform.localPosition = content_position;
  }

  public override void FixedUpdate()
  {
    base.FixedUpdate();
    UpdateContentPosition();
  }
}
