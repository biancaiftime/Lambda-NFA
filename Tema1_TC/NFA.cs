using System;
using System.Collections.Generic;
using System.Text;

namespace Tema1_TC
{
    class NFA
    {
        public int NumberOfStates { get; set; }

        public List<int> States { get; set; }

        public int InputState { get; set; }

        public int NumberOfFinalStates { get; set; }

        public List<int> FinalStates { get; set; }

        public List<string> InputSymbols { get; set; }

        public List<string> OutputSymbols { get; set; }

        public Dictionary<int, List<Tuple<string, int, string>>> Transitions { get; set; }



        public static NFA readFromFile(string filePath) 
        {
            var nfa = new NFA();
            string[] lines = System.IO.File.ReadAllLines(@filePath);

            //first line has the number of states
            nfa.NumberOfStates = Int32.Parse(lines[0]);

            //second line has the list of states
            var states = new List<string>(lines[1].Split(' '));
            nfa.States = new List<int>();
            states.ForEach(s => { nfa.States.Add(Int32.Parse(s)); });

            //third line has the input state
            nfa.InputState = Int32.Parse(lines[2]);

            //fourth line has the number of final states
            nfa.NumberOfFinalStates = Int32.Parse(lines[3]);

            //the list of final states
            var finalStates = new List<string>(lines[4].Split(' '));
            nfa.FinalStates = new List<int>();
            finalStates.ForEach(s => { nfa.FinalStates.Add(Int32.Parse(s)); });

            //input symbols
            nfa.InputSymbols = new List<string>(lines[5].Split(' '));

            //output symbols
            nfa.OutputSymbols = new List<string>(lines[6].Split(' '));

            //transitions

            nfa.Transitions = new Dictionary<int, List<Tuple<string, int, string>>>();
            var numberOfTransitons = Int32.Parse(lines[7]);

            for(int i = 1; i<= numberOfTransitons; i++)
            {
                var transition = lines[i + 7].Split(' ');
                if( nfa.Transitions.TryGetValue(Int32.Parse(transition[0]), out var list) )
                {
                    list.Add(new Tuple<string, int, string>(transition[1], Int32.Parse(transition[2]), transition[3]));
                    nfa.Transitions[Int32.Parse(transition[0])] = list;
                }
                else
                {
                    var elem = new Tuple<string, int, string>(transition[1], Int32.Parse(transition[2]), transition[3]);
                    var newList = new List<Tuple<string, int, string>>();
                    newList.Add(elem);
                    nfa.Transitions.Add(Int32.Parse(transition[0]), newList);
                }
            }

            return nfa;
        }
        // string1 : what's left from the initial word, int: the current state, string2: the output word
        public void SolvePerState(string current, string word, List<Tuple<string, int, string>> input, out List<Tuple<string, int, string>> output )
        {
            var list = new List<Tuple<string, int, string>>();
            input.ForEach(x => {
                if (x.Item1.Equals(current[0].ToString())) list.Add(new Tuple<string, int, string>(current.Substring(1),x.Item2, word + x.Item3));
                else if(x.Item1.Equals("lamda")) list.Add(new Tuple<string, int, string>(current, x.Item2, word + x.Item3));
            });
            output = list;
        }

        public List<Tuple<int, string>> Output(string word)
        {
            var result = new List<Tuple<int, string>>();
            SolvePerState(word, "", Transitions.GetValueOrDefault(InputState), out var firstResult);
            Queue<Tuple<string, int, string>> queue = new Queue<Tuple<string, int, string>>();
            HashSet<Tuple<string, int, string>>  data = new HashSet<Tuple<string, int, string>>();
            firstResult.ForEach(x => { queue.Enqueue(x); data.Add(x); });
            while(queue.Count != 0)
            {
                var x = queue.Dequeue();
                if (String.IsNullOrEmpty(x.Item1) && FinalStates.Contains(x.Item2)) result.Add(new Tuple<int, string>(x.Item2, x.Item3));
                else if(!String.IsNullOrEmpty(x.Item1) && Transitions.ContainsKey(x.Item2))
                {
                    SolvePerState(x.Item1, x.Item3, Transitions.GetValueOrDefault(x.Item2), out firstResult);
                    firstResult.ForEach(res =>
                    {
                        if (!data.Contains(res))
                        {
                            queue.Enqueue(res);
                            data.Add(res);
                        }
                    });
                }
            }
            return result;
        }
    }
}
