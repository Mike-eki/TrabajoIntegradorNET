using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FrontendWindowsForm
{
    public static class HttpClientManager
    {
        private static HttpClient _client;

        public static HttpClient GetClient()
        {
            if (_client == null)
            {
                _client = new HttpClient();
                _client.BaseAddress = new Uri("https://localhost:5001/");
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }
            return _client;
        }

        // Opcional: método para agregar token de autenticación
        //public static void SetAuthorizationToken(string token)
        //{
        //    if (_client != null)
        //    {
        //        _client.DefaultRequestHeaders.Authorization =
        //            new AuthenticationHeaderValue("Bearer", token);
        //    }
        //}

        //// Opcional: método para limpiar token (al cerrar sesión)
        //public static void ClearAuthorizationToken()
        //{
        //    if (_client != null)
        //    {
        //        _client.DefaultRequestHeaders.Authorization = null;
        //    }
        //}
    }
}
