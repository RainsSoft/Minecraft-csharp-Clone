using System;

namespace net.minecraft.src
{
	public class EntityTNTPrimed : Entity
	{
		/// <summary>
		/// How long the fuse is </summary>
		public int Fuse;

		public EntityTNTPrimed(World par1World) : base(par1World)
		{
			Fuse = 0;
			PreventEntitySpawning = true;
			SetSize(0.98F, 0.98F);
			YOffset = Height / 2.0F;
		}

        public EntityTNTPrimed(World par1World, float par2, float par4, float par6)
            : this(par1World)
		{
			SetPosition(par2, par4, par6);
			float f = (float)((new Random(1)).NextDouble() * Math.PI * 2D);
			MotionX = -(float)Math.Sin(f) * 0.02F;
			MotionY = 0.20000000298023224F;
			MotionZ = -(float)Math.Cos(f) * 0.02F;
			Fuse = 80;
			PrevPosX = par2;
			PrevPosY = par4;
			PrevPosZ = par6;
		}

		protected override void EntityInit()
		{
		}

		/// <summary>
		/// returns if this entity triggers Block.onEntityWalking on the blocks they walk on. used for spiders and wolves to
		/// prevent them from trampling crops
		/// </summary>
		protected override bool CanTriggerWalking()
		{
			return false;
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
			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;
			MotionY -= 0.039999999105930328F;
			MoveEntity(MotionX, MotionY, MotionZ);
			MotionX *= 0.98000001907348633F;
			MotionY *= 0.98000001907348633F;
			MotionZ *= 0.98000001907348633F;

			if (OnGround)
			{
				MotionX *= 0.69999998807907104F;
				MotionZ *= 0.69999998807907104F;
				MotionY *= -0.5F;
			}

			if (Fuse-- <= 0)
			{
				if (!WorldObj.IsRemote)
				{
					SetDead();
					Explode();
				}
				else
				{
					SetDead();
				}
			}
			else
			{
				WorldObj.SpawnParticle("smoke", PosX, PosY + 0.5D, PosZ, 0.0F, 0.0F, 0.0F);
			}
		}

		private void Explode()
		{
			float f = 4F;
			WorldObj.CreateExplosion(null, PosX, PosY, PosZ, f);
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			par1NBTTagCompound.SetByte("Fuse", (byte)Fuse);
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			Fuse = par1NBTTagCompound.GetByte("Fuse");
		}

		public override float GetShadowSize()
		{
			return 0.0F;
		}
	}
}