namespace SnakeGameV3
{
    internal class GameConstants
    {
        public const int ScreenHeight = 90;
        public const int ScreenWidth = 160;

        public const int GridCellHeight = 9;
        public const int GridCellWidth = 8;

        public const ConsoleColor SnakeHeadColor = ConsoleColor.DarkYellow;
        public const ConsoleColor SnakeBodyColor = ConsoleColor.Yellow;
        public const ConsoleColor BackgroundColor = ConsoleColor.Black;
        public const ConsoleColor FoodColor = ConsoleColor.DarkRed;
        public const ConsoleColor BoarderColor = ConsoleColor.White;

        public const float SnakeSpeed = 4f;

        public const int FramePerSecond = 60;
        public const double FrameDelay = 1000 / FramePerSecond;

        public const char PixelModel = '█';
    }
}