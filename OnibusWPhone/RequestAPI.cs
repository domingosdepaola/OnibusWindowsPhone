using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace OnibusWPhone
{
    public class RequestAPI
    {
        string url = "";
        bool TerminouRequisicao = false;
        List<Onibus> listaDados = new List<Onibus>();
        public static string getUrl(string numeroLinha,double latitude,double longitude)
        {
            string urlPadrao = "http://www.localizadordeonibus.com.br/OnibusWebAPI/api/onibus/getOnibusWP?x=" + numeroLinha + "&y=" + latitude.ToString() + "&z=" + longitude.ToString();
            return urlPadrao;
        }
        public static async Task<String> GetResponse(string caminhoURl)
        {
            try
            {
                //Criando o Client 
                var client = new HttpClient();
                //Definindo a URL. 
                var uri = new Uri(caminhoURl);
                //A URL utilizada não funciona, foi inserida somente para demonstração.

                //Executando a chamada na URL definida acima.
                var Response = await client.GetAsync(uri);

                var statusCode = Response.StatusCode;

                Response.EnsureSuccessStatusCode();

                var ResponseText = await Response.Content.ReadAsStringAsync();

                return ResponseText;

            }

            catch (Exception ex)
            {
                //...
            }
            return null;
        }
        public static async Task<List<T>> GetResponse<T>(string url)
        {
            List<T> items = null;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

            var response = (HttpWebResponse)await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);
            try
            {
                Stream stream = response.GetResponseStream();
                StreamReader strReader = new StreamReader(stream);
                string text = strReader.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<T>>(text);
            }
            catch (Exception ex)
            {
                
            }
            return items;
        }
        private async void GetFullResponse(string numeroLinha)
        {
            try
            {
                //Create Client 
                var client = new HttpClient();

                //Define URL. Replace current URL with your URL
                //Current URL is not a valid one


                var uri = new Uri(url + numeroLinha);

                //Call. Get response by Async
                var Response = await client.GetAsync(uri);

                //Result & Code
                var statusCode = Response.StatusCode;

                //If Response is not Http 200 
                //then EnsureSuccessStatusCode will throw an exception
                Response.EnsureSuccessStatusCode();

                //Read the content of the response.
                //In here expected response is a string.
                //Accroding to Response you can change the Reading method.
                //like ReadAsStreamAsync etc..
                var ResponseText = await Response.Content.ReadAsStringAsync();
            }

            catch (Exception ex)
            {
                //...
            }
        }
        public static async Task<List<Onibus>> GetString(string cainhoUrl)
        {
            try
            {
                List<Onibus> items = null;
                //Create HttpClient
                HttpClient httpClient = new HttpClient();

                //Define Http Headers
                httpClient.DefaultRequestHeaders.Accept.TryParseAdd("application/json");

                //Call
                string ResponseString = await httpClient.GetStringAsync(new Uri(cainhoUrl));
                items = JsonConvert.DeserializeObject<List<Onibus>>(ResponseString);
                return items;
            }

            catch (Exception ex)
            {
                //....
            }
            return null;
        }

        public List<Onibus> getOnibusJSON(string numeroLinha)
        {
            GetResponse(numeroLinha);
            while (!TerminouRequisicao)
            {
                Task.Delay(1000);
            }
            if (listaDados != null && listaDados.Count > 0)
            {
                return listaDados;
            }
            else
            {
                return null;
            }
            //string jsonURL = urlJsonPrefeitura;
            //List<OnibusBO> lstOnibus = new List<OnibusBO>();


            ////DataSet ds = getDadosFromCSV(pathCompleto);
            //getDadosFromJSON(jsonURL);

        }
       
    }
}
