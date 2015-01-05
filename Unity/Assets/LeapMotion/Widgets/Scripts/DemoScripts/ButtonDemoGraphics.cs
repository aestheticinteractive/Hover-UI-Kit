using UnityEngine;
using System.Collections;

public class ButtonDemoGraphics : MonoBehaviour 
{
  public void SetActive(bool status)
  {
    Renderer[] renderers = GetComponentsInChildren<Renderer>();
    foreach (Renderer renderer in renderers)
    {
      renderer.enabled = status;
    }
  }

  public void SetColor(Color color)
  {
    Renderer[] renderers = GetComponentsInChildren<Renderer>();
    foreach (Renderer renderer in renderers)
    {
      renderer.material.color = color;
    }
  }
}
