using System.Text;

namespace ValorantClient.Cli.Actions
{
    public class ConsoleAction
    {

        private const int _halfer = 5;
        private (int List, int Option) _selected = (0,2);
        private readonly string _prompt;
        private readonly List<string[]> _options;
        private (int Left, int Top) _cursor;

        public ConsoleColor SelectedColor { get; set; } = ConsoleColor.Red;

        public ConsoleAction(string prompt, string[] options)
        {
            _prompt = prompt;
            _options = SplitArray(options, _halfer);
        }

        public ConsoleAction(string prompt, Dictionary<string, string>.KeyCollection keys)
        {
            _prompt = prompt;
            _options = SplitArray(keys.ToArray(),_halfer);
        }

        public List<string[]> SplitArray(string[] inputArray, int size)
        {
            List<string[]> result = new List<string[]>();

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

                string[] subArray = new string[size];

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
        public string ReadValue()
        {
            Console.WriteLine(_prompt);
            _cursor = Console.GetCursorPosition();
            return WaitForValue();
        }

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
                        if (!string.IsNullOrEmpty(_options[_selected.List][_selected.Option])) // Ellenőrzi, hogy az opció nem üres
                        {
                            return _options[_selected.List][_selected.Option];
                        }
                        break;
                }
            }
        }

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

        private void DrawOptions()
        {
            Console.SetCursorPosition(_cursor.Left, _cursor.Top);

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
        }
    }
}
