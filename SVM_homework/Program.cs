using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace SVM_homework
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument train_source = new XmlDocument();
            train_source.Load("train.xml");
            
            //字典 加入一個keyP幫助儲存，如果要分成class要注意
            Dictionary<string, int> collectOfWords = new Dictionary<string, int>();
            int keyPointer = 1;

            List<List<int>> bookList = new List<List<int>>();
            XmlNodeList bookNode = train_source.SelectNodes("/RDF//Text");
            foreach (XmlNode node in bookNode)
            {
                string bookInnerText = node.InnerText;
                char[] spliter={' ',',','.','!','\r','\n','(',')','"','/'};
                string[]  bookWords=bookInnerText.Split(spliter);
                for (int i = 0; i < bookWords.Length; i++)
                {
                    bookWords[i] = Stemmer.Stem(bookWords[i]);
                }
                List<int> WordsIndex=new List<int>();
                //建立字典，並將字詞依字典編號存入wordsIndex
                //List<int> counter = new List<int>();
                for (int i = 0; i < bookWords.Length; i++)
                {
                    //if (bookWords[i].Length > 2 && bookWords[i].Length<10 && !char.IsDigit(bookWords[i][0])) 
                    if(true)
                    {
                        if (collectOfWords.ContainsKey(bookWords[i]) == false)
                        {
                            collectOfWords.Add( bookWords[i],keyPointer);
                            WordsIndex.Add(keyPointer);
                            //counter.Add(1);
                            keyPointer++;
                        }
                        else{
                            WordsIndex.Add(collectOfWords[bookWords[i]]);
                            //counter[collectOfWords[bookWords[i]]-1]++;
                        }

                    }
                }
                bookList.Add(WordsIndex);
            }

            /******
              輸出到theTr
            [label] [index1]:[value1] [index2]:[value2] ...
            +1 1:0.708 2:1 3:1 4:-0.320 5:-0.105 6:-1
            以上是錯的 label是分類標籤 index_t是各種feature value_t是feature的量值
            創新bool list m*n m為樣本總數 n為dic中words的總數
            //
             * ***************************************/
            StreamWriter test = new StreamWriter("./testS.t");
            StreamWriter train = new StreamWriter("./trainS.txt");
            for (int m = 0; m < 5000; m++)
            {
                //n從1開始，和字典統一
                //輸出至test
                System.Console.WriteLine(Convert.ToString(m) + "th data is writed");
                if (m % 1000 < 200)
                {
                    test.Write("+" + m / 1000);

                    

                    for (int n = 1; n < collectOfWords.Count + 1; n++)
                    {
                        if (bookList[m].Contains(n))
                        {
                            test.Write(" " + Convert.ToString(n) + ":1");
                        }
                        else
                        {
                            //test.Write(" " + Convert.ToString(n) + ":0");
                        }
                    }
                    test.WriteLine();
                }
                //輸出至train
                else
                {
                    train.Write("+" + m / 1000);

                    for (int n = 1; n < collectOfWords.Count + 1; n++)
                    {
                        if (bookList[m].Contains(n))
                        {
                            train.Write(" " + Convert.ToString(n) + ":1");
                        }
                        else
                        {
                            //train.Write(" " + Convert.ToString(n) + ":0");
                        }
                    }
                    train.WriteLine();
                }
            }
            test.Close();
            train.Close();
                /******************
                for (int n = 0; n < collectOfWords.Count; n++)
                {
                    if (bookList[i].Contains(k + 1))
                        theRightOne[i, k] = true;
                    else theRightOne[i, k] = false;
                }
                
            }



            for (int m = 0; m < 5000; m++)
            {
                if (m % 1000 < 200)
                {
                    test.Write("+" + m / 1000);
                    for (int n = 0; n < collectOfWords.Count; n++)
                    {
                        test.Write(" " + Convert.ToString(n) + ":");
                        if (theRightOne[m, n]) test.Write("1");
                        else test.Write("0");
                    }
                    test.WriteLine();
                }
                else
                {
                    train.Write("+" + m / 1000);
                    for (int n = 0; n < collectOfWords.Count; n++)
                    {
                        train.Write(" " + Convert.ToString(n) + ":");
                        if (theRightOne[m, n]) train.Write("1");
                        else train.Write("0");
                    }
                    train.WriteLine();
                }

            }
            test.Close();
            train.Close();
            /*******************
             * the Worng one
            StreamWriter sw = new StreamWriter("./rawData.txt");
            int j = 0;
            foreach (List<int> IndexVector in bookList)
            {
                //label
                sw.Write("+"+Convert.ToString((j/1000)+1));
                int k = 1;
                foreach (int x_k in IndexVector)
                {
                    sw.Write(" "+Convert.ToString(k)+":"+Convert.ToString(x_k));
                    k++;
                }
                sw.WriteLine();
                j++;
            }
            StreamWriter test = new StreamWriter("./test.t");
            StreamWriter raw = new StreamWriter("./raw.txt");
            j=0;
            foreach(List<int> IndexVector in bookList)
            {
                //label
                if ((j % 1000) < 200)
                {
                    test.Write("+" + Convert.ToString((j / 1000) + 1));
                    int k = 1;
                    foreach (int x_k in IndexVector)
                    {
                        test.Write(" " + Convert.ToString(k) + ":" + Convert.ToString(x_k));
                        k++;
                    }
                    test.WriteLine();
                    j++;
                    
                }
                else
                {
                    raw.Write("+" + Convert.ToString((j / 1000) + 1));
                    int k = 1;
                    foreach (int x_k in IndexVector)
                    {
                        raw.Write(" " + Convert.ToString(k) + ":" + Convert.ToString(x_k));
                        k++;
                    }
                    raw.WriteLine();
                    j++;
                    
                }
            }
            sw.Close();
            test.Close();
            raw.Close();
             *****************************/
            System.Console.WriteLine("DONE!");
            System.Console.ReadKey();


        }
    }
}
