/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

// Interface for all tools.
// NOTE: This file is a work in progress, changes to come.
public class ToolModel : MonoBehaviour {

  public float filtering = 0.5f;

  protected Tool tool_;
  protected HandController controller_;
  protected bool mirror_z_axis_ = false;

  public Tool GetLeapTool() {
    return tool_;
  }

  public void SetLeapTool(Tool tool) {
    tool_ = tool;
  }

  public void MirrorZAxis(bool mirror = true) {
    mirror_z_axis_ = mirror;
  }

  public HandController GetController() {
    return controller_;
  }

  public void SetController(HandController controller) {
    controller_ = controller;
  }
  
  public Quaternion GetToolRotation() {
    Quaternion local_rotation = Quaternion.FromToRotation(Vector3.forward,
                                                          tool_.Direction.ToUnity(mirror_z_axis_));
    return GetController().transform.rotation * local_rotation;
  }

  public Vector3 GetToolTipVelocity() {
    Vector3 local_velocity = tool_.TipVelocity.ToUnityScaled(mirror_z_axis_);
    Vector3 total_scale = Vector3.Scale(GetController().handMovementScale,
                                        GetController().transform.localScale);
    Vector3 scaled_velocity = Vector3.Scale(total_scale, local_velocity);
    return GetController().transform.TransformDirection(scaled_velocity);
  }

  public Vector3 GetToolTipPosition() {
    Vector3 local_point = tool_.TipPosition.ToUnityScaled(mirror_z_axis_);
    Vector3 scaled_point = Vector3.Scale(GetController().handMovementScale, local_point);
    return GetController().transform.TransformPoint(scaled_point);
  }

  public void InitTool() {
    transform.position = GetToolTipPosition();
    transform.rotation = GetToolRotation();
  }

  public void UpdateTool() {
    Vector3 target_position = GetToolTipPosition();
    if (Time.deltaTime != 0) {
      rigidbody.velocity = (target_position - transform.position) *
                           (1 - filtering) / Time.deltaTime;
    }

    // Set angular velocity.
    Quaternion target_rotation = GetToolRotation();
    Quaternion delta_rotation = target_rotation *
                                Quaternion.Inverse(transform.rotation);
    float angle = 0.0f;
    Vector3 axis = Vector3.zero;
    delta_rotation.ToAngleAxis(out angle, out axis);

    if (angle >= 180) {
      angle = 360 - angle;
      axis = -axis;
    }
    if (angle != 0)
      rigidbody.angularVelocity = (1 - filtering) * angle * axis;
  }
}
