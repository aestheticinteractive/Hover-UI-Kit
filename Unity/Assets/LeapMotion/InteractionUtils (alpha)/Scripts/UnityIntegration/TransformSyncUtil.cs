using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

using UnityEngine;

using Leap.Interact;

namespace Leap.Interact
{

  /// <summary>
  /// Utilities to automate gluing the Unity scene & Leap3dInteract functionality.
  /// </summary>
  public class TransformSyncUtil
  {

    /// <summary>
    /// Updates Leap information from your scene. This is usually done after your Unity physics is stepped, 
    /// and actions are applied. 
    /// </summary>
    static public void UpdateLeapFromUnity(Dictionary<UnityEngine.GameObject, Body> BodyMapper)
    {
      foreach (KeyValuePair<GameObject, Body> pair in BodyMapper)
      {
        GameObject gameObject = pair.Key;

        if (gameObject == null)
        {
          // Remove the body from mapping & 
          // TODO: remove from leap interact
          continue;
        }

        if (gameObject.name != UnityUtil.FingerBoneName && gameObject.name != UnityUtil.FingerTipBoneName && gameObject.name != UnityUtil.ThumbTipBoneName && gameObject.name != UnityUtil.HandPalmName)
        {
          LeapTransform nextFrameUnityTransform = CalcUnityTransformForNextFrame(gameObject);
          LeapTransform targetLeapTransform = UnityTransformToLeap(nextFrameUnityTransform, gameObject);

          Body body = pair.Value;
          body.Transform = targetLeapTransform;
        }
      }
    }

    /// <summary>
    /// Updates state of Unity bodies. This reflect how your hands push or hold objects.
    /// </summary>
    static public void UpdateUnityFromLeap(Dictionary<UnityEngine.GameObject, Body> BodyMapper)
    {
      foreach (KeyValuePair<GameObject, Body> pair in BodyMapper)
      {
        if (pair.Key == null) { continue; }
        if (pair.Key.name == UnityUtil.FingerBoneName || pair.Key.name == UnityUtil.FingerTipBoneName || pair.Key.name == UnityUtil.ThumbTipBoneName || pair.Key.name == UnityUtil.HandPalmName) {
          RepositionHandBone(pair.Key, pair.Value);
        } else {
          if (pair.Value.IsHeld) {
            RepositionHeldBody(pair.Key, pair.Value);
          } else {
            RepositionFreeBody(pair.Key, pair.Value);
          }
        }
      }
    }

    static protected LeapTransform LeapTransformToUnity(LeapTransform leapTransform, GameObject gameObject)
    {
      LeapTransform unityTransform = new LeapTransform();
      float scale = gameObject.transform.lossyScale.x;
      unityTransform.Position = leapTransform.Position - (Quaternion)leapTransform.Rotation * GetCenterFromCollider(gameObject) * scale;
      unityTransform.Rotation = leapTransform.Rotation;
      return unityTransform;
    }

    static  protected LeapTransform UnityTransformToLeap(LeapTransform unityTransform, GameObject gameObject)
    {
      LeapTransform leapTransform = new LeapTransform();
      float scale = gameObject.transform.lossyScale.x;
      leapTransform.Position = unityTransform.Position + (Quaternion)unityTransform.Rotation * GetCenterFromCollider(gameObject) * scale;
      leapTransform.Rotation = unityTransform.Rotation;
      return leapTransform;
    }

    static protected void ApplyTargetUnityTransformAsVelocities(LeapTransform targetUnityTransform, GameObject gameObject)
    {
      Vector3 angularVelocity = CalcAngularVelocityToTarget(gameObject, targetUnityTransform.Rotation);
      gameObject.rigidbody.velocity = CalcLinearVelocityToTarget(gameObject, targetUnityTransform.Position, targetUnityTransform.Rotation, angularVelocity);
      gameObject.rigidbody.angularVelocity = angularVelocity;
    }

    static protected LeapTransform CalcUnityTransformForNextFrame(GameObject gameObject)
    {
      //return gameObject.transform; // skip velocities -- return this frame

      Transform unityTransform = gameObject.transform;
      LeapTransform targetTransform = new LeapTransform();

      // apply angular velcocit - rotation
      Quaternion extraRotation = Quaternion.identity;
      Vector3 angularDisplacement = gameObject.rigidbody.angularVelocity * Time.deltaTime;
      float angularMagnitude = angularDisplacement.magnitude;
      if (angularMagnitude > 0.0001f)
      {
        extraRotation = Quaternion.AngleAxis(angularMagnitude, angularDisplacement / angularMagnitude);
      }
      targetTransform.Rotation = extraRotation * unityTransform.rotation;

      // apply angular & linear velocities to body
      Vector3 oldCom = unityTransform.position + unityTransform.rotation * unityTransform.rigidbody.centerOfMass;
      Vector3 newCom = oldCom + gameObject.rigidbody.velocity * Time.deltaTime;
      targetTransform.Position = newCom - (Quaternion)targetTransform.Rotation * unityTransform.rigidbody.centerOfMass;

      return targetTransform;
    }


    static protected Vector3 CalcLinearVelocityToTarget(GameObject gameObject, Vector3 targetPosition, Quaternion targetRotation, Vector3 angularVelocity)
    {
      // need to convert from referencePoint target
      // to center-of-mass points
      Vector3 currentCom = gameObject.transform.position + gameObject.transform.rotation * gameObject.rigidbody.centerOfMass;
      Vector3 targetCom = targetPosition + targetRotation * gameObject.rigidbody.centerOfMass;


      //Vector3 arm = - gameObject.rigidbody.centerOfMass;
      //Vector3 armInWorld = gameObject.transform.rotation * arm;
      //Vector3 relativePointVelocityInWorld = Vector3.Cross(gameObject.rigidbody.angularVelocity, armInWorld);
      return (targetCom - currentCom) / Time.deltaTime;// + relativePointVelocityInWorld;
    }

    static protected Vector3 CalcAngularVelocityToTarget(GameObject gameObject, Quaternion targetRotation)
    {
      Quaternion deltaToTargetRotation = targetRotation * Quaternion.Inverse(gameObject.transform.rotation);
      Vector3 rotationAxis = new Vector3(deltaToTargetRotation.x, deltaToTargetRotation.y, deltaToTargetRotation.z);
      float angle = 2.0f * Mathf.Acos(deltaToTargetRotation.w);
      float rotationAxisLenght = rotationAxis.magnitude;

      if (rotationAxisLenght > 0.001f && Time.deltaTime > 0.001f)
      {
        return rotationAxis / rotationAxisLenght * angle / Time.deltaTime;
      }
      else
      {
        return Vector3.zero;
      }
    }

    static protected void SetLayerForHierarchy(GameObject root, LayerMask layerMask)
    {
      root.layer = layerMask;
      foreach (Transform child in root.transform) { SetLayerForHierarchy(child.gameObject, layerMask); }
    }

    static protected void RepositionFreeBody(GameObject gameObject, Body leapBody)
    {
      LeapInteraction props = gameObject.GetComponent<LeapInteraction>();
      if (props.velocityToTransfer)
      {
        gameObject.rigidbody.velocity = props.tmpVelocity;
        gameObject.rigidbody.angularVelocity = props.tmpAngularVelocity;
        props.velocityToTransfer = false;
      }

      LeapInteraction leapInteraction = gameObject.GetComponent<LeapInteraction>();
      if (UnityUtil.FilterHandCollisionPerColliderExplicitly && leapInteraction.CollisionsWithHandFilteredOut)
      {
        HandViewer.Instance.EnableHandCollisionsWithGameObject(gameObject);
        leapInteraction.CollisionsWithHandFilteredOut = false;
      }

      if (UnityUtil.LayerForReleasedObjects != UnityUtil.LayerForHeldObjects) { SetLayerForHierarchy(gameObject, UnityUtil.LayerForReleasedObjects); }
    }

    static protected void RepositionHeldBody(GameObject gameObject, Body leapBody)
    {
      LeapTransform leapTransform = leapBody.Transform;
      LeapTransform targetUnityTransform = LeapTransformToUnity(leapTransform, gameObject);

      LeapInteraction leapInteraction = gameObject.GetComponent<LeapInteraction>();

      if (gameObject.rigidbody.isKinematic || !leapInteraction.UseVelocity) {

        ApplyTargetUnityTransformAsVelocities(targetUnityTransform, gameObject);

        gameObject.transform.position = targetUnityTransform.Position;
        gameObject.transform.rotation = targetUnityTransform.Rotation;
        if (true)
        {
          LeapInteraction props = gameObject.GetComponent<LeapInteraction>();
          props.tmpVelocity = gameObject.rigidbody.velocity;
          props.tmpAngularVelocity = gameObject.rigidbody.angularVelocity;
          props.velocityToTransfer = true;

          gameObject.rigidbody.velocity = Vector3.zero;
          gameObject.rigidbody.angularVelocity = Vector3.zero;
        }
      } else {
        ApplyTargetUnityTransformAsVelocities(targetUnityTransform, gameObject);
      }

      if (UnityUtil.FilterHandCollisionPerColliderExplicitly && !leapInteraction.CollisionsWithHandFilteredOut )
      {
        HandViewer.Instance.DisableHandCollisionsWithGameObject(gameObject);
        leapInteraction.CollisionsWithHandFilteredOut = true;
      }

      if (UnityUtil.LayerForHeldObjects != UnityUtil.LayerForReleasedObjects) { SetLayerForHierarchy(gameObject, UnityUtil.LayerForHeldObjects); }
    }

    

    static protected void RepositionHandBone(GameObject gameObject, Body leapBody)
    {
      //RepositionFreeBody(gameObject, leapBody);

      Transform unityTransform = gameObject.transform;
      LeapTransform leapTransform = leapBody.Transform;
      LeapTransform targetUnityTransform = LeapTransformToUnity(leapTransform, gameObject);

      if (gameObject.name == UnityUtil.FingerBoneName || gameObject.name == UnityUtil.FingerTipBoneName || gameObject.name == UnityUtil.ThumbTipBoneName)
      {
        // reorient the capsule.
        Quaternion flipAxes = Quaternion.Euler(90.0f, 0f, 0f);
        targetUnityTransform.Rotation = leapTransform.Rotation * flipAxes;

        // shift the capsule along it's local y-direction
        float halfHeight = unityTransform.localScale.y;
        float radius = unityTransform.localScale.x / 2f;
        Vector3 displacement = (Quaternion)targetUnityTransform.Rotation * new Vector3(0, -halfHeight+radius, 0);
        // if mirroring, reverse displacment
        if (UnityUtil.MirrorAlongZ) { displacement *= -1.0f; }
        targetUnityTransform.Position += displacement;
      }

      // either do a hard keyframe
      bool useHardKeyframe = false;
      if (useHardKeyframe || gameObject.rigidbody.isKinematic) {
        unityTransform.position = targetUnityTransform.Position;
        unityTransform.rotation = targetUnityTransform.Rotation;
        gameObject.rigidbody.velocity = Vector3.zero;
        gameObject.rigidbody.angularVelocity = Vector3.zero;
      } else {
        // or assigne velocities
        ApplyTargetUnityTransformAsVelocities(targetUnityTransform, gameObject);
      }
    }

    static public Vector3 GetCenterFromCollider(GameObject gameObject)
    {
      Vector3 result = new Vector3(0,0,0);
      foreach(Collider collider in gameObject.GetComponents<Collider>())
      {
        if(collider is CapsuleCollider) { result = (collider as CapsuleCollider).center; }
        if(collider is BoxCollider) { result = (collider as BoxCollider).center; }
        if(collider is SphereCollider) { result = (collider as SphereCollider).center; }
      }
      return result;
    }
  }
}
