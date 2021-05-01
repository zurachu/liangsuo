using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PracticeView : MonoBehaviour
{
    [SerializeField] private GameObject tapDefenseView;

    private CancellationTokenSource cancellationTokenSource;

    public async void Initialize(Field field)
    {
        cancellationTokenSource = new CancellationTokenSource();
        while (await Practice(field, cancellationTokenSource.Token));
    }

    private async UniTask<bool> Practice(Field field, CancellationToken cancellationToken)
    {
        UIUtility.TrySetActive(tapDefenseView, false);
        field.Drop(Wave.AllTilesInfos(), null, null);

        await UniTask.WaitUntil(() => cancellationToken.IsCancellationRequested || !field.TargetNumberRemained);
        if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        UIUtility.TrySetActive(tapDefenseView, true);
        await field.Flush();

        return !cancellationToken.IsCancellationRequested;
    }

    private void OnDestroy()
    {
        cancellationTokenSource?.Cancel();
    }
}
