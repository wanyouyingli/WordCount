using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class Trie
    {
        public static int SIZE = 100;
        public TrieNode root;
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
        public Trie()
        {
            root = new TrieNode();
        }

        public void insert(string str)
        {
            if (str == null || str.Length < 4) return;
            TrieNode node = root;
            char[] word = str.ToCharArray();
            int len = str.Length;
            for(int i = 0; i < len; ++i)
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
    }
    public class CountWord
    {
        char[] tmp = new char[100];
        int tot = 0;
        public List<Word> words = new List<Word>();
        public void Analyse(Trie.TrieNode node)
        {
            if (node.isEnd)
            {
                words.Add(new Word(new string(tmp, 0, tot), node.num));
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
        public void Write2File()
        {
            StreamWriter sw = new StreamWriter(@".\output.txt", false, Encoding.UTF8);
            foreach (Word word in words)
            {
                sw.WriteLine("{0}={1}", word.word, word.cnt);
            }
            sw.Close();
        }
    }
    class AnalyseTop10
    {
        List<Word> words = new List<Word>();
        char[] tmp = new char[100];
        int tot = 0;
        public static int Compare(Word a, Word b)
        {
            return b.cnt - a.cnt;
        }
        public void Analyse(Trie.TrieNode node)
        {
            if (node.isEnd)
            {
                words.Add(new Word(new string(tmp, 0, tot), node.num));
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
        public void Top10()
        {
            words.Sort(Compare);
            for(int i = 0; i < Math.Min(10, words.Count); ++i)
            {
                Console.WriteLine("<" + words[i].word + ">: " + words[i].cnt);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Trie trie = new Trie();
            CountWord word = new CountWord();
            AnalyseTop10 top10 = new AnalyseTop10();
            int character_cnt = 0;
            int line_cnt = 0;
            while(true)
            {
                string raw_input = Console.ReadLine();
                if (raw_input == "666") break;
                if (raw_input != null) line_cnt++;
                character_cnt += raw_input.Length;
                string[] input = raw_input.Split(' ');
                foreach (string s in input)
                {
                    trie.insert(s);
                }
            }
            word.Analyse(trie.root);
            word.Write2File();
            top10.Analyse(trie.root);
            top10.Top10();
            /*
             aaaa aaab aaac aaad aaae aaaf aaag aaba aaaca aada nmsl
             */
        }
    }
}
