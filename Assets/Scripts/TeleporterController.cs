using UnityEngine;

public class TeleporterController : MonoBehaviour
{
    [SerializeField] private GameObject teleporterOUT;
    [SerializeField] private int direction;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.transform.position = new Vector3(teleporterOUT.transform.position.x + (1*direction), other.transform.position.y, other.transform.position.z);
        }
    }
}
