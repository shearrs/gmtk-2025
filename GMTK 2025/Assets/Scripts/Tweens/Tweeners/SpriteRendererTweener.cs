using UnityEngine;

namespace Shears.Tweens
{
    public class SpriteRendererTweener : MonoBehaviour
    {
        public enum TweenType { Color }

        [Header("Data")]
        [SerializeField] private bool playOnEnable;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TweenData data;
        [SerializeField] private TweenType type;
        private ITween tween;

        [Header("Colors")]
        [SerializeField] private Color color1 = Color.white;
        [SerializeField] private Color color2 = Color.gray;

        public TweenData TweenData { get => data; set => data = value; }
        public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
        public TweenType Type { get => type; set => type = value; }
        public Color Color1 { get => color1; set => color1 = value; }
        public Color Color2 { get => color2; set => color2 = value; }

        private void OnEnable()
        {
            if (playOnEnable)
                Play1To2();
        }

        public void Play1To2() => Play(GetTween(color1, color2));
        public void Play2To1() => Play(GetTween(color2, color1));

        private void Play(ITween tween)
        {
            ClearTween();

            this.tween = tween;
            tween.Play();
        }

        public void Stop() => ClearTween();

        private void ClearTween()
        {
            tween?.Stop();
            tween?.Dispose();

            tween = null;
        }

        private ITween GetTween(Color from, Color to)
        {
            ITween tween = null;

            spriteRenderer.color = from;

            switch (type)
            {
                case TweenType.Color:
                    tween = spriteRenderer.GetColorTween(to, data);
                    break;
            }

            tween.AddOnComplete(ClearTween);

            return tween;
        }
    }
}
