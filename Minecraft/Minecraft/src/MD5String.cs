using System;
using System.Security.Cryptography;
using System.Text;

namespace net.minecraft.src
{
	public class MD5String
	{
		private string Field_27370_a;

		public MD5String(string par1Str)
		{
			Field_27370_a = par1Str;
		}

		/// <summary>
		/// Gets the MD5 string
		/// </summary>
		public virtual string GetMD5String(string par1Str)
		{
			try
			{
				string s = (new StringBuilder()).Append(Field_27370_a).Append(par1Str).ToString();
                MD5 md5 = MD5.Create();
                var stringBytes = Encoding.Default.GetBytes(s);
                return BitConverter.ToInt32(md5.ComputeHash(stringBytes, 0, stringBytes.Length), 0).ToString();
			}
			catch (Exception nosuchalgorithmexception)
			{
				throw nosuchalgorithmexception;
			}
		}
	}
}