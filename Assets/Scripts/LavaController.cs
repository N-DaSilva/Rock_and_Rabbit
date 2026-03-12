using UnityEngine;

public class LavaController : MonoBehaviour
{

    [SerializeField] private float speed;

    void Update()
    {
        if (this.transform.position.y <= 0f)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + speed, this.transform.position.z);
        }
    }
}
