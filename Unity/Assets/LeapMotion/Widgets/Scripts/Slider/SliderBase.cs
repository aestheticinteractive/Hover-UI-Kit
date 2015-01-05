using UnityEngine;
using System.Collections;

namespace LMWidgets
{
  public abstract class SliderBase : ButtonBase
  {
    public GameObject upperLimit;
    public GameObject lowerLimit;

    public abstract void SliderPressed();
    public abstract void SliderReleased();

    // When button is pressed, set the pivot and target_pivot in preparation for moving the handle
    public override void ButtonPressed()
    {
      SliderPressed();
    }

    // When button is released, reset the target_pivot
    public override void ButtonReleased()
    {
      SliderReleased();
    }

    // Apply constraint to prevent the slider by going pass the lower and upper limits
    protected override void ApplyConstraints()
    {
      Vector3 local_position = transform.localPosition;
      local_position.x = Mathf.Clamp(local_position.x, lowerLimit.transform.localPosition.x, upperLimit.transform.localPosition.x);
      local_position.y = 0.0f;
      local_position.z = Mathf.Clamp(local_position.z, min_distance_, max_distance_);
      transform.localPosition = local_position;
      transform.rigidbody.velocity = Vector3.zero;
    }
  }
}

