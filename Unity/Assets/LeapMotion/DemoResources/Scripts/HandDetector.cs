using UnityEngine;
using System.Collections;
using Leap;

public class HandDetector : MonoBehaviour {

  public HandController leap_controller_;

  HandModel GetHand(Collider other)
  {
    HandModel hand_model = null;
    if (other.transform.parent && other.transform.parent.parent && other.transform.parent.parent.GetComponent<HandModel>())
    {
      hand_model = other.transform.parent.parent.GetComponent<HandModel>();
    }
    return hand_model;
  }

  void OnTriggerEnter(Collider other)
  {
    HandModel hand_model = GetHand(other);
    if (hand_model != null)
    {
      int handID = hand_model.GetLeapHand().Id;
      HandModel[] hand_models = leap_controller_.GetAllGraphicsHands();
      for (int i = 0; i < hand_models.Length; ++i)
      {
        if (hand_models[i].GetLeapHand().Id == handID)
        {
          GameObject part = hand_models[i].transform.Find(other.transform.parent.name).Find(other.name).gameObject;
          Renderer[] renderers = part.GetComponentsInChildren<Renderer>();
          foreach(Renderer renderer in renderers) {
            renderer.material.color = Color.red;
          }
        }
      }
    }
  }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
