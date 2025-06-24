using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0, 20, 0); // height above player
    [SerializeField] private bool rotateWithPlayer = false;

    void LateUpdate()
    {
        if (player == null) return;

        transform.position = player.position + offset;

        if (rotateWithPlayer)
        {
            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}
