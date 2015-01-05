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
  public class UnityUtil
  {
    static public string ResourcesFolder = "LeapInteract/Prefabs/";
    static public string HandPalmTemplateName = "Hand Palm Template";
    static public string FingerBoneTemplateName = "Finger Bone Template";

    static public string HandPalmName = "Hand Palm";
    static public string FingerBoneName = "Finger Bone";
    static public string FingerTipBoneName = "Finger Tip Bone";
    static public string ThumbTipBoneName = "Thumb Tip Bone";

    static public string DynamicObjectContainerName = "Leap Dynamic Objects";

    static public int LayerForHands = 30;
    static public int LayerForHeldObjects = 31;
    static public int LayerForReleasedObjects = 0; // Default

    static public bool MirrorAlongZ = false;

    [HideInInspector]
    static public bool FilterHandCollisionPerColliderExplicitly = false;

    public UnityUtil(Scene scene) { Scene = scene; }

    static protected UnityUtil m_instance = null;
    static public UnityUtil Instance() { return m_instance; }

    public void InitLeap()
    {
      m_instance = this;
      if (Scene == null) { Scene = new Scene(); }
      BodyMapper = new Dictionary<GameObject, Body>();

      AddRemoveBodyUtil.Instance = new AddRemoveBodyUtil(Scene, BodyMapper);
      HandViewer.Instance = new HandViewer(Scene, BodyMapper);

      // Add listener to show Leap Interaction hands
      Scene.OnBodyAdded += new Scene.BodyNotification(HandViewer.Instance.OnBodyAddedCallback);
      Scene.OnBodyRemoved += new Scene.BodyNotification(HandViewer.Instance.OnBodyRemovedCallback);

      Scene.OnBodyScaled += new Scene.BodyNotification(OnBodyScaledCallback);

      // Create a container for the dynamic objects.
      GameObject dynamicObjectContainer = new GameObject();
      dynamicObjectContainer.name = DynamicObjectContainerName;

      if (!FilterHandCollisionPerColliderExplicitly) { SetLayerCollisions(); }
    }

    private void SetLayerCollisions()
    {
      Physics.IgnoreLayerCollision(LayerForHands, LayerForHands);
      if (LayerForHeldObjects != LayerForReleasedObjects) {
        Physics.IgnoreLayerCollision(LayerForHands, LayerForHeldObjects);
      }
    }

    public void OnBodyScaledCallback(Body body)
    {
      GameObject gameObject = UnityUtil.BodyMapper.FirstOrDefault(x => x.Value.BodyId.ptr == body.BodyId.ptr).Key;
      LeapInteraction properties = gameObject.GetComponent<LeapInteraction>();
      float bodyScale = body.Scale;
      float scaleRatio = bodyScale / properties.scale;
      gameObject.transform.localScale *= scaleRatio;
      properties.scale = bodyScale;
    }

    /// <summary>
    /// 
    /// </summary>
    public void StepLeap(float deltaTime)
    {
      // todo: AddAndRemoveBodiesOnLeap();
      TransformSyncUtil.UpdateLeapFromUnity(BodyMapper);
      //UpdateLeapFromUnity();
      Scene.Update(deltaTime);
      TransformSyncUtil.UpdateUnityFromLeap(BodyMapper);
      //UpdateUnityFromLeap();

      int error = Scene.LastError;
      if (error != 0)
      {
        Debug.Log("error in Leap Interaction. Code #" + error);
        Scene.ClearLastError();
      }
    }

    static public GameObject GetGameObject(Body body)
    {
      foreach(KeyValuePair<GameObject, Body> pair in UnityUtil.BodyMapper)
      {
        if (pair.Value == body)
        {
          return pair.Key;
        }
      }
      return null;
    }

    static public Scene Scene { get; protected set; }
    static public Dictionary<UnityEngine.GameObject, Body> BodyMapper;
  }
}
