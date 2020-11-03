using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace HttpHeaderCharScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestAllCharFrom0To256InHeader();

            WriteResultToFile_TestAllCharFrom0To256InHeader();
        }

        private static void WriteResultToFile_TestAllCharFrom0To256InHeader()
        {
            var sb = new StringBuilder();

            sb.AppendLine("char;statusCode;customResult;exception");

            var cert = new X509Certificate2("c:\\root_ca_swaggerdoc.pfx", "1234");
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(cert);
            handler.ServerCertificateCustomValidationCallback = (a, b, c, d) => true;
            var client = new HttpClient(handler);

            var chrono = new Stopwatch();

            for (int i = 0; i < 256; i++)
            {
                Console.WriteLine(i);

                sb.Append(i); sb.Append(";");

                chrono.Start();

                try
                {
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri("https://localhost:5001/Personnes/FromHeader"),
                        Method = HttpMethod.Get
                    };

                    request.Headers.Add("prenom", ((char)i).ToString());

                    var response = client.SendAsync(request).Result;

                    sb.Append((int)response.StatusCode); sb.Append(";");

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Le statuscode de la réponse est: " + response.StatusCode);
                        var responseContent = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(responseContent);
                    }
                    else
                    {
                        Console.WriteLine("Le statuscode de la réponse est: " + response.StatusCode);

                        try
                        {
                            var responseContent = response.Content.ReadAsStringAsync().Result;
                            Console.WriteLine(responseContent);
                            sb.Append(!string.IsNullOrEmpty(responseContent)); sb.Append(";");
                        }
                        catch
                        {
                            Console.Error.WriteLine("Erreur lors de la lecture du body");
                        }
                    }

                    Console.WriteLine("Fin heureuse");
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);

                    sb.Append(";;"); sb.Append(e.Message);

                    while (e.InnerException != null)
                    {
                        e = e.InnerException;
                        Console.Error.WriteLine(e.Message);
                    }

                    Console.WriteLine("Fin triste");
                }

                chrono.Stop();
                Console.WriteLine($"Durée : {chrono.ElapsedMilliseconds}");
                chrono.Reset();

                sb.AppendLine();
            }

            File.WriteAllText("result.dotnetcore3.1.csv", sb.ToString());
        }

        private static void TestAllCharFrom0To256InHeader()
        {
            Console.WriteLine("initialize client");

            var cert = new X509Certificate2("c:\\root_ca_swaggerdoc.pfx", "1234");
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(cert);
            handler.ServerCertificateCustomValidationCallback = (a, b, c, d) => true;
            var client = new HttpClient(handler);

            var chrono = new Stopwatch();

            for (int i = 0; i < 256; i++)
            {
                Console.WriteLine($"Test for char {i}");

                chrono.Start();

                try
                {
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri("https://localhost:5001/Personnes/FromHeader"),
                        Method = HttpMethod.Get
                    };

                    request.Headers.Add("prenom", ((char)i).ToString());

                    var response = client.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Le statuscode de la réponse est: " + response.StatusCode);
                        var responseContent = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(responseContent);
                    }
                    else
                    {
                        Console.WriteLine("Le statuscode de la réponse est: " + response.StatusCode);
                        try
                        {
                            var responseContent = response.Content.ReadAsStringAsync().Result;
                            Console.WriteLine(responseContent);
                        }
                        catch 
                        {
                            Console.Error.WriteLine("Erreur lors de la lecture du body");
                        }
                    }

                    Console.WriteLine("Fin heureuse");
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);

                    while (e.InnerException != null)
                    {
                        e = e.InnerException;
                        Console.Error.WriteLine(e.Message);
                    }

                    Console.WriteLine("Fin triste");
                }

                chrono.Stop();
                Console.WriteLine($"Durée : {chrono.ElapsedMilliseconds}");
                chrono.Reset();
            }
        }
    }
}
