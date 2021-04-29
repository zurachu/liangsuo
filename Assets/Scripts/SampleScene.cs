using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SampleScene : MonoBehaviour
{
    [SerializeField] private Field field;
    [SerializeField] private Timer timer;
    [SerializeField] private CanvasGroup titleCanvasGroup;
    [SerializeField] private CanvasGroup practiceCanvasGroup;

    private CancellationTokenSource cancellationTokenSource;

    private void Start()
    {
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
        var tileInfos = new List<Tile.Info>();
        tileInfos.AddRange(Enumerable.Range(1, 10).ToList().ConvertAll(_number => new Tile.Info(Tile.Info.Type.Man, _number)));
        tileInfos.AddRange(Enumerable.Range(1, 10).ToList().ConvertAll(_number => new Tile.Info(Tile.Info.Type.Pin, _number)));
        tileInfos.AddRange(Enumerable.Range(1, 10).ToList().ConvertAll(_number => new Tile.Info(Tile.Info.Type.Sou, _number)));
        tileInfos = ListUtility.Shuffle(tileInfos);
        field.Drop(tileInfos, null, null);

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
        var tileInfos = new List<Tile.Info>();
        tileInfos.AddRange(Enumerable.Range(1, 10).ToList().ConvertAll(_number => new Tile.Info(Tile.Info.Type.Man, _number)));
        tileInfos.AddRange(Enumerable.Range(1, 10).ToList().ConvertAll(_number => new Tile.Info(Tile.Info.Type.Pin, _number)));
        tileInfos.AddRange(Enumerable.Range(1, 10).ToList().ConvertAll(_number => new Tile.Info(Tile.Info.Type.Sou, _number)));
        tileInfos = ListUtility.Shuffle(tileInfos);
        field.Drop(tileInfos, OnHit, null);
    }

    private async void OnHit()
    {
        timer.Recover(1f);

        if (!field.TargetNumberRemained)
        {
            await field.Flush();
            Drop();
        }
    }
}
