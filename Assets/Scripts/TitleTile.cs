using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TitleTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int delay;

    private Image Image => GetComponent<Image>();

    private static readonly Color enteredColor = new Color(1f, 0.8f, 0.8f);

    private async void Start()
    {
        await UniTask.Delay(delay);
        _ = DOTween.Sequence()
            .Append(transform.DOLocalMoveY(10, 0.5f).SetEase(Ease.OutFlash, 2).SetRelative(true))
            .AppendInterval(2)
            .SetLoops(-1)
            .SetLink(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Image.color = enteredColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Image.color = Color.white;
    }
}
