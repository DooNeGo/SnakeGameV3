namespace SnakeGameV3.Model
{
    public enum EffectType
    {
        Speed,
        Scale,
        Length
    }

    internal class Effect
    {
        public Effect(float value, EffectType type)
        {
            Value = value;
            Type = type;
        }

        public float Value { get; }

        public EffectType Type { get; }
    }
}
