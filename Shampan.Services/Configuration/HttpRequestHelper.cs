using Newtonsoft.Json;
using System.Net;
using System.Net.Security;
using System.Text;
using Shampan.Models;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;

namespace Shampan.Services.Configuration
{
    public class HttpRequestHelper
    {
        string BaseURL = "";
        string BaseURLReport = "";

        //public HttpRequestHelper()
        //{
        //    BaseURL = ConfigurationManager.AppSettings["baseUrl"];
        //}
              
       
        private static IConfiguration _configuration;
        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public HttpRequestHelper()
        {
            BaseURL = _configuration["AppSettings:BaseUrl"];

        }


        public AuthModel GetLoginAuthentication(CredentialModel credentialModel)
        {
            try
            {
                var result = Login("api/UserLogin/SignIn", JsonConvert.SerializeObject(credentialModel));

                return JsonConvert.DeserializeObject<AuthModel>(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public AuthModel GetAuthentication(CredentialModel credentialModel)
        {
            try
            {
                var result = Login("api/Auth/login", JsonConvert.SerializeObject(credentialModel));

                return JsonConvert.DeserializeObject<AuthModel>(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string Login(string url, string payLoad)
        {
            // Set TLS version to TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Disable SSL certificate validation (for development purposes only)
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true);

            try
            {
                WebRequest request = (HttpWebRequest)WebRequest.Create(BaseURL + url);
                request.Method = "POST";         

                byte[] byteArray = Encoding.UTF8.GetBytes(payLoad);
                request.ContentLength = byteArray.Length;
                request.ContentType = "application/json";

                // Write the payload to the request stream
                using (Stream datastream = request.GetRequestStream())
                {
                    datastream.Write(byteArray, 0, byteArray.Length);
                }

                // Get the response
                WebResponse response = request.GetResponse();
                using (Stream datastream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(datastream))
                    {
                        string responseMessage = reader.ReadToEnd();
                        return responseMessage;
                    }
                }
            }
            catch (WebException ex)
            {
                // Handle WebException and log details
                string errorResponse = string.Empty;
                if (ex.Response != null)
                {
                    using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        errorResponse = reader.ReadToEnd();
                    }
                }

                return errorResponse;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return ex.Message;
            }
        }

        public string PostData(string url, AuthModel auth, string payLoad)
        {
            // Set TLS version to TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Disable SSL certificate validation (for development purposes only)
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true);

            try
            {
                WebRequest request = (HttpWebRequest)WebRequest.Create(BaseURL + url);
                request.Method = "POST";

                // Set headers or authorization if needed
                request.Headers.Add("Authorization", "Bearer " + auth.token);

                byte[] byteArray = Encoding.UTF8.GetBytes(payLoad);
                request.ContentLength = byteArray.Length;
                request.ContentType = "application/json";

                // Write the payload to the request stream
                using (Stream datastream = request.GetRequestStream())
                {
                    datastream.Write(byteArray, 0, byteArray.Length);
                }

                // Get the response
                WebResponse response = request.GetResponse();
                using (Stream datastream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(datastream))
                    {
                        string responseMessage = reader.ReadToEnd();
                        return responseMessage;
                    }
                }
            }
            catch (WebException ex)
            {
                // Handle WebException and log details
                string errorResponse = string.Empty;
                if (ex.Response != null)
                {
                    using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        errorResponse = reader.ReadToEnd();
                    }
                }


                return errorResponse;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return ex.Message;
            }
        }

        public Stream PostDataReport(string url, AuthModel auth, string payLoad)
        {
            // Set TLS version to TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Disable SSL certificate validation (for development purposes only)
            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true);

            try
            {
                WebRequest request = (HttpWebRequest)WebRequest.Create(BaseURLReport + url);
                request.Method = "POST";
                //request.Headers.Add("");
                byte[] byteArray = Encoding.UTF8.GetBytes(payLoad);
                request.ContentLength = byteArray.Length;
                request.ContentType = "application/json";
                //request.ContentType = "application/json charset=utf-8";
                ////NetworkCredential creds = GetCredentials();
                ////request.Credentials = creds;

                Stream datastream = request.GetRequestStream();
                datastream.Write(byteArray, 0, byteArray.Length);
                datastream.Close();

                WebResponse response = request.GetResponse();
                datastream = response.GetResponseStream();
                return datastream;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string PostFormUrlEncoded(string url, FormUrlEncodedContent formUrlEncodedContent)
        {
            try
            {
                WebRequest request = (HttpWebRequest)WebRequest.Create(BaseURL + url);
                request.Method = "POST";

                byte[] byteArray = Encoding.UTF8.GetBytes(formUrlEncodedContent.ReadAsStringAsync().Result);
                request.ContentLength = byteArray.Length;

                request.ContentType = "application/json";


                //NetworkCredential creds = GetCredentials();
                //request.Credentials = creds;

                Stream datastream = request.GetRequestStream();
                datastream.Write(byteArray, 0, byteArray.Length);
                datastream.Close();

                WebResponse response = request.GetResponse();
                datastream = response.GetResponseStream();

                StreamReader reader = new StreamReader(datastream);

                string responseMessage = reader.ReadToEnd();

                reader.Close();

                return responseMessage;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetData(string url, AuthModel auth)
        {
            try
            {
                WebRequest request = (HttpWebRequest)WebRequest.Create(BaseURL + url);
                request.Method = "GET";

                request.Headers.Add("Authorization", "Bearer " + auth.token);


                WebResponse response = request.GetResponse();
                Stream datastream = response.GetResponseStream();

                StreamReader reader = new StreamReader(datastream);
                string responseMessage = reader.ReadToEnd();

                reader.Close();

                return responseMessage;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetIntegrationData(string url)
        {
            try
            {
                WebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                //request.Headers.Add("Authorization", "Bearer " + auth.Access_token);

                //byte[] byteArray = Encoding.UTF8.GetBytes(payLoad);
                //request.ContentLength = byteArray.Length;

                //request.ContentType = "text/xml;charset=UTF-8";

                //NetworkCredential creds = GetCredentials();
                //request.Credentials = creds;

                //datastream.Write(byteArray, 0, byteArray.Length);
                //datastream.Close();

                WebResponse response = request.GetResponse();
                Stream datastream = response.GetResponseStream();

                StreamReader reader = new StreamReader(datastream);
                string responseMessage = reader.ReadToEnd();

                reader.Close();

                return responseMessage;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private NetworkCredential GetCredentials()
        {
            return new NetworkCredential("vatuser", "123456");
        }


    }
}

