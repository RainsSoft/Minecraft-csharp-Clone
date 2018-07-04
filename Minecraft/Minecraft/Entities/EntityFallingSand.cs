namespace net.minecraft.src
{
	public class EntityFallingSand : Entity
	{
		public int BlockID;

		/// <summary>
		/// How long the block has been falling for. </summary>
		public int FallTime;

		public EntityFallingSand(World par1World) : base(par1World)
		{
			FallTime = 0;
		}

        public EntityFallingSand(World par1World, float par2, float par4, float par6, int par8)
            : base(par1World)
		{
			FallTime = 0;
			BlockID = par8;
			PreventEntitySpawning = true;
			SetSize(0.98F, 0.98F);
			YOffset = Height / 2.0F;
			SetPosition(par2, par4, par6);
			MotionX = 0.0F;
			MotionY = 0.0F;
			MotionZ = 0.0F;
			PrevPosX = par2;
			PrevPosY = par4;
			PrevPosZ = par6;
		}

		/// <summary>
		/// returns if this entity triggers Block.onEntityWalking on the blocks they walk on. used for spiders and wolves to
		/// prevent them from trampling crops
		/// </summary>
		protected override bool CanTriggerWalking()
		{
			return false;
		}

		protected override void EntityInit()
		{
		}

		/// <summary>
		/// Returns true if other Entities should be prevented from moving through this Entity.
		/// </summary>
		public override bool CanBeCollidedWith()
		{
			return !IsDead;
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			if (BlockID == 0)
			{
				SetDead();
				return;
			}

			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;
			FallTime++;
			MotionY -= 0.039999999105930328F;
			MoveEntity(MotionX, MotionY, MotionZ);
			MotionX *= 0.98000001907348633F;
			MotionY *= 0.98000001907348633F;
			MotionZ *= 0.98000001907348633F;
			int i = MathHelper2.Floor_double(PosX);
			int j = MathHelper2.Floor_double(PosY);
			int k = MathHelper2.Floor_double(PosZ);

			if (FallTime == 1 && WorldObj.GetBlockId(i, j, k) == BlockID)
			{
				WorldObj.SetBlockWithNotify(i, j, k, 0);
			}
			else if (!WorldObj.IsRemote && FallTime == 1)
			{
				SetDead();
			}

			if (OnGround)
			{
				MotionX *= 0.69999998807907104F;
				MotionZ *= 0.69999998807907104F;
				MotionY *= -0.5F;

				if (WorldObj.GetBlockId(i, j, k) != Block.PistonMoving.BlockID)
				{
					SetDead();

					if ((!WorldObj.CanBlockBePlacedAt(BlockID, i, j, k, true, 1) || BlockSand.CanFallBelow(WorldObj, i, j - 1, k) || !WorldObj.SetBlockWithNotify(i, j, k, BlockID)) && !WorldObj.IsRemote)
					{
						DropItem(BlockID, 1);
					}
				}
			}
			else if (FallTime > 100 && !WorldObj.IsRemote && (j < 1 || j > 256) || FallTime > 600)
			{
				DropItem(BlockID, 1);
				SetDead();
			}
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
        public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			par1NBTTagCompound.SetByte("Tile", (byte)BlockID);
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
        public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			BlockID = par1NBTTagCompound.GetByte("Tile") & 0xff;
		}

		public override float GetShadowSize()
		{
			return 0.0F;
		}

		public virtual World GetWorld()
		{
			return WorldObj;
		}
	}
}