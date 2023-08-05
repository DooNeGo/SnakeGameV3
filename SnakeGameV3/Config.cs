﻿namespace SnakeGameV3
{
    internal class Config
    {
        public const int ScreenHeight = 160;
        public const int ScreenWidth = 480;

        public const int GridCellHeight = 8;
        public const int GridCellWidth = 12;

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