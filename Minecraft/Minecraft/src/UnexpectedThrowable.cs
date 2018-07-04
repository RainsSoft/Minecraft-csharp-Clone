using System;

namespace net.minecraft.src
{

	public class UnexpectedThrowable
	{
		/// <summary>
		/// A description of the error that has occurred. </summary>
		public readonly string Description;

		/// <summary>
		/// The Throwable object that was thrown. </summary>
		public readonly Exception Exception;

		public UnexpectedThrowable(string par1Str, Exception par2Throwable)
		{
			Description = par1Str;
			Exception = par2Throwable;
		}
	}

}