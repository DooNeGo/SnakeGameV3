namespace SnakeGameV3.Constants
{
    internal class GameConstants
    {
        public const int ScreenHeight = 64;
        public const int ScreenWidth = 128;

        public const int GridCellSize = 4;

        public const ConsoleColor SnakeHeadColor = ConsoleColor.DarkYellow;
        public const ConsoleColor SnakeBodyColor = ConsoleColor.Yellow;
        public const ConsoleColor BackgroundColor = ConsoleColor.Gray;
        public const ConsoleColor FoodColor = ConsoleColor.DarkRed;
        public const ConsoleColor BoarderColor = ConsoleColor.White;

        public const double SnakeSpeed = 4;

        public const int FramePerSecond = 60;
        public const double FrameDelay = 1000 / FramePerSecond;

        public const char PixelModel = '█';
    }
}