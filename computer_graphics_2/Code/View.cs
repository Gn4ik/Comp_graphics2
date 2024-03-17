using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computer_graphics_2.Code
{
	public class View
	{

		int VBOtexture;
		Bitmap TextureImage;

		public void SetupView(int width, int height)
		{
			GL.ShadeModel(ShadingModel.Smooth);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, Bin.X, 0, Bin.Y, -1, 1);
			GL.Viewport(0,0, width, height);
		}

		private Color TransferFunction(short value, int MinValue, int WidthTF)
		{
			int min = MinValue;
			int max = WidthTF + min;
			int newVal = Clamp((value - min) * 255 / (max - min), 0, 255);
			return Color.FromArgb(255, newVal, newVal, newVal);

		}

		private int Clamp(int val, int min, int max)
		{
			if (val < min) { return min; }
			if (val > max) { return max; }
			return val;
		}

		public void DrawQuads(int LayerNumber, int MinValue, int WidthTF)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Begin(BeginMode.QuadStrip);

			for (int y_coord = 0; y_coord < Bin.Y - 1; y_coord++)
			{
				if (LayerNumber == Bin.Z) LayerNumber--;

				for (int x_coord = 0; x_coord < Bin.X; x_coord++)
				{
					short value1;
					value1 = Bin.array[x_coord + y_coord * Bin.X + LayerNumber * Bin.X * Bin.Y];
					GL.Color3(TransferFunction(value1, MinValue, WidthTF));
					GL.Vertex2(x_coord, y_coord);

					value1 = Bin.array[x_coord + (y_coord + 1) * Bin.X + LayerNumber * Bin.X * Bin.Y];
					GL.Color3(TransferFunction(value1, MinValue, WidthTF));
					GL.Vertex2(x_coord, y_coord + 1);
				}

				short value = Bin.array[(Bin.X - 1) + y_coord * Bin.X + LayerNumber * Bin.X * Bin.Y];
				GL.Color3(TransferFunction(value, MinValue, WidthTF));
				GL.Vertex2(Bin.X - 1, y_coord);

				value = Bin.array[(Bin.X - 1) + (y_coord + 1) * Bin.X + LayerNumber * Bin.X * Bin.Y];
				GL.Color3(TransferFunction(value, MinValue, WidthTF));
				GL.Vertex2(Bin.X - 1, y_coord + 1);
			}

			GL.End();
		}

		public void Load2DTexture()
		{
			GL.BindTexture(TextureTarget.Texture2D, VBOtexture);
			BitmapData data = TextureImage.LockBits(
				new System.Drawing.Rectangle(0, 0, TextureImage.Width, TextureImage.Height),
				ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data .Height, 
				0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

			TextureImage.UnlockBits(data);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			ErrorCode err  = GL.GetError();
			string Error = err.ToString();
		}

		public void GenerateTextureImage(int LayerNumber, int MinValue, int WidthTF)
		{
			TextureImage = new Bitmap(Bin.X, Bin.Y);
			for(int i = 0; i < Bin.X; i++)
				for(int j = 0; j < Bin.Y; j++)
				{
					int PixelNumber = i + j * Bin.X + LayerNumber * Bin.X * Bin.Y;
					TextureImage.SetPixel(i, j, TransferFunction(Bin.array[PixelNumber], MinValue, WidthTF));
				}
		}

		public void DrawTexture()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(TextureTarget.Texture2D, VBOtexture);

			GL.Begin(BeginMode.Quads);
			GL.Color3(Color.White);
			GL.TexCoord2(0f, 0f);
			GL.Vertex2(0, 0);

			GL.TexCoord2(0f, 1f);
			GL.Vertex2(0, Bin.Y);

			GL.TexCoord2(1f, 1f);
			GL.Vertex2(Bin.X, Bin.Y);

			GL.TexCoord2(1f, 0f);
			GL.Vertex2(Bin.X, 0);

			GL.End();

			GL.Disable(EnableCap.Texture2D);
		}
	}
}
