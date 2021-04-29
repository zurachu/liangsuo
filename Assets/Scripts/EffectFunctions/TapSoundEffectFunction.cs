using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TapSoundEffectFunction : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        var button = GetComponent<Button>();
        if (button == null || button.interactable)
        {
            SEManager.Instance.Play(SEPath.NOTANOMORI_200812290000000032);
        }
    }
}
