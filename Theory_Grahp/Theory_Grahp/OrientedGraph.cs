using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Theory_Grahp
{
    class OrientedGraph
    {
        public class Node
        {
            Dictionary<string, int> AdjacencyList = new Dictionary<string, int>();
            public Node(Dictionary<string, int> al)
            {
                AdjacencyList = al;
            }
            public Node() { }
            public void AddVertex(string a, int v)
            {
                AdjacencyList.Add(a, v);
            }
            public Dictionary<string, int> GetAdjacencyList()
            {
                return AdjacencyList;
            }
        }
        private Dictionary<string, Node> graph = new Dictionary<string, Node>();
        public bool WeightedGraph;
        private Dictionary<string, int> used = new Dictionary<string, int>();
        public void UsedSet() //метод помечает все вершины графа как непросмотреные
        {
            foreach (KeyValuePair<string, Node> pair in graph)
            {
                used[pair.Key] = 0;
            }
        }
        private Dictionary<string, string> p = new Dictionary<string, string>();
        private void pSet()
        {
            foreach (KeyValuePair<string, Node> pair in graph)
            {
                p[pair.Key] = "LOL";
            }
        }
        // конструктор, создающий пустой граф
        public OrientedGraph() { }
        // конструктор, заполняющий данные графа из файла
        public OrientedGraph(string name, bool WG)
        {
            WeightedGraph = WG;
            using (StreamReader file = new StreamReader(name))
            {
                string[] settings = file.ReadLine().Split();
                if (settings[0] == "weighted")
                {
                    WeightedGraph = true;
                }
                else if (settings[0] == "unweighted")
                {
                    WeightedGraph = false;
                }

                int n = int.Parse(file.ReadLine());

                for (int i = 0; i < n; i++)
                {
                    string[] line = file.ReadLine().Split("|");
                    string node = line[0];
                    string[] mas = line[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    Dictionary<string, int> AL = new Dictionary<string, int>();

                    if (!WeightedGraph)
                    {
                        for (int j = 0; j < mas.Length; j++)
                        {
                            AL.Add(mas[j], 1);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < mas.Length; j++)
                        {
                            string[] para = mas[j].Split("=");
                            AL.Add(para[0], int.Parse(para[1]));
                        }
                    }

                    graph.Add(node, new Node(AL));
                }
            }
        }
        // конструктор-копия
        public OrientedGraph(OrientedGraph gr)
        {
            graph = gr.graph;
        }
        // метод, добавляющий вершину
        public void AddVertex(string name)
        {
            graph.Add(name, new Node());
        }
        // добавление дуги для невзвешенного
        public void AddEdge(string A, string B)
        {
            if (graph.ContainsKey(A) && graph.ContainsKey(B) && !graph[A].GetAdjacencyList().ContainsKey(B))
            {
                graph[A].AddVertex(B, 1);
            }
            else
            {
                Console.WriteLine("There is no vertex(es) or an edge already exists");
            }
        }
        // добавление дуги для взвешенного
        public void AddEdge(string A, string B, int v)
        {
            if (graph.ContainsKey(A) && graph.ContainsKey(B) && !graph[A].GetAdjacencyList().ContainsKey(B))
            {
                graph[A].AddVertex(B, v);
            }
            else
            {
                Console.WriteLine("There is no such vertex(es) or an edge already exists");
            }
        }
        // удаление вершины
        public void DeleteVertex(string v)
        {
            if (graph.Remove(v))
            {
                foreach (KeyValuePair<string, Node> pair in graph)
                {
                    if (pair.Value.GetAdjacencyList().ContainsKey(v))
                    {
                        pair.Value.GetAdjacencyList().Remove(v);
                    }
                }
            }
            else
            {
                Console.WriteLine("There is no such vertex");
            }
        }
        // удаление дуги
        public void DeleteEdge(string A, string B)
        {
            if (graph.ContainsKey(A) && graph.ContainsKey(B) && graph[A].GetAdjacencyList().ContainsKey(B))
            {
                graph[A].GetAdjacencyList().Remove(B);
            }
            else
            {
                Console.WriteLine("There is no such vertex(es) or the edge has already been removed");
            }
        }
        // вершины орграфа, смежные с данной
        public void PrintAdjacent(string v)
        {
            if (graph.ContainsKey(v))
            {
                Console.Write("Outgoing: ");
                foreach (KeyValuePair<string, int> x in graph[v].GetAdjacencyList())
                {
                    Console.Write("{0} ", x.Key);
                }
                Console.WriteLine();
                Console.Write("Incoming: ");
                foreach (KeyValuePair<string, Node> x in graph)
                {
                    if (x.Value.GetAdjacencyList().ContainsKey(v))
                    {
                        Console.Write("{0} ", x.Key);
                    }
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Vertex '{0}' not found", v);
            }
        }
        // объединение графов
        public void Union(string namefile, string newfile)
        {
            OrientedGraph gr = new OrientedGraph(namefile, WeightedGraph);

            foreach (KeyValuePair<string, Node> pair in gr.graph)
            {
                if (!graph.ContainsKey(pair.Key))
                {
                    graph.Add(pair.Key, pair.Value);
                }
                else
                {
                    Node node = graph[pair.Key];
                    Dictionary<string, int> al = node.GetAdjacencyList();
                    foreach (KeyValuePair<string, int> v in pair.Value.GetAdjacencyList())
                    {
                        if (!al.ContainsKey(v.Key))
                        {
                            node.GetAdjacencyList().Add(v.Key, v.Value);
                        }
                    }
                }
            }

            OutputToFile(newfile);
            Console.WriteLine(PrintGraph());
        }
        // 6 II. Вывести кратчайший (по числу дуг) цикл орграфа, содержащий вершину u.
        public Stack<string> BFS(string s, string t)
        {
            Dictionary<string, int> dist = new Dictionary<string, int>();
            Dictionary<string, string> p = new Dictionary<string, string>();
            foreach (KeyValuePair<string, Node> item in graph)
            {
                dist[item.Key] = graph.Count;
                p[item.Key] = "???";
            }
            dist[s] = 0;
            Queue<string> q = new Queue<string>();
            q.Enqueue(s);

            while(q.Count != 0)
            {
                string v = q.Dequeue(); 

                foreach(KeyValuePair<string, int> u in graph[v].GetAdjacencyList())
                {
                    if(dist[u.Key] > dist[v] + 1)
                    {
                        p[u.Key] = v;
                        dist[u.Key] = dist[v] + 1;
                        q.Enqueue(u.Key);
                    }
                }
            }
            Stack<string> path = new Stack<string>();
            // если пути не существует, возвращаем пустой стек
            if (dist[t] == graph.Count)
            {
                return path;
            }

            while(t != "???")
            {
                path.Push(t);
                //Console.Write("{0} ", t);
                t = p[t];
            }
            return path;
        }
        public void CycleSearch(string v)
        {
            bool f = true;
            Stack<string> minc = new Stack<string>();
            foreach(KeyValuePair<string, int> item in graph[v].GetAdjacencyList())
            {
                if (f || minc.Count == 0)
                {
                    minc = BFS(item.Key, v);
                    f = false;
                }
                else
                {
                    Stack<string> cycle = BFS(item.Key, v);
                    if (cycle.Count != 0 && cycle.Count < minc.Count)
                    {
                        minc = cycle;
                    }
                }
            }
            int n = minc.Count;
            if (n == 0)
            {
                Console.WriteLine("There is no cycle");
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    Console.Write("{0} ", minc.Pop());
                }
                Console.WriteLine();
            }
        }
        public Dictionary<string, Node> Floyd()
        {
            Dictionary<string, Node> a = new Dictionary<string, Node>();
            foreach (string i in graph.Keys)
            {
                a.Add(i, new Node());
                foreach (string j in graph.Keys)
                {
                    if (i == j)
                    {
                        a[i].AddVertex(j, 0);
                    }
                    else if (graph[i].GetAdjacencyList().ContainsKey(j))
                    {
                        a[i].AddVertex(j, graph[i].GetAdjacencyList()[j]);
                    }
                    else
                    {
                        a[i].AddVertex(j, 99999);
                    }
                }
            }
            //осуществляем поиск кратчайших путей
            foreach (string k in graph.Keys)
            {
                foreach (string i in graph.Keys)
                {
                    foreach (string j in graph.Keys)
                    {
                        int r = a[i].GetAdjacencyList()[k] + a[k].GetAdjacencyList()[j];
                        if (a[i].GetAdjacencyList()[j] > r)
                        {
                            a[i].GetAdjacencyList()[j] = r;
                        }
                    }
                }
            }
            return a;
        }

        public Dictionary<string, int> Dijkstra(string s)
        {
            Dictionary<string, int> d = new Dictionary<string, int>();
            Dictionary<string, bool> used = new Dictionary<string, bool>();
            foreach (KeyValuePair<string, Node> i in graph)
            {
                d[i.Key] = 99999;
                used[i.Key] = false;
            }
            d[s] = 0;
            foreach(KeyValuePair<string, Node> i in graph)
            {
                string v = "";
                foreach(KeyValuePair<string, Node> j in graph)
                {
                    if(!used[j.Key] && (v == "" || d[j.Key] < d[v]))
                    {
                        v = j.Key;
                    }
                }
                if (d[v] == 99999)
                {
                    break;
                }
                used[v] = true;
                foreach(KeyValuePair<string, int> e in graph[v].GetAdjacencyList())
                {
                    if(d[v] + e.Value < d[e.Key])
                    {
                        d[e.Key] = d[v] + e.Value;
                    }
                }
            }
            return d;
        }
        // Найти вершину, сумма длин кратчайших путей от которой до остальных вершин минимальна
        //public void MinSumDistanceFALSE()
        //{
        //    Dictionary<string, Node> a = Floyd();
        //    string v = "";
        //    int w = 99999;
        //    foreach (KeyValuePair<string, Node> i in a)
        //    {
        //        int cur = 0;
        //        // Console.WriteLine("{0}", i.Key);
        //        foreach (KeyValuePair<string, int> j in i.Value.GetAdjacencyList())
        //        {
        //            //Console.WriteLine("{0}={1} ", j.Key, j.Value);
        //            if (j.Value != 99999)
        //            {
        //                cur += j.Value;
        //            }
        //        }
        //        Console.WriteLine("{0} {1}", i.Key, cur);
        //        if (cur < w)
        //        {
        //            w = cur;
        //            v = i.Key;
        //        }
        //        //Console.WriteLine();
        //    }
        //    Console.WriteLine("{0}", v);
        //}
        public bool FordBellman(string s, ref Dictionary<string, string> pred)
        {
            Dictionary<string, int> d = new Dictionary<string, int>();
            //Dictionary<string, string> pred = new Dictionary<string, string>();
            foreach (KeyValuePair<string, Node> v in graph)
            {
                d[v.Key] = 99999; // ??
                pred[v.Key] = "?";
            }
            d[s] = 0;
            foreach (KeyValuePair<string, Node> i in graph)
            {
                foreach (KeyValuePair<string, Node> u in graph)
                {
                    foreach(KeyValuePair<string, int> v in u.Value.GetAdjacencyList())
                    {
                        if(d[v.Key] > d[u.Key] + v.Value)
                        {
                            d[v.Key] = d[u.Key] + v.Value;
                            pred[v.Key] = u.Key;
                        }
                    }
                }
            }

            //foreach (KeyValuePair<string, Node> u in graph)
            //{
                foreach (KeyValuePair<string, int> v in graph[s].GetAdjacencyList())
                {
                    if (d[v.Key] > d[s] + v.Value)
                    {
                        return false;
                    }
                }
            //}
            return true;
        }
        public void MinSumDistance()
        {
            string v = "";
            int w = 99999;
            foreach (KeyValuePair<string, Node> i in graph)
            {
                Dictionary<string, int> d = Dijkstra(i.Key);
                int sum = 0;
                foreach (KeyValuePair<string, int> j in d)
                {
                    //if (j.Value != 99999)
                    //{
                    sum += j.Value;
                    //}
                }
                Console.WriteLine("{0} {1}", i.Key, sum);
                if (sum < w)
                {
                    v = i.Key;
                    w = sum;
                }
            }
            Console.WriteLine("Ответ: {0}", v);
        }
        // Эксцентриситет вершины — максимальное расстояние из всех минимальных расстояний от 
        // других вершин до данной вершины. Найти радиус графа — минимальный из эксцентриситетов его вершин.
        public void Radius()
        {
            Dictionary<string, Node> a = Floyd();
            Dictionary<string, int> Eccentricity = new Dictionary<string, int>();

            foreach (KeyValuePair<string, Node> i in a)
            {
                Eccentricity[i.Key] = -99999;
            }

            foreach (KeyValuePair<string, Node> i in a)
            {
                foreach (KeyValuePair<string, int> j in i.Value.GetAdjacencyList())
                {
                    if (j.Value != 99999 && j.Value > Eccentricity[j.Key])
                    {
                        Eccentricity[j.Key] = j.Value;
                    }
                }
            }

            int min = 99999;
            foreach(KeyValuePair<string, int> i in Eccentricity)
            {
                Console.WriteLine("{0} {1}", i.Key, i.Value);
                if(i.Value < min)
                {
                    min = i.Value;
                }
            }
            Console.WriteLine(min);
        }
        private void PrintPath(Dictionary<string, string> pred, string s, string v)
        {
            if(v == s)
            {
                Console.Write("{0} ", s);
            }
            else if(pred[v] == "?")
            {
                Console.WriteLine("Путь из '{0}' в '{1}' отсутствует", s, v);
            }
            else
            {
                PrintPath(pred, s, pred[v]);
                Console.Write("{0} ", v);
            }
        }
        // Вывести кратчайший путь из вершины u до вершины v
        public void ShortestPath(string s, string v)
        {
            Dictionary<string, string> pred = new Dictionary<string, string>();
            if (FordBellman(s, ref pred))
            {
                PrintPath(pred, s, v);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("There is a negative weight cycle");
            }
        }

        Dictionary<string, Node> C = new Dictionary<string, Node>();
        Dictionary<string, Node> f = new Dictionary<string, Node>();
        public int Flow(string u, string t, int Cmin)
        {
            if(u == t)
            {
                return Cmin;
            }
            used[u] = 1;
            foreach(KeyValuePair<string, int> v in graph[u].GetAdjacencyList())
            {
                if(used[v.Key]==0 && f[u].GetAdjacencyList()[v.Key] < C[u].GetAdjacencyList()[v.Key])
                {
                    int min = Cmin;
                    if(C[u].GetAdjacencyList()[v.Key]- f[u].GetAdjacencyList()[v.Key] < min)
                    {
                        min = C[u].GetAdjacencyList()[v.Key] - f[u].GetAdjacencyList()[v.Key];
                    }
                    int delta = Flow(v.Key, t, min);
                    if(delta > 0)
                    {
                        f[u].GetAdjacencyList()[v.Key] += delta;
                        f[v.Key].GetAdjacencyList()[u] -= delta;
                        return delta;
                    }
                }
            }
            return 0;
        }
        public void MaxFlow(string s, string t)
        {
            foreach(KeyValuePair<string, Node> i in graph)
            {
                //C.Add(i.Key, i.Value);
                if (!C.ContainsKey(i.Key))
                {
                    C.Add(i.Key, new Node());
                }
                if (!f.ContainsKey(i.Key))
                {
                    f.Add(i.Key, new Node());
                }
                foreach (KeyValuePair<string, int> j in i.Value.GetAdjacencyList())
                {
                    if (!f.ContainsKey(j.Key))
                    {
                        f.Add(j.Key, new Node());
                    }
                    if (!C.ContainsKey(j.Key))
                    {
                        C.Add(j.Key, new Node());
                    }
                    f[i.Key].AddVertex(j.Key, 0);
                    f[j.Key].AddVertex(i.Key, 0);

                    C[i.Key].AddVertex(j.Key, j.Value);
                    C[j.Key].AddVertex(i.Key, -j.Value);
                }
            }

            Console.WriteLine("{0}", Flow(s, t, 0));
        }

        public int FordFulkerson(string start, string end)
        {
            //определили исток и сток
            Console.WriteLine($"start: {start}");
            Console.WriteLine($"end: {end}");

            int n = graph.Count;//кол-во вершин

            Dictionary<int, string> verticesNumbersAreKeys = new Dictionary<int, string>(); //вес ребер
            Dictionary<string, int> verticesNamesAreKeys = new Dictionary<string, int>();//поток

            int num = 0;
            foreach (var vertex in graph)
            {
                verticesNumbersAreKeys.Add(num, vertex.Key);
                verticesNamesAreKeys.Add(vertex.Key, num);
                num++;
            }

            int startNumber = verticesNamesAreKeys[start];
            int endNumber = verticesNamesAreKeys[end];

            int[,] cap = new int[n, n];

            num = 0;
            foreach (var item in graph)
            {
                foreach (var jtem in verticesNumbersAreKeys)
                {
                    if (item.Value.GetAdjacencyList().ContainsKey(jtem.Value))//если тек вершина(item) содержит вершину из словаря(т.е. смежна с текущей)
                    {
                        cap[num, jtem.Key] = item.Value.GetAdjacencyList()[jtem.Value];//величина ребра с 0 до 4 массив
                        cap[jtem.Key, num] = 0;//поток
                    }

                }
                num++;//прибавляем счетчик, чтобы дальше продолжать итерацию и верно заполнять массив
            }
            for (int flow = 0; ;)
            {
                bool[] used = new bool[n];

                int df = FindPath(cap, used, startNumber, endNumber, Int32.MaxValue);

                if (df == 0)
                {
                    Console.WriteLine();
                    return flow;
                }

                Console.WriteLine(df);
                flow += df;
            }
        }

        public int FindPath(int[,] capacity, bool[] used, int u, int t, int f)
        {

            Console.WriteLine(-100);
            if (u == t)
            {
                return f;
            }

            used[u] = true;//помечаем вершину просмотренной

            for (int v = 0; v < used.Length; ++v)
            {
                Console.Write($"{v} ");
                if (!used[v] && capacity[u, v] > 0)//если вершина не просмотр и сущ-ет ребро(т.е. значение больше 0)
                {

                    int df = FindPath(capacity, used, v, t, Math.Min(f, capacity[u, v])); //горлышко от бутылки

                    if (df > 0)
                    {
                        capacity[u, v] -= df;
                        capacity[v, u] += df;

                        return df;
                    }
                }
            }

            return 0;
        }

        // вывод графа в файл
        public void OutputToFile(string directory)
        {
            using (StreamWriter file = new StreamWriter(directory))
            {
                file.WriteLine("{0} directed", WeightedGraph ? "weighted" : "unweighted");

                int n = graph.Count;

                file.WriteLine("{0}", n);

                if (!WeightedGraph)
                {
                    foreach (KeyValuePair<string, Node> pair in graph)
                    {
                        file.Write("{0}|", pair.Key);
                        foreach (KeyValuePair<string, int> v in pair.Value.GetAdjacencyList())
                        {
                            file.Write("{0} ", v.Key);
                        }
                        file.Write("\n");
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, Node> pair in graph)
                    {
                        file.Write("{0}|", pair.Key);
                        foreach (KeyValuePair<string, int> v in pair.Value.GetAdjacencyList())
                        {
                            file.Write("{0}={1} ", v.Key, v.Value);
                        }
                        file.Write("\n");
                    }
                }
            }
        }
        public String PrintGraph()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(WeightedGraph ? "weighted" : "unweighted").Append(" ").Append("directed").Append('\n');
            sb.Append(graph.Count).Append("\n");
            if (!WeightedGraph)
            {
                foreach (KeyValuePair<string, Node> pair in graph)
                {
                    sb.Append(pair.Key).Append("|");
                    foreach (KeyValuePair<string, int> v in pair.Value.GetAdjacencyList())
                    {
                        sb.Append(v.Key).Append(" ");
                    }
                    sb.Append("\n");
                }
            }
            else
            {
                foreach (KeyValuePair<string, Node> pair in graph)
                {
                    sb.Append(pair.Key).Append("|");
                    foreach (KeyValuePair<string, int> v in pair.Value.GetAdjacencyList())
                    {
                        sb.Append(v.Key).Append("=").Append(v.Value).Append(" ");
                    }
                    sb.Append("\n");
                }
            }
            return sb.ToString();
        }
    }
}
