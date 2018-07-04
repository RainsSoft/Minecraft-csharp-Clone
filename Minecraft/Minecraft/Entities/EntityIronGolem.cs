using System;
using System.Text;

namespace net.minecraft.src
{
	public class EntityIronGolem : EntityGolem
	{
		private int Field_48119_b;
		Village VillageObj;
		private int Field_48120_c;
		private int Field_48118_d;

		public EntityIronGolem(World par1World) : base(par1World)
		{
			Field_48119_b = 0;
			VillageObj = null;
			Texture = "/mob/villager_golem.png";
			SetSize(1.4F, 2.9F);
			GetNavigator().Func_48664_a(true);
			Tasks.AddTask(1, new EntityAIAttackOnCollide(this, 0.25F, true));
			Tasks.AddTask(2, new EntityAIMoveTowardsTarget(this, 0.22F, 32F));
			Tasks.AddTask(3, new EntityAIMoveThroughVillage(this, 0.16F, true));
			Tasks.AddTask(4, new EntityAIMoveTwardsRestriction(this, 0.16F));
			Tasks.AddTask(5, new EntityAILookAtVillager(this));
			Tasks.AddTask(6, new EntityAIWander(this, 0.16F));
			Tasks.AddTask(7, new EntityAIWatchClosest(this, typeof(net.minecraft.src.EntityPlayer), 6F));
			Tasks.AddTask(8, new EntityAILookIdle(this));
			TargetTasks.AddTask(1, new EntityAIDefendVillage(this));
			TargetTasks.AddTask(2, new EntityAIHurtByTarget(this, false));
			TargetTasks.AddTask(3, new EntityAINearestAttackableTarget(this, typeof(net.minecraft.src.EntityMob), 16F, 0, false, true));
		}

		protected override void EntityInit()
		{
			base.EntityInit();
			DataWatcher.AddObject(16, Convert.ToByte((sbyte)0));
		}

		/// <summary>
		/// Returns true if the newer Entity AI code should be run
		/// </summary>
		protected override bool IsAIEnabled()
		{
			return true;
		}

		/// <summary>
		/// main AI tick function, replaces updateEntityActionState
		/// </summary>
		protected override void UpdateAITick()
		{
			if (--Field_48119_b <= 0)
			{
				Field_48119_b = 70 + Rand.Next(50);
				VillageObj = WorldObj.VillageCollectionObj.FindNearestVillage(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ), 32);

				if (VillageObj == null)
				{
					DetachHome();
				}
				else
				{
					ChunkCoordinates chunkcoordinates = VillageObj.GetCenter();
					SetHomeArea(chunkcoordinates.PosX, chunkcoordinates.PosY, chunkcoordinates.PosZ, VillageObj.GetVillageRadius());
				}
			}

			base.UpdateAITick();
		}

		public override int GetMaxHealth()
		{
			return 100;
		}

		/// <summary>
		/// Decrements the entity's air supply when underwater
		/// </summary>
		protected override int DecreaseAirSupply(int par1)
		{
			return par1;
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			base.OnLivingUpdate();

			if (Field_48120_c > 0)
			{
				Field_48120_c--;
			}

			if (Field_48118_d > 0)
			{
				Field_48118_d--;
			}

	//JAVA TO C# CONVERTER TODO TASK: Octal literals cannot be represented in C#:
			if (MotionX * MotionX + MotionZ * MotionZ > 2.5000002779052011E-007D && Rand.Next(5) == 0)
			{
				int i = MathHelper2.Floor_double(PosX);
				int j = MathHelper2.Floor_double(PosY - 0.20000000298023224D - (double)YOffset);
				int k = MathHelper2.Floor_double(PosZ);
				int l = WorldObj.GetBlockId(i, j, k);

				if (l > 0)
				{
					WorldObj.SpawnParticle((new StringBuilder()).Append("tilecrack_").Append(l).ToString(), PosX + ((double)Rand.NextFloat() - 0.5D) * (double)Width, BoundingBox.MinY + 0.10000000000000001D, PosZ + ((double)Rand.NextFloat() - 0.5D) * (double)Width, 4D * ((double)Rand.NextFloat() - 0.5D), 0.5D, ((double)Rand.NextFloat() - 0.5D) * 4D);
				}
			}
		}

		public override bool Func_48100_a(Type par1Class)
		{
			if (Func_48112_E_() && par1Class.IsSubclassOf((typeof(net.minecraft.src.EntityPlayer))))
			{
				return false;
			}
			else
			{
				return base.Func_48100_a(par1Class);
			}
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
			par1NBTTagCompound.Setbool("PlayerCreated", Func_48112_E_());
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
			Func_48115_b(par1NBTTagCompound.Getbool("PlayerCreated"));
		}

		public override bool AttackEntityAsMob(Entity par1Entity)
		{
			Field_48120_c = 10;
			WorldObj.SetEntityState(this, (sbyte)4);
			bool flag = par1Entity.AttackEntityFrom(DamageSource.CauseMobDamage(this), 7 + Rand.Next(15));

			if (flag)
			{
				par1Entity.MotionY += 0.40000000596046448F;
			}

			WorldObj.PlaySoundAtEntity(this, "mob.irongolem.throw", 1.0F, 1.0F);
			return flag;
		}

		public override void HandleHealthUpdate(byte par1)
		{
			if (par1 == 4)
			{
				Field_48120_c = 10;
				WorldObj.PlaySoundAtEntity(this, "mob.irongolem.throw", 1.0F, 1.0F);
			}
			else if (par1 == 11)
			{
				Field_48118_d = 400;
			}
			else
			{
				base.HandleHealthUpdate(par1);
			}
		}

		public virtual Village GetVillage()
		{
			return VillageObj;
		}

		public virtual int Func_48114_ab()
		{
			return Field_48120_c;
		}

		public virtual void Func_48116_a(bool par1)
		{
			Field_48118_d = par1 ? 400 : 0;
			WorldObj.SetEntityState(this, (sbyte)11);
		}

		/// <summary>
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			return "none";
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.irongolem.hit";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.irongolem.death";
		}

		/// <summary>
		/// Plays step sound at given x, y, z for the entity
		/// </summary>
		protected override void PlayStepSound(int par1, int par2, int par3, int par4)
		{
			WorldObj.PlaySoundAtEntity(this, "mob.irongolem.walk", 1.0F, 1.0F);
		}

		/// <summary>
		/// Drop 0-2 items of this living's type
		/// </summary>
		protected override void DropFewItems(bool par1, int par2)
		{
			int i = Rand.Next(3);

			for (int j = 0; j < i; j++)
			{
				DropItem(Block.PlantRed.BlockID, 1);
			}

			int k = 3 + Rand.Next(3);

			for (int l = 0; l < k; l++)
			{
				DropItem(Item.IngotIron.ShiftedIndex, 1);
			}
		}

		public virtual int Func_48117_D_()
		{
			return Field_48118_d;
		}

		public virtual bool Func_48112_E_()
		{
			return (DataWatcher.GetWatchableObjectByte(16) & 1) != 0;
		}

		public virtual void Func_48115_b(bool par1)
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
	}
}