using UnityEngine;

public class HoverObject : MonoBehaviour
{
  [Header("Hover Settings")]
  [Tooltip("How high and low the object will move from its starting position.")]
  public float hoverAmplitude = 0.25f;

  [Tooltip("How fast the object bobs up and down.")]
  public float hoverSpeed = 2f;

  private Vector3 startPosition;

  void Start()
  {
    startPosition = transform.position;
  }

  void Update()
  {
    float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
    transform.position = new Vector3(startPosition.x, newY, startPosition.z);
  }
}
