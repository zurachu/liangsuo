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
    [SerializeField] private CanvasGroup titleCanvasGroup;
    [SerializeField] private CanvasGroup practiceCanvasGroup;

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

    public void OnClickStartGame()
    {
        UIUtility.TrySetActive(titleCanvasGroup.gameObject, false);
        Drop();
        timer.Reset();
        timer.IsRunning = true;
    }

    private void Drop()
    {
        var tileInfos = Wave.RandomAllTypeTilesInfos(5);
        field.Drop(tileInfos, OnHit, OnMissed);
    }

    private async void OnHit()
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
            timer.IsRunning = false;
            await field.Flush();
            timer.IsRunning = true;
            Drop();
        }
    }

    private void OnMissed()
    {
        Combo = 0;
    }
}
