using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiDiagram
{
	public class Voronoi
	{
		public Voronoi(List<Point> pointList, double xSize, double ySize, double precision) {
			XSize = xSize;
			YSize = ySize;
			Precision = precision;

			// 优先队列，制定排序规则：Y 小先出队，Y 相等X 小先出队（从上到下进行扫描）
			priorityQueue = new PriorityQueue<IEvent>(
				(e1, e2) => {
					if (e1.QueryPoint().Y < e2.QueryPoint().Y) return -1;
					if (e1.QueryPoint().Y > e2.QueryPoint().Y) return 1;
					if (e1.QueryPoint().X < e2.QueryPoint().X) return -1;
					if (e1.QueryPoint().X > e2.QueryPoint().X) return 1;
					return 0;
				}	
			);

			foreach (var point in pointList) {
				// 根据初始点集创建相应的站点事件加入优先队列
				IEvent e = new SiteEvent(point.X, point.Y);
				priorityQueue.Push(e);

				Sites.Add(e.QueryPoint());

				// 创建维诺图结构体
				faces[e.QueryPoint()] = new Face(e.QueryPoint());
			}

			// 开始构建维诺图
			Build();
		}

		private void Build() {

			// 优先队列非空时，取出事件进行处理（站点事件或圆事件）
			while (!priorityQueue.Empty()) {
				var e = priorityQueue.Pop();
				if (e is SiteEvent) {
					ProcSiteEvent(e as SiteEvent);
				} else if (e is CircleEvent) {
					ProcCircleEvent(e as CircleEvent);
				} else {
					throw new Exception("...");
				}
			}

			// 处理排序二叉树中剩余的交点
			Finish();
		}

		private void ProcSiteEvent(SiteEvent e) {
			if (tree.Empty()) {
				// 树为空时，直接添加成第一个结点
				IData data = new ArcData(e.QueryPoint(), null, false);
				tree.AddFirstArc(data);
			} else {
				// 先找出站点正上方的弧
				ArcData oldArc = tree.GetAboveArc(e.QueryPoint());

				// 正上方的弧将被拆成两段，所以肯定会破坏其的圆事件
				if (oldArc.CircleEvent != null) {
					oldArc.CircleEvent.Deleted = true;
					oldArc.CircleEvent = null;
				}

				//if (oldArc.A.Y == e.QueryPoint().Y) {
				//	// 可以选择不要这个分支，直接统一走下面那个
				//	// 这样的话，计算交点的方法需要做特殊处理

				//	// 如果正上方弧的 y 坐标和新站点的 y 坐标一致
				//	// 则是一条弧分裂成两条弧，将原先的弧删掉，新增一个内部结点和两个叶子结点
				//	var intersection = new IntersectionData(oldArc.A, e.QueryPoint(), oldArc.Fa, oldArc.IsLeft(), tree);
				//	var arc01 = new ArcData(oldArc.A, intersection, true);
				//	var arc02 = new ArcData(e.QueryPoint(), intersection, false);

				//	// 维护双向链表
				//	arc01.Prev = oldArc.Prev;
				//	arc02.Prev = arc01;
				//	if (oldArc.Next != null) {
				//		oldArc.Next.Prev = arc02;
				//	}

				//	if (oldArc.Prev != null) {
				//		oldArc.Prev.Next = arc01;
				//	}
				//	arc01.Next = arc02;
				//	arc02.Next = oldArc.Next;

				//	arc01.S0 = oldArc.S0;

				//	// 交点目前坐标，用于创建维诺图边
				//	var p = intersection.CalcIntersection(e.QueryPoint().Y);

				//	// 交点计算
				//	var seg = new Segment(p, QueryFace(e.QueryPoint()), QueryFace(oldArc.A));
				//	arc01.S1 = seg;
				//	arc02.S0 = seg;

				//	FixCircleEvent(arc01);
				//} else {
				
					// 正常情况下，正上方的弧会被新增弧分割成两段
					// 将原先的弧删掉，新增两个内部结点和三个叶子结点
					var intersection01 = new IntersectionData(oldArc.A, e.QueryPoint(), oldArc.Fa, oldArc.IsLeft(), tree);
					var intersection02 = new IntersectionData(e.QueryPoint(), oldArc.A, intersection01, false, tree);
					var arc01 = new ArcData(oldArc.A, intersection01, true);
					var arc02 = new ArcData(e.QueryPoint(), intersection02, true);
					var arc03 = new ArcData(oldArc.A, intersection02, false);

					// 维护双向链表
					arc01.Prev = oldArc.Prev;
					arc02.Prev = arc01;
					arc03.Prev = arc02;
					if (oldArc.Next != null) {
						oldArc.Next.Prev = arc03;
					}

					if (oldArc.Prev != null) {
						oldArc.Prev.Next = arc01;
					}
					arc01.Next = arc02;
					arc02.Next = arc03;
					arc03.Next = oldArc.Next;

					// 原弧分裂出来的两段弧分别继承S0、S1
					arc01.S0 = oldArc.S0;
					arc03.S1 = oldArc.S1;

					// 交点目前坐标，用于创建维诺图边
					// 目前两个交点应该是重叠的，由这两个点勾勒出同一条维诺图边
					var p = intersection01.CalcIntersection(e.QueryPoint().Y);

					var seg = new Segment(p, QueryFace(e.QueryPoint()), QueryFace(oldArc.A));
					arc01.S1 = seg;
					arc02.S0 = seg;

					seg = new Segment(p, QueryFace(e.QueryPoint()), QueryFace(oldArc.A));
					arc02.S1 = seg;
					arc03.S0 = seg;
					
					FixCircleEvent(arc01);
					FixCircleEvent(arc03);
			//	}
			}
		}

		// 检查是否可能发生圆事件，arc 为新增的弧
		private void FixCircleEvent(ArcData arc) {
			if (arc == null || arc.Prev == null || arc.Next == null) {
				// 凑不齐三个焦点
				return ;
			}

			if (arc.CircleEvent != null) {
				arc.CircleEvent.Deleted = true;
				arc.CircleEvent = null;
			}

			Point a = arc.Prev.A;
			Point b = arc.A;
			Point c = arc.Next.A;

			Point center;	// 圆心
			Point bottom;	// 圆最低点
			
			// 一般来说，三点肯定可以算出一个圆，除非三点共线
			if (GenerateCircle(a, b, c, out center, out bottom)) {
				// 并不是所有圆都作数，因为后期一个焦点并不只对应一段弧，可能被分割成好几段
				// 只有当两个交点重合时，才当作是圆事件

				// 这里可以去树里面找出两个交点的结点
				// 也可以直接创建个新的直接算
				var temp01 = new IntersectionData(a, b);
				var temp02 = new IntersectionData(b, c);

				// 重合是在扫描线扫到圆最低点时才发生，所以这里传的参数应该是最低点坐标
				var l = temp01.CalcIntersection(bottom.Y);
				var r = temp02.CalcIntersection(bottom.Y);

				// 判断l、r 两点是否重合，这里应该是造成较大误差的地方，可以想办法改进判断方法
				if (EqualTo(l, r))
				{
					// 产生一个圆事件，加入优先队列
					arc.CircleEvent = new CircleEvent(center, bottom, arc);
					priorityQueue.Push(arc.CircleEvent);
				}
			}
		}

		private void ProcCircleEvent(CircleEvent e) {
			if (e.Deleted) {
				return ;
			}

			// 发生圆事件，说明有一段弧缩小成一个点，所以要将该弧删除
			var arc = e.Arc;

			// arc 被删除，其前置弧和后置弧相交，需要新生成一条边
			Segment seg = null;
			if (arc.Prev != null && arc.Next != null) {
				seg = new Segment(e.Center, QueryFace(arc.Prev.A), QueryFace(arc.Next.A));
			} else {
				throw new Exception("...");
			}

			// 1、维护双向链表
			// 2、弧删除，需要检查前后弧是否有圆事件，如果有，需要置为无效
			if (arc.Prev != null) {
				if (arc.Prev.CircleEvent != null) {
					arc.Prev.CircleEvent.Deleted = true;
					arc.Prev.CircleEvent = null;
				}
				arc.Prev.Next = arc.Next;
				arc.Prev.S1 = seg;
			}
			if (arc.Next != null) {
				if (arc.Next.CircleEvent != null) {
					arc.Next.CircleEvent.Deleted = true;
					arc.Next.CircleEvent = null;
				}
				arc.Next.Prev = arc.Prev;
				arc.Next.S0 = seg;
			}

			// 弧删除时，要顺带删除父亲结点，然后兄弟结点替代父亲结点
			// 发生圆事件时，弧的父亲结点一定不会空
			var brother = arc.IsLeft() ? arc.Fa.R : arc.Fa.L;

			// 替换 Arc 的弧
			var otherArc= arc.IsLeft() ? tree.FindMin(brother) : tree.FindMax(brother);

			brother.Fa = arc.Fa.Fa;
			if (arc.Fa.Fa != null) {
				if (arc.Fa.IsLeft()) {
					arc.Fa.Fa.L = brother;
				} else {
					arc.Fa.Fa.R = brother;
				}
			}
			else {
				throw new Exception("...");
			}

			// Arc 删除，需要更新其祖先的A、B值
			var curr = brother;
			while (curr.Fa != null) {
				if (curr.IsLeft()) {
					(curr.Fa as IntersectionData).A = tree.FindMax(curr).A;
				} else {
					(curr.Fa as IntersectionData).B = tree.FindMin(curr).A;
				}
				curr = curr.Fa;
			}

			if (arc.S0 != null) {
				FinishSegment(arc.S0, e.Center);
			}

			if (arc.S1 != null) {
				FinishSegment(arc.S1, e.Center);
			}

			// 重新检查是否发生圆事件
			FixCircleEvent(otherArc.Next);
			FixCircleEvent(otherArc);
			FixCircleEvent(otherArc.Prev);
		}

		// 完成线段
		private void FinishSegment(Segment seg, Point end)
		{
			if (seg.Done) {
				return ;
			}

			seg.End = end;
			seg.Done = true;

			// 将线段限制在边界内（由于计算时用double，绘图用float，不限制画图时可能越界）
			if (!FixSegment(seg)) {
				// 进入该分支说明整条边都不在边界内，但由于边可表示相邻关系，对于绘画德洛内三角网有用，所以保留该边
				seg.Start = new Point(0, 0);
				seg.End = new Point(0, 0);
			}

			seg.F1.Segments.Add(seg);
			seg.F2.Segments.Add(seg);
			segments.Add(seg);
		}

		// 判断两点是否相等
		private bool EqualTo(Point a, Point b) {
			return LessTo(a.X, b.X) && LessTo(b.X, a.X)
				&& LessTo(a.Y, b.Y) && LessTo(b.Y, a.Y);
		}

		private bool LessTo(double x, double y) {
			// 由于存在计算误差，所以这里不能直接 x < y
			if (x < y + Precision) {
				return true;
			} 
			return false;
		}

		// 判断是否可以生成圆，如果可以，将计算出圆心和圆最低点
		private bool GenerateCircle(Point a, Point b, Point c, out Point center, out Point bottom) {
			center = new Point();
			bottom = new Point();
			if ((a.X - b.X) * (c.Y - b.Y) - (c.X - b.X) * (a.Y - b.Y) == 0)
			{
				return false;
			}

			center.X = a.X * a.X * (b.Y - c.Y) + b.X * b.X * (c.Y - a.Y) + c.X * c.X * (a.Y - b.Y) - (a.Y - b.Y) * (b.Y - c.Y) * (c.Y - a.Y);
			center.X = center.X / (2 * (a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y)));

			center.Y = (a.X - b.X) * (b.X - c.X) * (c.X - a.X) - a.Y * a.Y * (b.X - c.X) - b.Y * b.Y * (c.X - a.X) - c.Y * c.Y * (a.X - b.X);
			center.Y = center.Y / (2 * (a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y)));

			double r = Math.Sqrt((center.X - a.X) * (center.X - a.X) + (center.Y - a.Y) * (center.Y - a.Y));

			bottom.X = center.X;
			bottom.Y = center.Y + r;

			return true;
		}

		private void Finish() {
			// 找出一条扫描线，保证抛物线交点一定在边框外
			var y = XSize + YSize;

			// 先找到双向链表头
			var curr = tree.FindMin();

			// 遍历双向链表
			while (curr != null) {
				if (curr.Next != null) {
					var intersection = new IntersectionData(curr.A, curr.Next.A);
					if (curr.S1 != null) {
						FinishSegment(curr.S1, intersection.CalcIntersection(y));
					}
				}
				curr = curr.Next;
			}
		}

		// 和维诺图关系不大，画图所需，可以先忽略
		// 该函数只是用于将边限制在(0, 0)到(XSize, YSize)的边界内
		private bool FixSegment(Segment seg) {
			var a = new Point(seg.Start.X, seg.Start.Y);
			var b = new Point(seg.End.X, seg.End.Y);

			// a、b 排序，X 小靠前，X 相等Y 小靠前
			if (a.X > b.X) {
				var tmp = a;
				a = b;
				b = tmp;
			} else if (a.X == b.X && a.Y > b.Y) {
				var tmp = a;
				a = b;
				b = tmp;
			}

			// 竖线 
			if (a.X == b.X) {
				if (a.X < 0 || a.X >= XSize || a.Y >= YSize || b.Y < 0) {
					// 整条边都不在边界内
					return false;
				}
				else {
					if (!IsValid(a)) {
						a.Y = 0;
					}
					if (!IsValid(b)) {
						b.Y = YSize - 1;
					}
					return true;
				}
			}
			else {
				var k = (b.Y - a.Y) / (b.X - a.X);
				int a1, a2 = -1;
				int b1, b2 = -1;
				if (k == 0) {
					if (a.Y < 0 || a.Y >= XSize) {
						return false;
					}
					a1 = 2;
					b1 = 3;
				} else if (k < 0) {
					if (a.Y < 0 || a.X >= XSize || b.Y >= YSize || b.X < 0) {
						return false;
					}
					// 从左下角到右上角
					a1 = 2;
					a2 = 1;
					b1 = 0;
					b2 = 3;
				} else {
					if (a.Y >= YSize || a.X >= XSize || b.Y < 0 || b.X < 0) {
						return false;
					}
					// 从左上角到右下角
					a1 = 0;
					a2 = 2;
					b1 = 3;
					b2 = 1;
				}

				if (!IsValid(a)) {
					if (!FixPoint(a, k, a1)) {
						if (!FixPoint(a, k, a2)) {
							return false;
						}
					}
				}

				if (!IsValid(b)) {
					if (!FixPoint(b, k, b1)) {
						if (!FixPoint(b, k, b2)) {
							return false;
						}
					}
				}

				seg.Start = a;
				seg.End = b;
				return true;
			}
        }

		// 和维诺图关系不大，可以先忽略
		// dir 如下：
		//	0 
		// 2 3
		//	1
		private bool FixPoint(Point p, double k, int dir) {
			if (dir == -1) {
				return false;
			}
			// y = kx + c
			// 先算出 c
			var c = p.Y - k * p.X;
			double x = 0;
			double y = 0;
			if (dir == 0) {
				y = 0;
				x = -c / k;
			} else if (dir == 1) {
				y = YSize - 1;
				x = (y - c) / k;
			} else if (dir == 2) {
				x = 0;
				y = c;
			} else if (dir == 3) {
				x = XSize - 1;
				y = (XSize - 1) * k + c;
			}

			if (x < 0 || x >= XSize || y < 0 || y >= YSize) {
				return false;
			}

			p.X = x;
			p.Y = y;
			return true;
		}

		// 点是否处在边界内
		private bool IsValid(Point p) {
			return 0 <= p.X && p.X < XSize && 0 <= p.Y && p.Y < YSize;
		}

		public Face QueryFace(Point site) {
			return faces[site];
		}

		public List<Segment> QuerySegments() {
			return segments;
		}

		public List<Point> Sites = new List<Point>();

		private PriorityQueue<IEvent> priorityQueue;
		private SortedTree tree = new SortedTree();
		private Dictionary<Point, Face> faces = new Dictionary<Point, Face>();
		private List<Segment> segments = new List<Segment>();

		public double XSize { get; private set; }
		public double YSize { get; private set; }
		public double Precision { get; private set; }
	}
}













