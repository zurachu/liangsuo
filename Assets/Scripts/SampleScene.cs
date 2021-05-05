using System.Threading;
using Cysharp.Threading.Tasks;
using KanKikuchi.AudioManager;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SampleScene : MonoBehaviour
{
    [SerializeField] private Field field;
    [SerializeField] private Timer timer;
    [SerializeField] private Text waveCountText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup titleCanvasGroup;
    [SerializeField] private PracticeView practiceViewPrefab;
    [SerializeField] private LeaderboardView leaderboardViewPrefab;
    [SerializeField] private TimeBonusView timeBonusViewPrefab;
    [SerializeField] private ResultLeaderboardView resultLeaderboardViewPrefab;
    [SerializeField] private GameObject tapDefenseView;

    private int WaveCount
    {
        get => waveCount;
        set
        {
            if (waveCount < value)
            {
                // animation
            }

            waveCount = value;
            UIUtility.TrySetText(waveCountText, $"{waveCount:#,0}");
        }
    }

    private int waveCount;

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
            UIUtility.TrySetText(scoreText, $"{score:#,0}");
        }
    }

    private int score;

    private int combo;
    private LeaderboardView leaderboardView;

    private async void Start()
    {
        WaveCount = 0;
        Score = 0;
        combo = 0;
        field.ClearTiles();
        BGMManager.Instance.Stop();

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
        BGMManager.Instance.Play(BGMPath.THINKINGTIME5);

        var cancellationTokenSource = new CancellationTokenSource();
        timer.Reset();
        timer.OnTimedUp = () =>
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
        };

        do
        {
            WaveCount++;
        } while (await PlayWave(level, WaveCount, cancellationTokenSource.Token));

        timer.IsRunning = false;
        UIUtility.TrySetActive(tapDefenseView, true);
        BGMManager.Instance.Stop();
        if (timer.IsTimedUp)
        {
            SEManager.Instance.Play(SEPath.BOMB4, 2f);
            _ = Camera.main.DOShakePosition(1f, 1f);
            await PlayFabLeaderboardUtility.UpdatePlayerStatisticWithRetry(level.StatisticName, Score, 1000);
            await UniTask.Delay(2000);
        }
        else
        {
            var timeBonusView = Instantiate(timeBonusViewPrefab, canvas.transform);
            var baseScore = Score;
            await timeBonusView.Play(level, (int)(timer.Remaining * 1000), (_added) => { Score = baseScore + _added; });
            await PlayFabLeaderboardUtility.UpdatePlayerStatisticWithRetry(level.StatisticName, Score, 1000);
            await UniTask.Delay(2000);
            Destroy(timeBonusView.gameObject);
        }

        ShowResultLeaderboardView(level);
    }

    private async UniTask<bool> PlayWave(Level.ILevel level, int waveCount, CancellationToken cancellationToken)
    {
        timer.IsRunning = true;
        UIUtility.TrySetActive(tapDefenseView, false);
        field.Drop(level.WaveTileInfos(waveCount), () => { OnHit(level, waveCount); }, OnMissed);

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

    private void OnHit(Level.ILevel level, int waveCount)
    {
        combo++;
        Score += ScoreCalculator.ScoreByCombo(combo);

        if (field.TargetNumberRemained)
        {
            timer.Recover(level.RecoveryTimeByHit(waveCount));
        }
        else
        {
            timer.Recover(level.RecoveryTimeByClear(waveCount));
        }
    }

    private async void OnMissed()
    {
        combo = 0;
        UIUtility.TrySetActive(tapDefenseView, true);
        await UniTask.Delay(500);
        UIUtility.TrySetActive(tapDefenseView, false);
    }

    private void ShowResultLeaderboardView(Level.ILevel level)
    {
        var view = Instantiate(resultLeaderboardViewPrefab, canvas.transform);
        view.Initialize(level, 30, Score);
    }
}
