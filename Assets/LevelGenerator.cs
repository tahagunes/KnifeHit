using UnityEngine;

[System.Serializable]
public class LevelGenSettings
{
    public float baseSpeed = 100f;
    public float speedGrowth = 0.5f;
    public float[] cycleMultipliers = { 0.85f, 0.95f, 1f, 1.1f, 1f };
    public int cycleLength = 5;
    public int obstacleStartLevel = 3;
    public int maxObstacles = 4;
    public int minApples = 1;
    public int maxApples = 3;
    public int bossReward = 10;
}

public static class LevelGenerator
{
    public static LevelData Generate(int level, int slotCount, LevelGenSettings s)
    {
        var random = new System.Random(level);
        int cyclePos = (level - 1) % s.cycleLength;
        bool isBoss = cyclePos == s.cycleLength - 1;

        float speed = s.baseSpeed * (1f + s.speedGrowth * Mathf.Log(level));
        speed *= s.cycleMultipliers[cyclePos];
        if (random.Next(2) == 0) speed = -speed;

        int obstacles = 0;
        if (level >= s.obstacleStartLevel)
        {
            obstacles = Mathf.Min(1 + (level - s.obstacleStartLevel) / s.cycleLength, s.maxObstacles);
            if (cyclePos == 0) obstacles = Mathf.Max(0, obstacles - 1);
        }

        int apples = random.Next(s.minApples, s.maxApples + 1);
        int knives = Mathf.Min(level, slotCount - obstacles - apples);

        return new LevelData
        {
            knifeCount = Mathf.Max(1, knives),
            rotationSpeed = speed,
            appleCount = apples,
            obstacleKnives = obstacles,
            isBoss = isBoss,
            bossReward = isBoss ? s.bossReward : 0,
            seed = level
        };
    }
}