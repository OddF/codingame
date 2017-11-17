/* 83% solution */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    static Dictionary<int, Node> Nodes = new Dictionary<int, Node>();
    static List<int> gatewayIndexes = new List<int>();

    class Node
    {
        public Node (int index)
        {
            Index = index;
        }

        public bool IsGateway { get; set; }
        public int Index { get; set; }
        public List<int?> ConnectedTo { get; set; }

        public IEnumerable<int?> GatewayConnections => ConnectedTo.Where(x => gatewayIndexes.Any(y => y == x));
        public int NumberOfExits => GatewayConnections.Count();

    }

    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int N = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
        int L = int.Parse(inputs[1]); // the number of links
        int E = int.Parse(inputs[2]); // the number of exit gateways


        for (int i = 0; i < L; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int N1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
            int N2 = int.Parse(inputs[1]);

            AddLink(N1, N2);
            AddLink(N2, N1);
        }

        for (int i = 0; i < E; i++)
        {
            var index = int.Parse(Console.ReadLine());
            gatewayIndexes.Add(index);
            Nodes[index].IsGateway = true;
        }

        // game loop
        while (true)
        {
            int SI = int.Parse(Console.ReadLine()); // The index of the node on which the Skynet agent is positioned this turn
            var positionNode = Nodes[SI];
            var target = FindTarget(positionNode);

            // Skynet has a winning move available. Sever the link.
            if (target != null)
            {
                Remove(target[0], target[1]);
            }
            // Skynet has no winning moves available. Disconnect node with most connections to gateway
            else
            {
                var node = Nodes
                    .Select(x => x.Value)
                    .OrderBy(x => x.NumberOfExits)
                    .ThenBy(x => x.ConnectedTo.Count())
                    .Last();

                Remove(node.Index, (int)(node.GatewayConnections.FirstOrDefault() ?? node.ConnectedTo.FirstOrDefault()));
            }

        }
    }

    private static void AddLink(int from, int to)
    {
        if (Nodes.TryGetValue(from, out var node))
        {
            node.ConnectedTo.Add(to);
        }
        else
        {
            Nodes.Add(from, new Node(from) { ConnectedTo = new List<int?> { to } });
        }
    }

    // returns first linked gateway node from the given position (or null).
    private static int[] FindTarget(Node node)
    {
        if (node.GatewayConnections.Any()) {
            return new int[] { node.Index, (int)node.GatewayConnections.First() };
        }
        return node.ConnectedTo
            .Select(connectedTo => Nodes[(int)connectedTo])
            .Where(n => n.NumberOfExits > 1)
            .Select(x => new int[] { x.Index, (int)x.GatewayConnections.First() })
            .FirstOrDefault();
    }

    // Severs a link between two nodes and removes their representation in the source dictionary.
    // This ensures that we don't sever links twice.
    private static void Remove(int to, int from)
    {
        RemoveConnectionFromNode(to, from);
        RemoveConnectionFromNode(from, to);

        Console.WriteLine(to + " " + from);
    }

    private static void RemoveConnectionFromNode(int index, int to)
    {
        if (Nodes.TryGetValue(index, out var node))
        {
            node.ConnectedTo.Remove(to);
        }
    }
}
