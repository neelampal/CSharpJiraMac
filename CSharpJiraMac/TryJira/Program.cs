using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace TryJira
{
    class Program
    {
        static void Main(string[] args)
        {
            String StepsToReproduce = "Steps to Reproduce" + Environment.NewLine + "1.Launch MBP application on QA environment" + Environment.NewLine + "2.Click on Hamburger icon on LHS";
            String IssueType = "Bug";
            String project = "TPFR";
            String DefectSummary = "MBP: Mobile Browser| Failed in opening menu options";
            String defectNbr = LogDefectInJira_Mac(project, IssueType, DefectSummary, StepsToReproduce);
            Console.Write("Defect ID: " + defectNbr);


        }

        public static String LogDefectInJira_Mac(String project, String IssueType, String DefectSummary, String StepsToReproduce)
        {
            string postUrl = "https://spotqa.atlassian.net/rest/api/2/issue/";
            String defectNbr = "";
            var httpWebRequest = (System.Net.HttpWebRequest)WebRequest.Create(postUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("neelam@spotqa.com:neelam123"));

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {               
                string json = @"{""fields"":{""project"":{""key"": ""TPFR""},""summary"": ""MBP: Mobile Browser| Failed in opening menu options"",""description"": ""Steps to Reproduce: \r\n 1.Launch MBP application on QA environment \r\n 2.Click on Hamburger icon on LHS"",""issuetype"": {""name"": ""Bug""}}}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    String result = streamReader.ReadToEnd();
                    List<string> defectKey = new List<string>(result.Split(new string[] { "," }, StringSplitOptions.None));
                    List<string> defectID = new List<string>(defectKey[1].Split(new string[] { ":" }, StringSplitOptions.None));
                    defectNbr = defectID[1];
                }
            }
            return defectNbr;
        }

    }
}