using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiDiagram
{
	public class PriorityQueue<T>
	{
		public PriorityQueue(Func<T, T, int> compare)
		{
			this.compare = compare;
			elements.Add(default(T));
		}

		public bool Empty()
		{
			return elements.Count <= 1;
		}

		public void Push(T value)
		{
			int index = elements.Count;
			elements.Add(value);

			while (index > 1)
			{
				int fa = index / 2;
				if (compare(elements[fa], value) == 1)
				{
					elements[index] = elements[fa];
				}
				else
				{
					break;
				}

				index = fa;
			}

			elements[index] = value;
		}

		public T Top()
		{
			if (elements.Count <= 1)
			{
				return default(T);
			}

			return elements[1];
		}

		public T Pop()
		{
			if (elements.Count <= 1)
			{
				return default(T);
			}

			T ret = elements[1];
			elements[1] = elements[elements.Count - 1];
			elements.RemoveAt(elements.Count - 1);

			int halfCount = (elements.Count + 1) / 2;

			int index = 1;
			while (index < halfCount)
			{
				int son = index * 2;

				if (elements.Count > son + 1)
				{
					son = compare(elements[son], elements[son + 1]) == 1 ? son + 1 : son;
				}

				if (compare(elements[son], elements[index]) == -1)
				{
					T tmp = elements[son];
					elements[son] = elements[index];
					elements[index] = tmp;
					index = son;
				}
				else
				{
					break;
				}
			}

			return ret;
		}

		private Func<T, T, int> compare;
		private List<T> elements = new List<T>();
	}
}
