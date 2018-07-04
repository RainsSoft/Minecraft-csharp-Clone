using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace net.minecraft.src
{
	public class PostHttp
	{
		private PostHttp()
		{
		}

		public static string Func_52016_a(IDictionary par0Map)
		{
			StringBuilder stringbuilder = new StringBuilder();
			IEnumerator iterator = par0Map.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				DictionaryEntry entry = (DictionaryEntry)iterator.Current;

				if (stringbuilder.Length > 0)
				{
					stringbuilder.Append('&');
				}

				//try
				{
					stringbuilder.Append(entry.Key);
				}/*
				catch (UnsupportedEncodingException unsupportedencodingexception)
				{
					Console.WriteLine(unsupportedencodingexception.ToString());
					Console.Write(unsupportedencodingexception.StackTrace);
				}*/

				if (entry.Value != null)
				{
					stringbuilder.Append('=');

					//try
					{
						stringbuilder.Append(entry.Value.ToString());
					}/*
					catch (UnsupportedEncodingException unsupportedencodingexception1)
					{
						Console.WriteLine(unsupportedencodingexception1.ToString());
						Console.Write(unsupportedencodingexception1.StackTrace);
					}*/
				}
			}
			while (true);

			return stringbuilder.ToString();
		}

		public static string Func_52018_a(string par0URL, IDictionary par1Map, bool par2)
		{
			return GetHttpResponse(par0URL, Func_52016_a(par1Map), par2);
		}

		public static string GetHttpResponse(string url, string requestString, bool par2)
		{
            try
            {
                byte[] postData = Encoding.ASCII.GetBytes(requestString);

                HttpWebRequest httpurlconnection = (HttpWebRequest)WebRequest.Create(url);
                httpurlconnection.Method = "POST";
                httpurlconnection.ContentType = "application/x-www-form-urlencoded";
                httpurlconnection.ContentLength = postData.Length;
                //httpurlconnection.setRequestProperty("Content-Language", "en-US");
                //httpurlconnection.setUseCaches(false);
                //httpurlconnection.setDoInput(true);
                //httpurlconnection.setDoOutput(true);
                Stream dataoutputstream = httpurlconnection.GetRequestStream();
                dataoutputstream.Write(postData, 0, postData.Length);
                dataoutputstream.Flush();
                dataoutputstream.Close();
                StreamReader bufferedreader = new StreamReader(httpurlconnection.GetResponse().GetResponseStream());
                StringBuilder stringbuffer = new StringBuilder();
                string s1;

                while ((s1 = bufferedreader.ReadLine()) != null)
                {
                    stringbuffer.Append(s1);
                    stringbuffer.Append('\r');
                }

                bufferedreader.Close();
                return stringbuffer.ToString();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Console.WriteLine();

                if (!par2)
                {
                    //Logger.getLogger("Minecraft").log(Level.SEVERE, (new StringBuilder()).Append("Could not post to ").Append(url).ToString(), exception);
                }
            }

			return "";
		}
	}
}