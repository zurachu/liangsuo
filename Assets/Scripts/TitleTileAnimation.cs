using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class TitleTileAnimation : MonoBehaviour
{
    [SerializeField] private int delay;

    private async void Start()
    {
        await UniTask.Delay(delay);
        _ = DOTween.Sequence()
            .Append(transform.DOLocalMoveY(10, 0.5f).SetEase(Ease.OutFlash, 2).SetRelative(true).SetLink(gameObject))
            .AppendInterval(2)
            .SetLoops(-1)
            .SetLink(gameObject);
    }
}
