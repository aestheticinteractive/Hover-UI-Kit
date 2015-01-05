using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Leap.Interact;

namespace Leap.Interact
{

  /// <summary>
  /// Utilities to automate gluing the Unity scene & Leap3dInteract functionality.
  /// </summary>
  public class HandViewer
  {
    static public HandViewer Instance { get; set; }

    public HandViewer(Scene scene, Dictionary<UnityEngine.GameObject, Body> bodyMapper)
    {
      Scene = scene;
      BodyMapper = bodyMapper;
    }

    protected UnityEngine.GameObject latestHandPalmAdded;
    protected int fingerBoneCount = 0;

    public void OnBodyAddedCallback(Body body)// IntPtr bodyHandle)
    {
      // Create the game object
      bool isFinger = false;
      GameObject gameObject;
      Shape.ShapeType type = body.Shape.Type;
      if (type == Shape.ShapeType.Capsule)
      {
        float capsuleRadius = body.Shape.CapsuleRadius;
        float capsuleSegmentLength = body.Shape.CapsuleSegmentLength;

        gameObject = GameObject.Instantiate(Resources.Load(UnityUtil.ResourcesFolder + UnityUtil.FingerBoneTemplateName) as GameObject) as GameObject;

        // Adjust dimensions
        float capsuleHeight = capsuleRadius * 2f + capsuleSegmentLength;
        float yScale = capsuleHeight / 2f;
        float zxScale = capsuleRadius / 0.5f;
        gameObject.transform.localScale = new Vector3(zxScale, yScale, zxScale);
        gameObject.name = UnityUtil.FingerBoneName;

        LeapTransform t = new LeapTransform();
        t = new LeapTransform(Native.AccessPropertyAsTransform(body, Native.Property.BodyTransform, Native.Mode.Get, t.ToNative()));
        gameObject.transform.position = t.Position;
        gameObject.transform.rotation = t.Rotation;

        gameObject.rigidbody.maxAngularVelocity = 100.0f;

        isFinger = true;
        fingerBoneCount++;
      }
      else
      {
        gameObject = GameObject.Instantiate(Resources.Load(UnityUtil.ResourcesFolder + UnityUtil.HandPalmTemplateName) as GameObject) as GameObject;
        gameObject.name = UnityUtil.HandPalmName;
        LeapTransform t = new LeapTransform();
        t = new LeapTransform(Native.AccessPropertyAsTransform(body, Native.Property.BodyTransform, Native.Mode.Get, t.ToNative()));
        gameObject.transform.position = t.Position;
        gameObject.transform.rotation = t.Rotation;

        gameObject.rigidbody.maxAngularVelocity = 100.0f;

        latestHandPalmAdded = gameObject;
        fingerBoneCount = 0;
      }

      BodyMapper.Add(gameObject, body);

      GameObject container = isFinger && latestHandPalmAdded != null ? latestHandPalmAdded : GameObject.Find(UnityUtil.DynamicObjectContainerName) as GameObject;
      gameObject.transform.parent = container.transform;

      gameObject.layer = UnityUtil.LayerForHands;

      if (fingerBoneCount%3 == 0 && fingerBoneCount > 0)
      {
        gameObject.name = UnityUtil.FingerTipBoneName;
        if (fingerBoneCount == 3)
        {
          gameObject.name = UnityUtil.ThumbTipBoneName;
        }
      }

      if (fingerBoneCount == 15)
      {
        latestHandPalmAdded = null;
        fingerBoneCount = 0;

        if (UnityUtil.FilterHandCollisionPerColliderExplicitly) { DisableHandSelfCollisions(gameObject.transform.parent); }
      }
    }

    public void OnBodyRemovedCallback(Body body)
    {
      // find gameobject
      GameObject gameObject = BodyMapper.FirstOrDefault(x => x.Value.BodyId.ptr == body.BodyId.ptr).Key;
      GameObject.Destroy(gameObject);
      BodyMapper.Remove(gameObject);
    }

    private void DisableHandSelfCollisions(Transform handContainer)
    {
      foreach(Transform child in handContainer)
      {
        foreach(Transform child2 in handContainer)
        {
          if (child != child2)
            Physics.IgnoreCollision(child.collider, child2.collider);
        }
      }
    }

    public void DisableHandCollisionsWithGameObject(GameObject gameObject)
    {
      // Find hands root
      GameObject handsContainer = GameObject.Find(UnityUtil.DynamicObjectContainerName) as GameObject;
      Collider[] collidersInHands = handsContainer.GetComponentsInChildren<Collider>();
      Collider[] collidersInObject = gameObject.GetComponentsInChildren<Collider>();
      foreach(Collider c in collidersInHands) {
        if (c.enabled) {
          foreach(Collider c2 in collidersInObject) {
            if (c2.enabled) { Physics.IgnoreCollision(c2, c);}
          }
        }
      }
    }

    public void EnableHandCollisionsWithGameObject(GameObject gameObject)
    {
      Collider[] collidersInObject = gameObject.GetComponentsInChildren<Collider>();
      foreach(Collider c in collidersInObject)
      {
        if (c.enabled) { c.enabled = false; c.enabled = true;}
      }
    }

    public Scene Scene;
    public Dictionary<UnityEngine.GameObject, Body> BodyMapper;
  }
}
