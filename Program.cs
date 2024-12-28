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
        static int[] textures = new int[10];
        static void Main(string[] args)
        {
            window = new GameWindow(1000, 1000);
            window.Load += loaded;
            window.Resize += resizeF;
            window.RenderFrame += renderFrame;
            window.KeyPress += KeyPress;
            window.KeyDown += KeyDown;
            window.KeyUp += KeyUp;
            
            window.Run(200);
        }
        static void resizeF(object o, EventArgs e)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 matrix = Matrix4.CreatePerspectiveFieldOfView(1.5f, window.Width / window.Height, 1.5f, 200.0f);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);
            window.SwapBuffers();
        }

        static void renderFrame(object o, EventArgs e)
        {
            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (angle == true)
            {
                GL.Translate(5.0, -10.0, 0.0);
                GL.Rotate(10.0, 1.0, 0.0, 1.0); //for a better point of view.
            }


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

            GL.PushMatrix();
            GL.Translate(0.0, 7.0, -60);
            background();
            GL.PopMatrix();

            if (speedUp == true)
            {
                th0 *= 2;
                th1 *= 2;
                th2 *= 2;
                th3 *= 2;
                th4 *= 2;
            }

            if (stop == false)
                th0 += 1.5;
            if (th0 > 360) th0 -= 360;

            if (stop == false)
                th1 += 1.2;
            if (th1 > 360)  th1 -= 360;

            if (stop == false)
                th2 += 0.9;
            if (th2 > 360) th2 -= 360;

            if (stop == false)
                th3 += 0.6;
            if (th3 > 360) th3 -= 360;

            if (stop == false)
                th4 += 0.4;
            if (th4 > 360) th4 -= 360;

            window.SwapBuffers();
            // System.Threading.Thread.Sleep(5);
        }
        static void loaded(object o, EventArgs e)
        {
            
            GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.Texture2D);
            GL.GenTextures(6, textures);

            //Define the first texture
            GL.BindTexture(TextureTarget.Texture2D, textures[0]);
            Bitmap bmp = new Bitmap("galaxy.jpg");
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpdata = bmp.LockBits(rect, ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb,
            bmpdata.Width, bmpdata.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
            PixelType.UnsignedByte, bmpdata.Scan0);
            bmp.UnlockBits(bmpdata);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            // Define the second texture
            GL.BindTexture(TextureTarget.Texture2D, textures[1]);
            bmp = new Bitmap("8k_sun.jpg");
            rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            bmpdata = bmp.LockBits(rect, ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb,
            bmpdata.Width, bmpdata.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
            PixelType.UnsignedByte, bmpdata.Scan0);
            bmp.UnlockBits(bmpdata);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, textures[2]);
            bmp = new Bitmap("earth2k.jpg");
            rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            bmpdata = bmp.LockBits(rect, ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb,
            bmpdata.Width, bmpdata.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
            PixelType.UnsignedByte, bmpdata.Scan0);
            bmp.UnlockBits(bmpdata);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, textures[3]);
            bmp = new Bitmap("2k_mars.jpg");
            rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            bmpdata = bmp.LockBits(rect, ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb,
            bmpdata.Width, bmpdata.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
            PixelType.UnsignedByte, bmpdata.Scan0);
            bmp.UnlockBits(bmpdata);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, textures[4]);
            bmp = new Bitmap("2k_mercury.jpg");
            rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            bmpdata = bmp.LockBits(rect, ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb,
            bmpdata.Width, bmpdata.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
            PixelType.UnsignedByte, bmpdata.Scan0);
            bmp.UnlockBits(bmpdata);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, textures[5]);
            bmp = new Bitmap("venus.jpg");
            rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            bmpdata = bmp.LockBits(rect, ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb,
            bmpdata.Width, bmpdata.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
            PixelType.UnsignedByte, bmpdata.Scan0);
            bmp.UnlockBits(bmpdata);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, textures[6]);
            bmp = new Bitmap("2k_jupiter.jpg");
            rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            bmpdata = bmp.LockBits(rect, ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb,
            bmpdata.Width, bmpdata.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
            PixelType.UnsignedByte, bmpdata.Scan0);
            bmp.UnlockBits(bmpdata);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            //GL.BindTexture(TextureTarget.Texture2D, textures[7]);
            //bmp = new Bitmap("2k_neptune.jpg");
            //rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            //bmpdata = bmp.LockBits(rect, ImageLockMode.ReadOnly,
            //System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb,
            //bmpdata.Width, bmpdata.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
            //PixelType.UnsignedByte, bmpdata.Scan0);
            //bmp.UnlockBits(bmpdata);
            //GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            //GL.BindTexture(TextureTarget.Texture2D, textures[8]);
            //bmp = new Bitmap("sample_sd.bmp");
            //rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            //bmpdata = bmp.LockBits(rect, ImageLockMode.ReadOnly,
            //System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb,
            //bmpdata.Width, bmpdata.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgr,
            //PixelType.UnsignedByte, bmpdata.Scan0);
            //bmp.UnlockBits(bmpdata);
            //GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            //2k_neptune.jpg

            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

        }
        static void KeyPress(object o, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'q')
            {
                show = !show;
            }
        }
        static void KeyDown(object o,KeyboardKeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                stop = !stop;
            }
            if (e.Key == Key.Tab)
            {
                angle = !angle;
            }
        }
        static void KeyUp(object o, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.ShiftLeft)
            {
                speedUp = !speedUp;
            }
        }
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
        static void background()
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