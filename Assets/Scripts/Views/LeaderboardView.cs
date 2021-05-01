using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] private PlayFabLeaderboardScrollView leaderboardScrollView;
    [SerializeField] private Text titleText;
    [SerializeField] private CanvasGroup buttonCanvasGroup;

    public static readonly List<Level.ILevel> levels = new List<Level.ILevel>
    {
        new Level.Easy(),
        new Level.Normal(),
        new Level.Hard(),
        new Level.Endless(),
    };

    private int index;
    private Action<int> onChangedIndex;

    public async void Initialize(int index, int maxResultsCount, Action<int> onChangedIndex)
    {
        this.index = index;
        this.onChangedIndex = onChangedIndex;

        var level = levels[index];
        UIUtility.TrySetText(titleText, $"{level.Name}ランキング");

        buttonCanvasGroup.interactable = false;
        await leaderboardScrollView.Initialize(level.StatisticName, maxResultsCount, int.MinValue);
        buttonCanvasGroup.interactable = true;
    }

    public void OnClickPrevious()
    {
        onChangedIndex?.Invoke((index <= 0) ? levels.Count - 1 : index - 1);
    }

    public void OnClickNext()
    {
        onChangedIndex?.Invoke((index + 1 >= levels.Count) ? 0 : index + 1);
    }
}
