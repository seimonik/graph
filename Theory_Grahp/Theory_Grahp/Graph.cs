using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Theory_Grahp
{
    class Graph
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
            public int Degree()
            {
                return AdjacencyList.Count;
            }
        }
        private Dictionary<string, Node> graph = new Dictionary<string, Node>();
        bool WeightedGraph;
        private Dictionary<string, bool> nov = new Dictionary<string, bool>();
        //private bool[] nov; //вспомогательный массив: если i-ый элемент массива равен
        //true, то i-ая вершина еще не просмотрена; если i-ый
        //элемент равен false, то i-ая вершина просмотрена
        public void NovSet() //метод помечает все вершины графа как непросмотреные
        {
            foreach(KeyValuePair<string, Node> pair in graph)
            {
                nov[pair.Key] = true;
            }
        }
        public Graph() { }
        // конструктор, заполняющий данные графа из файла
        public Graph(string name, bool WG)
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
                NovSet();
            }
        }
        // конструктор-копия
        public Graph(Graph gr)
        {
            graph = new Dictionary<string, Node>(gr.graph);
        }
        public Graph(Dictionary<string, Node> gr)
        {
            graph = new Dictionary<string, Node>();
            foreach (KeyValuePair<string, Node> i in gr)
            {
                graph.Add(i.Key, new Node(new Dictionary<string, int>(i.Value.GetAdjacencyList())));
            }
        }
        // метод, добавляющий вершину
        public void AddVertex(string name)
        {
            graph.Add(name, new Node());
        }
        // добавление ребра для невзвешенного
        public void AddEdge(string A, string B)
        {
            if (graph.ContainsKey(A) && graph.ContainsKey(B) && !graph[A].GetAdjacencyList().ContainsKey(B) && !graph[B].GetAdjacencyList().ContainsKey(A))
            {
                graph[A].AddVertex(B, 1);
                graph[B].AddVertex(A, 1);
            }
            else 
            {
                Console.WriteLine("There is no vertex(es) or an edge already exists");
            }
        }
        // добавление ребра для взвешенного
        public void AddEdge(string A, string B, int v)
        {
            if (graph.ContainsKey(A) && graph.ContainsKey(B) && !graph[A].GetAdjacencyList().ContainsKey(B) && !graph[B].GetAdjacencyList().ContainsKey(A))
            {
                graph[A].AddVertex(B, v);
                graph[B].AddVertex(A, v);
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
        // удаление ребра
        public void DeleteEdge(string A, string B)
        {
            if (graph.ContainsKey(A) && graph.ContainsKey(B) && graph[A].GetAdjacencyList().ContainsKey(B) && graph[B].GetAdjacencyList().ContainsKey(A))
            {
                graph[A].GetAdjacencyList().Remove(B);
                graph[B].GetAdjacencyList().Remove(A);
            }
            else
            {
                Console.WriteLine("There is no such vertex(es) or the edge has already been removed");
            }
        }
        // Для каждой вершины графа вывести её степень.
        public void Degree()
        {
            foreach (KeyValuePair<string, Node> x in graph)
            {
                Console.Write("deg({0}) = {1}", x.Key, x.Value.Degree());
                Console.WriteLine();
            }
        }
        // Алгоритм обхода графа в глубину
        public void DFS (string v, string prev, ref bool cycle)
        {
            //Console.Write("{0} ", v); //просматриваем текущую вершину
            nov[v] = false; //помечаем ее как просмотренную
            // в матрице смежности просматриваем строку с номером v
            foreach(KeyValuePair<string, int> adjv in graph[v].GetAdjacencyList())
            {
                if (nov[adjv.Key]) // если вершина не просмотрена
                {
                    DFS(adjv.Key, v, ref cycle);
                }
                else if(adjv.Key != prev)
                {
                    cycle = true;
                    break;
                }
            }
        }
        // алгоритм обхода графа в ширину
        public void BFS(string v)
        {
            Queue<string> q = new Queue<string>(); // инициализируем очередь
            q.Enqueue(v); //помещаем вершину v в очередь
            nov[v] = false; // помечаем вершину v как просмотренную
            while (q.Count != 0) // пока очередь не пуста
            {
                v = q.Dequeue(); //извлекаем вершину из очереди
                Console.Write("{0} ", v); //просматриваем ее
                foreach(KeyValuePair<string, int> pair in graph[v].GetAdjacencyList())
                {
                    if (nov[pair.Key])
                    {
                        q.Enqueue(pair.Key);
                        nov[pair.Key] = false;
                    }
                }
            }
        }

        // Подсчет количества ребер
        private int CountEdge()
        {
            int count = 0;
            foreach(KeyValuePair<string, Node> pair in graph)
            {
                count += pair.Value.GetAdjacencyList().Count;
            }
            return count / 2;
        }
        //public void CanGetTree()
        //{
        //    NovSet();

        //    List<string> DelVertex = new List<string>();

        //    Queue<string> q = new Queue<string>(); // инициализируем очередь
        //    string v = graph.First().Key;
        //    q.Enqueue(v); //помещаем вершину v в очередь
        //    nov[v] = false; // помечаем вершину v как просмотренную
        //    while (q.Count != 0) // пока очередь не пуста
        //    {
        //        v = q.Dequeue(); //извлекаем вершину из очереди
        //        int k = 0;
        //        foreach (KeyValuePair<string, int> pair in graph[v].GetAdjacencyList())
        //        {
        //            if (nov[pair.Key])
        //            {
        //                q.Enqueue(pair.Key);
        //                nov[pair.Key] = false;
        //            }
        //            else
        //            {
        //                k++;
        //            }
        //        }
        //        if (k > 1)
        //        {
        //            DelVertex.Add(v);
        //        }
        //    }

        //    if (nov.ContainsValue(true))
        //    {
        //        Console.WriteLine("No. This is a disconnected graph");
        //    }
        //    else if(DelVertex.Count == 0)
        //    {
        //        Console.WriteLine("This is a tree");
        //    }
        //    else if(DelVertex.Count == 2)
        //    {
        //        Console.WriteLine("It is necessary to delete the vertex '{0}' or '{1}'", DelVertex[0], DelVertex[1]);
        //    }
        //    //else if (DelVertex.Count == 3)
        //    //{
        //    //    foreach(string vt in DelVertex)
        //    //    {
        //    //        int col = 0;
        //    //        foreach (string neighbour in DelVertex)
        //    //        {
        //    //            foreach (KeyValuePair<string, int> pair in graph[neighbour].GetAdjacencyList())
        //    //            {

        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    else
        //    {
        //        Console.WriteLine("It is impossible to get a tree");
        //    }
        //}

        public void CanGetTree()
        {
            bool can = false;
            if (Connectivity()) // delete
            {
                foreach (KeyValuePair<string, Node> v in graph)
                {
                    Graph gr = new Graph(graph);
                    gr.DeleteVertex(v.Key);
                    bool cycle = false;
                    gr.NovSet();
                    gr.DFS(gr.graph.FirstOrDefault().Key, "?", ref cycle);
                    if (gr.Connectivity() && !cycle)
                    {
                        Console.WriteLine("Можно удалить вершину {0}, чтобы получилось дерево", v.Key);
                        can = true;
                        break;
                    }
                }
            }
            if (!can)
            {
                Console.WriteLine("Невозможно удалить вершину, чтобы получилось дерево");
            }
        }

        // Prima
        private int dfs(string u)
        {
            int visitedVertices = 1;
            nov[u] = false;
            foreach (KeyValuePair<string, int> v in graph[u].GetAdjacencyList())
            {
                if (nov[v.Key])
                {
                    visitedVertices += dfs(v.Key);
                }
            }
            return visitedVertices;
        }
        private bool Connectivity()
        {
            NovSet();
            if (graph.Count == dfs(graph.First().Key))
            {
                return true;
            }
            return false;
        }

        public void algPrima()
        {
            if (!Connectivity())
            {
                Console.WriteLine("Disconnected graph");
            }
            else
            {

                Dictionary<string, bool> used = new Dictionary<string, bool>();
                Dictionary<string, int> min_e = new Dictionary<string, int>();
                Dictionary<string, string> sel_e = new Dictionary<string, string>();
                foreach (KeyValuePair<string, Node> pair in graph)
                {
                    used[pair.Key] = false;
                    min_e[pair.Key] = 999999;
                    sel_e[pair.Key] = "???";
                }
                min_e["???"] = 999999;

                min_e[used.First().Key] = 0;
                foreach (KeyValuePair<string, Node> pair in graph)
                {
                    Tuple<string, Node> v = new Tuple<string, Node>("???", new Node());
                    foreach (KeyValuePair<string, Node> item in graph)
                    {
                        if (!used[item.Key] && (v.Item1 == "???" || min_e[item.Key] < min_e[v.Item1]))
                        {
                            v = new Tuple<string, Node>(item.Key, item.Value);
                        }
                    }

                    used[v.Item1] = true;
                    if (sel_e[v.Item1] != "???")
                    {
                        Console.WriteLine("{0}|{1}", v.Item1, sel_e[v.Item1]);
                    }

                    foreach (KeyValuePair<string, int> item in v.Item2.GetAdjacencyList())
                    {
                        if (item.Value < min_e[item.Key])
                        {
                            min_e[item.Key] = item.Value;
                            sel_e[item.Key] = v.Item1;
                        }
                    }
                }
            }
        }
        //if(min_e[v.Item1] == 999999)
        //{
        //    Console.WriteLine("No MST");
        //    break;
        //}

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
            foreach (KeyValuePair<string, Node> i in graph)
            {
                string v = "";
                foreach (KeyValuePair<string, Node> j in graph)
                {
                    if (!used[j.Key] && (v == "" || d[j.Key] < d[v]))
                    {
                        v = j.Key;
                    }
                }
                if (d[v] == 99999)
                {
                    break;
                }
                used[v] = true;
                foreach (KeyValuePair<string, int> e in graph[v].GetAdjacencyList())
                {
                    if (d[v] + e.Value < d[e.Key])
                    {
                        d[e.Key] = d[v] + e.Value;
                    }
                }
            }
            return d;
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
                    if (j.Value != 99999)
                    {
                        sum += j.Value;
                    }
                }
                //Console.WriteLine("{0} {1}", i.Key, sum);
                if (sum < w)
                {
                    v = i.Key;
                    w = sum;
                }
            }
            Console.WriteLine("{0}", v);
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
        // Эксцентриситет вершины — максимальное расстояние из всех минимальных расстояний от 
        // других вершин до данной вершины. Найти радиус графа — минимальный из эксцентриситетов его вершин.
        public void Radius()
        {
            Dictionary<string, Node> a = Floyd();

            //foreach(KeyValuePair<string, Node> i in a)
            //{
            //    foreach(KeyValuePair<string, int> j in i.Value.GetAdjacencyList())
            //    {
            //        Console.Write("{0}={1}", j.Key, j.Value);
            //    }
            //    Console.WriteLine();
            //}

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
            foreach (KeyValuePair<string, int> i in Eccentricity)
            {
                Console.WriteLine("{0} {1}", i.Key, i.Value);
                if (i.Value < min)
                {
                    min = i.Value;
                }
            }
            Console.WriteLine(min);
        }

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
                    foreach (KeyValuePair<string, int> v in u.Value.GetAdjacencyList())
                    {
                        if (d[v.Key] > d[u.Key] + v.Value)
                        {
                            d[v.Key] = d[u.Key] + v.Value;
                            pred[v.Key] = u.Key;
                        }
                    }
                }
            }

            foreach (KeyValuePair<string, Node> u in graph)
            {
                foreach (KeyValuePair<string, int> v in u.Value.GetAdjacencyList())
                {
                    if (d[v.Key] > d[u.Key] + v.Value)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void PrintPath(Dictionary<string, string> pred, string s, string v)
        {
            if (v == s)
            {
                Console.Write("{0} ", s);
            }
            else if (pred[v] == "?")
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

        // вывод графа в файл
        public void OutputToFile(string directory)
        {
            using (StreamWriter file = new StreamWriter(directory))
            {
                file.WriteLine("{0} undirected", WeightedGraph ? "weighted" : "unweighted");
                
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
            sb.Append(WeightedGraph ? "weighted" : "unweighted").Append(" ").Append("undirected").Append('\n');
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