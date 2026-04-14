using UnityEngine;

public class LavaController : MonoBehaviour
{

    [SerializeField] private float speed = 0.5f;

    void Update()
    {
        if (transform.position.y <= 0f)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
    }
}
