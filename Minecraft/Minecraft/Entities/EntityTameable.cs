using System;

namespace net.minecraft.src
{
	public abstract class EntityTameable : EntityAnimal
	{
		protected EntityAISit AiSit;

		public EntityTameable(World par1World) : base(par1World)
		{
			AiSit = new EntityAISit(this);
		}

		protected override void EntityInit()
		{
			base.EntityInit();
			DataWatcher.AddObject(16, 0);
			DataWatcher.AddObject(17, "");
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);

			if (GetOwnerName() == null)
			{
				par1NBTTagCompound.SetString("Owner", "");
			}
			else
			{
				par1NBTTagCompound.SetString("Owner", GetOwnerName());
			}

			par1NBTTagCompound.Setbool("Sitting", IsSitting());
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
			string s = par1NBTTagCompound.GetString("Owner");

			if (s.Length > 0)
			{
				SetOwner(s);
				SetTamed(true);
			}

			AiSit.Func_48407_a(par1NBTTagCompound.Getbool("Sitting"));
		}

		protected virtual void Func_48142_a(bool par1)
		{
			string s = "heart";

			if (!par1)
			{
				s = "smoke";
			}

			for (int i = 0; i < 7; i++)
			{
				double d = Rand.NextGaussian() * 0.02D;
				double d1 = Rand.NextGaussian() * 0.02D;
				double d2 = Rand.NextGaussian() * 0.02D;
				WorldObj.SpawnParticle(s, (PosX + (double)(Rand.NextFloat() * Width * 2.0F)) - (double)Width, PosY + 0.5D + (double)(Rand.NextFloat() * Height), (PosZ + (double)(Rand.NextFloat() * Width * 2.0F)) - (double)Width, d, d1, d2);
			}
		}

		public override void HandleHealthUpdate(byte par1)
		{
			if (par1 == 7)
			{
				Func_48142_a(true);
			}
			else if (par1 == 6)
			{
				Func_48142_a(false);
			}
			else
			{
				base.HandleHealthUpdate(par1);
			}
		}

		public virtual bool IsTamed()
		{
			return (DataWatcher.GetWatchableObjectByte(16) & 4) != 0;
		}

		public virtual void SetTamed(bool par1)
		{
			byte byte0 = DataWatcher.GetWatchableObjectByte(16);

			if (par1)
			{
				DataWatcher.UpdateObject(16, byte0 | 4);
			}
			else
			{
				DataWatcher.UpdateObject(16, byte0 & -5);
			}
		}

		public virtual bool IsSitting()
		{
			return (DataWatcher.GetWatchableObjectByte(16) & 1) != 0;
		}

		public virtual void Func_48140_f(bool par1)
		{
			byte byte0 = DataWatcher.GetWatchableObjectByte(16);

			if (par1)
			{
				DataWatcher.UpdateObject(16, byte0 | 1);
			}
			else
			{
				DataWatcher.UpdateObject(16, byte0 & -2);
			}
		}

		public virtual string GetOwnerName()
		{
			return DataWatcher.GetWatchableObjectString(17);
		}

		public virtual void SetOwner(string par1Str)
		{
			DataWatcher.UpdateObject(17, par1Str);
		}

		public virtual EntityLiving GetOwner()
		{
			return WorldObj.GetPlayerEntityByName(GetOwnerName());
		}

		public virtual EntityAISit Func_50008_ai()
		{
			return AiSit;
		}
	}
}