using computer_graphics_2.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace computer_graphics_2
{
	public partial class MainForm : Form
	{
		private int currentLayer;
		private Bin bin = new Bin();
		private bool loaded = false;
		private Code.View view = new Code.View();
		private int FrameCount;
		private DateTime NextFPSUpdate = DateTime.Now.AddSeconds(1);
		private bool needReloaded = false;
		public struct RenderVar
		{
			public bool Quad;
			public bool Texture;
		}
		private RenderVar render;
		private int Minvalue = 1;
		private int widthTF = 1;

		public MainForm()=>
			InitializeComponent();

		private void glControl1_Paint(object sender, PaintEventArgs e)
		{
			if (loaded)
			{
				if (render.Quad)
				{
					view.DrawQuads(currentLayer, Minvalue, widthTF);
					glControl1.SwapBuffers();
					return;
				}
				if (render.Texture)
				{
					if (needReloaded)
					{
						view.GenerateTextureImage(currentLayer, Minvalue, widthTF);
						view.Load2DTexture();
						needReloaded = false;
					}
					view.DrawTexture();
					glControl1.SwapBuffers();
				}
			}
		}

		private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				bin.ReadBIN(dialog.FileName);
				view.SetupView(glControl1.Width, glControl1.Height);
				loaded = true;
				glControl1.Invalidate();
				trackBar1.Maximum = Bin.Z;
			}
		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			currentLayer = trackBar1.Value;
			needReloaded = true;
		}
		
		void Application_Idle(object sender, EventArgs e)
		{
			while (glControl1.IsIdle)
			{
				DisplayFPS();
				glControl1.Invalidate();
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Application.Idle += Application_Idle;
		}

		void DisplayFPS()
		{
			if(DateTime.Now >= NextFPSUpdate)
			{
				this.Text = String.Format("CT Visualizer (fps = {0})", FrameCount);
				NextFPSUpdate = DateTime.Now.AddSeconds(1);
				FrameCount = 0;
			}
			FrameCount++;
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			render.Quad = true;
			render.Texture = false;
		}

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			render.Quad = false;
			render.Texture = true;
		}

		private void задатьTFToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Form newForm = new Form();
			newForm.Width = 400;
			newForm.Height = 200;

			Label minVal = new Label();
			minVal.Top = 20;
			minVal.Left = 10;
			minVal.Width = 100;
			minVal.Text = "Min TF: ";
			newForm.Controls.Add(minVal);

			TrackBar trackBarMinValue = new TrackBar();
			trackBarMinValue.Top = 10;
			trackBarMinValue.Left = minVal.Right;
			trackBarMinValue.Width = newForm.Width - 150;
			trackBarMinValue.Minimum = 1;
			trackBarMinValue.Maximum = 1000;
			newForm.Controls.Add(trackBarMinValue);
			trackBarMinValue.Scroll += MinValue_Scroll;

			Label width = new Label();
			width.Top = trackBarMinValue.Bottom + 20;
			width.Left = 10;
			width.Width = 100;
			width.Text = "Ширина TF: ";
			newForm.Controls.Add(width);

			TrackBar TrackBarWidthTF = new TrackBar();
			TrackBarWidthTF.Top = trackBarMinValue.Bottom + 5;
			TrackBarWidthTF.Left = width.Right;
			TrackBarWidthTF.Width = newForm.Width - 150;
			TrackBarWidthTF.Minimum = 1;
			TrackBarWidthTF.Maximum = 3000;
			newForm.Controls.Add(TrackBarWidthTF);
			TrackBarWidthTF.Scroll += WidthTF_Scroll;

			trackBarMinValue.Tag = "MinValue_Scroll";
			TrackBarWidthTF.Tag = "WidthTF_Scroll";

			newForm.ShowDialog();
		}

		private void WidthTF_Scroll(object sender, EventArgs e)
		{
			TrackBar trackBar = (TrackBar)sender;
			string handlerName = trackBar.Tag.ToString();

			if (handlerName == "WidthTF_Scroll")
			{
				widthTF = trackBar.Value;
			}
		}

		private void MinValue_Scroll(object sender, EventArgs e)
		{
			TrackBar trackBar = (TrackBar)sender;
			string handlerName = trackBar.Tag.ToString();

			if (handlerName == "MinValue_Scroll")
			{
				Minvalue = trackBar.Value;
			}
		}

		//private void MainForm_SizeChanged(object sender, EventArgs e)
		//{

		//	float scaleX = (float)this.ClientSize.Width / initialFormWidth;
		//	float scaleY = (float)this.ClientSize.Height / initialFormHeight;

		//	foreach (Control control in Controls)
		//	{
		//		control.Left = (int)(control.Left * scaleX);
		//		control.Top = (int)(control.Top * scaleY);
		//		control.Width = (int)(control.Width * scaleX);
		//		control.Height = (int)(control.Height * scaleY);
		//	}

		//	initialFormWidth = this.ClientSize.Width;
		//	initialFormHeight = this.ClientSize.Height;
		//}
	}
}
