namespace SnakeGameV3.Constants
{
    internal class GameConstants
    {
        public GameConstants()
        {
            FrameDelay = 1000 / FramePerSecond;
        }

        public int ScreenHeight { get; } = 64;

        public int ScreenWidth { get; } = 128;

        public int GridCellSize { get; } = 8;

        public ConsoleColor SnakeHeadColor { get; } = ConsoleColor.DarkYellow;

        public ConsoleColor SnakeBodyColor { get; } = ConsoleColor.Yellow;

        public ConsoleColor BackgroundColor { get; } = ConsoleColor.Black;

        public double SnakeSpeed { get; } = 4;

        public int FramePerSecond { get; } = 60;

        public double FrameDelay { get; }

    }
}
