using System;
using System.Collections.Generic;

namespace hwj.CommonLibrary.DataStructure
{
    public class AdjacencyList<T>
    {
        /// <summary>
        /// 图的顶点集合
        /// </summary>
        List<Vertex<T>> items;
        /// <summary>
        /// 构造方法
        /// </summary>
        public AdjacencyList() : this(10) { }
        /// <summary>
        /// 指定容量的构造方法
        /// </summary>
        /// <param name="capacity"></param>
        public AdjacencyList(int capacity)
        {
            items = new List<Vertex<T>>(capacity);
        }
        /// <summary>
        /// 添加一个顶点
        /// </summary>
        /// <param name="item"></param>
        public void AddVertex(T item)
        {   //不允许插入重复值
            if (Contains(item))
            {
                throw new ArgumentException("插入了重复顶点！");
            }
            items.Add(new Vertex<T>(item));
        }
        /// <summary>
        /// 添加无向边
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void AddEdge(T from, T to)
        {
            Vertex<T> fromVer = Find(from); //找到起始顶点
            if (fromVer == null)
            {
                throw new ArgumentException("头顶点并不存在！");
            }
            Vertex<T> toVer = Find(to); //找到结束顶点
            if (toVer == null)
            {
                throw new ArgumentException("尾顶点并不存在！");
            }
            //无向边的两个顶点都需记录边信息
            AddDirectedEdge(fromVer, toVer);
            AddDirectedEdge(toVer, fromVer);
        }
        /// <summary>
        /// 查找图中是否包含某项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            foreach (Vertex<T> v in items)
            {
                if (v.data.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 查找指定项并返回
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Vertex<T> Find(T item)
        {
            foreach (Vertex<T> v in items)
            {
                if (v.data.Equals(item))
                {
                    return v;
                }
            }
            return null;
        }
        /// <summary>
        /// 添加有向边
        /// </summary>
        /// <param name="fromVer"></param>
        /// <param name="toVer"></param>
        private void AddDirectedEdge(Vertex<T> fromVer, Vertex<T> toVer)
        {
            if (fromVer.firstEdge == null) //无邻接点时
            {
                fromVer.firstEdge = new Node(toVer);
            }
            else
            {
                Node tmp, node = fromVer.firstEdge;
                do
                {   //检查是否添加了重复边
                    if (node.adjvex.data.Equals(toVer.data))
                    {
                        throw new ArgumentException("添加了重复的边！");
                    }
                    tmp = node;
                    node = node.next;
                } while (node != null);
                tmp.next = new Node(toVer); //添加到链表未尾
            }
        }

        public List<VertexRelation<T>> GetRelation()
        {
            List<VertexRelation<T>> lst = new List<AdjacencyList<T>.VertexRelation<T>>();

            foreach (Vertex<T> v in items)
            {
                VertexRelation<T> o = new AdjacencyList<T>.VertexRelation<T>();
                o.Vertex = v;

                if (v.firstEdge != null)
                {
                    Node tmp = v.firstEdge;
                    while (tmp != null)
                    {
                        o.VertexEdge.Add(tmp.adjvex);
                        tmp = tmp.next;
                    }
                }
                lst.Add(o);
            }
            return lst;
        }
        public class VertexRelation<V>
        {
            public Vertex<V> Vertex { get; set; }
            public List<Vertex<V>> VertexEdge { get; set; }

            public VertexRelation()
            {
                Vertex = null;
                VertexEdge = new List<AdjacencyList<T>.Vertex<V>>();
            }
        }


        public PathList<T> Search(T from, T to)
        {
            return Search(Find(from), Find(to), from);
        }
        private PathList<T> Search(Vertex<T> from, Vertex<T> to, T path)
        {
            List<T> pathLst = new List<T>();
            //pathLst.Add(path);

            return Search(from, to, GetRelation(), new List<AdjacencyList<T>.Vertex<T>>(), ref pathLst);
        }
        private PathList<T> Search(Vertex<T> from, Vertex<T> to, List<VertexRelation<T>> relation, List<Vertex<T>> passed, ref List<T> path)
        {
            Predicate<AdjacencyList<T>.VertexRelation<T>> test = delegate (AdjacencyList<T>.VertexRelation<T> p) { return p.Vertex.Equals(from); };
            PathList<T> result = new AdjacencyList<T>.PathList<T>();

            passed.Add(from);

            if (!relation.Exists(test))
            {
                return result;
            }
            else
            {
                foreach (Vertex<T> item in relation.Find(test).VertexEdge)
                {
                    if (passed.Contains(item))
                    {
                        continue;
                    }
                    if (item.Equals(to))
                    {
                        Path<T> p = new AdjacencyList<T>.Path<T>();
                        //path.Add(to.data);
                        p.Value = path;
                        p.Value.Add(to.data);
                        result.Add(p);
                        List<T> tmpPath = new List<T>();
                        //tmpPath.Add(path[0]);
                        path = tmpPath;

                    }
                    else
                    {
                        path.Add(item.data);
                        PathList<T> tmpLst = Search(item, to, relation, passed, ref path);
                        result.AddRange(tmpLst);
                    }
                }
            }
            passed.Remove(from);
            return result;
        }

        public class Node
        {
            /// <summary>
            /// 邻接点域
            /// </summary>
            public Vertex<T> adjvex { get; set; }
            /// <summary>
            /// 下一个邻接点指针域
            /// </summary>
            public Node next { get; set; }

            public Node(Vertex<T> value)
            {
                adjvex = value;
            }
        }
        /// <summary>
        /// 嵌套类，表示存放于数组中的表头结点
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        public class Vertex<TValue>
        {
            /// <summary>
            /// 数据
            /// </summary>
            public TValue data;
            /// <summary>
            /// 邻接点链表头指针
            /// </summary>
            public Node firstEdge;
            /// <summary>
            /// 访问标志,遍历时使用
            /// </summary>
            public Boolean visited;

            /// <summary>
            /// 构造方法
            /// </summary>
            /// <param name="value"></param>
            public Vertex(TValue value)
            {
                data = value;
            }
        }

        #region Path Class
        public class Path<P>
        {
            public List<P> Value { get; set; }

            public Path()
            {
                Value = new List<P>();
            }
        }
        public class PathList<P> : List<Path<P>>
        { }
        #endregion

        #region Depth-First Search
        public void DFSTraverse() //深度优先遍历
        {
            InitVisited(); //将visited标志全部置为false
            DFS(items[0]); //从第一个顶点开始遍历
        }
        private void DFS(Vertex<T> v) //使用递归进行深度优先遍历
        {
            v.visited = true; //将访问标志设为true
            Console.Write(v.data + " "); //访问
            Node node = v.firstEdge;
            while (node != null) //访问此顶点的所有邻接点
            {   //如果邻接点未被访问，则递归访问它的边
                if (!node.adjvex.visited)
                {
                    DFS(node.adjvex); //递归
                }
                node = node.next; //访问下一个邻接点
            }
        }
        private void InitVisited() //初始化visited标志
        {
            foreach (Vertex<T> v in items)
            {
                v.visited = false; //全部置为false
            }
        }
        #endregion

        #region Breadth_First Search
        public void BFSTraverse() //广度优先遍历
        {
            InitVisited(); //将visited标志全部置为false
            BFS(items[0]); //从第一个顶点开始遍历
        }
        private void BFS(Vertex<T> v) //使用队列进行广度优先遍历
        {
            //创建一个队列
            Queue<Vertex<T>> queue = new Queue<Vertex<T>>();
            Console.Write(v.data + " "); //访问

            v.visited = true; //设置访问标志
            queue.Enqueue(v); //进队
            while (queue.Count > 0) //只要队不为空就循环
            {
                Vertex<T> w = queue.Dequeue();
                Node node = w.firstEdge;
                while (node != null) //访问此顶点的所有邻接点
                {   //如果邻接点未被访问，则递归访问它的边
                    if (!node.adjvex.visited)
                    {
                        Console.Write(node.adjvex.data + " "); //访问
                        node.adjvex.visited = true; //设置访问标志
                        queue.Enqueue(node.adjvex); //进队
                    }
                    node = node.next; //访问下一个邻接点
                }
            }
        }
        #endregion

    }
}