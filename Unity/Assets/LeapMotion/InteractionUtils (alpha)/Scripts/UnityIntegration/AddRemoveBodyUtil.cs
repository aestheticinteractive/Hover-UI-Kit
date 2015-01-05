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
  public class AddRemoveBodyUtil
  {
    static public AddRemoveBodyUtil Instance { get; set; }

    public AddRemoveBodyUtil(Scene scene, Dictionary<UnityEngine.GameObject, Body> bodyMapper)
    {
      Scene = scene;
      BodyMapper = bodyMapper;
    }

    public Body AddBodyToLeapFromUnity(Rigidbody rigidbody)
    {
      LeapInteraction properties = rigidbody.GetComponent<LeapInteraction>();

      if (rigidbody.collider && properties)
      {
        Collider[] colliders = rigidbody.GetComponents<Collider>();
        
        Shape shape = new Shape();
        foreach(Collider collider in colliders)
        {
          if (collider is SphereCollider)
          {
            float scale = rigidbody.transform.lossyScale.x;
            SphereCollider sc = collider as SphereCollider;
            shape = Shape.CreateSphere(sc.radius * scale);
          }
          else if (collider is CapsuleCollider)
          {
            float scale = rigidbody.transform.lossyScale.x;
            CapsuleCollider cc = collider as CapsuleCollider;
            shape = Shape.CreateCapsule((Shape.CapsuleOrientation)cc.direction, Math.Max(0f, cc.height / 2f - cc.radius) * scale, cc.radius * scale);
          }
          else if (collider is BoxCollider)
          {
            BoxCollider bc = collider as BoxCollider;
            Vector3 scale = collider.transform.lossyScale;
            shape = Shape.CreateBox(Vector3.Scale(bc.size, scale) / 2f, 0f);
          }
        }
        
        if (shape != IntPtr.Zero)
        {
          Body body = new Body();//shape);
          body.Shape = shape;
          body.Mass = rigidbody.mass;
          
          // Add body anchors.
          for (int i = 0; i < rigidbody.transform.childCount; i++)
          {
            Transform child = rigidbody.transform.GetChild(i);
            if (child.name.StartsWith("Anchor") || child.name.StartsWith("ClickAnchor"))
            {
              LeapTransform anchor = new LeapTransform();
              anchor.Position = Vector3.Scale(child.localPosition - rigidbody.transform.rotation * TransformSyncUtil.GetCenterFromCollider(rigidbody.gameObject), rigidbody.transform.lossyScale);
              anchor.Rotation = child.localRotation;
              if (child.name.StartsWith("Anchor")) { body.Shape.AddAnchor(anchor); }
              if (child.name.StartsWith("ClickAnchor")) 
              {
                body.Shape.AddClickAnchor(anchor); 
              }
            }
          }
          
          // Apply BodyProperties
          properties.ApplyToBody(body);

          Scene.AddBody(body);
          BodyMapper.Add(rigidbody.gameObject, body);

          rigidbody.maxAngularVelocity = 100.0f;

          return body;
        }
      }
      return null;
    }

    public void RemoveBodyFromLeap(Rigidbody rigidbody)
    {
      Body body = BodyMapper[rigidbody.gameObject];
      BodyMapper.Remove(rigidbody.gameObject);
      Scene.RemoveBody(body);
    }


    public Scene Scene;
    public Dictionary<UnityEngine.GameObject, Body> BodyMapper;
  }
}
