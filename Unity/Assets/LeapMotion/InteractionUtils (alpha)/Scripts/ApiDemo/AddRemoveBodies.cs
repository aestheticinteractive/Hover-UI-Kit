using UnityEngine;
using System.Collections;

public class AddRemoveBodies : MonoBehaviour {

  private float m_time = 0.0f;
  private float m_lastOperation = 0.0f;
  private float m_operationPeriod = 1.0f;
  private int m_operationCount = 0;
  private int m_numDynamicObjects = 5;

  public GameObject m_objectToInstantiate;
  private GameObject[] m_dynamicObjects;

  // Use this for initialization
  void Start () {
    m_dynamicObjects = new GameObject[m_numDynamicObjects];
  }
  
  // Update is called once per frame
  void Update () {
    m_time += Time.deltaTime;

    // trigger operation
    if (m_time - m_lastOperation > m_operationPeriod)
    {
      const float positionOffset = 1.5f;
      // Add or remove
      if (m_operationCount < m_numDynamicObjects)
      {
        GameObject newObject = (GameObject)GameObject.Instantiate(m_objectToInstantiate);
        newObject.transform.position = m_objectToInstantiate.transform.position + Vector3.right * positionOffset * (1 + m_operationCount);
        m_dynamicObjects[m_operationCount] = newObject;
      } else if (m_operationCount < 2 * m_numDynamicObjects) {
        GameObject.Destroy(m_dynamicObjects[m_operationCount - m_numDynamicObjects]);
      }

      m_lastOperation = m_time;
      m_operationCount++;
    }
  }
}
