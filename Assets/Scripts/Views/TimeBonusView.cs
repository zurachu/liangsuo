using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.UI;

public class TimeBonusView : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text bonusText;

    private int Bonus
    {
        get => bonus;
        set
        {
            bonus = value;
            UIUtility.TrySetText(bonusText, $"{bonus:#,0}");
        }
    }

    private int bonus;

    public async UniTask Play(Level.ILevel level, int bonus, Action<int> onAdded)
    {
        Bonus = bonus;

        UIUtility.TrySetText(titleText, $"{level.Name}クリア！");
        await UniTask.Delay(1500);
        SEManager.Instance.Play(SEPath.NOTANOMORI_200812290000000032);
        await UniTask.Delay(1500);

        await DOTween.To(() => Bonus, (_bonus) => {
            Bonus = _bonus;
            onAdded?.Invoke(bonus - Bonus);
        }, 0, 1f)
            .SetEase(Ease.Linear);
    }
}
