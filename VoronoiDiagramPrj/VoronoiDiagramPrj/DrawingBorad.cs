using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace VoronoiDiagram
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			HandleInput(sender, e);
		}

		private int xSize;
		private int ySize;
		private double precision;
		private List<Point> pointList = new List<Point>();
		private void HandleInput(object sender, EventArgs e) {

			pointList.Clear();

			var seed = (int) numericUpDown1.Value;
			xSize	 = (int) numericUpDown2.Value;
			ySize	 = (int) numericUpDown3.Value;
			var cnt  = (int) numericUpDown4.Value;
			precision= Math.Pow(10, - (int) numericUpDown5.Value);

			Random rd = new Random(seed);
			for (int i = 0; i < cnt; ++i)
			{
				var x = rd.NextDouble() * xSize;
				var y = rd.NextDouble() * ySize;
				pointList.Add(new Point(x, y));
			}

			Fresh();
		}

		private void Fresh() {

			// 调整窗口大小
			Size = new Size(xSize + ControlPanel.Size.Width + Size.Width - ClientSize.Width, ySize + Size.Height - ClientSize.Height);

			// 刷新画板
			DrawingBroad.Image = new Bitmap(xSize, ySize);

			// 初始点集
			foreach (var point in pointList) {
				DrawPoint((float)point.X, (float)point.Y, Color.Blue);
			}

            Voronoi v = new Voronoi(pointList, xSize, ySize, precision);
			// 画维诺图
			var segments = v.QuerySegments();
			foreach (var seg in segments)
			{
				DrawLine(seg, Color.Black);
			}
			// 画三角网
			//foreach (var site in v.Sites)
			//{
			//	var face = v.QueryFace(site);
			//	foreach (var seg in face.Segments)
			//	{
			//		var edge = new Segment(seg.F1.Site, seg.F2.Site);
			//		DrawLine(edge, Color.Red);
			//	}
			//}
		}

		private void DrawPoint(float x, float y, Color c)
		{
			var g = Graphics.FromImage(DrawingBroad.Image);
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			g.FillEllipse(new SolidBrush(c), x - 1.5f, y - 1.5f, 3, 3);
		}

		private void DrawLine(Segment seg, Color c) {
			var g = Graphics.FromImage(DrawingBroad.Image);
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			g.DrawLine(new Pen(c), new PointF((float)seg.Start.X, (float)seg.Start.Y), new PointF((float)seg.End.X, (float)seg.End.Y));
		}
	}
}










