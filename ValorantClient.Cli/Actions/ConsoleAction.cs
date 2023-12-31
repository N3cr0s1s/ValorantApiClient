﻿using System.Text;

namespace ValorantClient.Cli.Actions
{
    public class ConsoleAction
    {

        public int Halfer { get; set; } = 5;
        private (int List, int Option) _selected = (0,0);
        private readonly string _prompt;
        private readonly List<string[]> _options;
        private (int Left, int Top) _cursor;
        private IDictionary<string, Func<Task>> _actions;

        public ConsoleColor SelectedColor { get; set; } = ConsoleColor.Red;

        public ConsoleAction(string prompt, string[] options,int halfer = 5)
        {
            Halfer = halfer;
            _prompt = prompt;
            _options = SplitArray(options, Halfer);
        }

        public ConsoleAction(string prompt, IDictionary<string, Func<Task>> options,int halfer = 5)
        {
            Halfer = halfer;
            _prompt = prompt;
            _options = SplitArray(options.Keys.ToArray(), Halfer);
            _actions = options;
        }

        public ConsoleAction(string prompt, Dictionary<string, string>.KeyCollection keys,int halfer = 5)
        {
            Halfer = halfer;
            _prompt = prompt;
            _options = SplitArray(keys.ToArray(), Halfer);

        }

        /// <summary>
        /// Split array, to separate into columns
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <param name="inputArray">Array to split</param>
        /// <param name="size">Column size</param>
        /// <returns><see cref="List{T}"/> of new <param name="size">size</param> arrays</returns>
        private List<T[]> SplitArray<T>(T[] inputArray, int size)
        {
            List<T[]> result = new List<T[]>();

            int length = inputArray.Length;
            int numOfArrays = length / size;

            if (length % size > 0)
            {
                numOfArrays++;
            }

            for (int i = 0; i < numOfArrays; i++)
            {
                int start = i * size;
                int end = start + size;

                if (end > length)
                {
                    end = length;
                }

                T[] subArray = new T[size];

                for (int j = start; j < end; j++)
                {
                    subArray[j - start] = inputArray[j];
                }

                if (subArray.Length < size)
                {
                    Array.Resize(ref subArray, size);
                }

                result.Add(subArray);
            }

            return result;
        }

        /// <summary>
        /// Read input to execute an option.
        /// If option don't have action, then return with the option name.
        /// </summary>
        /// <returns>Option name</returns>
        public async Task<string> ReadValueAsync()
        {
            Console.WriteLine(_prompt);
            _cursor = Console.GetCursorPosition();
            string result =  WaitForValue();
            if (_actions is not null)
            {
                Func<Task> task = _actions[result];
                await task();
            }
            return result;
        }

        /// <summary>
        /// This method starts a while cycle,
        /// while <see cref="ConsoleKey.Enter"/> not pressed.
        /// This method responsible for controls as well.
        /// </summary>
        /// <returns>Selected option key</returns>
        private string WaitForValue()
        {
            while (true)
            {
                DrawOptions();
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        MoveOption(0, -1);
                        break;
                    case ConsoleKey.DownArrow:
                        MoveOption(0, 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        MoveOption(-1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        MoveOption(1, 0);
                        break;
                    case ConsoleKey.Enter:
                        if (!string.IsNullOrEmpty(_options[_selected.List][_selected.Option]))
                        {
                            return _options[_selected.List][_selected.Option];
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Move selection relatively
        /// </summary>
        /// <param name="rowChange">Row change</param>
        /// <param name="colChange">Column change</param>
        private void MoveOption(int rowChange, int colChange)
        {
            int newRow = _selected.List + rowChange;
            int newCol = _selected.Option + colChange;

            if (newRow >= 0 && newRow < _options.Count && newCol >= 0 && newCol < _options[newRow].Length && _options[newRow][newCol] != null)
            {
                _selected.List = newRow;
                _selected.Option = newCol;
            }
        }

        /// <summary>
        /// Draw options to console
        /// </summary>
        private void DrawOptions()
        {
            Console.SetCursorPosition(0, _cursor.Top);
            for (int i = 0; i < _options[0].Length; i++)
            {
                for (int j = 0; j < _options.Count; j++)
                {
                    if (_options[j][i] is null) 
                        continue;

                    if (_selected.List == j && _selected.Option == i)
                    {
                        Console.BackgroundColor = SelectedColor;
                    }
                    Console.Write($" [ {_options[j][i]} ] \t");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            _cursor.Top = Console.GetCursorPosition().Top - Halfer;
        }
    }
}
