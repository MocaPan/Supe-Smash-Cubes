using System;

public static class PowerUpTypeExtensions
{
    public static PowerUpType GetRandomPowerUp()
    {
        Array values = Enum.GetValues(typeof(PowerUpType));
        return (PowerUpType)values.GetValue(UnityEngine.Random.Range(1, values.Length));
    }
} 