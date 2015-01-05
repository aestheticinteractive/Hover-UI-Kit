using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Leap.Interact;

public class HoldingCallbacks : MonoBehaviour {

  public Material freeMaterial;
  public Material hoverMaterial;
  public Material heldMaterial;

  private GameObject lastHovered = null;

  void Start () {
    UnityUtil.Scene.OnHoldingHoverOver += new Scene.HoldingNotification(OnHoldingHovers);
    UnityUtil.Scene.OnHoldingStarts += new Scene.HoldingNotification(OnHoldingStarts);
    UnityUtil.Scene.OnHoldingUpdates += new Scene.HoldingNotification(OnHoldingUpdates);
    UnityUtil.Scene.OnHoldingEnds += new Scene.HoldingNotification(OnHoldingEnds);
  }
  
  void Update () {
  }

  public void OnHoldingHovers(Holding holding) {
    Body body = holding.Body;
    GameObject gameObject = null;
    if (body != null && body.IsValid())
      gameObject = UnityUtil.BodyMapper.FirstOrDefault(x => x.Value.BodyId.ptr == body.BodyId.ptr).Key;
    if (lastHovered != gameObject)
    {
      if (lastHovered)
        lastHovered.renderer.material = freeMaterial;
      if (gameObject)
        gameObject.renderer.material = hoverMaterial;
      lastHovered = gameObject;
    }
  }

  public void OnHoldingStarts(Holding holding) {
    Body body = holding.Body;
    GameObject gameObject = null;
    if (body != null && body.IsValid())
      gameObject = UnityUtil.BodyMapper.FirstOrDefault(x => x.Value.BodyId.ptr == body.BodyId.ptr).Key;
    //Debug.Log("holding started " + gameObject);
    if (gameObject)
      gameObject.renderer.material = heldMaterial;
  }

  public void OnHoldingUpdates(Holding holding) {
  }

  public void OnHoldingEnds(Holding holding) {
    Body body = holding.Body;
    GameObject gameObject = null;
    if (body != null && body.IsValid())
      gameObject = UnityUtil.BodyMapper.FirstOrDefault(x => x.Value.BodyId.ptr == body.BodyId.ptr).Key;
    if (gameObject)
      gameObject.renderer.material = freeMaterial;
  }
}
