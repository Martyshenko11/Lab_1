using System;
using System.Collections.Generic;

namespace Lab_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Board first_board = new Board(8);
            first_board.Generator();
            first_board.Make();
            Console.WriteLine(first_board.GetConflicts());
            Anew_star new_star = new Anew_star();
            List<bool[,]> q = new List<bool[,]>();
            List<bool[,]> visitedpoints = new List<bool[,]>();
            while (first_board.GetConflicts() != 0)
            {
                first_board = new_star.Search(first_board, ref q, ref visitedpoints);
                first_board.Make();
                Console.WriteLine(first_board.GetConflicts());
                Console.WriteLine();
            }
            Console.WriteLine("Visited: " + visitedpoints.Count);
            Console.ReadKey();

            Board second_board = new Board(8);
            second_board.Generator();
            second_board.Make();
            Console.WriteLine(second_board.GetConflicts());
            IDS IDS = new IDS();
            List<bool[,]> visited = new List<bool[,]>();
            Queue<bool[,]> que = new Queue<bool[,]>();
            bool[,] _second_map = new bool[second_board.Size, second_board.Size];
            _second_map = Extra.Print(second_board.Size, second_board.Map);
            que.Enqueue(_second_map);
            while (second_board.GetConflicts() != 0)
            {
                second_board = IDS.Search(second_board, ref que, ref visited);
                second_board.Make();
                Console.WriteLine(second_board.GetConflicts());
                Console.WriteLine();
                System.Threading.Thread.Sleep(100);
            }
            Console.WriteLine("Visited: " + visited.Count);
            Console.ReadKey();
        }
    }

    public class Board
    {
        public int Size { get; set; }
        public bool[,] Map { get; set; }
        public Board()
        {
            Size = 8;
            Map = new bool[Size, Size];
        }
        public Board(int _size)
        {
            Size = _size;
            Map = new bool[_size, _size];
        }
        public Board(int _size, bool[,] _map)
        {
            Size = _size;
            Map = _map;
        }



        public void Make()
        {
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    if (this.Map[i, j])
                    {
                        Console.Write(" 1 ");
                    }
                    else
                    {
                        Console.Write(" o ");
                    }
                }
                Console.WriteLine();
            }
        }

        public int GetConflicts()
        {
            int conf_num = 0;

            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    for (int row = 0; row < this.Size; row++)
                    {
                        if (this.Map[i, j] && this.Map[i, row] && row != j)
                        {
                            conf_num++;
                        }
                        if (i - row >= 0 && j - row >= 0 && this.Map[i, j] && this.Map[i - row, j - row] && row != 0)
                        {
                            conf_num++;
                        }
                        if (i - row >= 0 && j + row < this.Size && this.Map[i, j] && this.Map[i - row, j + row] && row != 0)
                        {
                            conf_num++;
                        }
                        if (i + row < this.Size && j - row >= 0 && this.Map[i, j] && this.Map[i + row, j - row] && row != 0)
                        {
                            conf_num++;
                        }
                        if (i + row < this.Size && j + row < this.Size && this.Map[i, j] && this.Map[i + row, j + row] && row != 0)
                        {
                            conf_num++;
                        }
                    }
                }
            }
            return (conf_num / 2);
        }

        public static int GetConflicts(int size, bool[,] board_map)
        {
            int conf_num = 0;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int row = 0; row < size; row++)
                    {
                        if (board_map[i, j] && board_map[i, row] && row != j)
                        {
                            conf_num++;
                        }
                        if (i - row >= 0 && j - row >= 0 && board_map[i, j] && board_map[i - row, j - row] && row != 0)
                        {
                            conf_num++;
                        }
                        if (i - row >= 0 && j + row < size && board_map[i, j] && board_map[i - row, j + row] && row != 0)
                        {
                            conf_num++;
                        }
                        if (i + row < size && j - row >= 0 && board_map[i, j] && board_map[i + row, j - row] && row != 0)
                        {
                            conf_num++;
                        }
                        if (i + row < size && j + row < size && board_map[i, j] && board_map[i + row, j + row] && row != 0)
                        {
                            conf_num++;
                        }
                    }
                }
            }
            return (conf_num / 2);
        }

        public void Generator()
        {
            Random rnd = new Random();
            int i = 0;
            for (int j = 0; j < this.Size; j++)
            {
                i = rnd.Next(0, 8);
                this.Map[i, j] = true;
            }
            return;
        }
    }

    public class IDS
    {
        public Board Search(Board map, ref Queue<bool[,]> que, ref List<bool[,]> visited)
        {
            bool enter_key = true;
            while (enter_key)    //
            {
                enter_key = false;
                foreach (bool[,] matrix in visited)
                {
                    if (Extra.Idem(map.Size, que.Peek(), matrix))
                    {
                        que.Dequeue();
                        enter_key = true;
                    }
                }
            }

            bool[,] _empty = new bool[map.Size, map.Size];
            _empty = Extra.Print(map.Size, que.Peek());
            bool[,] _blank = new bool[map.Size, map.Size];

            for (int j = 0; j < map.Size; j++)
            {
                _empty = Extra.Print(map.Size, que.Peek());
                for (int i = 0; i < map.Size; i++)
                {
                    _empty[i, j] = false;
                }

                for (int i = 0; i < map.Size; i++)
                {
                    _empty[i, j] = true;
                    if (map.Map[i, j] != true)
                    {
                        que.Enqueue(Extra.Print(map.Size, _empty));
                    }
                    _empty[i, j] = false;
                }
            }

            visited.Add(que.Peek());
            return new Board(map.Size, que.Dequeue());
        }
    }

    public class Anew_star
    {
        public Board Search(Board map, ref List<bool[,]> q, ref List<bool[,]> visitedpoints)
        {
            bool[,] _empty = new bool[map.Size, map.Size];
            _empty = Extra.Print(map.Size, map.Map);
            int cnt = map.GetConflicts();
            bool[,] _blank = new bool[map.Size, map.Size];

            for (int j = 0; j < map.Size; j++)
            {
                _empty = Extra.Print(map.Size, map.Map);
                for (int i = 0; i < map.Size; i++)
                {
                    _empty[i, j] = false;
                }

                for (int i = 0; i < map.Size; i++)
                {
                    _empty[i, j] = true;
                    if (map.Map[i, j] != true)
                    {
                        q.Add(Extra.Print(map.Size, _empty));
                    }
                    _empty[i, j] = false;
                }
            }

            bool key = true;
            while (key)
            {
                key = false;
                int counter = 0;
                int clk = 0;
                foreach (bool[,] node in q)
                {
                    counter++;
                    if (cnt >= Board.GetConflicts(map.Size, node))
                    {
                        cnt = Board.GetConflicts(map.Size, node);
                        _blank = Extra.Print(map.Size, node);
                        clk = counter;
                    }

                }

                foreach (bool[,] node in visitedpoints)
                {
                    if (Extra.Idem(map.Size, _blank, node))
                    {
                        key = true;
                    }
                }
                visitedpoints.Add(_blank);
                q.RemoveAt(clk - 1);
            }

            return new Board(map.Size, _blank);
        }
    }

    public class Extra
    {
        public static bool[,] Print(int size, bool[,] fst)
        {
            bool[,] array = new bool[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    array[i, j] = fst[i, j];
                }
            }
            return array;
        }

        public static bool Idem(int size, bool[,] fst, bool[,] scd)
        {
            int count = 0;
            bool[,] array = new bool[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (fst[i, j] == scd[i, j])
                        count++;
                }
            }
            if (count == (size * size))
            {
                return true;
            }
            return false;
        }
    }
}

