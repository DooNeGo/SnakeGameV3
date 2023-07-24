using SnakeGameV3.Controllers;
using static SnakeGameV3.Constants.GameConstants;

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
        Console.SetBufferSize(ScreenWidth + GridCellSize, ScreenHeight);
    }
}