using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AnalyseTop10;
using CountWord;

namespace WordAnalyse
{
    public struct Word
    {

        public string word;
        public int cnt;
        public Word(string a, int b)
        {
            word = a;
            cnt = b;
        }
    }
    public static class Global
    {
        public static List<Word> words = new List<Word>();
        public static List<Word> top10words = new List<Word>();
    }
    public class Priority_Queue<T>
    {
        IComparer<T> comparer;
        T[] heap;

        public int Count { get; private set; }
        public Priority_Queue() { }
        public Priority_Queue(IComparer<T> comparer, int capcity)
        {
            this.comparer = (comparer == null) ? Comparer<T>.Default : comparer;
            this.heap = new T[capcity];
        }

        public void Push(T v)
        {
            if (Count >= heap.Length) Array.Resize(ref heap, Count * 2);
            heap[Count] = v;
            SiftUp(Count++);
        }

        public T Pop()
        {
            var v = Top();
            heap[0] = heap[--Count];
            if (Count > 0) SiftDown(0);
            return v;
        }

        public T Top()
        {
            if (Count > 0) return heap[0];
            throw new InvalidOperationException("优先队列为空!");
        }

        void SiftUp(int n)
        {
            var v = heap[n];
            for (var n2 = n / 2; n > 0 && comparer.Compare(v, heap[n2]) > 0; n = n2, n2 /= 2)
                heap[n] = heap[n2];
            heap[n] = v;
        }

        void SiftDown(int n)
        {
            var v = heap[n];
            for (var n2 = n * 2; n2 < Count; n = n2, n2 *= 2)
            {
                if (n2 + 1 < Count && comparer.Compare(heap[n2 + 1], heap[n2]) > 0) n2++;
                if (comparer.Compare(v, heap[n2]) >= 0) break;
                heap[n] = heap[n2];
            }
            heap[n] = v;
        }
    }
    public class Trie
    {
        private static int SIZE = 100;
        public TrieNode root;
        public List<AnalyseTop10.Word> words = new List<AnalyseTop10.Word>();
        private string pre_path = null;

        public class TrieNode
        {
            public int num;
            public TrieNode[] son;
            public bool isEnd;
            public char val;

            public TrieNode()
            {
                num = 0;
                son = new TrieNode[SIZE];
                isEnd = false;
            }
        }
        public Trie(string path)
        {
            if(pre_path != path)
            {
                root = new TrieNode();
                string str = ReadFile.cal(path).Replace('\r', ' ').Replace('\n', ' ');
                string[] input = str.Split(' ');
                for (int i = 0; i < input.Length; ++i)
                {
                    insert(input[i]);
                }
                Analyse(root);
                pre_path = path;
            }
        }

        public void insert(string str)
        {
            if (str == null || str.Length < 4) return;
            TrieNode node = root;
            char[] word = str.ToCharArray();
            int len = str.Length;
            for (int i = 0; i < len; ++i)
            {
                if (word[i] >= 'A' && word[i] <= 'Z') word[i] = (char)(word[i] - 'A' + 'a');
                if (i <= 3 && !(word[i] >= 'a' && word[i] <= 'z')) return;
                int pos = word[i] - 33;
                if (node.son[pos] == null)
                {
                    node.son[pos] = new TrieNode();
                    node.son[pos].val = word[i];
                }
                node = node.son[pos];
            }
            node.num++;
            node.isEnd = true;
        }

        char[] tmp = new char[100];
        int tot = 0;

        public void Analyse(TrieNode node)
        {
            if (node.isEnd)
            {
<<<<<<< HEAD
                words.Add(new AnalyseTop10.Word(new string(tmp, 0, tot), node.num));
=======
                Global.words.Add(new Word(new string(tmp, 0, tot), node.num));
>>>>>>> 56deae3be9013c6f057f9e4cf8b394bd8296ce57
            }
            for (int i = 0; i < Trie.SIZE; ++i)
            {
                if (node.son[i] != null)
                {
                    tmp[tot++] = node.son[i].val;
                    Analyse(node.son[i]);
                    tot--;
                }
            }
        }
<<<<<<< HEAD
=======
    }
    public class CountWord
    {
        public CountWord(Trie trie)
        {
            if (Global.words.Count == 0)
            {
                trie.Analyse(trie.root);
            }
            Write2File();
        }
        private void Write2File()
        {
            StreamWriter sw = new StreamWriter(@".\output.txt", false, Encoding.UTF8);
            foreach (Word word in Global.words)
            {
                sw.WriteLine("{0} -> {1}", word.word, word.cnt);
            }
            sw.Close();
        }
>>>>>>> 56deae3be9013c6f057f9e4cf8b394bd8296ce57
    }
    public static class ReadFile
    {
<<<<<<< HEAD
        public static string cal(string str)
        {
            string output = null;
            try
            {
                StreamReader sr = new StreamReader(str, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    output += line;
=======
        public AnalyseTop10(Trie trie)
        {
            if (Global.words.Count == 0)
            {
                trie.Analyse(trie.root);
            }
            Top10();
        }
        private static Compare _com = null;
        public static IComparer<Word> com
        {
            get
            {
                if (_com == null)
                {
                    _com = new Compare();
>>>>>>> 56deae3be9013c6f057f9e4cf8b394bd8296ce57
                }
                return _com;
            }
<<<<<<< HEAD
            catch
            {
                Console.WriteLine("文件不存在!");
=======
        }
        private class Compare : IComparer<Word>
        {
            int IComparer<Word>.Compare(Word x, Word y)
            {
                return (x.cnt == y.cnt) ? y.word.CompareTo(x.word) : x.cnt.CompareTo(y.cnt);
>>>>>>> 56deae3be9013c6f057f9e4cf8b394bd8296ce57
            }
            return output;
        }
        private void Top10()
        {
            Priority_Queue<Word> Q = new Priority_Queue<Word>(com, 11);
            for (int i = 0; i < Global.words.Count; ++i)
            {
                if (Q.Count < 10)
                {
                    Q.Push(Global.words[i]);
                }
                else if (Q.Top().cnt < Global.words[i].cnt || (Q.Top().cnt == Global.words[i].cnt && Q.Top().word.CompareTo(Global.words[i].word) > 0))
                {
                    Q.Pop();
                    Q.Push(Global.words[i]);
                }
            }
            Console.WriteLine(Q.Count);
            while (Q.Count != 0)
            {
                Global.top10words.Add(Q.Top());
                Q.Pop();
            }
        }
    }
    class CountCharacter
    {
        public CountCharacter() { }
        public int Cal(string str)
        {
            str = str.Replace("\r\n", "\n");
            return str.Length;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
<<<<<<< HEAD

           if (args.Count() != 0)
            {
                Trie trie = null;
                int init_output_num = 10, words_len = -1;
                string output_path = @".\output.txt", path = null;
                for(int i = 0; i < args.Length; ++i) { 
                    if (args[i] == "-i") path = args[++i];
                    else if (args[i] == "-n") init_output_num = int.Parse(args[++i]);
                    else if (args[i] == "-o") output_path = args[++i];
                    else if (args[i] == "-m") words_len = int.Parse(args[++i]);
                }
                if(path == null || File.Exists(path) == false)
                {
                    Console.WriteLine("文件不存在!");
                    return;
                }
                trie = new Trie(path);
                Analyse analysetop10 = new Analyse();
                CountWord.CountWord countWord = new CountWord.CountWord();
                CountCharacter.CountChar countChar = new CountCharacter.CountChar();
                Console.WriteLine("characters: " + countChar.Cnt(path));
                Console.WriteLine("words: " + countWord.cnt(trie.words, output_path));
                analysetop10.top10(trie.words, init_output_num);
                foreach(AnalyseTop10.Word word in trie.words)
=======
            Trie trie = new Trie();
            CountCharacter countCharacter = new CountCharacter();
            int character_cnt = 0;
            int line_cnt = 0;
            while (true)
            {
                string raw_input = Console.ReadLine();
                if (raw_input == "666") break;
                if (raw_input != null) line_cnt++;
                character_cnt += countCharacter.Cal(raw_input);
                string[] input = raw_input.Split(' ');
                foreach (string s in input)
>>>>>>> 56deae3be9013c6f057f9e4cf8b394bd8296ce57
                {
                    Console.WriteLine("<{0}>: {1}", word.word, word.cnt);
                }
            }
<<<<<<< HEAD
=======
            /*
             aaaa aaab aaac aaad aaae aaaf aaag aaba aaaca aada nmsl aaag aaaca
             */
            CountWord countWord = new CountWord(trie);
            AnalyseTop10 top10 = new AnalyseTop10(trie);
            Console.WriteLine("characters: " + character_cnt);
            Console.WriteLine("words: " + Global.words.Count);
            Console.WriteLine("lines: " + line_cnt);
            foreach (Word x in Global.top10words)
            {
                Console.WriteLine("<{0}>: {1}", x.word, x.cnt);
            }
>>>>>>> 56deae3be9013c6f057f9e4cf8b394bd8296ce57
        }
    }
}
