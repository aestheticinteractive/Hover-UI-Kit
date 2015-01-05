/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

namespace Leap {

  // Extension to the unity Vector class.
  // Provides converting points and directions into Unity scene space.
  public static class UnityVectorExtension {

    // Leap coordinates are in mm and Unity is in meters. So scale by 1000.
    public const float INPUT_SCALE = 0.001f;
    public static readonly Vector3 Z_FLIP = new Vector3(1, 1, -1);

    // For directions.
    public static Vector3 ToUnity(this Vector leap_vector, bool mirror = false) {
      if (mirror)
        return ToVector3(leap_vector);

      return FlipZ(ToVector3(leap_vector));
    }

    // For positions and scaled vectors.
    public static Vector3 ToUnityScaled(this Vector leap_vector, bool mirror = false) {
      if (mirror)
        return INPUT_SCALE * ToVector3(leap_vector);
      
      return INPUT_SCALE * FlipZ(ToVector3(leap_vector));
    }

    private static Vector3 FlipZ(Vector3 vector) {
      return Vector3.Scale(vector, Z_FLIP);
    }

    private static Vector3 ToVector3(Vector vector) {
      return new Vector3(vector.x, vector.y, vector.z);
    }
  }

  // Extension to the unity Matrix class.
  // Provides conversions to Quaternion and translations.
  public static class UnityMatrixExtension {
    public static readonly Vector LEAP_UP = new Vector(0, 1, 0);
    public static readonly Vector LEAP_FORWARD = new Vector(0, 0, -1);
    public static readonly Vector LEAP_ORIGIN = new Vector(0, 0, 0);

    public static Quaternion Rotation(this Matrix matrix, bool mirror = false) {
      Vector3 up = matrix.TransformDirection(LEAP_UP).ToUnity(mirror);
      Vector3 forward = matrix.TransformDirection(LEAP_FORWARD).ToUnity(mirror);
      return Quaternion.LookRotation(forward, up);
    }

    public static Vector3 Translation(this Matrix matrix, bool mirror = false) {
      return matrix.TransformPoint(LEAP_ORIGIN).ToUnityScaled(mirror);
    }
  }
}
