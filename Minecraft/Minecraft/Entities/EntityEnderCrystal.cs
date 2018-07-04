using System;

namespace net.minecraft.src
{
	public class EntityEnderCrystal : Entity
	{
		/// <summary>
		/// Used to create the rotation animation when rendering the crystal. </summary>
		public int InnerRotation;
		public int Health;

		public EntityEnderCrystal(World par1World) : base(par1World)
		{
			InnerRotation = 0;
			PreventEntitySpawning = true;
			SetSize(2.0F, 2.0F);
			YOffset = Height / 2.0F;
			Health = 5;
			InnerRotation = Rand.Next(0x186a0);
		}

        public EntityEnderCrystal(World par1World, float par2, float par4, float par6)
            : this(par1World)
		{
			SetPosition(par2, par4, par6);
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
			DataWatcher.AddObject(8, Convert.ToInt32(Health));
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;
			InnerRotation++;
			DataWatcher.UpdateObject(8, Convert.ToInt32(Health));
			int i = MathHelper2.Floor_double(PosX);
			int j = MathHelper2.Floor_double(PosY);
			int k = MathHelper2.Floor_double(PosZ);

			if (WorldObj.GetBlockId(i, j, k) != Block.Fire.BlockID)
			{
				WorldObj.SetBlockWithNotify(i, j, k, Block.Fire.BlockID);
			}
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
        public override void WriteEntityToNBT(NBTTagCompound nbttagcompound)
		{
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
        public override void ReadEntityFromNBT(NBTTagCompound nbttagcompound)
		{
		}

		public override float GetShadowSize()
		{
			return 0.0F;
		}

		/// <summary>
		/// Returns true if other Entities should be prevented from moving through this Entity.
		/// </summary>
		public override bool CanBeCollidedWith()
		{
			return true;
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			if (!IsDead && !WorldObj.IsRemote)
			{
				Health = 0;

				if (Health <= 0)
				{
					if (!WorldObj.IsRemote)
					{
						SetDead();
						WorldObj.CreateExplosion(null, PosX, PosY, PosZ, 6F);
					}
					else
					{
						SetDead();
					}
				}
			}

			return true;
		}
	}

}