using System;
using System.Text;

namespace net.minecraft.src
{
	public class PotionEffect
	{
		/// <summary>
		/// ID value of the potion this effect matches. </summary>
		private int PotionID;

		/// <summary>
		/// The duration of the potion effect </summary>
		private int Duration;

		/// <summary>
		/// The amplifier of the potion effect </summary>
		private int Amplifier;

		public PotionEffect(int par1, int par2, int par3)
		{
			PotionID = par1;
			Duration = par2;
			Amplifier = par3;
		}

		public PotionEffect(PotionEffect par1PotionEffect)
		{
			PotionID = par1PotionEffect.PotionID;
			Duration = par1PotionEffect.Duration;
			Amplifier = par1PotionEffect.Amplifier;
		}

		/// <summary>
		/// merges the input PotionEffect into this one if this.amplifier <= tomerge.amplifier. The duration in the supplied
		/// potion effect is assumed to be greater.
		/// </summary>
		public virtual void Combine(PotionEffect par1PotionEffect)
		{
			if (PotionID != par1PotionEffect.PotionID)
			{
				Console.Error.WriteLine("This method should only be called for matching effects!");
			}

			if (par1PotionEffect.Amplifier > Amplifier)
			{
				Amplifier = par1PotionEffect.Amplifier;
				Duration = par1PotionEffect.Duration;
			}
			else if (par1PotionEffect.Amplifier == Amplifier && Duration < par1PotionEffect.Duration)
			{
				Duration = par1PotionEffect.Duration;
			}
		}

		/// <summary>
		/// Retrieve the ID of the potion this effect matches.
		/// </summary>
		public virtual int GetPotionID()
		{
			return PotionID;
		}

		public virtual int GetDuration()
		{
			return Duration;
		}

		public virtual int GetAmplifier()
		{
			return Amplifier;
		}

		public virtual bool OnUpdate(EntityLiving par1EntityLiving)
		{
			if (Duration > 0)
			{
				if (Potion.PotionTypes[PotionID].IsReady(Duration, Amplifier))
				{
					PerformEffect(par1EntityLiving);
				}

				DeincrementDuration();
			}

			return Duration > 0;
		}

		private int DeincrementDuration()
		{
			return --Duration;
		}

		public virtual void PerformEffect(EntityLiving par1EntityLiving)
		{
			if (Duration > 0)
			{
				Potion.PotionTypes[PotionID].PerformEffect(par1EntityLiving, Amplifier);
			}
		}

		public virtual string GetEffectName()
		{
			return Potion.PotionTypes[PotionID].GetName();
		}

		public override int GetHashCode()
		{
			return PotionID;
		}

		public override string ToString()
		{
			string s = "";

			if (GetAmplifier() > 0)
			{
				s = (new StringBuilder()).Append(GetEffectName()).Append(" x ").Append(GetAmplifier() + 1).Append(", Duration: ").Append(GetDuration()).ToString();
			}
			else
			{
				s = (new StringBuilder()).Append(GetEffectName()).Append(", Duration: ").Append(GetDuration()).ToString();
			}

			if (Potion.PotionTypes[PotionID].IsUsable())
			{
				return (new StringBuilder()).Append("(").Append(s).Append(")").ToString();
			}
			else
			{
				return s;
			}
		}

		public override bool Equals(object par1Obj)
		{
			if (!(par1Obj is PotionEffect))
			{
				return false;
			}
			else
			{
				PotionEffect potioneffect = (PotionEffect)par1Obj;
				return PotionID == potioneffect.PotionID && Amplifier == potioneffect.Amplifier && Duration == potioneffect.Duration;
			}
		}
	}
}