using System;
using System.Collections;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class WeightedRandom
	{
		public WeightedRandom()
		{
		}

		/// <summary>
		/// Returns the total weight of all items in a collection.
		/// </summary>
        public static int GetTotalWeight(ICollection par0Collection)
		{
			int i = 0;

            for (IEnumerator iterator = par0Collection.GetEnumerator(); iterator.MoveNext(); )
			{
				WeightedRandomChoice weightedrandomchoice = (WeightedRandomChoice)iterator.Current;
				i += weightedrandomchoice.ItemWeight;
			}

			return i;
		}

		/// <summary>
		/// Returns a random choice from the input items, with a total weight value.
		/// </summary>
        public static WeightedRandomChoice GetRandomItem(Random par0Random, ICollection par1Collection, int par2)
		{
			if (par2 <= 0)
			{
				throw new System.ArgumentException();
			}

			int i = par0Random.Next(par2);

            for (IEnumerator iterator = par1Collection.GetEnumerator(); iterator.MoveNext(); )
			{
				WeightedRandomChoice weightedrandomchoice = (WeightedRandomChoice)iterator.Current;
				i -= weightedrandomchoice.ItemWeight;

				if (i < 0)
				{
					return weightedrandomchoice;
				}
			}

			return null;
		}

		/// <summary>
		/// Returns a random choice from the input items.
		/// </summary>
        public static WeightedRandomChoice GetRandomItem(Random par0Random, ICollection par1Collection)
		{
			return GetRandomItem(par0Random, par1Collection, GetTotalWeight(par1Collection));
		}

		/// <summary>
		/// Returns the total weight of all items in a array.
		/// </summary>
		public static int GetTotalWeight(WeightedRandomChoice[] par0ArrayOfWeightedRandomChoice)
		{
			int i = 0;
			WeightedRandomChoice[] aweightedrandomchoice = par0ArrayOfWeightedRandomChoice;
			int j = aweightedrandomchoice.Length;

			for (int k = 0; k < j; k++)
			{
				WeightedRandomChoice weightedrandomchoice = aweightedrandomchoice[k];
				i += weightedrandomchoice.ItemWeight;
			}

			return i;
		}

		/// <summary>
		/// Returns a random choice from the input array of items, with a total weight value.
		/// </summary>
		public static WeightedRandomChoice GetRandomItem(Random par0Random, WeightedRandomChoice[] par1ArrayOfWeightedRandomChoice, int par2)
		{
			if (par2 <= 0)
			{
				throw new System.ArgumentException();
			}

			int i = par0Random.Next(par2);
			WeightedRandomChoice[] aweightedrandomchoice = par1ArrayOfWeightedRandomChoice;
			int j = aweightedrandomchoice.Length;

			for (int k = 0; k < j; k++)
			{
				WeightedRandomChoice weightedrandomchoice = aweightedrandomchoice[k];
				i -= weightedrandomchoice.ItemWeight;

				if (i < 0)
				{
					return weightedrandomchoice;
				}
			}

			return null;
		}

		/// <summary>
		/// Returns a random choice from the input items.
		/// </summary>
		public static WeightedRandomChoice GetRandomItem(Random par0Random, WeightedRandomChoice[] par1ArrayOfWeightedRandomChoice)
		{
			return GetRandomItem(par0Random, par1ArrayOfWeightedRandomChoice, GetTotalWeight(par1ArrayOfWeightedRandomChoice));
		}
	}
}