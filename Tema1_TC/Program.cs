using System;

namespace Tema1_TC
{
    class Program
    {
        static void Main(string[] args)
        {
            var nfa = NFA.readFromFile("E:\\Projects\\Laboratoare an III sem II\\Tehnici de Compilare\\Tema1_TC\\input_data.txt");
            var words = System.IO.File.ReadAllLines(@"E:\Projects\Laboratoare an III sem II\Tehnici de Compilare\Tema1_TC\words.txt");
            foreach(string word in words)
            {
                Console.WriteLine("For the word " + word + ":");
                var list = nfa.Output(word);
                list.ForEach(l => {
                    Console.WriteLine("final state: "+ l.Item1 + ", output: " + l.Item2);
                });
            }
        }
    }
}
