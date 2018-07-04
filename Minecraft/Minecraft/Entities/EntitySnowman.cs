namespace net.minecraft.src
{
	public class EntitySnowman : EntityGolem
	{
		public EntitySnowman(World par1World) : base(par1World)
		{
			Texture = "/mob/snowman.png";
			SetSize(0.4F, 1.8F);
			GetNavigator().Func_48664_a(true);
			Tasks.AddTask(1, new EntityAIArrowAttack(this, 0.25F, 2, 20));
			Tasks.AddTask(2, new EntityAIWander(this, 0.2F));
			Tasks.AddTask(3, new EntityAIWatchClosest(this, typeof(net.minecraft.src.EntityPlayer), 6F));
			Tasks.AddTask(4, new EntityAILookIdle(this));
			TargetTasks.AddTask(1, new EntityAINearestAttackableTarget(this, typeof(net.minecraft.src.EntityMob), 16F, 0, true));
		}

		/// <summary>
		/// Returns true if the newer Entity AI code should be run
		/// </summary>
		protected override bool IsAIEnabled()
		{
			return true;
		}

		public override int GetMaxHealth()
		{
			return 4;
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			base.OnLivingUpdate();

			if (IsWet())
			{
				AttackEntityFrom(DamageSource.Drown, 1);
			}

			int i = MathHelper2.Floor_double(PosX);
			int k = MathHelper2.Floor_double(PosZ);

			if (WorldObj.GetBiomeGenForCoords(i, k).GetFloatTemperature() > 1.0F)
			{
				AttackEntityFrom(DamageSource.OnFire, 1);
			}

			for (int j = 0; j < 4; j++)
			{
				int l = MathHelper2.Floor_double(PosX + (double)((float)((j % 2) * 2 - 1) * 0.25F));
				int i1 = MathHelper2.Floor_double(PosY);
				int j1 = MathHelper2.Floor_double(PosZ + (double)((float)(((j / 2) % 2) * 2 - 1) * 0.25F));

				if (WorldObj.GetBlockId(l, i1, j1) == 0 && WorldObj.GetBiomeGenForCoords(l, j1).GetFloatTemperature() < 0.8F && Block.Snow.CanPlaceBlockAt(WorldObj, l, i1, j1))
				{
					WorldObj.SetBlockWithNotify(l, i1, j1, Block.Snow.BlockID);
				}
			}
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			return Item.Snowball.ShiftedIndex;
		}

		/// <summary>
		/// Drop 0-2 items of this living's type
		/// </summary>
		protected override void DropFewItems(bool par1, int par2)
		{
			int i = Rand.Next(16);

			for (int j = 0; j < i; j++)
			{
				DropItem(Item.Snowball.ShiftedIndex, 1);
			}
		}
	}
}