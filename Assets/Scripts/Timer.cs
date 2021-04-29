using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private Image dangerBarImage;
    [SerializeField] private float barMaxTime;
    [SerializeField] private float initialTime;
    [SerializeField] private float dangerTime;

    public bool IsRunning { get; set; }
    public Action OnTimedUp { get; set; }

    public float Remaining
    {
        get => remainingTime;
        set
        {
            var isPreviousRemaining = remainingTime > 0f;

            remainingTime = value;

            if (isPreviousRemaining && Remaining <= 0)
            {
                OnTimedUp?.Invoke();
            }

            var ratio = remainingTime / barMaxTime;
            barImage.fillAmount = ratio;
            dangerBarImage.fillAmount = ratio;
            var isDanger = remainingTime <= dangerTime;
            UIUtility.TrySetActive(barImage, !isDanger);
            UIUtility.TrySetActive(dangerBarImage, isDanger);
        }
    }

    private float remainingTime;
    private bool isRecovering;

    public void Reset()
    {
        Remaining = initialTime;
    }

    public async void Recover(float time)
    {
        isRecovering = true;
        await DOTween.To(() => Remaining, (_remaining) => Remaining = _remaining, time, 1f)
            .SetRelative(true)
            .SetEase(Ease.Linear);
        isRecovering = false;
    }

    private void Start()
    {
        Remaining = 0f;
    }

    private void Update()
    {
        if (IsRunning && !isRecovering)
        {
            Remaining -= Time.deltaTime;
        }
    }
}
