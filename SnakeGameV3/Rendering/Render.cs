using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Drawing;

namespace SnakeGameV3.Rendering
{
    internal class Render
    {
        private const string VertexShaderSource = @"
        #version 330 core

        layout (location = 0) in vec3 vectorPosition;
        layout (location = 1) in vec4 vectorColor;

        out vec4 outColor;

        void main()
        {
            outColor = vectorColor;
            gl_Position = vec4(vectorPosition.x, vectorPosition.y, vectorPosition.z, 1.0);
        }";

        private const string FragmentShaderSource = @"
        #version 330 core

        out vec4 FragmentColor;
        in vec4 outColor;

        void main()
        {
            FragmentColor = outColor;
        }";

        private float[] _vertexArray;
            //{
            //    0.0f, 0.0f, 1.0f,
            //    -0.5f, 0.5f, 1.0f,
            //    -0.5f, -0.5f, 1.0f,
            //    0.0f, -0.5f, 1.0f,
            //    0.5f, -0.5f, 1.0f,
            //    0.5f, 0.5f, 1.0f,
            //    -0.5f, 0.5f, 1.0f,
            //};

        private float[] _colorArray;
        //{
        //    0.5f, 0.0f, 0.5f, 1.0f,
        //    1.0f, 0.0f, 0.0f, 1.0f,
        //    0.0f, 1.0f, 0.0f, 1.0f,
        //    0.0f, 0.0f, 1.0f, 1.0f,
        //    0.5f, 0.0f, 0.5f, 1.0f,
        //    1.0f, 0.0f, 0.0f, 1.0f,
        //    0.0f, 1.0f, 0.0f, 1.0f,
        //    1.0f, 0.0f, 0.0f, 1.0f,
        //};

        private uint[] _indexArray;
        //{
        //    0, 1, 2, 3, 4, 5, 6
        //};

        private int _counter = 0;
        private float _aspectRatio;
        private Vector3D<float> _circlePosition = new(0, 0, 1);

        public IWindow ActiveWindow { get; }

        private GL _openGL;
        private IInputContext _input;

        private uint _vao = 0;
        private uint _vbo = 0;
        private uint _ebo = 0;
        private uint _color = 0;
        private uint _shader = 0;

        public Render(int screenWidth, int screenHeight)
        {
            WindowOptions options = WindowOptions.Default;
            options.API = GraphicsAPI.Default;
            options.VideoMode = VideoMode.Default;
            options.WindowState = WindowState.Normal;
            options.IsVisible = true;
            options.Size = new Vector2D<int>(screenWidth, screenHeight);
            options.FramesPerSecond = 60;

            _aspectRatio = (float)screenWidth / screenHeight;

            ActiveWindow = Window.Create(options);

            ActiveWindow.Load += OnLoad;
            ActiveWindow.Update += OnUpdate;
            ActiveWindow.Render += OnRender;
            ActiveWindow.Resize += OnResize;
            ActiveWindow.Closing += OnClosing;

            ActiveWindow.Run();
        }

        private unsafe void OnLoad()
        {
            CreateCircle(_circlePosition, Color.Orange, 0.2f);

            _input = ActiveWindow.CreateInput();
            _openGL = ActiveWindow.CreateOpenGL();

            foreach (IKeyboard keyboard in _input.Keyboards)
            {
                keyboard.KeyDown += OnKeyDown;
            }

            _openGL.ClearColor(Color.DarkSeaGreen);

            _vao = _openGL.GenVertexArray();
            _vbo = _openGL.GenBuffer();
            _ebo = _openGL.GenBuffer();
            _color = _openGL.GenBuffer();

            _openGL.BindVertexArray(_vao);

            _openGL.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
            _openGL.BufferData<float>(BufferTargetARB.ArrayBuffer, GetTransformedVertexArray(), BufferUsageARB.StreamDraw);
            _openGL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, null);
            _openGL.EnableVertexAttribArray(0);

            _openGL.BindBuffer(BufferTargetARB.ArrayBuffer, _color);
            _openGL.BufferData<float>(BufferTargetARB.ArrayBuffer, _colorArray, BufferUsageARB.StaticDraw);
            _openGL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, null);
            _openGL.EnableVertexAttribArray(1);

            _openGL.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
            _openGL.BufferData<uint>(BufferTargetARB.ElementArrayBuffer, _indexArray, BufferUsageARB.StaticDraw);

            uint vertexShader = _openGL.CreateShader(ShaderType.VertexShader);
            uint fragmentShader = _openGL.CreateShader(ShaderType.FragmentShader);
            _openGL.ShaderSource(vertexShader, VertexShaderSource);
            _openGL.ShaderSource(fragmentShader, FragmentShaderSource);
            _openGL.CompileShader(vertexShader);
            _openGL.CompileShader(fragmentShader);

            _shader = _openGL.CreateProgram();
            _openGL.AttachShader(_shader, vertexShader);
            _openGL.AttachShader(_shader, fragmentShader);
            _openGL.LinkProgram(_shader);

            _openGL.DetachShader(_shader, vertexShader);
            _openGL.DetachShader(_shader, fragmentShader);
            _openGL.DeleteShader(vertexShader);
            _openGL.DeleteShader(fragmentShader);

            //ApplyOffset(new Vector2D<float>(0, -0.25f));
        }

        private unsafe void OnRender(double deltaTime)
        {
            _openGL.Clear(ClearBufferMask.ColorBufferBit);
            _openGL.BindVertexArray(_vao);
            _openGL.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
            _openGL.BufferData<float>(BufferTargetARB.ArrayBuffer, GetTransformedVertexArray(), BufferUsageARB.StreamDraw);

            _openGL.UseProgram(_shader);
            _openGL.DrawArrays(PrimitiveType.LineLoop, 0, (uint)_indexArray.Length);
        }

        private unsafe void OnUpdate(double deltaTime)
        {
            _counter++;
            Vector2D<float> offset = new((MathF.Cos(_counter / 15f)) / 50, (MathF.Sin(_counter / 15f)) / 50);
            ApplyOffset(offset);
        }

        private void OnResize(Vector2D<int> screenSize)
        {
            _aspectRatio = (float)screenSize.X / screenSize.Y;
        }

        private void OnClosing()
        {
            _openGL.DeleteVertexArray(_vao);
            _openGL.DeleteBuffer(_vbo);
            _openGL.DeleteBuffer(_ebo);
            _openGL.DeleteBuffer(_color);
            _openGL.DeleteProgram(_shader);
        }

        private void OnKeyDown(IKeyboard keyboard, Key key, int number)
        {
            if (key == Key.Escape)
            {
                ActiveWindow.Close();
            }
        }

        private void ApplyOffset(Vector2D<float> offset)
        {
            for (var i = 0; i < _vertexArray.Length; i += 3)
            {
                _vertexArray[i] += offset.X;
                _vertexArray[i + 1] += offset.Y;
            }
        }

        private float[] GetTransformedVertexArray()
        {
            float[] transformedArray = new float[_vertexArray.Length];
            _vertexArray.CopyTo(transformedArray, 0);

            for (var i = 0; i < transformedArray.Length; i += 3)
            {
                if (_aspectRatio > 1)
                    transformedArray[i] /= _aspectRatio;
                else
                    transformedArray[i + 1] *= _aspectRatio;
            }

            return transformedArray;
        }

        private void CreateCircle(Vector3D<float> circlePosition, Color color, float radius)
        {
            _vertexArray = new float[361 * 3];
            _colorArray = new float[361 * 4];
            _indexArray = new uint[_vertexArray.Length];

            _vertexArray[0] = circlePosition.X;
            _vertexArray[1] = circlePosition.Y;
            _vertexArray[2] = circlePosition.Z;

            var counter = 0;

            for (var i = 3; i < _vertexArray.Length; i += 3, counter++)
            {
                _vertexArray[i] = circlePosition.X + MathF.Cos(counter) * radius;
                _vertexArray[i + 1] = circlePosition.Y + MathF.Sin(counter) * radius;
                _vertexArray[i + 2] = 1;
            }

            for (var i = 0; i < _colorArray.Length; i += 4)
            {
                _colorArray[i] = color.R;
                _colorArray[i + 1] = color.G;
                _colorArray[i + 2] = color.B;
                _colorArray[i + 3] = color.A;
            }

            for (uint i = 0; i < _indexArray.Length; i++)
            {
                _indexArray[i] = i;
            }
        }
    }
}
