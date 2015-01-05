using UnityEngine;
using System.Collections;

namespace LMWidgets
{
  public abstract class ButtonToggleBase : ButtonBase
  {
    public float onDistance = 0.0f;
    public float offDistance = 0.0f;

    protected bool toggle_state_;

    public abstract void ButtonTurnsOn();
    public abstract void ButtonTurnsOff();

    public override void ButtonReleased()
    {
    }

    public override void ButtonPressed()
    {
      if (toggle_state_ == false)
      {
        ButtonTurnsOn();
        SetMinDistance(onDistance);
        toggle_state_ = !toggle_state_;
      } 
      else
      {
        ButtonTurnsOff();
        SetMinDistance(offDistance);
        toggle_state_ = !toggle_state_;
      }
    }

    public override void Awake()
    {
      base.Awake();
      onDistance = Mathf.Min(onDistance, triggerDistance - cushionThickness - 0.001f);
      offDistance = Mathf.Min(offDistance, triggerDistance - cushionThickness - 0.001f);
    }
  }
}
