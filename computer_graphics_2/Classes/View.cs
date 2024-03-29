﻿using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_graphics2.Classes
{
	public class View
	{
		public void SetupView(int width, int height)
		{
			GL.ShadeModel(ShadingModel.Smooth);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, Bin.X, 0, Bin.Y, -1, 1);
			GL.Viewport(0,0, width, height);
		}

		Color TransferFunction(short value)
		{
			int min = 0;
			int max = 2000;
			int newVal = Clamp((value - min) * 255 / (max - min), 0, 255);
			return Color.FromArgb(255, newVal, newVal, newVal);

		}

		private int Clamp(int v1, int v2, int v3)
		{
			return Math.Max(Math.Max(Math.Min(v1, v3), v2), v3);
		}

		public void DrawQuads(int LayerNumber)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Begin(BeginMode.Quads);
			for(int x_coord = 0; x_coord < Bin.X - 1; x_coord++) 
				for(int y_coord = 0; y_coord < Bin.Y - 1; y_coord++)
				{
					short value;
					//1 вершина
					value = Bin.array[x_coord + y_coord * Bin.X + LayerNumber * Bin.X * Bin.Y];
					GL.Color3(TransferFunction(value));
					GL.Vertex2(x_coord, y_coord);

					//2 вершина
					value = Bin.array[x_coord + (y_coord + 1) * Bin.X + LayerNumber * Bin.X * Bin.Y];
					GL.Color3(TransferFunction(value));
					GL.Vertex2(x_coord, y_coord + 1);

					//3 вершина
					value = Bin.array[x_coord + 1 + y_coord * Bin.X + LayerNumber * Bin.X * Bin.Y];
					GL.Color3(TransferFunction(value));
					GL.Vertex2(x_coord + 1, y_coord +1);

					//4 вершина
					value = Bin.array[x_coord + 1 + (y_coord + 1) * Bin.X + LayerNumber * Bin.X * Bin.Y];
					GL.Color3(TransferFunction(value));
					GL.Vertex2(x_coord + 1, y_coord);

				}
			GL.End();
		}
	}
}
