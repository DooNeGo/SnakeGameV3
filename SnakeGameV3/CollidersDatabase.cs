using SnakeGameV3.Model;

namespace SnakeGameV3
{
    internal enum ColliderType
    {
        Square,
        Circle,
    }

    internal class CollidersDatabase
    {
        private readonly Dictionary<ColliderType, Collider> _colliders = new();
        private readonly Dictionary<ColliderConfig, Collider> _transformedColliders = new();
        private readonly Grid _grid;

        public CollidersDatabase(Grid grid)
        {
            _grid = grid;
            LoadColliders();
        }

        public Collider GetTransformedCollider(ColliderConfig colliderConfig, float scale)
        {
            if (_transformedColliders.ContainsKey(colliderConfig))
                return _transformedColliders[colliderConfig];

            Collider collider = _colliders[colliderConfig.Type];

            int transformedColliderHeight = (int)(_grid.CellSize.Height * scale);
            int transformedColiderWidth = (int)(_grid.CellSize.Width * scale);

            bool[,] transformedBounds = new bool[transformedColliderHeight, transformedColiderWidth];

            int offsetX = collider.Size.Width / transformedColiderWidth;
            int offsetY = collider.Size.Height / transformedColliderHeight;

            for (var y = 0; y < transformedColliderHeight; y++)
            {
                for (var x = 0; x < transformedColiderWidth; x++)
                {
                    bool value = collider.GetValue(x * offsetX, y * offsetY);
                    transformedBounds[y, x] = value;
                }
            }

            Collider transformedCollider = new(transformedBounds);
            _transformedColliders.Add(colliderConfig, transformedCollider);

            return transformedCollider;
        }

        private unsafe void LoadColliders()
        {
            ColliderType[] names = Enum.GetValues<ColliderType>();

            int* height = stackalloc int[1];
            int* width = stackalloc int[1];
            int*[] size =
            {
                height,
                width,
            };

            foreach (ColliderType name in names)
            {
                DirectoryInfo saveInfo = new($"..\\..\\..\\Colliders\\{name}.bin");
                FileStream fileStream = File.OpenRead(saveInfo.FullName);

                for (int i = 0; i < size.Length; i++)
                {
                    for (int j = 0; j < sizeof(int); j++)
                    {
                        ((byte*)size[i])[j] = (byte)fileStream.ReadByte();
                    }
                }

                bool[,] pixels = new bool[*size[0], *size[1]];

                for (var y = 0; y < pixels.GetLength(0); y++)
                {
                    for (var x = 0; x < pixels.GetLength(1); x++)
                    {
                        pixels[y, x] = Convert.ToBoolean(fileStream.ReadByte());
                    }
                }

                fileStream.Close();

                Collider collider = new(pixels);
                _colliders.Add(name, collider);
            }
        }
    }
}