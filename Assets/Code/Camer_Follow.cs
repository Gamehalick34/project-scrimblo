using UnityEngine;

public class Camer_Follow : MonoBehaviour
{
    private Transform player;
    // Start is called before the first frame update
    void Awake()
    {
        //finds player, make sure player sprite tag is set to
        player = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate()
    {
        //controls camera to follow player
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = player.position.x;
        transform.position = cameraPosition;
    }
}