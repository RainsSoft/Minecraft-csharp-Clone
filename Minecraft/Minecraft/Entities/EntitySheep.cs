using System;

namespace net.minecraft.src
{
	public class EntitySheep : EntityAnimal
	{
		public static readonly float[][] FleeceColorTable = { new float[] { 1.0F, 1.0F, 1.0F }, new float[] { 0.95F, 0.7F, 0.2F }, new float[] { 0.9F, 0.5F, 0.85F }, new float[] { 0.6F, 0.7F, 0.95F }, new float[] { 0.9F, 0.9F, 0.2F }, new float[] { 0.5F, 0.8F, 0.1F }, new float[] { 0.95F, 0.7F, 0.8F }, new float[] { 0.3F, 0.3F, 0.3F }, new float[] { 0.6F, 0.6F, 0.6F }, new float[] { 0.3F, 0.6F, 0.7F }, new float[] { 0.7F, 0.4F, 0.9F }, new float[] { 0.2F, 0.4F, 0.8F }, new float[] { 0.5F, 0.4F, 0.3F }, new float[] { 0.4F, 0.5F, 0.2F }, new float[] { 0.8F, 0.3F, 0.3F }, new float[] { 0.1F, 0.1F, 0.1F } };

		/// <summary>
		/// Used to control movement as well as wool regrowth. Set to 40 on handleHealthUpdate and counts down with each
		/// tick.
		/// </summary>
		private int SheepTimer;

		/// <summary>
		/// The eat grass AI task for this mob. </summary>
		private EntityAIEatGrass AiEatGrass;

		public EntitySheep(World par1World) : base(par1World)
		{
			AiEatGrass = new EntityAIEatGrass(this);
			Texture = "/mob/sheep.png";
			SetSize(0.9F, 1.3F);
			float f = 0.23F;
			GetNavigator().Func_48664_a(true);
			Tasks.AddTask(0, new EntityAISwimming(this));
			Tasks.AddTask(1, new EntityAIPanic(this, 0.38F));
			Tasks.AddTask(2, new EntityAIMate(this, f));
			Tasks.AddTask(3, new EntityAITempt(this, 0.25F, Item.Wheat.ShiftedIndex, false));
			Tasks.AddTask(4, new EntityAIFollowParent(this, 0.25F));
			Tasks.AddTask(5, AiEatGrass);
			Tasks.AddTask(6, new EntityAIWander(this, f));
			Tasks.AddTask(7, new EntityAIWatchClosest(this, typeof(net.minecraft.src.EntityPlayer), 6F));
			Tasks.AddTask(8, new EntityAILookIdle(this));
		}

		/// <summary>
		/// Returns true if the newer Entity AI code should be run
		/// </summary>
		protected override bool IsAIEnabled()
		{
			return true;
		}

		protected override void UpdateAITasks()
		{
			SheepTimer = AiEatGrass.Func_48396_h();
			base.UpdateAITasks();
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			if (WorldObj.IsRemote)
			{
				SheepTimer = Math.Max(0, SheepTimer - 1);
			}

			base.OnLivingUpdate();
		}

		public override int GetMaxHealth()
		{
			return 8;
		}

		protected override void EntityInit()
		{
			base.EntityInit();
			DataWatcher.AddObject(16, new sbyte?((sbyte)0));
		}

		/// <summary>
		/// Drop 0-2 items of this living's type
		/// </summary>
		protected override void DropFewItems(bool par1, int par2)
		{
			if (!GetSheared())
			{
				EntityDropItem(new ItemStack(Block.Cloth.BlockID, 1, GetFleeceColor()), 0.0F);
			}
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			return Block.Cloth.BlockID;
		}

		public override void HandleHealthUpdate(byte par1)
		{
			if (par1 == 10)
			{
				SheepTimer = 40;
			}
			else
			{
				base.HandleHealthUpdate(par1);
			}
		}

		public virtual float Func_44003_c(float par1)
		{
			if (SheepTimer <= 0)
			{
				return 0.0F;
			}

			if (SheepTimer >= 4 && SheepTimer <= 36)
			{
				return 1.0F;
			}

			if (SheepTimer < 4)
			{
				return ((float)SheepTimer - par1) / 4F;
			}
			else
			{
				return -((float)(SheepTimer - 40) - par1) / 4F;
			}
		}

		public virtual float Func_44002_d(float par1)
		{
			if (SheepTimer > 4 && SheepTimer <= 36)
			{
				float f = ((float)(SheepTimer - 4) - par1) / 32F;
				return ((float)Math.PI / 5F) + ((float)Math.PI * 7F / 100F) * MathHelper2.Sin(f * 28.7F);
			}

			if (SheepTimer > 0)
			{
				return ((float)Math.PI / 5F);
			}
			else
			{
				return RotationPitch / (180F / (float)Math.PI);
			}
		}

		/// <summary>
		/// Called when a player interacts with a mob. e.g. gets milk from a cow, gets into the saddle on a pig.
		/// </summary>
		public override bool Interact(EntityPlayer par1EntityPlayer)
		{
			ItemStack itemstack = par1EntityPlayer.Inventory.GetCurrentItem();

			if (itemstack != null && itemstack.ItemID == Item.Shears.ShiftedIndex && !GetSheared() && !IsChild())
			{
				if (!WorldObj.IsRemote)
				{
					SetSheared(true);
					int i = 1 + Rand.Next(3);

					for (int j = 0; j < i; j++)
					{
						EntityItem entityitem = EntityDropItem(new ItemStack(Block.Cloth.BlockID, 1, GetFleeceColor()), 1.0F);
						entityitem.MotionY += Rand.NextFloat() * 0.05F;
						entityitem.MotionX += (Rand.NextFloat() - Rand.NextFloat()) * 0.1F;
						entityitem.MotionZ += (Rand.NextFloat() - Rand.NextFloat()) * 0.1F;
					}
				}

				itemstack.DamageItem(1, par1EntityPlayer);
			}

			return base.Interact(par1EntityPlayer);
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
			par1NBTTagCompound.Setbool("Sheared", GetSheared());
			par1NBTTagCompound.SetByte("Color", (byte)GetFleeceColor());
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
			SetSheared(par1NBTTagCompound.Getbool("Sheared"));
			SetFleeceColor(par1NBTTagCompound.GetByte("Color"));
		}

		/// <summary>
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			return "mob.sheep";
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.sheep";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.sheep";
		}

		public virtual int GetFleeceColor()
		{
			return DataWatcher.GetWatchableObjectByte(16) & 0xf;
		}

		public virtual void SetFleeceColor(int par1)
		{
			byte byte0 = DataWatcher.GetWatchableObjectByte(16);
			DataWatcher.UpdateObject(16, Convert.ToByte((sbyte)(byte0 & 0xf0 | par1 & 0xf)));
		}

		/// <summary>
		/// returns true if a sheeps wool has been sheared
		/// </summary>
		public virtual bool GetSheared()
		{
			return (DataWatcher.GetWatchableObjectByte(16) & 0x10) != 0;
		}

		/// <summary>
		/// make a sheep sheared if set to true
		/// </summary>
		public virtual void SetSheared(bool par1)
		{
			byte byte0 = DataWatcher.GetWatchableObjectByte(16);

			if (par1)
			{
				DataWatcher.UpdateObject(16, Convert.ToByte((sbyte)(byte0 | 0x10)));
			}
			else
			{
				DataWatcher.UpdateObject(16, Convert.ToByte((sbyte)(byte0 & 0xffffffef)));
			}
		}

		/// <summary>
		/// This method is called when a sheep spawns in the world to select the color of sheep fleece.
		/// </summary>
		public static int GetRandomFleeceColor(Random par0Random)
		{
			int i = par0Random.Next(100);

			if (i < 5)
			{
				return 15;
			}

			if (i < 10)
			{
				return 7;
			}

			if (i < 15)
			{
				return 8;
			}

			if (i < 18)
			{
				return 12;
			}

			return par0Random.Next(500) != 0 ? 0 : 6;
		}

		/// <summary>
		/// This function is used when two same-species animals in 'love mode' breed to generate the new baby animal.
		/// </summary>
		public override EntityAnimal SpawnBabyAnimal(EntityAnimal par1EntityAnimal)
		{
			EntitySheep entitysheep = (EntitySheep)par1EntityAnimal;
			EntitySheep entitysheep1 = new EntitySheep(WorldObj);

			if (Rand.NextBool())
			{
				entitysheep1.SetFleeceColor(GetFleeceColor());
			}
			else
			{
				entitysheep1.SetFleeceColor(entitysheep.GetFleeceColor());
			}

			return entitysheep1;
		}

		/// <summary>
		/// This function applies the benefits of growing back wool and faster growing up to the acting entity. (This
		/// function is used in the AIEatGrass)
		/// </summary>
		public override void EatGrassBonus()
		{
			SetSheared(false);

			if (IsChild())
			{
				int i = GetGrowingAge() + 1200;

				if (i > 0)
				{
					i = 0;
				}

				SetGrowingAge(i);
			}
		}
	}
}