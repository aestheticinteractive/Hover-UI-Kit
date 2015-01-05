using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LMWidgets
{
  public abstract class ScrollTextBase : ButtonBase
  {
    public GameObject content;

    private Vector3 local_pivot_ = Vector3.zero;
    private Vector3 target_pivot_ = Vector3.zero;
    private Vector3 content_pivot_ = Vector3.zero;
    private ExponentialSmoothingXYZ content_velocity_ = new ExponentialSmoothingXYZ(0.5f);

    private Vector3 prev_content_pos_ = Vector3.zero;

    private GameObject target_ = null;

    public abstract void ScrollActive();
    public abstract void ScrollInactive();

    private bool IsHand(Collision other)
    {
      return other.transform.parent && other.transform.parent.parent && other.transform.parent.parent.GetComponent<HandModel>();
    }

    void OnCollisionEnter(Collision other)
    {
      if (target_ == null && IsHand(other))
      {
        target_ = other.gameObject;
      }
    }

    private void UpdatePosition(Vector3 target_position)
    {
      Vector3 displacement = target_position - target_pivot_;
      Vector3 local_displacement = transform.InverseTransformDirection(displacement);
      Vector3 local_position = transform.localPosition;
      local_position.x = 0.0f;
      local_position.y = local_pivot_.y + local_displacement.y;
      local_position.z = Mathf.Max(local_position.z, 0.0f);
      transform.localPosition = local_position;

      prev_content_pos_ = content.transform.localPosition;
      Vector3 content_displacement = displacement;
      Vector3 content_position = content.transform.localPosition;
      content_position.y = content_pivot_.y + content_displacement.y;
      content.transform.localPosition = content_position;
      Vector3 curr_velocity = (content_position - prev_content_pos_) / Time.deltaTime;
      content_velocity_.Calculate(curr_velocity.x, curr_velocity.y, curr_velocity.z);
    }

    public override void ButtonPressed()
    {
      if (target_ != null)
      {
        local_pivot_ = transform.localPosition;
        target_pivot_ = target_.transform.position;
        content_pivot_ = content.transform.localPosition;
      }
      ScrollActive();
    }

    public override void ButtonReleased()
    {
      target_ = null;
      transform.localPosition = Vector3.zero;
      transform.rigidbody.velocity = Vector3.zero;
      content.rigidbody2D.velocity = new Vector2(content_velocity_.GetX(), content_velocity_.GetY());
      ScrollInactive();
    }

    public override void FixedUpdate()
    {
      base.FixedUpdate();
      if (is_pressed_)
      {
        if (target_ != null)
        {
          UpdatePosition(target_.transform.position);
        }
      }

      // Velocity will be greater than 0 if it starts bouncing at the edges
      if (Mathf.Abs(content.transform.parent.GetComponent<ScrollRect>().velocity.y) > 0.001f)
      {
        content.rigidbody2D.velocity = Vector2.zero;
      }
    }
  }
}
