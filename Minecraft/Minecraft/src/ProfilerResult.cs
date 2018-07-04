using System;

namespace net.minecraft.src
{

	public sealed class ProfilerResult : IComparable
	{
		/// <summary>
		/// Percentage of time spent in this ProfilerResult relative to its parent ProfilerResult
		/// </summary>
		public double SectionPercentage;

		/// <summary>
		/// Percentage of time spent in this ProfilerResult relative to the entire game
		/// </summary>
		public double GlobalPercentage;

		/// <summary>
		/// The name of this ProfilerResult </summary>
		public string Name;

		public ProfilerResult(string par1Str, double par2, double par4)
		{
			Name = par1Str;
			SectionPercentage = par2;
			GlobalPercentage = par4;
		}

		/// <summary>
		/// Called from compareTo()
		/// </summary>
		public int CompareProfilerResult(ProfilerResult par1ProfilerResult)
		{
			if (par1ProfilerResult.SectionPercentage < SectionPercentage)
			{
				return -1;
			}

			if (par1ProfilerResult.SectionPercentage > SectionPercentage)
			{
				return 1;
			}
			else
			{
				return par1ProfilerResult.Name.CompareTo(Name);
			}
		}

		/// <summary>
		/// Compute the color used to display this ProfilerResult on the debug screen
		/// </summary>
		public int GetDisplayColor()
		{
			return (Name.GetHashCode() & 0xaaaaaa) + 0x444444;
		}

		public int CompareTo(object par1Obj)
		{
			return CompareProfilerResult((ProfilerResult)par1Obj);
		}
	}

}