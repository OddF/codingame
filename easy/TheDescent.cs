using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * The while loop represents the game.
 * Each iteration represents a turn of the game
 * where you are given inputs (the heights of the mountains)
 * and where you have to print an output (the index of the mountain to fire on)
 * The inputs you are given are automatically updated according to your last actions.
 **/
class Player
{
    static void Main(string[] args)
    {

        // game loop
        while (true)
        {
            var toShootHeight = 0;
            var toShootIndex = -1;

            for (int i = 0; i < 8; i++)
            {
                var height = int.Parse(Console.ReadLine());
                if (height > toShootHeight) {
                    toShootHeight = height;
                    toShootIndex = i;
                }
            }

            Console.WriteLine(toShootIndex); // The index of the mountain to fire on.
        }
    }
}
