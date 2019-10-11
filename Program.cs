using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace TW
{
    class Program
    {
        public static void Main(string[] args)
        {
            int count;
            int wordCount;
            int sentenceCount;
            dynamic listVowelCount;
            dynamic json;


            //Pass 1 
            json = GetDataFromTWServer();
            if (json.message != "You have already finished all the stages, someone from TW will call you!")
            {
                count = json.text.Value.Length;
                string json1 = new JavaScriptSerializer().Serialize(new
                {
                    count = count
                });
                PostToTWServer(json1);


                //Pass 2
                json = GetDataFromTWServer();
                wordCount = countWordsInString(json.text.Value);
                string json2 = new JavaScriptSerializer().Serialize(new
                {
                    wordCount = wordCount
                });
                PostToTWServer(json2);


                //Pass 3
                json = GetDataFromTWServer();
                sentenceCount = countSentenceinPara(json.text.Value);
                string json3 = new JavaScriptSerializer().Serialize(new
                {
                    sentenceCount = sentenceCount
                });
                PostToTWServer(json3);


                //Pass 4
                json = GetDataFromTWServer();
                listVowelCount = countandReturnListofVowel(json.text.Value);
                PostToTWServer(listVowelCount);
            }
            else
            {
                Console.WriteLine(json.message);
                Console.ReadLine();
            }
        }

        public static void PostToTWServer(dynamic dataToSend)
        {
            string url = "https://http-hunt.thoughtworks-labs.net/challenge/output/";
            WebRequest reqObj = WebRequest.Create(url);
            reqObj.Method = "POST";
            reqObj.ContentType = "application/json";
            reqObj.Headers.Add("userId", "gzJZbRnmy");
            reqObj.Headers.Add("ContentType", "application/json");
            using (var streamWriter = new StreamWriter(reqObj.GetRequestStream()))
            {
                streamWriter.Write(dataToSend);
                streamWriter.Flush();
                streamWriter.Close();
                var httpResponse = (HttpWebResponse)reqObj.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result2 = streamReader.ReadToEnd();
                    Console.WriteLine(result2);
                    Console.ReadLine();
                }
            }
        }

        public static dynamic GetDataFromTWServer()
        {
            string html = string.Empty;
            string url1 = @"https://http-hunt.thoughtworks-labs.net/challenge/input";
            dynamic json;
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(url1);
            request1.Headers.Add("userId", "gzJZbRnmy");
            using (HttpWebResponse response = (HttpWebResponse)request1.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
                json = JsonConvert.DeserializeObject(html);
            }

            return json;
        }

        public static int countWordsInString(string str)
        {
            int a = 0, wordCounter = 1;
            while (a <= str.Length - 1)
            {
                if (str[a] == ' ' || str[a] == '\n' || str[a] == '\t')
                {
                    wordCounter++;
                }
                a++;
            }
            return wordCounter;
        }

        public static int countSentenceinPara(string str)
        {
            int a = 0, wordCounter = 0;
            while (a <= str.Length - 1)
            {
                if (str[a] == '.' || str[a] == '?' || str[a] == '!')
                {
                    wordCounter++;
                }
                a++;
            }
            return wordCounter;
        }

        public static string countandReturnListofVowel(string str)
        {
            int a = 0, e = 0, i = 0, o = 0, u = 0,x = 0;
            while (x <= str.Length - 1)
            {
                switch (str[x])
                {
                    case 'a':
                        a++;
                        break;
                    case 'e':
                        e++;
                        break;
                    case 'i':
                        i++;
                        break;
                    case 'o':
                        o++;
                        break;
                    case 'u':
                        u++;
                        break;
                }
                x++;
            }
            string json4 = new JavaScriptSerializer().Serialize(new
            {
                a = a,
                e = e,
                i = i,
                o = o,
                u = u
            });
            return json4;
        }

        
    }
}
