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

        char[] tmp = new char[100];
        int tot = 0;

        public void Analyse(TrieNode node)
        {
            if (node.isEnd)
            {
                words.Add(new AnalyseTop10.Word(new string(tmp, 0, tot), node.num));
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
    }
    public static class ReadFile
    {
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
                }
            }
            catch
            {
                Console.WriteLine("文件不存在!");
            }
            return output;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

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
                {
                    Console.WriteLine("<{0}>: {1}", word.word, word.cnt);
                }
            }
        }
    }
}
