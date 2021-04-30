using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PracticeView : MonoBehaviour
{
    private CancellationTokenSource cancellationTokenSource;

    public async void Initialize(Field field)
    {
        cancellationTokenSource = new CancellationTokenSource();
        while (await Practice(field, cancellationTokenSource.Token));
    }

    private async UniTask<bool> Practice(Field field, CancellationToken cancellationToken)
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

    private void OnDestroy()
    {
        cancellationTokenSource?.Cancel();
    }
}
