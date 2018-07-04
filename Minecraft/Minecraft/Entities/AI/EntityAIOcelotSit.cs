namespace net.minecraft.src
{
	public class EntityAIOcelotSit : EntityAIBase
	{
		private readonly EntityOcelot Field_50085_a;
		private readonly float Field_50083_b;
		private int Field_50084_c;
		private int Field_52011_h;
		private int Field_50081_d;
		private int Field_50082_e;
		private int Field_50079_f;
		private int Field_50080_g;

		public EntityAIOcelotSit(EntityOcelot par1EntityOcelot, float par2)
		{
			Field_50084_c = 0;
			Field_52011_h = 0;
			Field_50081_d = 0;
			Field_50082_e = 0;
			Field_50079_f = 0;
			Field_50080_g = 0;
			Field_50085_a = par1EntityOcelot;
			Field_50083_b = par2;
			SetMutexBits(5);
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public override bool ShouldExecute()
		{
			return Field_50085_a.IsTamed() && !Field_50085_a.IsSitting() && Field_50085_a.GetRNG().NextDouble() <= 0.0065000001341104507D && Func_50077_h();
		}

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public override bool ContinueExecuting()
		{
			return Field_50084_c <= Field_50081_d && Field_52011_h <= 60 && Func_50078_a(Field_50085_a.WorldObj, Field_50082_e, Field_50079_f, Field_50080_g);
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public override void StartExecuting()
		{
			Field_50085_a.GetNavigator().Func_48666_a((double)(float)Field_50082_e + 0.5D, Field_50079_f + 1, (double)(float)Field_50080_g + 0.5D, Field_50083_b);
			Field_50084_c = 0;
			Field_52011_h = 0;
			Field_50081_d = Field_50085_a.GetRNG().Next(Field_50085_a.GetRNG().Next(1200) + 1200) + 1200;
			Field_50085_a.Func_50008_ai().Func_48407_a(false);
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public override void ResetTask()
		{
			Field_50085_a.Func_48140_f(false);
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public override void UpdateTask()
		{
			Field_50084_c++;
			Field_50085_a.Func_50008_ai().Func_48407_a(false);

			if (Field_50085_a.GetDistanceSq(Field_50082_e, Field_50079_f + 1, Field_50080_g) > 1.0D)
			{
				Field_50085_a.Func_48140_f(false);
				Field_50085_a.GetNavigator().Func_48666_a((double)(float)Field_50082_e + 0.5D, Field_50079_f + 1, (double)(float)Field_50080_g + 0.5D, Field_50083_b);
				Field_52011_h++;
			}
			else if (!Field_50085_a.IsSitting())
			{
				Field_50085_a.Func_48140_f(true);
			}
			else
			{
				Field_52011_h--;
			}
		}

		private bool Func_50077_h()
		{
			int i = (int)Field_50085_a.PosY;
			double d = 2147483647D;

			for (int j = (int)Field_50085_a.PosX - 8; (double)j < Field_50085_a.PosX + 8D; j++)
			{
				for (int k = (int)Field_50085_a.PosZ - 8; (double)k < Field_50085_a.PosZ + 8D; k++)
				{
					if (!Func_50078_a(Field_50085_a.WorldObj, j, i, k) || !Field_50085_a.WorldObj.IsAirBlock(j, i + 1, k))
					{
						continue;
					}

					double d1 = Field_50085_a.GetDistanceSq(j, i, k);

					if (d1 < d)
					{
						Field_50082_e = j;
						Field_50079_f = i;
						Field_50080_g = k;
						d = d1;
					}
				}
			}

			return d < 2147483647D;
		}

		private bool Func_50078_a(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockId(par2, par3, par4);
			int j = par1World.GetBlockMetadata(par2, par3, par4);

			if (i == Block.Chest.BlockID)
			{
				TileEntityChest tileentitychest = (TileEntityChest)par1World.GetBlockTileEntity(par2, par3, par4);

				if (tileentitychest.NumUsingPlayers < 1)
				{
					return true;
				}
			}
			else
			{
				if (i == Block.StoneOvenActive.BlockID)
				{
					return true;
				}

				if (i == Block.Bed.BlockID && !BlockBed.IsBlockFootOfBed(j))
				{
					return true;
				}
			}

			return false;
		}
	}
}