namespace net.minecraft.src
{
	public class TileEntityMobSpawner : TileEntity
	{
		/// <summary>
		/// The stored delay before a new spawn. </summary>
		public int Delay;

		/// <summary>
		/// The string ID of the mobs being spawned from this spawner. Defaults to pig, apparently.
		/// </summary>
		private string MobID;
		public double Yaw;
		public double Yaw2;

		public TileEntityMobSpawner()
		{
			Delay = -1;
			Yaw2 = 0.0F;
			MobID = "Pig";
			Delay = 20;
		}

		public virtual string GetMobID()
		{
			return MobID;
		}

		public virtual void SetMobID(string par1Str)
		{
			MobID = par1Str;
		}

		/// <summary>
		/// Returns true if there is a player in range (using World.getClosestPlayer)
		/// </summary>
		public virtual bool AnyPlayerInRange()
		{
			return WorldObj.GetClosestPlayer(XCoord + 0.5F, YCoord + 0.5F, ZCoord + 0.5F, 16) != null;
		}

		/// <summary>
		/// Allows the entity to update its state. Overridden in most subclasses, e.g. the mob spawner uses this to count
		/// ticks and creates a new spawn inside its implementation.
		/// </summary>
		public override void UpdateEntity()
		{
			Yaw2 = Yaw;

			if (!AnyPlayerInRange())
			{
				return;
			}

			double d = (float)XCoord + WorldObj.Rand.NextFloat();
			double d1 = (float)YCoord + WorldObj.Rand.NextFloat();
			double d2 = (float)ZCoord + WorldObj.Rand.NextFloat();
			WorldObj.SpawnParticle("smoke", d, d1, d2, 0.0F, 0.0F, 0.0F);
			WorldObj.SpawnParticle("flame", d, d1, d2, 0.0F, 0.0F, 0.0F);

			for (Yaw += 1000F / ((float)Delay + 200F); Yaw > 360D;)
			{
				Yaw -= 360D;
				Yaw2 -= 360D;
			}

			if (!WorldObj.IsRemote)
			{
				if (Delay == -1)
				{
					UpdateDelay();
				}

				if (Delay > 0)
				{
					Delay--;
					return;
				}

				sbyte byte0 = 4;

				for (int i = 0; i < byte0; i++)
				{
					EntityLiving entityliving = (EntityLiving)EntityList.CreateEntityByName(MobID, WorldObj);

					if (entityliving == null)
					{
						return;
					}

					int j = WorldObj.GetEntitiesWithinAABB(entityliving.GetType(), AxisAlignedBB.GetBoundingBoxFromPool(XCoord, YCoord, ZCoord, XCoord + 1, YCoord + 1, ZCoord + 1).Expand(8F, 4F, 8F)).Count;

					if (j >= 6)
					{
						UpdateDelay();
						return;
					}

					if (entityliving == null)
					{
						continue;
					}

                    float d3 = XCoord + (WorldObj.Rand.NextFloat() - WorldObj.Rand.NextFloat()) * 4F;
                    float d4 = (YCoord + WorldObj.Rand.Next(3)) - 1;
                    float d5 = ZCoord + (WorldObj.Rand.NextFloat() - WorldObj.Rand.NextFloat()) * 4F;
					entityliving.SetLocationAndAngles(d3, d4, d5, WorldObj.Rand.NextFloat() * 360F, 0.0F);

					if (entityliving.GetCanSpawnHere())
					{
						WorldObj.SpawnEntityInWorld(entityliving);
						WorldObj.PlayAuxSFX(2004, XCoord, YCoord, ZCoord, 0);
						entityliving.SpawnExplosionParticle();
						UpdateDelay();
					}
				}
			}

			base.UpdateEntity();
		}

		/// <summary>
		/// Sets the delay before a new spawn (base delay of 200 + random number up to 600).
		/// </summary>
		private void UpdateDelay()
		{
			Delay = 200 + WorldObj.Rand.Next(600);
		}

		/// <summary>
		/// Reads a tile entity from NBT.
		/// </summary>
		public override void ReadFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadFromNBT(par1NBTTagCompound);
			MobID = par1NBTTagCompound.GetString("EntityId");
			Delay = par1NBTTagCompound.GetShort("Delay");
		}

		/// <summary>
		/// Writes a tile entity to NBT.
		/// </summary>
		public override void WriteToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteToNBT(par1NBTTagCompound);
			par1NBTTagCompound.SetString("EntityId", MobID);
			par1NBTTagCompound.SetShort("Delay", (short)Delay);
		}
	}
}