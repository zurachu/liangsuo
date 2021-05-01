using UnityEngine;

public static class ScoreCalculator
{
    private static readonly int[] scoreByCombo = { 1000, 1300, 1600, 1900, 2200, 2500, 2800, 3100, 3400 };

    public static int ScoreByCombo(int combo)
    {
        combo = Mathf.Clamp(combo, 0, scoreByCombo.Length - 1);
        return scoreByCombo[combo];
    }
}
