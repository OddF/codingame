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
    static void Main(string[] args)
    {
        var gatewayIndexes = new List<int>();
        var links = new Dictionary<int, List<int?>>();


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

            AddToDictonaryList(links, N1, N2);
            AddToDictonaryList(links, N2, N1);
        }

        for (int i = 0; i < E; i++)
        {
            gatewayIndexes.Add(int.Parse(Console.ReadLine()));
        }

        var turn = 0;
        // game loop
        while (true)
        {
            int SI = int.Parse(Console.ReadLine()); // The index of the node on which the Skynet agent is positioned this turn
            var target = FindTarget(links, gatewayIndexes, SI);

            // Skynet has a winning move available. Sever the link.
            if (target != null)
            {
                Remove(links, SI, (int)target);
            }
            // Skynet has no winning moves available. Disconnect random node from gateway
            else
            {
                foreach (var a in gatewayIndexes)
                {
                    if (links[a].Any())
                    {
                        Remove(links, a, (int)links[a].First());
                        break;
                    }
                }
            }

        }
    }

    private static void AddToDictonaryList(Dictionary<int, List<int?>> source, int key, int value)
    {
        if (source.TryGetValue(key, out var l))
        {
            l.Add(value);
        }
        else
        {
            source.Add(key, new List<int?> { value });
        }
    }

    // returns first linked gateway node from the given position (or null).
    private static int? FindTarget(Dictionary<int, List<int?>> links, List<int> gateways, int position)
    {
        return links[position].FirstOrDefault(option => gateways.Any(g => g == option));
    }

    // Severs a link between two nodes and removes their representation in the source dictionary.
    // This ensures that we don't sever links twice.
    private static void Remove(Dictionary<int, List<int?>> source, int a, int b)
    {
        RemoveEntryFromLinks(source, a, b);
        RemoveEntryFromLinks(source, b, a);

        Console.Error.WriteLine("remove: " + a + " " + b);
        Console.WriteLine(a + " " + b);
    }

    private static void RemoveEntryFromLinks(Dictionary<int, List<int?>> source, int key, int value)
    {
        if (source.TryGetValue(key, out var list))
        {
            list.Remove(value);
        }
    }
}
