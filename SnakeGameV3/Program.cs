using SnakeGameV3.Controllers;
using static SnakeGameV3.Config;

internal class Program
{
    private static void Main()
    {
        while (true)
        {
            PrepareConsole();
            Game game = new();
            game.Start();
            Console.Clear();
        }
    }

    private static void PrepareConsole()
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(ScreenWidth, ScreenHeight);
        Console.SetBufferSize(ScreenWidth, ScreenHeight);
    }
}