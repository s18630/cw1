using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace cw1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            string parametrUrl = "https://www.pja.edu.pl/";

            try
            {
                Console.WriteLine(" Try -> próba przypisana pierwszego argumentu do zmiennej string");
                string firstArg = getFirstArgument(args);

                Console.WriteLine("w try -> Sprawdzam czy dostarczony argument jest prawidłowym adresem URL");
                try
                {
                    bool czyUrl = UrlIsValid(firstArg);
                    Console.WriteLine(czyUrl);
                    if (czyUrl == true)
                    {
                        parametrUrl = firstArg;
                    }
                    Console.WriteLine(" Teraz wartośc parametru Url");
                    Console.WriteLine(parametrUrl);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Wystąpił błąd ArgumenT Exception w metodzie Main");
                    Console.WriteLine("ex.message :");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("ex.StackTrace :");
                    Console.WriteLine(ex.StackTrace);

                }
            }


            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Wystąpił błąd ArgumentNullException w metodzie Main");
                Console.WriteLine("ex.message :");
                Console.WriteLine(ex.Message);
                Console.WriteLine("ex.StackTrace :");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("ex.Data :");
                Console.WriteLine(ex.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił bład w metodzie Main");
                Console.WriteLine(ex.StackTrace);
            }



            /* Console.WriteLine("Sprawdzam działanie metody  sprawdzania adresu - https://www.pja.edu.pl/ ");
             bool urlisValid = UrlIsValid("https://www.pja.edu.pl/");
             Console.WriteLine(urlisValid);

             Console.WriteLine("Sprawdzam działanie metody  sprawdzania adresu - https://www.google.com/search?q=gfi+languard+no+data&oq=gfi+languard+no+data&aqs=chrome..69i57j69i60l2.10592j0j7&sourceid=chrome&ie=UTF-8 ");
             urlisValid = UrlIsValid("https://www.google.com/search?q=gfi+languard+no+data&oq=gfi+languard+no+data&aqs=chrome..69i57j69i60l2.10592j0j7&sourceid=chrome&ie=UTF-8");
             Console.WriteLine(urlisValid);

            /* Console.WriteLine("Sprawdzam czy dostarczony argument jest prawidłowym adresem URL");
             urlisValid = UrlIsValid(firstArg);
             Console.WriteLine(urlisValid);*/



            //  string parametrURL = "https://www.pja.edu.pl/";


            var httpClient = new HttpClient();

            // Console.WriteLine("adres URL" + adresURL);
            var response = await httpClient.GetAsync(parametrUrl);


            if (response.IsSuccessStatusCode) {// czy serwer zwraca odpowiedz z kodem 200


                try
                {
                    var html = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("W MAINIE SPRAWDZANIE ->");
                    var regex = new System.Text.RegularExpressions.Regex("[a-z0-9]+@[a-z.]+");
                    var matches = regex.Matches(html);
                    foreach (var i in matches)
                    {
                        Console.WriteLine(i);
                    }
                    Console.WriteLine("W MAINIE WYWOŁANA METODA ->");
                    var emailAdresses = getEmailAdresses(html);
                    Console.WriteLine("SPRAWDZENIE LISTY W MAINIE ->");
                    foreach (var i in emailAdresses)
                    {
                        Console.WriteLine(i);
                    }
                    Console.WriteLine(" Sprawdzanie metody wypisywania   ");
                    printDistinct(emailAdresses);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(" Błąd w czasie pobierania strony");

                }
                List<string> listaDoSprawdzea = new List<string> { "string2" , "string1","string2","string3", "string3", "string3","stringN" };

                printDistinct(listaDoSprawdzea);



            }
            Console.WriteLine("Koniec  ");
            // Console.WriteLine(args[0]);

        }
        public static void printDistinct(List<string> list)
        {
            Console.WriteLine("Metoda print distinct - > ");
            Console.WriteLine(" ");
            if (list.Count == 0)
            {
                Console.WriteLine(" Lista jest pusta ");
            }
            Console.WriteLine(" PRZED :");

            foreach (var i in list)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine(" P0 :");
            list = list.Distinct().ToList();


        
            foreach (var i in list)
            {
                Console.WriteLine(i);
            }



        }
        public static List<string> getEmailAdresses(string html)
        {
            Console.WriteLine("Metoda get Email Adresses");
            var emailAdresses = new List<string>();
            var regex = new System.Text.RegularExpressions.Regex("[a-z0-9]+@[a-z.]+");
            var matches = regex.Matches(html);
            int k = 1;
            foreach (var i in matches)
            {
                Console.WriteLine("Znaleziony adres "+k+ "  koleckcja Marches : " );
                k++;
                Console.WriteLine(i);
                emailAdresses.Add(i.ToString());
                Console.WriteLine(" Zapisany do listy adres email :");
                Console.WriteLine(i.ToString());
            }
            if(emailAdresses.Count == 0)
            {
                Console.WriteLine("Nie znaleziono adresów email");
            }
            
            return emailAdresses;


        }
        public static string  getFirstArgument(string[] argument)
        {

            Console.WriteLine(" Metoda pobierania argumentu");
            if (argument.Length == 0)
            {
                Console.WriteLine("Tablica argumentów jest pusta");
                Console.WriteLine("Zoatanie wyrzucony wyjątek ArgumentNullException");
                throw new ArgumentNullException();
            }
            Console.WriteLine(" Pierwszym dostarczonym argumentem jest : ");
            Console.WriteLine(argument[0]);
            return argument[0];



        }
      
        public static bool UrlIsValid(string url)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 5000; //set the timeout to 5 seconds to keep the user from waiting too long for the page to load
                request.Method = "HEAD"; //Get only the header information -- no need to download any content

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                int statusCode = (int)response.StatusCode;
                if (statusCode >= 100 && statusCode < 400) //Good requests
                {
                    Console.WriteLine("Prawidłowy adres URL");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Nieprawidłowy adres");
                Console.WriteLine("Zoatanie wyrzucony wyjątek ArgumentException");
                throw new ArgumentException();

            }
             return false;
        }


    
    }


    }

