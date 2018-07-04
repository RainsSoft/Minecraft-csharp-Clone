using System;
using System.IO;

namespace net.minecraft.src
{
	public class CodecMus
	{
		public CodecMus()
		{
		}

		protected virtual FileStream OpenFileStream()
		{/*
			try
			{
				return new MusFileStream(this, url, urlConnection.GetFileStream());
			}
			catch (Exception t)*/
			{
				return null;
			}
		}
	}
}