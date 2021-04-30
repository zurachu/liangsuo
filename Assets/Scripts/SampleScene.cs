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
    [SerializeField] private CanvasGroup practiceCanvasGroup;
    [SerializeField] private ResultLeaderboardView resultLeaderboardViewPrefab;

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

    private CancellationTokenSource cancellationTokenSource;

    private void Start()
    {
        Score = 0;
        Combo = 0;
        OnClickTitle();

        if (!PlayFabLoginManagerService.Instance.LoggedIn)
        {
            _ = PlayFabLoginManagerService.Instance.LoginAsyncWithRetry(1000);
        }
    }

    public void OnClickTitle()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
        }

        field.ClearTiles();

        UIUtility.TrySetActive(titleCanvasGroup.gameObject, true);
        UIUtility.TrySetActive(practiceCanvasGroup.gameObject, false);
        timer.Remaining = 0f;
        timer.IsRunning = false;
    }

    public async void OnClickPractice()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
        }

        UIUtility.TrySetActive(titleCanvasGroup.gameObject, false);
        UIUtility.TrySetActive(practiceCanvasGroup.gameObject, true);

        cancellationTokenSource = new CancellationTokenSource();
        while (await Practice(cancellationTokenSource.Token));
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

    private async UniTask<bool> Practice(CancellationToken cancellationToken)
    {
        field.Drop(Wave.AllTilesInfos(), null, null);

        await UniTask.WaitUntil(() => cancellationToken.IsCancellationRequested || !field.TargetNumberRemained);
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        await field.Flush();

        return !cancellationToken.IsCancellationRequested;
    }

    private async void StartLevel(Level.ILevel level)
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
        }

        UIUtility.TrySetActive(titleCanvasGroup.gameObject, false);
        UIUtility.TrySetActive(practiceCanvasGroup.gameObject, false);

        var wave = 1;
        timer.Reset();
        timer.OnTimedUp = () =>
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
        };

        cancellationTokenSource = new CancellationTokenSource();
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

        field.Drop(level.WaveTileInfos(waveCount), OnHit, OnMissed);

        await UniTask.WaitUntil(() => cancellationToken.IsCancellationRequested || !field.TargetNumberRemained);
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        timer.IsRunning = false;
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
