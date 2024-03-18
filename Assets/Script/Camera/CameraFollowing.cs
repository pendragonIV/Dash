using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera _virtualCamera;

    public void Follow(Transform player)
    {
        _virtualCamera.Follow = player;
    }

}
