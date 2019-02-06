using System;
using AVLTree;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string[] values = { "andrew", "kisya", "kotik", "evgen", "wkola", "ryry", "kiol",
                "sam", "slava", "baking", "brut", "truba", "lul", "poggers", "pog", "pogu",
                "strong", "easy", "hard", "imp"};
                var tree = new AVLTree<string>();
                Random rand = new Random();
                string entered = "";
                while (true)
                {
                    Console.Clear();
                    tree.Draw();

                    Console.SetCursorPosition(0, 15);
                    Console.WriteLine("Press 0 for restart");
                    Console.WriteLine("Entered values: " + entered);
                    Console.Write("Enter the value: ");
                    int value = Convert.ToInt32(Console.ReadLine());
                    if (value == 0)
                        break;
                    tree.Add(value, values[rand.Next(1,20)]);
                    entered += value.ToString() + " - " + tree.At(value) + ", ";
                }
            }
        }
    }
}