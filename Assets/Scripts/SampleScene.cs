using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SampleScene : MonoBehaviour
{
    [SerializeField] private Field field;
    [SerializeField] private Timer timer;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text comboText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup titleCanvasGroup;
    [SerializeField] private PracticeView practiceViewPrefab;
    [SerializeField] private LeaderboardView leaderboardViewPrefab;
    [SerializeField] private ResultLeaderboardView resultLeaderboardViewPrefab;
    [SerializeField] private GameObject tapDefenseView;

    private int Score
    {
        get => score;
        set
        {
            if (score < value)
            {
                // animation
            }

            score = value;
            UIUtility.TrySetText(scoreText, $"{score}");
        }
    }

    private int score;

    private int Combo
    {
        get => combo;
        set
        {
            if (combo < value)
            {
                // animation
            }

            combo = value;
            UIUtility.TrySetText(comboText, $"{combo}");
        }
    }

    private int combo;
    private LeaderboardView leaderboardView;

    private async void Start()
    {
        Score = 0;
        Combo = 0;
        field.ClearTiles();

        UIUtility.TrySetActive(titleCanvasGroup.gameObject, true);
        timer.Remaining = 0f;
        timer.IsRunning = false;

        UIUtility.TrySetActive(tapDefenseView, true);
        if (!PlayFabLoginManagerService.Instance.LoggedIn)
        {
            await PlayFabLoginManagerService.Instance.LoginAsyncWithRetry(1000);
        }

        UIUtility.TrySetActive(tapDefenseView, false);
    }

    public void OnClickPractice()
    {
        UIUtility.TrySetActive(titleCanvasGroup.gameObject, false);
        var view = Instantiate(practiceViewPrefab, canvas.transform);
        view.Initialize(field);
    }

    public void OnClickEasy()
    {
        StartLevel(new Level.Easy());
    }

    public void OnClickNormal()
    {
        StartLevel(new Level.Normal());
    }

    public void OnClickHard()
    {
        StartLevel(new Level.Hard());
    }

    public void OnClickEndless()
    {
        StartLevel(new Level.Endless());
    }

    public void OnClickLederboard(int index = 0)
    {
        UIUtility.TrySetActive(titleCanvasGroup.gameObject, false);

        if (leaderboardView != null)
        {
            Destroy(leaderboardView.gameObject);
        }

        leaderboardView = Instantiate(leaderboardViewPrefab, canvas.transform);
        leaderboardView.Initialize(index, 30, OnClickLederboard);
    }

    private async void StartLevel(Level.ILevel level)
    {
        UIUtility.TrySetActive(titleCanvasGroup.gameObject, false);

        var cancellationTokenSource = new CancellationTokenSource();
        var wave = 1;
        timer.Reset();
        timer.OnTimedUp = () =>
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
        };

        while (await PlayWave(level, wave, cancellationTokenSource.Token))
        {
            wave++;
        }

        if (timer.IsTimedUp)
        {
            // game over
        }
        else
        {
            // clear
        }

        await PlayFabLeaderboardUtility.UpdatePlayerStatisticWithRetry(level.StatisticName, Score, 1000);
        ShowResultLeaderboardView(level);
    }

    private async UniTask<bool> PlayWave(Level.ILevel level, int waveCount, CancellationToken cancellationToken)
    {
        timer.IsRunning = true;
        UIUtility.TrySetActive(tapDefenseView, false);
        field.Drop(level.WaveTileInfos(waveCount), OnHit, OnMissed);

        await UniTask.WaitUntil(() => cancellationToken.IsCancellationRequested || !field.TargetNumberRemained);
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        timer.IsRunning = false;
        UIUtility.TrySetActive(tapDefenseView, true);
        await field.Flush();

        return !level.IsLimitedWaveCount || waveCount < level.WaveCount;
    }

    private void OnHit()
    {
        Combo++;
        Score += ScoreCalculator.ScoreByCombo(Combo);

        if (field.TargetNumberRemained)
        {
            timer.RecoverByHit();
        }
        else
        {
            timer.RecoverByClear();
        }
    }

    private void OnMissed()
    {
        Combo = 0;
    }

    private void ShowResultLeaderboardView(Level.ILevel level)
    {
        var view = Instantiate(resultLeaderboardViewPrefab, canvas.transform);
        view.Initialize(level, 30, Score);
    }
}
