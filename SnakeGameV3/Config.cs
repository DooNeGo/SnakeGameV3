namespace SnakeGameV3
{
    internal class Config
    {
        public const int ScreenHeight = GridCellHeight * 9;
        public const int ScreenWidth = GridCellWidth * 16;

        public const int GridCellHeight = (int)(ConsoleCharWidth * Scale);
        public const int GridCellWidth = (int)(ConsoleCharHeight * Scale);

        public const int ConsoleCharHeight = 6;
        public const int ConsoleCharWidth = 4;

        public const float Scale = 2.5f;

        public const ConsoleColor SnakeHeadColor = ConsoleColor.DarkYellow;
        public const ConsoleColor SnakeBodyColor = ConsoleColor.Yellow;
        public const ConsoleColor BackgroundColor = ConsoleColor.Black;
        public const ConsoleColor FoodColor = ConsoleColor.DarkRed;
        public const ConsoleColor BoarderColor = ConsoleColor.White;

        public const float SnakeSpeed = 4f;

        public const int FramePerSecond = 60;
        public const double FrameDelay = 1000 / FramePerSecond;
    }
}