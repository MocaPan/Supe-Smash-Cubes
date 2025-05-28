using UnityEngine;
using System.Collections.Generic;

public enum PowerUpType
{
    None,
    StrongShot,
    DoubleJump, // Allows one extra jump in midair, jump is 2.5x normal
    Shield      // Player is impervious to push from enemy shots for 5s
}

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }
    private Dictionary<PowerUpType, float> activePowerUps = new Dictionary<PowerUpType, float>();
    private readonly float shieldDuration = 3f;
    private readonly float strongShotDuration = 5f;
    private readonly float doubleJumpDuration = 5f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        List<PowerUpType> expired = new List<PowerUpType>();
        var keys = new List<PowerUpType>(activePowerUps.Keys);
        foreach (var type in keys)
        {
            activePowerUps[type] -= Time.deltaTime;
            if (activePowerUps[type] <= 0f)
                expired.Add(type);
        }
        foreach (var type in expired)
            activePowerUps.Remove(type);
    }

    public void SetPowerUp(PowerUpType type)
    {
        float duration = 0f;
        switch (type)
        {
            case PowerUpType.Shield:
                duration = shieldDuration;
                break;
            case PowerUpType.StrongShot:
                duration = strongShotDuration;
                break;
            case PowerUpType.DoubleJump:
                duration = doubleJumpDuration;
                break;
        }
        if (duration > 0f)
            activePowerUps[type] = duration;
    }

    public void ClearPowerUp(PowerUpType type)
    {
        if (activePowerUps.ContainsKey(type))
            activePowerUps.Remove(type);
    }

    public bool HasPowerUp(PowerUpType type)
    {
        return activePowerUps.ContainsKey(type);
    }
}

public enum FragmentType { BST, AVL, Heap }

public class TreeFragment : MonoBehaviour
{
    public FragmentType tipo;
    public int valor; // Por ejemplo, el valor del nodo
}

"""public class PlayerInventory : MonoBehaviour
{
    public List<TreeFragment> fragmentos = new List<TreeFragment>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        TreeFragment fragmento = other.GetComponent<TreeFragment>();
        if (fragmento != null)
        {
            fragmentos.Add(fragmento);
            Destroy(other.gameObject);
            // Aquí puedes actualizar la UI o verificar retos
        }
    }
}"""


