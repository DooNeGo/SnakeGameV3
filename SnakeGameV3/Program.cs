﻿using static SnakeGameV3.Config;
using SnakeGameV3.Controllers;

internal class Program
{
    private static void Main()
    {
        PrepareConsole();
        GameController controller = new();
        controller.StartGame();
    }

    private static void PrepareConsole()
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(ScreenWidth, ScreenHeight);
        Console.SetBufferSize(ScreenWidth, ScreenHeight);
    }
}