using System;

namespace net.minecraft.src
{
	public class EntityVillager : EntityAgeable
	{
		private int RandomTickDivider;
		private bool IsMatingFlag;
		private bool IsPlayingFlag;
		Village VillageObj;

		public EntityVillager(World par1World) : this(par1World, 0)
		{
		}

		public EntityVillager(World par1World, int par2) : base(par1World)
		{
			RandomTickDivider = 0;
			IsMatingFlag = false;
			IsPlayingFlag = false;
			VillageObj = null;
			SetProfession(par2);
			Texture = "/mob/villager/villager.png";
			MoveSpeed = 0.5F;
			GetNavigator().SetBreakDoors(true);
			GetNavigator().Func_48664_a(true);
			Tasks.AddTask(0, new EntityAISwimming(this));
			Tasks.AddTask(1, new EntityAIAvoidEntity(this, typeof(net.minecraft.src.EntityZombie), 8F, 0.3F, 0.35F));
			Tasks.AddTask(2, new EntityAIMoveIndoors(this));
			Tasks.AddTask(3, new EntityAIRestrictOpenDoor(this));
			Tasks.AddTask(4, new EntityAIOpenDoor(this, true));
			Tasks.AddTask(5, new EntityAIMoveTwardsRestriction(this, 0.3F));
			Tasks.AddTask(6, new EntityAIVillagerMate(this));
			Tasks.AddTask(7, new EntityAIFollowGolem(this));
			Tasks.AddTask(8, new EntityAIPlay(this, 0.32F));
			Tasks.AddTask(9, new EntityAIWatchClosest2(this, typeof(net.minecraft.src.EntityPlayer), 3F, 1.0F));
			Tasks.AddTask(9, new EntityAIWatchClosest2(this, typeof(net.minecraft.src.EntityVillager), 5F, 0.02F));
			Tasks.AddTask(9, new EntityAIWander(this, 0.3F));
			Tasks.AddTask(10, new EntityAIWatchClosest(this, typeof(net.minecraft.src.EntityLiving), 8F));
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
			if (--RandomTickDivider <= 0)
			{
				WorldObj.VillageCollectionObj.AddVillagerPosition(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ));
				RandomTickDivider = 70 + Rand.Next(50);
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

		protected override void EntityInit()
		{
			base.EntityInit();
			DataWatcher.AddObject(16, Convert.ToInt32(0));
		}

		public override int GetMaxHealth()
		{
			return 20;
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			base.OnLivingUpdate();
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
			par1NBTTagCompound.SetInteger("Profession", GetProfession());
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
			SetProfession(par1NBTTagCompound.GetInteger("Profession"));
		}

		/// <summary>
		/// Returns the texture's file path as a String.
		/// </summary>
		public override string GetTexture()
		{
			switch (GetProfession())
			{
				case 0:
					return "/mob/villager/farmer.png";

				case 1:
					return "/mob/villager/librarian.png";

				case 2:
					return "/mob/villager/priest.png";

				case 3:
					return "/mob/villager/smith.png";

				case 4:
					return "/mob/villager/butcher.png";
			}

			return base.GetTexture();
		}

		/// <summary>
		/// Determines if an entity can be despawned, used on idle far away entities
		/// </summary>
		protected override bool CanDespawn()
		{
			return false;
		}

		/// <summary>
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			return "mob.villager.default";
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.villager.defaulthurt";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.villager.defaultdeath";
		}

		public virtual void SetProfession(int par1)
		{
			DataWatcher.UpdateObject(16, Convert.ToInt32(par1));
		}

		public virtual int GetProfession()
		{
			return DataWatcher.GetWatchableObjectInt(16);
		}

		public virtual bool GetIsMatingFlag()
		{
			return IsMatingFlag;
		}

		public virtual void SetIsMatingFlag(bool par1)
		{
			IsMatingFlag = par1;
		}

		public virtual void SetIsPlayingFlag(bool par1)
		{
			IsPlayingFlag = par1;
		}

		public virtual bool GetIsPlayingFlag()
		{
			return IsPlayingFlag;
		}

		public override void SetRevengeTarget(EntityLiving par1EntityLiving)
		{
			base.SetRevengeTarget(par1EntityLiving);

			if (VillageObj != null && par1EntityLiving != null)
			{
				VillageObj.AddOrRenewAgressor(par1EntityLiving);
			}
		}
	}
}