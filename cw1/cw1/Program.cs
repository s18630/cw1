using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace cw1
{
    public class Program 
    {
        public static async Task Main(string[] args)
        {

            



            try
            {
                string firstArg = getFirstArgument(args);


                try
                {
                    
                    bool czyUrl = UrlIsValid(firstArg);
                    string parametrUrl= firstArg;
                    
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(parametrUrl);

                    if (response.IsSuccessStatusCode)
                    {

                        try
                        {
                            var html = await response.Content.ReadAsStringAsync();
                            httpClient.Dispose();
                            var emailAdresses = getEmailAdresses(html);

                            if (emailAdresses.Count != 0)
                            {
                                printDistinct(emailAdresses);
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(" Błąd w czasie pobierania strony");
                        }

                    }


                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine( " Przekazany parametr nie jest prawidłowym adresem URL");
                  
                }


            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(" Nie został przekazany parametr");
                
            }
          


          

           
          
        }








        public static void printDistinct(List<string> list)
        {
            
            if (list.Count == 0)
            {
                Console.WriteLine(" Lista jest pusta ");
            }
            

            list = list.Distinct().ToList();


            foreach (var i in list)
            {
                Console.WriteLine(i);
            }


        }






        public static List<string> getEmailAdresses(string html)
        {
            var emailAdresses = new List<string>();
            var regex = new System.Text.RegularExpressions.Regex("[a-z0-9]+@[a-z.]+");
            var matches = regex.Matches(html);
       
            foreach (var i in matches)
            {
                emailAdresses.Add(i.ToString());
            
            }

            if(emailAdresses.Count == 0)
            {
                Console.WriteLine("Nie znaleziono adresów email");
            }
            
            return emailAdresses;

        }



        public static string  getFirstArgument(string[] argument)
        {
            if (argument.Length == 0)
            {
                throw new ArgumentNullException();
            }

            return argument[0];
        }
      




        public static bool UrlIsValid(string url)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 5000; 
                request.Method = "HEAD";

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                int statusCode = (int)response.StatusCode;
                if (statusCode >= 100 && statusCode < 400) 
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException();
            }
             return false;
        }

       
    }


    }

