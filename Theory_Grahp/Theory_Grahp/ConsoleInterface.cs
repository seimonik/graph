using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Theory_Grahp
{
    public class ConsoleInterface
    {
        private const string ReadGraph = "READ";
        private static readonly string[] ReadGraphArgs = { "NAME" };

        private const string WriteGraph = "WRITE";
        private static readonly string[] WriteGraphArgs = { "NAME" };

        private const string AddVertex = "ADDVERTEX";
        private static readonly string[] AddVertexArgs = { "VERTEX" };

        private const string AddEdge = "ADDEDGE";
        private static readonly string[] AddEdgeArgs = { "STARTING VERTEX", "FINAL VERTEX", "WEIGHTH (nothing if the unweighted graph)" };

        private const string DeleteVertex = "DELETEVERTEX";
        private static readonly string[] DeleteVertexArgs = { "VERTEX" };

        private const string DeleteEdge = "DELETEEDGE";
        private static readonly string[] DeleteEdgeArgs = { "STARTING VERTEX", "FINAL VERTEX" };

        private const string Degree = "DEGREE";
        private static readonly string[] DegreeArgs = { };

        private const string AdjacentVertices = "ADJACENTVERTICES";
        private static readonly string[] AdjacentVerticesArgs = { "VERTEX" };

        private const string Union = "UNIONWITH";
        private static readonly string[] UnionArgs = { "NAME FILE", "NAME NEW FILE" };

        private const string Dfs = "DFS";
        private static readonly string[] DfsArgs = { "VERTEX" };

        private const string Bfs = "BFS";
        private static readonly string[] BfsArgs = { "VERTEX" };

        private const string CanGetTree = "CANGETTREE";
        private static readonly string[] CanGetTreeArgs = { };

        private const string Cycle = "CYCLE";
        private static readonly string[] CycleArgs = { "VERTEX" };

        private const string Prima = "PRIMA";
        private static readonly string[] PrimaArgs = { };

        private const string MinSumDistance = "MINSUMDISTANCE";
        private static readonly string[] MinSumDistanceArgs = { };

        private const string Radius = "RADIUS";
        private static readonly string[] RadiusArgs = { };

        private const string ShortestPath = "SHORTESTPATH";
        private static readonly string[] ShortestPathArgs = { "VERTEX_1", "VERTEX_2" };

        private const string Flow = "FLOW";
        private static readonly string[] FlowArgs = { "SOURCE", "STOCK" };

        private const string Hint = "HINT";
        private const string Exit = "EXIT";
        private const string Print = "PRINT";

        private const string UnknownCommand = "UNKNOWN COMMAND";
        private const string WrongArgument = "Wrong argument(s)";

        public ConsoleInterface() { }
        private Graph gr = new Graph();
        private OrientedGraph orgr = new OrientedGraph();
        private bool weight;
        private bool oriented;

        public void Start()
        {
            Console.WriteLine(GetHint());
            for (; ; )
            {
                //try
                //{
                    Console.Write(">>> ");
                    List<String> arguments = new List<String>(Console.ReadLine().Split(" "));
                    string command = arguments[0].ToUpper();
                    arguments.RemoveAt(0);
                    switch (command)
                    {
                        case ReadGraph:
                            if (arguments.Count != ReadGraphArgs.Length)
                            {
                                Console.WriteLine(WrongArgument);
                            }
                            else
                            {
                                using (StreamReader file = new StreamReader(arguments[0]))
                                {
                                    string[] line = file.ReadLine().Split();
                                    if (line[1] == "directed")
                                    {
                                        oriented = true;
                                    }
                                    else if (line[1] == "undirected")
                                    {
                                        oriented = false;
                                    }
                                    if (line[0] == "weighted")
                                    {
                                        weight = true;
                                    }
                                    else if (line[0] == "unweighted")
                                    {
                                        weight = false;
                                    }
                                }
                                if (oriented)
                                {
                                    orgr = new OrientedGraph(arguments[0], true);
                                    Console.WriteLine(orgr.PrintGraph());
                                }
                                else
                                {
                                    gr = new Graph(arguments[0], true);
                                    Console.WriteLine(gr.PrintGraph());
                                }
                            }
                            break;
                        case WriteGraph:
                            if (arguments.Count != WriteGraphArgs.Length)
                            {
                                Console.WriteLine(WrongArgument);
                            }
                            else if (!oriented)
                            {
                                gr.OutputToFile(arguments[0]);
                            }
                            else
                            {
                                orgr.OutputToFile(arguments[0]);
                            }
                            break;
                        case AddVertex:
                            if (arguments.Count != AddVertexArgs.Length)
                            {
                                Console.WriteLine(WrongArgument);
                            }
                            else if (!oriented)
                            {
                                gr.AddVertex(arguments[0]);
                                Console.WriteLine(gr.PrintGraph());
                            }
                            else
                            {
                                orgr.AddVertex(arguments[0]);
                                Console.WriteLine(orgr.PrintGraph());
                            }
                            break;
                        case AddEdge:
                            if ((weight && arguments.Count != AddEdgeArgs.Length) || (!weight && arguments.Count != AddEdgeArgs.Length - 1))
                            {
                                Console.WriteLine(WrongArgument);
                            }
                            else if (!oriented && !weight)
                            {
                                gr.AddEdge(arguments[0], arguments[1]);
                                Console.WriteLine(gr.PrintGraph());
                            }
                            else if (!oriented && weight)
                            {
                                gr.AddEdge(arguments[0], arguments[1], int.Parse(arguments[2]));
                                Console.WriteLine(gr.PrintGraph());
                            }
                            else if (oriented && !weight)
                            {
                                orgr.AddEdge(arguments[0], arguments[1]);
                                Console.WriteLine(orgr.PrintGraph());
                            }
                            else if (oriented && weight)
                            {
                                orgr.AddEdge(arguments[0], arguments[1], int.Parse(arguments[2]));
                                Console.WriteLine(orgr.PrintGraph());
                            }
                            break;
                        case DeleteVertex:
                            if (arguments.Count != DeleteVertexArgs.Length)
                            {
                                Console.WriteLine(WrongArgument);
                            }
                            else if (!oriented)
                            {
                                gr.DeleteVertex(arguments[0]);
                                Console.WriteLine(gr.PrintGraph());
                            }
                            else
                            {
                                orgr.DeleteVertex(arguments[0]);
                                Console.WriteLine(orgr.PrintGraph());
                            }
                            break;
                        case DeleteEdge:
                            if (arguments.Count != DeleteEdgeArgs.Length)
                            {
                                Console.WriteLine(WrongArgument);
                            }
                            else if (!oriented)
                            {
                                gr.DeleteEdge(arguments[0], arguments[1]);
                                Console.WriteLine(gr.PrintGraph());
                            }
                            else
                            {
                                orgr.DeleteEdge(arguments[0], arguments[1]);
                                Console.WriteLine(orgr.PrintGraph());
                            }
                            break;
                        case Degree:
                            if (arguments.Count != DegreeArgs.Length)
                            {
                                Console.WriteLine(WrongArgument);
                            }
                            else if (!oriented)
                            {
                                gr.Degree();
                            }
                            else
                            {
                                Console.WriteLine("Only for undirected graphs");
                            }
                            break;
                        case AdjacentVertices:
                            if (arguments.Count != AdjacentVerticesArgs.Length)
                            {
                                Console.WriteLine(WrongArgument);
                            }
                            else if (oriented)
                            {
                                orgr.PrintAdjacent(arguments[0]);
                            }
                            else
                            {
                                Console.WriteLine("Only for directed graphs");
                            }
                            break;
                        case Union:
                            if (arguments.Count != UnionArgs.Length)
                            {
                                Console.WriteLine(WrongArgument);
                            }
                            else if (!oriented)
                            {
                                Console.WriteLine("Only for directed graphs");
                            }
                            else
                            {
                                var workingDirectory = Environment.CurrentDirectory;
                                var f = @$"{workingDirectory}\{arguments[0]}";
                                if (File.Exists(f))
                                {
                                    using (StreamReader file = new StreamReader(arguments[0]))
                                    {
                                        string[] line = file.ReadLine().Split();
                                        if (line[1] == "undirected")
                                        {
                                            Console.WriteLine("There is an undirected graph in the file");
                                        }
                                        else if ((line[0] == "weighted" && !weight) || (line[0] == "unweighted" && weight))
                                        {
                                            Console.WriteLine("One graph is weighted and the other is unweighted");
                                        }
                                        else
                                        {
                                            orgr.Union(arguments[0], arguments[1]);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("File {0} not found", arguments[0]);
                                }
                            }
                            break;
                        //case Dfs:
                        //    if (arguments.Count != DfsArgs.Length)
                        //    {
                        //        Console.WriteLine(WrongArgument);
                        //    }
                        //    else if (!oriented)
                        //    {
                        //        gr.DFS(arguments[0]);
                        //    }
                        //    break;
                        case Bfs:
                            if (arguments.Count != DfsArgs.Length)
                            {
                                Console.WriteLine(WrongArgument);
                            }
                            else if (!oriented)
                            {
                                gr.NovSet();
                                gr.BFS(arguments[0]);
                            }
                            break;
                    case CanGetTree:
                        if (arguments.Count != CanGetTreeArgs.Length)
                        {
                            Console.WriteLine(WrongArgument);
                        }
                        else if (!oriented)
                        {
                            gr.CanGetTree();
                        }
                        break;
                    case Cycle:
                            if (arguments.Count != CycleArgs.Length)
                            {
                                Console.WriteLine(WrongArgument);
                            }
                            else if (oriented)
                            {
                                orgr.CycleSearch(arguments[0]);
                            }
                            break;
                        case Prima:
                            if (arguments.Count != PrimaArgs.Length)
                            {
                                Console.WriteLine(WrongArgument);
                            }
                            else if (oriented || !weight)
                            {
                                Console.WriteLine("Only for an undirected weighted graph");
                            }
                            else
                            {
                                gr.algPrima();
                            }
                            break;
                        case MinSumDistance:
                        if (arguments.Count != MinSumDistanceArgs.Length)
                        {
                            Console.WriteLine(WrongArgument);
                        }
                        else if (!weight)
                        {
                            Console.WriteLine("Only for weighted");
                        }
                        else if (oriented)
                        {
                            orgr.MinSumDistance();
                        }
                        else
                        {
                            gr.MinSumDistance();
                        }
                            break;
                    case Radius:
                        if (arguments.Count != RadiusArgs.Length)
                        {
                            Console.WriteLine(WrongArgument);
                        }
                        else if (!weight)
                        {
                            Console.WriteLine("Only for weighted");
                        }
                        else if (oriented)
                        {
                            orgr.Radius();
                        }
                        else
                        {
                            gr.Radius();
                        }
                        break;
                    case ShortestPath:
                        if (arguments.Count != ShortestPathArgs.Length)
                        {
                            Console.WriteLine(WrongArgument);
                        }
                        else if (!weight)
                        {
                            Console.WriteLine("Only for weighted");
                        }
                        else if (oriented)
                        {
                            orgr.ShortestPath(arguments[0], arguments[1]);
                        }
                        else
                        {
                            gr.ShortestPath(arguments[0], arguments[1]);
                        }
                        break;
                    case Flow:
                        if (arguments.Count != FlowArgs.Length)
                        {
                            Console.WriteLine(WrongArgument);
                        }
                        else if (!weight)
                        {
                            Console.WriteLine("Only for weighted");
                        }
                        else if (oriented)
                        {
                            orgr.FordFulkerson(arguments[0], arguments[1]);
                        }
                        else
                        {
                            Console.WriteLine("Only for oriented");
                        }
                        break;

                    case Print:
                        if (!oriented)
                        {
                            Console.WriteLine(gr.PrintGraph());
                        }
                        else
                        {
                            Console.WriteLine(orgr.PrintGraph());
                        }
                        break;

                    case Hint:
                            Console.WriteLine(GetHint());
                            break;
                        case Exit:
                            return;
                        default:
                            Console.WriteLine(UnknownCommand);
                            break;
                    }
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //}
            }
        }
        // вывод информации на экран
        private static String GetHint()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ReadGraph).Append(": ").Append(String.Join(", ", ReadGraphArgs)).Append('\n');
            sb.Append(WriteGraph).Append(": ").Append(String.Join(", ", WriteGraphArgs)).Append('\n');
            sb.Append(AddVertex).Append(": ").Append(String.Join(", ", AddVertexArgs)).Append('\n');
            sb.Append(AddEdge).Append(": ").Append(String.Join(", ", AddEdgeArgs)).Append('\n');
            sb.Append(DeleteVertex).Append(": ").Append(String.Join(", ", DeleteVertexArgs)).Append('\n');
            sb.Append(DeleteEdge).Append(": ").Append(String.Join(", ", DeleteEdgeArgs)).Append('\n');
            sb.Append(Degree).Append('\n');
            sb.Append(AdjacentVertices).Append(": ").Append(String.Join(", ", AdjacentVerticesArgs)).Append('\n');
            sb.Append(Union).Append(": ").Append(String.Join(", ", UnionArgs)).Append('\n');
            sb.Append(CanGetTree).Append('\n');
            sb.Append(Cycle).Append(": ").Append(String.Join(", ", CycleArgs)).Append('\n');
            sb.Append(Prima).Append('\n');
            sb.Append(MinSumDistance).Append('\n');
            sb.Append(Radius).Append('\n');
            sb.Append(ShortestPath).Append(": ").Append(String.Join(", ", ShortestPathArgs)).Append('\n');
            sb.Append(Flow).Append(": ").Append(String.Join(", ", FlowArgs)).Append('\n');
            ;
            sb.Append(Hint).Append('\n');
            sb.Append(Exit).Append('\n');

            return sb.ToString();
        }
    }
}
