using System;
using System.Collections.Generic;
using System.Linq;


namespace Common.Library
{
    #region Hanoi Moves (Facebook interview question)
    /*//
     * There are K pegs. Each peg can hold discs in decreasing order 
     * of radius when looked from bottom to top of the peg. 
     * There are N discs which have radius 1 to N; 
     * Given the initial configuration of the pegs 
     * and the final configuration of the pegs, output the moves required 
     * to transform from the initial to final configuration. 
     * You are required to do the transformations in minimal number of moves.
     * A move consists of picking the topmost disc of any one of the pegs 
     * and placing it on top of anyother peg.
     * At anypoint of time, the decreasing radius property of 
     * all the pegs must be maintained.

     * Constraints: 1<= N <=8, 3<= K <=5

     * InputFormat: N K
     * 2nd line contains N integers (numbers of discs).
     * Each integer in the second line is in the range 1 to K where 
     * the i-th integer denotes the peg to which 
     * disc of radius i is present in the initial configuration.
     * 3rd line denotes the final configuration in a format 
     * similar to the initial configuration.

     * Output Format:
     * The first line contains M - The minimal number of moves required to complete the transformation.
     * The following M lines describe a move, by a peg number to pick from and a peg number to place on.
     * If there are more than one solutions, it's sufficient to output any one of them. 
     * You can assume, there is always a solution with less than 7 moves 
     * and the initial confirguration will not be same as the final one.

     * Sample Input #00:
            2 3
            1 1
            2 2

     * Sample Output #00:
            3
            1 3
            1 2
            3 2

     * Sample Input #01:
            6 4
            4 2 4 3 1 1
            1 1 1 1 1 1

     * Sample Output #01:
            5
            3 1
            4 3
            4 1
            2 1
            3 1

     * NOTE: You need to write the full code taking all inputs are from stdin and outputs to stdout
     * If you are using "Java", the classname is "Solution"
    //*/
    public class Solution
    {
        #region Fields

        const int MIN_K_DSC = 3;
        const int MAX_K_DSC = 5;
        const int MIN_N_PEG = 1;
        const int MAX_N_PEG = 8;
        const int MAX_MOVES = 7;

        int[][] _data;
        int _countMoves = MAX_MOVES;
        int _n_Disc = MIN_K_DSC, _k_Pegs = MIN_N_PEG;
        Stack<int>[] _beginStack;
        Stack<int>[] _finalStack;
        List<string> _result = new List<string>();

        #endregion

        public static string[] Start(string[] inputLines)
        {
            Solution p = new Solution();
            p.ReadInput(' ', inputLines);
            p.Process();
            p._result = p._result.OrderBy((x) => x.Length).ToList();

            string[] result = p.GetResult(p._result);

            return result;
        }

        #region Properties

        #endregion

        #region Functions

        void BuildStack(ref Stack<int>[] stack, int[] discs)
        {
            for(int i = 0; i < stack.Length; i++)
            {
                stack[i] = new Stack<int>();
            }
            //NOTE: add to stack from biggest disc
            for(int i = discs.Length - 1; i >= 0; i--)
            {
                int peg_column = discs[i];
                stack[peg_column - 1].Push(i + 1);
            }
        }

        Stack<int>[] CopyStacks(Stack<int>[] init)
        {
            Stack<int>[] ret = new Stack<int>[init.Length];
            int i = 0;
            foreach(Stack<int> s in init)
            {
                ret[i++] = new Stack<int>(s.Reverse());
            }
            return ret;
        }

        List<int[]> GetValidMoves(Stack<int>[] stacks)
        {
            List<int[]>[] ret = new List<int[]>[stacks.Length];
            for(int i = 0; i < stacks.Length; i++)
            {
                if (stacks[i].Count() != 0)
                {
                    int toMove = stacks[i].Peek();
                    ret[i] = new List<int[]>();

                    for(int j = 0; j < stacks.Length; j++)
                    {
                        if (j == i) continue;

                        int[] subarr = { i + 1, j + 1 }; // assume movable from i to j

                        if (stacks[j].Count != 0)
                        {
                            int targetPeek = stacks[j].Peek();
                            if (targetPeek - 1 == toMove && _finalStack[j].Count != 0 && _finalStack[j].Peek() <= toMove)
                            {
                                ret[i].Clear();
                                ret[i].Add(subarr);
                                break;
                            }
                            if (toMove < targetPeek)
                            {
                                ret[i].Add(subarr);
                            }
                        }
                        else
                        {
                            ret[i].Add(subarr);
                        }
                    }
                }
            }

            List<int[]> append = new List<int[]>();

            foreach(List<int[]> move in ret)
            {
                if (move != null)
                {
                    append.AddRange(move);
                }
            }
            return append;
        }

        bool HasMoreWork(Stack<int>[] init, Stack<int>[] final)
        {
            for(int i = 0; i < init.Length; i++)
            {
                if (init[i].Count != final[i].Count)
                {
                    return true;
                }
                for(int j = 0; j < init[i].Count; j++)
                {
                    if (init[i].ElementAt(j) != final[i].ElementAt(j))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool IsJumping(string moving, int from, int to)
        {
            if (string.IsNullOrEmpty(moving))
            {
                return false;
            }
            char[] c = { '\n' };
            var numbers = moving.Split(c, StringSplitOptions.RemoveEmptyEntries).Last().Split(' ').Select(x => int.Parse(x));

            if (numbers.ElementAt(0) == to && numbers.ElementAt(1) == from)
            {
                return true;
            }
            return false;
          //return (from == oldTo && to == oldFrom && to != 0 )? true : false;
        }

        void Move(Stack<int>[] init, Stack<int>[] final, int from, int to, string moving, int max)//, List<int[]> validmoves)
        {
            if (_countMoves <= max)
            {
                return;
            }
            if (HasMoreWork(init, final))
            {
                List<int[]> validmoves = GetValidMoves(init);

                for(int i = 0; i < validmoves.Count; i++)
                {
                    Stack<int>[] temp = CopyStacks(init);
                    validmoves = GetValidMoves(temp);

                    if (i < validmoves.Count)
                    {
                        from = validmoves.ElementAt(i)[0];
                        to = validmoves.ElementAt(i)[1];
                        /*//
                        if (Jumping(moving, from, to)) continue;
                        //*/
                        MoveDisc(temp, from, to);
                        string str = moving + from + " " + to + "\n";
                        //Console.WriteLine(str);
                        Move(temp, final, from, to, str, max + 1);//, validmoves);
                    }
                }
            }
            else
            {
                _result.Add(moving);
                return;
            }
        }

        void MoveDisc(Stack<int>[] init, int from, int to)
        {
            int popped = init[from - 1].Pop();
            init[to - 1].Push(popped);
        }

        void ReadInput(char splitter, string[] inputLines)
        {
            String[] inputArgs = inputLines[0].Split(splitter);

            if (inputArgs.Length > 0)
            {
                Int32.TryParse(inputArgs[0], out _n_Disc);
            }
            if (inputArgs.Length > 1)
            {
                Int32.TryParse(inputArgs[1], out _k_Pegs);
            }

            _data = new int[2][];

            for(int i = 0; i < 2; i++)
            {
                String[] inputLine = inputLines[i+1].Split(splitter);
                _data[i] = ParsePegsPositionForDiscs(inputLine, _n_Disc, _k_Pegs);
            }

            _beginStack = new Stack<int>[_k_Pegs];
            _finalStack = new Stack<int>[_k_Pegs];

            BuildStack(ref _beginStack, _data[0]);
            BuildStack(ref _finalStack, _data[1]);
        }

        int[] ParsePegsPositionForDiscs(string[] arr, int numbersOfDiscs, int numbersOfPegs)
        {
            int[] ret = new int[numbersOfDiscs];

            for(int i = 0; i < numbersOfDiscs; i++)
            {
                ret[i] = 1;

                if (arr.Length > i)
                {
                    Int32.TryParse(arr[i], out ret[i]);
                }
                if (ret[i] > numbersOfPegs)
                {
                    ret[i] = numbersOfPegs;
                }
                if (ret[i] < 1)
                {
                    ret[i] = 1;
                }
            }
            return ret;
        }

        string[] GetResult(List<string> sorted)
        {
            string[] x = sorted.ElementAt(0).Trim(new char[]{'\n'}).Split('\n');
            string[] result = new string[x.Length + 1];

            System.Diagnostics.Trace.WriteLine(x.Length - 1);

            int index = 0;
            result[index++] = x.Length.ToString();

            foreach(string s in x)
            {
                System.Diagnostics.Trace.WriteLine(s);

                result[index++] = s;
            }

            return result;
        }

        void Process()
        {
            int i = 1;

            while(_result.Count == 0)
            {
                _countMoves = i++;
                Move(_beginStack, _finalStack, 0, 0, "", 0);//, GetValidMoves(_beginStack));
            }
        }

        #endregion
    }

    #endregion

    public class HanoiDisk
    {
        public HanoiDisk(int size)
        {
            _size = (size > 1) ? size : 1;
        }

        private int _size;
        public int Size { get { return _size; } }

        public override string ToString()
        {
            return String.Format("{0}({1})", "Disk", _size);
        }
    }

    public class HanoiMove
    {
        public HanoiMove(HanoiDisk disk, HanoiPeg source, HanoiPeg target)
        {
            _disk = disk;
            _sourcePeg = source;
            _targetPeg = target;
        }

        private HanoiDisk _disk;
        public HanoiDisk Disk
        {
            get { return _disk; }
        }

        private HanoiPeg _sourcePeg;
        public HanoiPeg Source
        {
            get { return _sourcePeg; }
        }

        private HanoiPeg _targetPeg;
        public HanoiPeg Target
        {
            get { return _targetPeg; }
        }

        public override string ToString()
        {
            return String.Format("{0} {1} from {2} to {3}", "Move", _disk, _sourcePeg, _targetPeg);
        }
    }

    public class HanoiPeg : List<HanoiDisk>
    {
        public HanoiPeg(int sequence) : base()
        {
            if (sequence >= 0)
            {
                _pegNumber = sequence;
            }
        }

        public string Name
        {
            get { return _pegNumber.ToString(); }
        }

        private int _pegNumber = 0;
        public int PegNumber
        {
            get { return _pegNumber; }
        }

        public HanoiDisk Pop()
        {
            if (this.Count > 0)
            {
                var disk = this.FirstOrDefault();
                this.RemoveAt(0);
                return disk;
            }
            return null;
        }
        public void Push(HanoiDisk disk)
        {
            if (disk == null) return;

            this.Insert(0, disk);
        }

        public override string ToString()
        {
            return String.Format("{0}[{1}])", "Peg", _pegNumber);
        }
    }

    /// <summary>
    /// The Tower of Hanoi, T(n): n-disk 3-peg problem can be solved in 2^n-1 moves
    /// </summary>
    public class HanoiTower
    {
        public const int MAX_PEGS = Int16.MaxValue;
        public const int MAX_SIZE = Int16.MaxValue;
        public const int MIN_PEGS = 3;
        public const int MIN_SIZE = 1;

        public HanoiTower(int diskNumbers, int pegNumbers = MIN_PEGS)
        {
            if (diskNumbers >= MIN_SIZE && diskNumbers <= MAX_SIZE)
            {
                _diskNumbers = diskNumbers;
            }
            if (pegNumbers >= MIN_PEGS && pegNumbers <= MAX_PEGS)
            {
                _pegNumbers = pegNumbers;
            }
            _pegs = new HanoiPeg[_pegNumbers];

            for(int i = 0; i < _pegNumbers; i++)
            {
                _pegs[i] = new HanoiPeg(i);
            }

            Moves = new List<HanoiMove>();
            Reset();
        }

        #region Fields
        private int _diskNumbers = MIN_SIZE;
        private int _pegNumbers = MIN_PEGS;
        private HanoiPeg[] _pegs;

        #endregion

        #region Properties

        public int DiskNumbers { get { return _diskNumbers; } }
        public int PegNumbers { get { return _pegNumbers; } }

        public List<HanoiMove> Moves { get; private set; }

        #endregion

        #region Methods

        public virtual void MoveAll(bool reset = false)
        {
            if (reset) Reset();

            MoveDisks(_diskNumbers, _pegs[0], _pegs[2], _pegs[1]);
        }

        public virtual void MoveDisks()
        {
            for(int x = 0; x < (1 << _diskNumbers); x++)
            {
                int frPegIndex = (x & x - 1) % 3;
                int toPegIndex = (x | x - 1) % 3;

                MoveOne(_pegs[frPegIndex], _pegs[toPegIndex]);
            }
        }

        public virtual void MoveDisks(int diskNumbers, HanoiPeg fromPeg, HanoiPeg toPeg, HanoiPeg viaPeg)
        {
            if (diskNumbers == 1)
            {
                MoveOne(fromPeg, toPeg);
            }
            else // recursive moves
            {
                MoveDisks(diskNumbers - 1, fromPeg, viaPeg, toPeg);
                MoveOne(fromPeg, toPeg);
                MoveDisks(diskNumbers - 1, viaPeg, toPeg, fromPeg);
            }
        }

        public void MoveOne(HanoiPeg fromPeg, HanoiPeg toPeg)
        {
            HanoiDisk disk = fromPeg.Pop();
            HanoiDisk peek = toPeg.FirstOrDefault();

            if (disk != null && (peek == null || disk.Size <= peek.Size))
            {
                toPeg.Push(disk);

                HanoiMove move = new HanoiMove(disk, fromPeg, toPeg);
                Moves.Add(move);
            }
        }

        public virtual void Reset()
        {
            for(int x = 0; x < _pegNumbers; x++)
            {
                _pegs[x].Clear();
            }
            for(int i = _diskNumbers; i > 0; i--)
            {
                var disk = new HanoiDisk(i);
                _pegs[0].Push(disk);
            }
        }

        public override string ToString()
        {
            return String.Format("{0}({1},{2})", "Tower", _diskNumbers, _pegNumbers);
        }

        #endregion

    }

}
