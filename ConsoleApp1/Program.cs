
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    public class Program
    {
        static HttpClient client = new HttpClient();
        public static string dir = @"c:\Users";
        public static string filePath = @$"{dir}\nasa.txt";
        public static string apiKey = "NCPH9PtgXJ6fjJWc55MTv1NL9Hg6TOvnpfWfKLRM";
        public static string apiUrl = "https://api.nasa.gov/planetary/apod?api_key={0}&date={1}";
        public static string imageFolderPath = @$"{dir}\nasa_imgs\";
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static bool SaveFile()
        {
            string[] dates = { "02/27/17", "June 2, 2018", "Jul-13-2016", "April 31, 2018" };
            if (!File.Exists(filePath))
            {
                CultureInfo provider = CultureInfo.InvariantCulture;

                using (StreamWriter sw = File.CreateText(filePath))
                {
                    foreach (var d in dates)
                    {
                        sw.WriteLine(d);
                    }
                }
            }

            return true;
        }

        static async Task<NasaData> GetNasaDataAsync(string date)
        {
            NasaData nsaData = null;
            HttpResponseMessage response = await client.GetAsync(string.Format(apiUrl, apiKey, date));
            if (response.IsSuccessStatusCode)
            {
                nsaData = await response.Content.ReadAsAsync<NasaData>();
            }

            return nsaData;
        }

        static void ShowNasaImg(NasaData nsaData)
        {
            Process.Start(new ProcessStartInfo("chrome.exe")
            { UseShellExecute = true, Arguments = nsaData.Url });

        }
        public static async Task RunAsync()
        {

            try
            {

                var isFileExists = SaveFile();
                if (isFileExists)
                {

                    var format = "yyy-M-d";
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    StreamReader file = new System.IO.StreamReader(@$"{filePath}");
                    string d;
                    while ((d = file.ReadLine()) != null)
                    {

                        string[] newDateSplitted = { };
                        _ = new DateTime();
                        bool formttedDate;
                        DateTime newDate;

                        if (d.Contains("/"))
                        {
                            newDateSplitted = d.Split("/");
                            newDate = new DateTime(Convert.ToInt32(newDateSplitted[2].Length < 4 ? ("20" + newDateSplitted[2]) : newDateSplitted[2]),
                                Convert.ToInt32(newDateSplitted[0]),
                                Convert.ToInt32(newDateSplitted[1]));
                            formttedDate = true;
                        }
                        else
                        {
                            formttedDate = DateTime.TryParse(d, out newDate);
                        }

                        if (formttedDate)
                        {
                            NasaData nsaData = await GetNasaDataAsync(newDate.ToString(format, provider));
                            await SaveImage(nsaData.Url);
                            ShowNasaImg(nsaData);
                        }


                    }


                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        static async Task<bool> SaveImage(string url)
        {
            using (WebClient client = new WebClient())
            {
                var fileNameSplitted = url.Split("/");
                var index = fileNameSplitted.Length - 1;
                Directory.CreateDirectory(imageFolderPath);
                await client.DownloadFileTaskAsync(new Uri(url), imageFolderPath + fileNameSplitted[index]);
            }

            return true;
        }

    }

    public class NasaData
    {
        public DateTime Date { get; set; }
        public string Url { get; set; }
    }

}
