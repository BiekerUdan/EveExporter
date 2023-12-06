using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EveExporter
{
    public partial class ModelPreview : UserControl
    {
        private GLControl glControl;
        private int _vertexBufferObject;
        private int _programHandle;
        private int _vertexArrayObject;

        public ModelPreview()
        {
            InitializeComponent();

            glControl = new GLControl();
            glControl.Dock = DockStyle.Fill;
            glControl.Load += GlControl_Load;
            glControl.Paint += GlControl_Paint;
            glControl.Resize += GlControl_Resize;
            GLPanel.Controls.Add(glControl);

        }

        private void GlControl_Resize(object? sender, EventArgs e)
        {
            Debug.WriteLine("GL_Control Resize");

            glControl.MakeCurrent();
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            
        }

        private void GlControl_Paint(object? sender, PaintEventArgs e)
        {
            Debug.WriteLine("GL_Control Paint");
            GL.ClearColor(System.Drawing.Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            GL.UseProgram(_programHandle);
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            glControl.SwapBuffers();

        }

        private void GlControl_Load(object? sender, EventArgs e)
        {

            Debug.WriteLine("GL_Control Load");
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            float[] vertices = {
                -0.5f, -0.5f, 0.0f, //Bottom-left vertex
                0.5f, -0.5f, 0.0f, //Bottom-right vertex
                0.0f,  0.5f, 0.0f  //Top vertex
            };

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            string vertexShaderSource = @"
                #version 330 core
                layout (location = 0) in vec3 aPos;
                void main()
                {
                    gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);
                }";

            string fragmentShaderSource = @"
                #version 330 core
                out vec4 FragColor;

                void main()
                {
                    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
                }";


            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int status_code);
            if (status_code != 1)
            {
                Debug.WriteLine(GL.GetShaderInfoLog(vertexShader));
            }


            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);  
            GL.CompileShader(fragmentShader);
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out status_code);
            if (status_code != 1)
            {
                Debug.WriteLine(GL.GetShaderInfoLog(fragmentShader));
            }

            _programHandle = GL.CreateProgram();
            GL.AttachShader(_programHandle, vertexShader);
            GL.AttachShader(_programHandle, fragmentShader);
            GL.LinkProgram(_programHandle);
            GL.GetProgram(_programHandle, GetProgramParameterName.LinkStatus, out status_code);
            if (status_code != 1)
            {
                Debug.WriteLine(GL.GetProgramInfoLog(_programHandle));
            }

            GL.DetachShader(_programHandle, vertexShader);
            GL.DetachShader(_programHandle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);



            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

        }


    }
}
