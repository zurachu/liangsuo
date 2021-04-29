using UnityEngine;

public static class ScoreCalculator
{
    private static readonly int[] scoreByCombo = { 2600, 3900, 5200, 6400, 7700, 8000 };

    public static int ScoreByCombo(int combo)
    {
        combo = Mathf.Clamp(combo, 0, scoreByCombo.Length - 1);
        return scoreByCombo[combo];
    }
}
