using UnityEngine;

public enum PowerUpType { Shield, DoubleJump, StrongShot }

public class PowerUpBall : MonoBehaviour
{
    public PowerUpType powerUpType;
    public float strongShotMultiplier = 2.5f;
    public float strongShotDuration = 2f;
    public float shieldDuration = 5f;

    void OnMyCollisionEnter(CustomPhysics2D.MyCollider2D other)
{
        var maskGuy = other.GetComponent<MaskGuy>();
        var frog = other.GetComponent<FrogMovement>();
        var playerHealth = other.GetComponent<PlayerHealth>();

        if (maskGuy != null || frog != null)
        {
            switch (powerUpType)
            {
                case PowerUpType.Shield:
                    if (playerHealth != null)
                    {
                        playerHealth.isShielded = true;
                        playerHealth.shieldEndTime = Time.time + shieldDuration;
                        if (playerHealth.shieldActivateSound != null)
                            playerHealth.shieldActivateSound.Play();
                    }
                    break;
                case PowerUpType.DoubleJump:
                    if (maskGuy != null) maskGuy.ActivateDoubleJump();
                    if (frog != null) frog.ActivateDoubleJump();
                    break;
                case PowerUpType.StrongShot:
                    if (maskGuy != null) maskGuy.ActivateStrongShot(strongShotDuration, strongShotMultiplier);
                    if (frog != null) frog.ActivateStrongShot(strongShotDuration, strongShotMultiplier);
                    break;
            }

            // Play effect, sound, etc. (optional)

            Destroy(gameObject); // Power-up used
        }
    }
}
