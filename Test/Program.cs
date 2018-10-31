using GameCore.Model;
using ILibrary.Maths.Geometry2D;

namespace Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            GameSettings gameSettings = new GameSettings(new BacteriumData[1] { new BacteriumData(GameCore.Enums.OwnerType.Enemy, new Transform(1, 1, new Circle(new UnityEngine.Vector2(1, 1), 1)), 3) });
            byte[] buffer = GameSettings.BitConverter.GetBytes(gameSettings);
            gameSettings = GameSettings.BitConverter.GetInstance(buffer);
        }
    }
}