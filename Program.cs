using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Input;

namespace Solar_system
{
    internal class Program
    {
        // Game window and transformation variables
        static GameWindow window;
        static double th0 = 0.0;
        static double th1 = 0.0;
        static double th2 = 0.0;
        static double th3 = 0.0;
        static double th4 = 0.0;
        static bool show = true;
        static bool stop = false;
        static bool angle = false;
        static bool speedUp = false;
        static int[] textures = new int[10]; // Array to hold texture IDs

        // Main entry point for the application
        static void Main(string[] args)
        {
            window = new GameWindow(1000, 1000);
            window.Load += loaded;  // Event handler for window load
            window.Resize += resizeF;  // Event handler for window resize
            window.RenderFrame += renderFrame;  // Event handler for rendering frames
            window.KeyPress += KeyPress;  // Event handler for key presses
            window.KeyDown += KeyDown;  // Event handler for key down events
            window.KeyUp += KeyUp;  // Event handler for key up events

            window.Run(200);  // Run the OpenGL application
        }

        // Resize event handler to adjust projection matrix when window is resized
        static void resizeF(object o, EventArgs e)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 matrix = Matrix4.CreatePerspectiveFieldOfView(1.5f, window.Width / window.Height, 1.5f, 200.0f);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);
            window.SwapBuffers();
        }

        // Render the solar system in each frame
        static void renderFrame(object o, EventArgs e)
        {
            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (angle == true) // Adjust view angle
            {
                GL.Translate(5.0, -10.0, 0.0);
                GL.Rotate(10.0, 1.0, 0.0, 1.0); // Apply rotation for better view
            }

            // Drawing the Sun and planets
            GL.PushMatrix();
            GL.Translate(0.0, -5.0, -65.0);
            GL.Rotate(th0, 7.0, 1.0, 5.0);
            drawSphere(8.0, 30, 30,1);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0.0, 7.0, -60);
            GL.Rotate(th0, 0.0, 1.0, 0.0);
            Marcury(15);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0.0, 7.0, -60);
            GL.Rotate(th1, 0.0, 1.0, 0.0);
            venus(20);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0.0, 7.0, -60);
            GL.Rotate(th2, 0.0, 1.0, 0.0);
            Earth(25);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0.0, 7.0, -60);
            GL.Rotate(th3, 0.0, 1.0, 0.0);
            Mars(30);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0.0, 7.0, -60);
            GL.Rotate(th4, 0.0, 1.0, 0.0);
            jupiter(35);
            GL.PopMatrix();

            // Background rendering
            GL.PushMatrix();
            GL.Translate(0.0, 7.0, -60);
            Background();
            GL.PopMatrix();

            // Adjust rotation speeds if speedUp is active
            if (speedUp == true)
            {
                th0 *= 2;
                th1 *= 2;
                th2 *= 2;
                th3 *= 2;
                th4 *= 2;
            }

            UpdateAngles(); // Update angles of rotation for planets

            window.SwapBuffers(); // Swap buffers to display the current frame

            // System.Threading.Thread.Sleep(5);
           
        }

        // Load textures for the planets and background
        static void loaded(object o, EventArgs e)
        {
            
            GL.Enable(EnableCap.DepthTest); // Enable depth testing for 3D rendering

            GL.Enable(EnableCap.Texture2D); // Enable 2D texturing
            GL.GenTextures(6, textures); // Generate texture IDs for planets and background

            // Load textures for each planet and background
            LoadTexture("galaxy.jpg", 0);  // Background texture
            LoadTexture("8k_sun.jpg", 1);  // Sun texture
            LoadTexture("earth2k.jpg", 2);  // Earth texture
            LoadTexture("2k_mars.jpg", 3);  // Mars texture
            LoadTexture("2k_mercury.jpg", 4);  // Mercury texture
            LoadTexture("venus.jpg", 5);  // Venus texture
            LoadTexture("2k_jupiter.jpg", 6);  // jupiter texture

            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);  // Set background color
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);  // Set blending mode
        }

        // Helper function to load a texture from a file and bind it
        static void LoadTexture(string filePath, int textureIndex)
        {
            Bitmap bmp = new Bitmap(filePath);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpdata = bmp.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.BindTexture(TextureTarget.Texture2D, textures[textureIndex]);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, bmpdata.Width, bmpdata.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, bmpdata.Scan0);
            bmp.UnlockBits(bmpdata);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);  // Generate mipmaps for better texture filtering
        }

        // Update angles of rotation for all planets
        static void UpdateAngles()
        {
            if (!stop)
            {
                th0 += 1.5;
                th1 += 1.2;
                th2 += 0.9;
                th3 += 0.6;
                th4 += 0.4;
            }

            // Keep the angles within 0-360 degrees
            th0 %= 360;
            th1 %= 360;
            th2 %= 360;
            th3 %= 360;
            th4 %= 360;
        }

        // Key press event to toggle planet visibility
        static void KeyPress(object o, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'q')
            {
                show = !show;
            }
        }

        // Key down event to control animation behaviors
        static void KeyDown(object o,KeyboardKeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                stop = !stop; // Toggle animation stop
            }
            if (e.Key == Key.Tab)
            {
                angle = !angle; // Toggle camera angle
            }
        }

        // Key up event to control speed-up functionality
        static void KeyUp(object o, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.ShiftLeft)
            {
                speedUp = !speedUp;
            }
        }

        // Function to draw a sphere with given parameters (radius, latitude, longitude, texture ID)
        static void drawSphere(double r, int latitude, int longitude,int t)
        {
            //latitude is the number of stacks(num of horizontal lines)
            //longitude is the number of sectors(num of vertical lines)
            int i, j;
            for (i = 0; i <= latitude; i++)
            {
                double lat0 = Math.PI * (-0.5 + (double)(i - 1) / latitude);
                double z0 = Math.Sin(lat0);
                double zr0 = Math.Cos(lat0);
                double lat1 = Math.PI * (-0.5 + (double)i / latitude);
                double z1 = Math.Sin(lat1);
                double zr1 = Math.Cos(lat1);
                GL.BindTexture(TextureTarget.Texture2D, textures[t]);
                GL.Begin(PrimitiveType.QuadStrip);
                for (j = 0; j <= longitude; j++)
                {
                    double lng = 2 * Math.PI * (double)(j - 1) / longitude;
                    double x = Math.Cos(lng);
                    double y = Math.Sin(lng);
                    
                    GL.TexCoord2(lng / (2.0f * Math.PI), lat1 / Math.PI);
                   
                    GL.Vertex3(r * x * zr0, r * y * zr0, r * z0);
                    
                    GL.TexCoord2(lng / (2.0f * Math.PI), lat1 / Math.PI + 1.0f / latitude);
                    GL.Vertex3(r * x * zr1, r * y * zr1, r * z1);
                }
                GL.End();
            }
        }

        static void Marcury(int r)
        {
            int xc = 0, zc = 0;
            GL.PushMatrix();
            GL.Translate(0.0, -10.0, -15.0);
            GL.Rotate(th0, 1.0, 0.0, 0.0);
            drawSphere(0.6, 20, 20, 4);
            GL.PopMatrix();
            if (show == true)
            {
            GL.Begin(PrimitiveType.Points);

            for (double theta = 0; theta < 360; theta += 0.01)
            {
                double x = r * Math.Cos(theta);
                double z = r * Math.Sin(theta);
                double zz = zc + z;
                GL.Vertex3(xc + x, -10.0, zz);
            }
            GL.End();
            }

        }
        static void venus(int r)
        {
            int xc = 0, zc = 0;

            GL.PushMatrix();
            GL.Translate(0.0, -10.0, -20.0);
            GL.Rotate(th1, 1.0, 0.0, 0.0);
            drawSphere(0.9, 20, 20, 5);
            GL.PopMatrix();
            if (show == true)
            {
            GL.Begin(PrimitiveType.Points);

            for (double theta = 0; theta < 360; theta += 0.01)
            {
                double x = r * Math.Cos(theta);
                double z = r * Math.Sin(theta);
                double zz = zc + z;
                GL.Vertex3(xc + x, -10.0, zz);
            }
            GL.End();
            }


        }
        static void Earth(int r)
        {
            int xc = 0, zc = 0;

            GL.PushMatrix();
            GL.Translate(0.0, -10.0, -25.0);
            GL.Rotate(th2, 1.7, 0.0, 0.0);
            drawSphere(1.1, 20, 20, 2);
            GL.PopMatrix();
            if (show == true)
            {
            GL.Begin(PrimitiveType.Points);
            
            for (double theta = 0; theta < 360; theta += 0.01)
            {
                double x= r * Math.Cos(theta);
                double z= r * Math.Sin(theta);
                double zz = zc + z;
                GL.Vertex3(xc+x, -10.0, zz);
            }
            GL.End();
            }

        }
        static void Mars(int r)
        {
            int xc = 0, zc = 0;
            GL.PushMatrix();
            GL.Translate(0.0, -10.0, -30.0);
            GL.Rotate(th3, 1.0, 0.0, 0.0);
            drawSphere(1.0, 20, 20, 3);
            GL.PopMatrix();
            if (show == true)
            {
            GL.Begin(PrimitiveType.Points);

            for (double theta = 0; theta < 360; theta += 0.01)
            {
                double x = r * Math.Cos(theta);
                double z = r * Math.Sin(theta);
                double zz = zc + z;
                GL.Vertex3(xc + x, -10.0, zz);
            }
            GL.End();
            }
        }
        static void jupiter(int r)
        {
            int xc = 0, zc = 0;
            GL.PushMatrix();
            GL.Translate(0.0, -10.0, -35.0);
            GL.Rotate(th4, 1.0, 0.0, 0.0);
            drawSphere(2.0, 20, 20, 6);
            GL.PopMatrix();
            if (show == true)
            {
            GL.Begin(PrimitiveType.Points);

            for (double theta = 0; theta < 360; theta += 0.01)
            {
                double x = r * Math.Cos(theta);
                double z = r * Math.Sin(theta);
                double zz = zc + z;
                GL.Vertex3(xc + x, -10.0, zz);
            }
            GL.End();
            }
        }
        static void Background()
        {
            GL.BindTexture(TextureTarget.Texture2D, textures[0]);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex3(-190, -190, -100.0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(190, -190, -100.0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(190, 190, -100.0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(-190, 190, -100.0);
            GL.End();
        }

    }
}
