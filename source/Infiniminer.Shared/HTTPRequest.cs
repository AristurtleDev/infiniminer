/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2009 Zach Barth
Copyright (c) 2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using System.Text;
using System.Net;

namespace Infiniminer
{
    public static class HttpRequest
    {
        public static string Post(string url, Dictionary<string, string> parameters)
        {
            WebRequest request = WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            // Write the POST data.
            string paramString = EncodeParameters(parameters);
            byte[] bytes = Encoding.ASCII.GetBytes(paramString);
            Stream? os = null;

            try
            {
                request.ContentLength = bytes.Length;
                os = request.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
            }
            catch (WebException ex)
            {
                throw new Exception("Request error", ex);
            }
            finally
            {
                if (os != null) os.Close();
            }

            return ReadResponse(request);
        }

        public static string Get(string url, Dictionary<string, string> parameters)
        {
            // Append the parameters to the URL.
            string paramString = EncodeParameters(parameters);
            if (paramString != "") url = url + "?" + paramString;
            WebRequest request = WebRequest.Create(url);
            return ReadResponse(request);
        }

        private static string ReadResponse(WebRequest request)
        {
            string responseText;

            try
            {
                WebResponse response = request.GetResponse();
                if (response == null) throw new Exception("No response");

                StreamReader sr = new StreamReader(response.GetResponseStream());
                responseText = sr.ReadToEnd().Trim();
            }
            catch (WebException ex)
            {
                throw new Exception("Response error", ex);
            }

            return responseText;
        }

        public static string EncodeParameters(Dictionary<string, string> parameters)
        {
            if (parameters == null) return "";

            // Parameters are of the form: "name1=value1&name2=value2"
            string[] entryStrings = new string[parameters.Count];
            int i = 0;
            foreach (KeyValuePair<string, string> entry in parameters)
            {
                entryStrings[i] = entry.Key + "=" + entry.Value;
                i += 1;
            }
            return string.Join("&", entryStrings);
        }
    }
}
