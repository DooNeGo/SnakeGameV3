using SnakeGameV3.Controllers;

internal class Program
{
    private static void Main()
    {
        GameController controller = new();
        controller.StartGame();
    }
}