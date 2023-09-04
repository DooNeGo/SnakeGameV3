using SnakeGameV3.Controllers;
using static SnakeGameV3.Config;

internal class Program
{
    private static void Main()
    {
        PrepareConsole();
        Game game = new();
        game.Start();
    }

    private static void PrepareConsole()
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(ScreenWidth, ScreenHeight);
        Console.SetBufferSize(ScreenWidth, ScreenHeight);
    }
}