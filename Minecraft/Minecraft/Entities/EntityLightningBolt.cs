using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityLightningBolt : EntityWeatherEffect
	{
		/// <summary>
		/// Declares which state the lightning bolt is in. Whether it's in the air, hit the ground, etc.
		/// </summary>
		private int LightningState;

		/// <summary>
		/// A random long that is used to change the vertex of the lightning rendered in RenderLightningBolt
		/// </summary>
		public long BoltVertex;

		/// <summary>
		/// Determines the time before the EntityLightningBolt is destroyed. It is a random integer decremented over time.
		/// </summary>
		private int BoltLivingTime;

        public EntityLightningBolt(World par1World, float par2, float par4, float par6)
            : base(par1World)
		{
			BoltVertex = 0L;
			SetLocationAndAngles(par2, par4, par6, 0.0F, 0.0F);
			LightningState = 2;
			BoltVertex = Rand.Next();
			BoltLivingTime = Rand.Next(3) + 1;

			if (par1World.DifficultySetting >= 2 && par1World.DoChunksNearChunkExist(MathHelper2.Floor_double(par2), MathHelper2.Floor_double(par4), MathHelper2.Floor_double(par6), 10))
			{
				int i = MathHelper2.Floor_double(par2);
				int k = MathHelper2.Floor_double(par4);
				int i1 = MathHelper2.Floor_double(par6);

				if (par1World.GetBlockId(i, k, i1) == 0 && Block.Fire.CanPlaceBlockAt(par1World, i, k, i1))
				{
					par1World.SetBlockWithNotify(i, k, i1, Block.Fire.BlockID);
				}

				for (int j = 0; j < 4; j++)
				{
					int l = (MathHelper2.Floor_double(par2) + Rand.Next(3)) - 1;
					int j1 = (MathHelper2.Floor_double(par4) + Rand.Next(3)) - 1;
					int k1 = (MathHelper2.Floor_double(par6) + Rand.Next(3)) - 1;

					if (par1World.GetBlockId(l, j1, k1) == 0 && Block.Fire.CanPlaceBlockAt(par1World, l, j1, k1))
					{
						par1World.SetBlockWithNotify(l, j1, k1, Block.Fire.BlockID);
					}
				}
			}
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			base.OnUpdate();

			if (LightningState == 2)
			{
				WorldObj.PlaySoundEffect(PosX, PosY, PosZ, "ambient.weather.thunder", 10000F, 0.8F + Rand.NextFloat() * 0.2F);
				WorldObj.PlaySoundEffect(PosX, PosY, PosZ, "random.explode", 2.0F, 0.5F + Rand.NextFloat() * 0.2F);
			}

			LightningState--;

			if (LightningState < 0)
			{
				if (BoltLivingTime == 0)
				{
					SetDead();
				}
				else if (LightningState < -Rand.Next(10))
				{
					BoltLivingTime--;
					LightningState = 1;
					BoltVertex = Rand.Next();

					if (WorldObj.DoChunksNearChunkExist(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ), 10))
					{
						int i = MathHelper2.Floor_double(PosX);
						int j = MathHelper2.Floor_double(PosY);
						int k = MathHelper2.Floor_double(PosZ);

						if (WorldObj.GetBlockId(i, j, k) == 0 && Block.Fire.CanPlaceBlockAt(WorldObj, i, j, k))
						{
							WorldObj.SetBlockWithNotify(i, j, k, Block.Fire.BlockID);
						}
					}
				}
			}

			if (LightningState >= 0)
			{
                float d = 3;
				List<Entity> list = WorldObj.GetEntitiesWithinAABBExcludingEntity(this, AxisAlignedBB.GetBoundingBoxFromPool(PosX - d, PosY - d, PosZ - d, PosX + d, PosY + 6 + d, PosZ + d));

				for (int l = 0; l < list.Count; l++)
				{
					Entity entity = list[l];
					entity.OnStruckByLightning(this);
				}

				WorldObj.LightningFlash = 2;
			}
		}

		protected override void EntityInit()
		{
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
        public override void ReadEntityFromNBT(NBTTagCompound nbttagcompound)
		{
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
        public override void WriteEntityToNBT(NBTTagCompound nbttagcompound)
		{
		}

		/// <summary>
		/// Checks using a Vec3d to determine if this entity is within range of that vector to be rendered. Args: vec3D
		/// </summary>
		public override bool IsInRangeToRenderVec3D(Vec3D par1Vec3D)
		{
			return LightningState >= 0;
		}
	}
}