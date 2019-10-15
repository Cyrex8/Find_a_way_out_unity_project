using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject door;
    [SerializeField]
    private Animator d2_animator;
    void OnTriggerEnter(Collider other)
    {
        d2_animator.SetBool("character_nearby", true);
      
        transform.localPosition += new Vector3(0, -0.1f, 0);
      
    }

    void OnTriggerExit(Collider other)
    {
        d2_animator.SetBool("character_nearby", false);
        transform.localPosition -= new Vector3(0, -0.1f, 0);
      
    }
}