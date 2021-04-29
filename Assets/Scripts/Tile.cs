using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public struct Info
    {
        public enum Type
        {
            Man,
            Pin,
            Sou,
        }

        public Type type;
        public int number;

        public Info(Type type, int number)
        {
            this.type = type;
            this.number = number;
        }

        public string SpritePath => $"Sprites/{type.ToString().ToLower()}{number}-66-90-l-emb";
    }

    public Info TileInfo { get; private set; }

    private SpriteRenderer Renderer => GetComponent<SpriteRenderer>();

    private static readonly Color enteredColor = new Color(1f, 0.8f, 0.8f);

    private Action<Tile> onTouched;

    public void Initialize(Info info, Action<Tile> onTouched)
    {
        TileInfo = info;
        this.onTouched = onTouched;

        Renderer.sprite = Resources.Load<Sprite>(info.SpritePath);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onTouched?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Renderer.color = enteredColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Renderer.color = Color.white;
    }
}
