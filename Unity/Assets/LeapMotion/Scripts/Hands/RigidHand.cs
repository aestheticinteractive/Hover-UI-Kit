/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

// The model for our rigid hand made out of various polyhedra.
public class RigidHand : SkeletalHand {

  public float filtering = 0.5f;

  void Start() {
    palm.rigidbody.maxAngularVelocity = Mathf.Infinity;
    Leap.Utils.IgnoreCollisions(gameObject, gameObject);
  }

  public override void InitHand() {
    base.InitHand();
  }

  public override void UpdateHand() {
    for (int f = 0; f < fingers.Length; ++f) {
      if (fingers[f] != null)
        fingers[f].UpdateFinger();
    }

    if (palm != null) {
      // Set palm velocity.
      Vector3 target_position = GetPalmCenter();
      palm.rigidbody.velocity = (target_position - palm.transform.position) *
                                (1 - filtering) / Time.deltaTime;

      // Set palm angular velocity.
      Quaternion target_rotation = GetPalmRotation();
      Quaternion delta_rotation = target_rotation *
                                  Quaternion.Inverse(palm.transform.rotation);
      float angle = 0.0f;
      Vector3 axis = Vector3.zero;
      delta_rotation.ToAngleAxis(out angle, out axis);

      if (angle >= 180) {
        angle = 360 - angle;
        axis = -axis;
      }
      if (angle != 0) {
        float delta_radians = (1 - filtering) * angle * Mathf.Deg2Rad;
        palm.rigidbody.angularVelocity = delta_radians * axis / Time.deltaTime;
      }
    }

    if (forearm != null)
    {
      // Set arm velocity.
      Vector3 target_position = GetArmCenter();
      forearm.rigidbody.velocity = (target_position - forearm.transform.position) *
                                   (1 - filtering) / Time.deltaTime;

      // Set arm velocity.
      Quaternion target_rotation = GetArmRotation();
      Quaternion delta_rotation = target_rotation *
                                  Quaternion.Inverse(forearm.transform.rotation);
      float angle = 0.0f;
      Vector3 axis = Vector3.zero;
      delta_rotation.ToAngleAxis(out angle, out axis);

      if (angle >= 180)
      {
        angle = 360 - angle;
        axis = -axis;
      }
      if (angle != 0)
      {
        float delta_radians = (1 - filtering) * angle * Mathf.Deg2Rad;
        forearm.rigidbody.angularVelocity = delta_radians * axis / Time.deltaTime;
      }
    }
  }
}
