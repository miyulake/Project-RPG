using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttributes", menuName = "Miyu/Player/Attributes")]
public class PlayerAttributes : ScriptableObject
{
    [Header("Attributes")]
    public float lookSensitivity = 180f;
    public float lookSmoothTime = 0.1f;
    public float yawLookDuration = 0.33f;
    public float lookingUpAmount = -60f;
    public float lookingDownAmount = 60f;

    [Header("Stats")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float gravity = -30f;

    [Header("Ground")]
    public LayerMask groundCastLayers = ~0;
    public float extraDistance = 0.03f;
}
