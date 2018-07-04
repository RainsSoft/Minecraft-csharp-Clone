namespace net.minecraft.src
{
	public class BlockPumpkin : BlockDirectional
	{
		/// <summary>
		/// bool used to seperate different states of blocks </summary>
		private bool BlockType;

        public BlockPumpkin(int par1, int par2, bool par3)
            : base(par1, Material.Pumpkin)
		{
			BlockIndexInTexture = par2;
			SetTickRandomly(true);
			BlockType = par3;
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par1 == 1)
			{
				return BlockIndexInTexture;
			}

			if (par1 == 0)
			{
				return BlockIndexInTexture;
			}

			int i = BlockIndexInTexture + 1 + 16;

			if (BlockType)
			{
				i++;
			}

			if (par2 == 2 && par1 == 2)
			{
				return i;
			}

			if (par2 == 3 && par1 == 5)
			{
				return i;
			}

			if (par2 == 0 && par1 == 3)
			{
				return i;
			}

			if (par2 == 1 && par1 == 4)
			{
				return i;
			}
			else
			{
				return BlockIndexInTexture + 16;
			}
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			if (par1 == 1)
			{
				return BlockIndexInTexture;
			}

			if (par1 == 0)
			{
				return BlockIndexInTexture;
			}

			if (par1 == 3)
			{
				return BlockIndexInTexture + 1 + 16;
			}
			else
			{
				return BlockIndexInTexture + 16;
			}
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			base.OnBlockAdded(par1World, par2, par3, par4);

			if (par1World.GetBlockId(par2, par3 - 1, par4) == Block.BlockSnow.BlockID && par1World.GetBlockId(par2, par3 - 2, par4) == Block.BlockSnow.BlockID)
			{
				if (!par1World.IsRemote)
				{
					par1World.SetBlock(par2, par3, par4, 0);
					par1World.SetBlock(par2, par3 - 1, par4, 0);
					par1World.SetBlock(par2, par3 - 2, par4, 0);
					EntitySnowman entitysnowman = new EntitySnowman(par1World);
					entitysnowman.SetLocationAndAngles(par2 + 0.5F, par3 - 1.95F, par4 + 0.5F, 0.0F, 0.0F);
					par1World.SpawnEntityInWorld(entitysnowman);
					par1World.NotifyBlockChange(par2, par3, par4, 0);
					par1World.NotifyBlockChange(par2, par3 - 1, par4, 0);
					par1World.NotifyBlockChange(par2, par3 - 2, par4, 0);
				}

				for (int i = 0; i < 120; i++)
				{
					par1World.SpawnParticle("snowshovel", (double)par2 + par1World.Rand.NextDouble(), (double)(par3 - 2) + par1World.Rand.NextDouble() * 2.5D, (double)par4 + par1World.Rand.NextDouble(), 0.0F, 0.0F, 0.0F);
				}
			}
			else if (par1World.GetBlockId(par2, par3 - 1, par4) == Block.BlockSteel.BlockID && par1World.GetBlockId(par2, par3 - 2, par4) == Block.BlockSteel.BlockID)
			{
				bool flag = par1World.GetBlockId(par2 - 1, par3 - 1, par4) == Block.BlockSteel.BlockID && par1World.GetBlockId(par2 + 1, par3 - 1, par4) == Block.BlockSteel.BlockID;
				bool flag1 = par1World.GetBlockId(par2, par3 - 1, par4 - 1) == Block.BlockSteel.BlockID && par1World.GetBlockId(par2, par3 - 1, par4 + 1) == Block.BlockSteel.BlockID;

				if (flag || flag1)
				{
					par1World.SetBlock(par2, par3, par4, 0);
					par1World.SetBlock(par2, par3 - 1, par4, 0);
					par1World.SetBlock(par2, par3 - 2, par4, 0);

					if (flag)
					{
						par1World.SetBlock(par2 - 1, par3 - 1, par4, 0);
						par1World.SetBlock(par2 + 1, par3 - 1, par4, 0);
					}
					else
					{
						par1World.SetBlock(par2, par3 - 1, par4 - 1, 0);
						par1World.SetBlock(par2, par3 - 1, par4 + 1, 0);
					}

					EntityIronGolem entityirongolem = new EntityIronGolem(par1World);
					entityirongolem.Func_48115_b(true);
					entityirongolem.SetLocationAndAngles(par2 + 0.5F, par3 - 1.95F, par4 + 0.5F, 0.0F, 0.0F);
					par1World.SpawnEntityInWorld(entityirongolem);

					for (int j = 0; j < 120; j++)
					{
						par1World.SpawnParticle("snowballpoof", (double)par2 + par1World.Rand.NextDouble(), (double)(par3 - 2) + par1World.Rand.NextDouble() * 3.8999999999999999D, (double)par4 + par1World.Rand.NextDouble(), 0.0F, 0.0F, 0.0F);
					}

					par1World.NotifyBlockChange(par2, par3, par4, 0);
					par1World.NotifyBlockChange(par2, par3 - 1, par4, 0);
					par1World.NotifyBlockChange(par2, par3 - 2, par4, 0);

					if (flag)
					{
						par1World.NotifyBlockChange(par2 - 1, par3 - 1, par4, 0);
						par1World.NotifyBlockChange(par2 + 1, par3 - 1, par4, 0);
					}
					else
					{
						par1World.NotifyBlockChange(par2, par3 - 1, par4 - 1, 0);
						par1World.NotifyBlockChange(par2, par3 - 1, par4 + 1, 0);
					}
				}
			}
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockId(par2, par3, par4);
			return (i == 0 || Block.BlocksList[i].BlockMaterial.IsGroundCover()) && par1World.IsBlockNormalCube(par2, par3 - 1, par4);
		}

		/// <summary>
		/// Called when the block is placed in the world.
		/// </summary>
		public override void OnBlockPlacedBy(World par1World, int par2, int par3, int par4, EntityLiving par5EntityLiving)
		{
			int i = MathHelper2.Floor_double((double)((par5EntityLiving.RotationYaw * 4F) / 360F) + 2.5D) & 3;
			par1World.SetBlockMetadataWithNotify(par2, par3, par4, i);
		}
	}
}