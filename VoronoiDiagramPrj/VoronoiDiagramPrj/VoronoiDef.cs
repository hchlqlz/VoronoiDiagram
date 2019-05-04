using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiDiagram
{
	public class Point {
		public int ID;
		public double X;
		public double Y;

		public Point(double x, double y, bool mark = false) {
			X = x;
			Y = y;
			if (mark) {
				ID = ++index;
			}
		}

		public Point(bool mark = false) {
			if (mark) {
				ID = ++index;
			}
		}

		private static int index = 0;
	}

	interface IEvent {
		Point QueryPoint();
	}

	// 站点事件
	class SiteEvent : IEvent
	{
		Point site;

		public SiteEvent(double x, double y) {
			site = new Point(x, y, true);
		}

		public Point QueryPoint()
		{
			return site;
		}
	}

	// 圆事件
	class CircleEvent : IEvent
	{
		public bool Deleted;
		public Point Center;
		public ArcData Arc;

		Point bottom;

		public CircleEvent(Point center, Point bottom, ArcData arc) {
			Center = center;
			Arc = arc;
			this.bottom = bottom;
			Deleted = false;
		}

		public Point QueryPoint()
		{
			return bottom;
		}
	}

	// 排序二叉树
	class SortedTree
	{
		public void AddFirstArc(IData data) {
			if (root != null) {
				throw new Exception("...");
			}

			root = data;
		}

		// 获取点正上方的弧，此时扫描线 y 坐标即点的 y 坐标
		public ArcData GetAboveArc(Point point) {
			if (root == null) {
				throw new Exception("...");
			}

			IData curr = root;
			while (curr is IntersectionData) {
				if (point.X < curr.CalcIntersection(point.Y).X) {
					curr = curr.L;
				} else {
					curr = curr.R;
				}
			}

			if (!(curr is ArcData)) {
				throw new Exception("...");
			}

			return curr as ArcData;
		}

		// 找双向链表的表头
		public ArcData FindMin() {
			if (root == null) {
				return null;
			}

			return FindMin(root);
		}

		// 找 data 子树最右边结点
		public ArcData FindMax(IData data) {
			var curr = data;
			while (curr is IntersectionData) {
				curr = curr.R;
			}
			if (!(curr is ArcData)) {
				throw new Exception("...");
			}
			return curr as ArcData;
		}

		// 找 data 子树最左边结点
		public ArcData FindMin(IData data)
		{
			var curr = data;
			while (curr is IntersectionData)
			{
				curr = curr.L;
			}
			if (!(curr is ArcData))
			{
				throw new Exception("...");
			}
			return curr as ArcData;
		}

		public void SetRoot(IData data) {
			root = data;
		}

		// 树是否为空
		public bool Empty() {
			return root == null;
		}

		#region 输出测试接口，与维诺图无关
		private void Print(IData curr)
		{
			if (curr is IntersectionData)
			{
				Console.Write(string.Format("[{0},{1}]", (curr as IntersectionData).A.ID, (curr as IntersectionData).B.ID));
			}
			else
			{
				Console.Write(string.Format("({0})", (curr as ArcData).A.ID));
			}
		}

		private void Print(IData curr, int level)
		{
			if (curr == null)
			{
				return;
			}

			Print(curr.L, level + 1);

			for (int i = 0; i < level; ++i)
			{
				Console.Write("\t");
			}

			Print(curr);
			
			Console.WriteLine();

			Print(curr.R, level + 1);
		}

		public void Print()
		{
			Console.WriteLine("====================");
			Print(root, 0);
			Console.WriteLine("====================");
		}
		#endregion

		private IData root;	// 树根
	}

	// 排序二叉树结点
	abstract class IData {

		// 由于随着扫描线的移动，相邻抛物线的交点也会发生变化，因此不能直接存交点位置，必须用到的时候计算
		// y 是当前扫描线的 y 坐标
		public abstract Point CalcIntersection(double y);

		// 返回自己是否为左孩子
		public bool IsLeft() {
			if (Fa == null) {
				return false;	// 返回值无所谓
			}

			if (Fa.L == this) {
				return true;
			} else if (Fa.R == this) {
				return false;
			} else {
				throw new Exception("...");
			}
		}

		public IData Fa;// 父亲
		public IData L;	// 左孩子
		public IData R;	// 右孩子
	}

	// 排序二叉树内部结点（相邻弧交点）
	class IntersectionData : IData
	{
		public Point A;	// A、B 标识该结点是哪两条抛物线的交点
		public Point B;

		public IntersectionData(Point a, Point b, IData fa, bool left, SortedTree tree) {
			A = a;
			B = b;
			Fa= fa;

			if (fa != null) {
				if (left) {
					fa.L = this;
				} else {
					fa.R = this;
				}
			} else {
				tree.SetRoot(this);
			}
		}

		// 仅用于方便计算交点
		public IntersectionData(Point a, Point b)
		{
			A = a;
			B = b;
		}

		// 只有内部结点才可以调用该方法，返回交点坐标
		public override Point CalcIntersection(double y)
		{
			// 已知两个抛物线的焦点，和准线，可以使用公式求出两抛物线的交点。
			// 不过需要注意一些特殊情况。
			Point res = new Point();

			// 我们先算出 X 坐标，然后代入某条抛物线算出 Y 值
			Point focus = A;

			if (A.Y == B.Y) {
				// 当 A 和 B 在同一水平线上时，由于准线又相同，所以交点一定在A、B 垂直平分线上
				// 不考虑 y == A.Y 导致两抛物线无交点的情况，因为 y 是会不断变化的，所以这里假定交点必然存在
				res.X = (A.X + B.X) / 2;
			} else if (A.Y == y) {
				// 焦点坐标在准线上，这时候的抛物线是一条竖线，所以交点 x 坐标等于焦点 x 坐标
				res.X = A.X;

				// 当 A 点在扫描线上时，A 抛物线是一条竖线，所以得用 B 抛物线才能算出 Y 坐标
				focus = B;
			} else if (B.Y == y) {
				res.X = B.X;
			} else {
				// (x + n)^2 = 2p(y + m)
				// 无特殊情况，则正常计算交点坐标

				double p1 = 2 * (A.Y - y);  // 2p1
				double p2 = 2 * (B.Y - y);  // 2p2

				double a = 1 / p1 - 1 / p2;
				double b = -2 * (A.X / p1 - B.X / p2);
				double c = (A.X * A.X + A.Y * A.Y - y * y) / p1 - (B.X * B.X + B.Y * B.Y - y * y) / p2;

				double o = b * b - 4 * a * c;

				res.X = (-b - Math.Sqrt(o)) / (2 * a);
			}

			// 已知 X 坐标，求 Y 坐标
			if (focus.Y == y)
			{
				// 这里就是 A、B 都在扫描线上的情况，这时，y 坐标返回 0 即可。
				// 此时的 A、B 抛物线其实并没有交点，只有当扫描线继续移动后才会产生交点
				// 并且在刚越过的时候，交点是在 y 无穷小的位置，肯定超过了维诺图的边界，所以 y 坐标返回 0 即可。
				res.Y = 0;
			}
			else
			{
				res.Y = ((focus.X - res.X) * (focus.X - res.X) + focus.Y * focus.Y - y * y) / (2 * focus.Y - 2 * y);
			}

			return res;
		}
	}

	// 排序二叉树叶子结点（弧）
	class ArcData : IData
	{
		// 对应弧的焦点
		public Point A;

		// 圆事件指针，如果圆事件还未发生，该弧已经消失或原先的三元组被破环，则需要将圆事件设置成无效的
		public CircleEvent CircleEvent;

		public ArcData Prev;	// 双向链表，前置弧
		public ArcData Next;	// 双向链表，后置弧

		public Segment S0;		// 与左边弧交点创建的边
		public Segment S1;		// 与右边弧交点创建的边

		public ArcData(Point point, IData fa, bool left) {
			A = point;
			Fa= fa;

			if (fa != null) {
				if (left) {
					fa.L = this;
				} else {
					fa.R = this;
				}
			}
		}

		public override Point CalcIntersection(double y)
		{
			throw new NotImplementedException();
		}
	}

	public class Segment {
		public Face F1;
		public Face F2;
		public Point Start;
		public Point End;
		public bool Done;

		public Segment(Point start, Face f1, Face f2) {
			F1 = f1;
			F2 = f2;
			Start = start;
			Done = false;
		}

		public Segment(Point start, Point end) {
			Start = start;
			End = end;
			Done = true;
		}
	}

	// 面结构体，与初始点一一对应，描述某初始点对应的维诺图信息
	public class Face {
		public Point Site;
		public List<Segment> Segments = new List<Segment>();	// 维诺图边
		
		public Face(Point site) {
			Site = site;
		}
	}
}












