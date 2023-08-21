using SnakeGameV3.Controllers;
using System.Drawing;
using static SnakeGameV3.Config;

internal class Program
{
    private static void Main()
    {
        PrepareConsole();
        GameController controller = new();
        controller.StartGame();
        //Convert1();
    }

    private static void PrepareConsole()
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(ScreenWidth, ScreenHeight);
        Console.SetBufferSize(ScreenWidth, ScreenHeight);
    }

    private static void Convert1()
    {
        string name = "SnakeBody";

        DirectoryInfo directoryInfo = new($"..\\..\\..\\Textures\\{name}.bmp");
        using Bitmap bitmap = (Bitmap)Image.FromFile(directoryInfo.FullName);

        bool[,] pixels = new bool[bitmap.Height, bitmap.Height];

        for (var y = 0; y < pixels.GetLength(0); y++)
        {
            for (var x = 0; x < pixels.GetLength(1); x++)
            {
                Color pixel = bitmap.GetPixel(x, y);

                if (pixel.R > 0 && pixel.B > 0 && pixel.G > 0)
                {
                    pixels[y, x] = true;
                }
            }
        }

        DirectoryInfo saveInfo = new($"..\\..\\..\\Colliders\\Circle.bin");

        byte[] bytes = new byte[sizeof(int) * 2 + pixels.GetLength(0) * pixels.GetLength(1)];

        unsafe
        {
            int* height = stackalloc int[1];
            int* width = stackalloc int[1];

            *height = pixels.GetLength(0);
            *width = pixels.GetLength(1);

            int*[] size =
            {
                height,
                width,
            };

            var stream = new FileStream(saveInfo.FullName, FileMode.OpenOrCreate);

            for (var i = 0; i < size.Length; i++)
            {
                for (var j = 0; j < sizeof(int); j++)
                    bytes[i * sizeof(int) + j] = ((byte*)size[i])[j];
            }

            for (var i = 0; i < pixels.GetLength(0); i++)
                for (var j = 0; j < pixels.GetLength(1); j++)
                    bytes[sizeof(int) * 2 + i * pixels.GetLength(0) + j] = Convert.ToByte(pixels[i, j]);

            stream.Write(bytes);

            stream.Flush();
            stream.Close();
        }
    }
}