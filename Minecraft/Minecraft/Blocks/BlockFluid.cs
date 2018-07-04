using System;

namespace net.minecraft.src
{
	public abstract class BlockFluid : Block
	{
		protected BlockFluid(int par1, Material par2Material) : base(par1, (par2Material != Material.Lava ? 12 : 14) * 16 + 13, par2Material)
		{
			float f = 0.0F;
			float f1 = 0.0F;
			SetBlockBounds(0.0F + f1, 0.0F + f, 0.0F + f1, 1.0F + f1, 1.0F + f, 1.0F + f1);
			SetTickRandomly(true);
		}

		public override bool GetBlocksMovement(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return BlockMaterial != Material.Lava;
		}

		public override int GetBlockColor()
		{
			return 0xffffff;
		}

		/// <summary>
		/// Returns a integer with hex for 0xrrggbb with this color multiplied against the blocks color. Note only called
		/// when first determining what to render.
		/// </summary>
		public override int ColorMultiplier(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			if (BlockMaterial == Material.Water)
			{
				int i = 0;
				int j = 0;
				int k = 0;

				for (int l = -1; l <= 1; l++)
				{
					for (int i1 = -1; i1 <= 1; i1++)
					{
						int j1 = par1IBlockAccess.GetBiomeGenForCoords(par2 + i1, par4 + l).WaterColorMultiplier;
						i += (j1 & 0xff0000) >> 16;
						j += (j1 & 0xff00) >> 8;
						k += j1 & 0xff;
					}
				}

				return (i / 9 & 0xff) << 16 | (j / 9 & 0xff) << 8 | k / 9 & 0xff;
			}
			else
			{
				return 0xffffff;
			}
		}

		/// <summary>
		/// Returns the percentage of the fluid block that is air, based on the given flow decay of the fluid.
		/// </summary>
		public static float GetFluidHeightPercent(int par0)
		{
			if (par0 >= 8)
			{
				par0 = 0;
			}

			float f = (float)(par0 + 1) / 9F;
			return f;
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			if (par1 == 0 || par1 == 1)
			{
				return BlockIndexInTexture;
			}
			else
			{
				return BlockIndexInTexture + 1;
			}
		}

		/// <summary>
		/// Returns the amount of fluid decay at the coordinates, or -1 if the block at the coordinates is not the same
		/// material as the fluid.
		/// </summary>
		protected virtual int GetFlowDecay(World par1World, int par2, int par3, int par4)
		{
			if (par1World.GetBlockMaterial(par2, par3, par4) != BlockMaterial)
			{
				return -1;
			}
			else
			{
				return par1World.GetBlockMetadata(par2, par3, par4);
			}
		}

		/// <summary>
		/// Returns the flow decay but converts values indicating falling liquid (values >=8) to their effective source block
		/// value of zero.
		/// </summary>
		protected virtual int GetEffectiveFlowDecay(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			if (par1IBlockAccess.GetBlockMaterial(par2, par3, par4) != BlockMaterial)
			{
				return -1;
			}

			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4);

			if (i >= 8)
			{
				i = 0;
			}

			return i;
		}

		/// <summary>
		/// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
		/// </summary>
		public override bool RenderAsNormalBlock()
		{
			return false;
		}

		/// <summary>
		/// Is this block (a) opaque and (b) a full 1m cube?  This determines whether or not to render the shared face of two
		/// adjacent blocks and also whether the player can attach torches, redstone wire, etc to this block.
		/// </summary>
		public override bool IsOpaqueCube()
		{
			return false;
		}

		/// <summary>
		/// Returns whether this block is collideable based on the arguments passed in Args: blockMetaData, unknownFlag
		/// </summary>
		public override bool CanCollideCheck(int par1, bool par2)
		{
			return par2 && par1 == 0;
		}

		/// <summary>
		/// Returns Returns true if the given side of this block type should be rendered (if it's solid or not), if the
		/// adjacent block is at the given coordinates. Args: blockAccess, x, y, z, side
		/// </summary>
		public override bool IsBlockSolid(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			Material material = par1IBlockAccess.GetBlockMaterial(par2, par3, par4);

			if (material == BlockMaterial)
			{
				return false;
			}

			if (par5 == 1)
			{
				return true;
			}

			if (material == Material.Ice)
			{
				return false;
			}
			else
			{
				return base.IsBlockSolid(par1IBlockAccess, par2, par3, par4, par5);
			}
		}

		/// <summary>
		/// Returns true if the given side of this block type should be rendered, if the adjacent block is at the given
		/// coordinates.  Args: blockAccess, x, y, z, side
		/// </summary>
		public override bool ShouldSideBeRendered(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			Material material = par1IBlockAccess.GetBlockMaterial(par2, par3, par4);

			if (material == BlockMaterial)
			{
				return false;
			}

			if (par5 == 1)
			{
				return true;
			}

			if (material == Material.Ice)
			{
				return false;
			}
			else
			{
				return base.ShouldSideBeRendered(par1IBlockAccess, par2, par3, par4, par5);
			}
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int i)
		{
			return null;
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 4;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return 0;
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 0;
		}

		/// <summary>
		/// Returns a vector indicating the direction and intensity of fluid flow.
		/// </summary>
		private Vec3D GetFlowVector(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			Vec3D vec3d = Vec3D.CreateVector(0.0F, 0.0F, 0.0F);
			int i = GetEffectiveFlowDecay(par1IBlockAccess, par2, par3, par4);

			for (int j = 0; j < 4; j++)
			{
				int k = par2;
				int l = par3;
				int i1 = par4;

				if (j == 0)
				{
					k--;
				}

				if (j == 1)
				{
					i1--;
				}

				if (j == 2)
				{
					k++;
				}

				if (j == 3)
				{
					i1++;
				}

				int j1 = GetEffectiveFlowDecay(par1IBlockAccess, k, l, i1);

				if (j1 < 0)
				{
					if (par1IBlockAccess.GetBlockMaterial(k, l, i1).BlocksMovement())
					{
						continue;
					}

					j1 = GetEffectiveFlowDecay(par1IBlockAccess, k, l - 1, i1);

					if (j1 >= 0)
					{
						int k1 = j1 - (i - 8);
						vec3d = vec3d.AddVector((k - par2) * k1, (l - par3) * k1, (i1 - par4) * k1);
					}

					continue;
				}

				if (j1 >= 0)
				{
					int l1 = j1 - i;
					vec3d = vec3d.AddVector((k - par2) * l1, (l - par3) * l1, (i1 - par4) * l1);
				}
			}

			if (par1IBlockAccess.GetBlockMetadata(par2, par3, par4) >= 8)
			{
				bool flag = false;

				if (flag || IsBlockSolid(par1IBlockAccess, par2, par3, par4 - 1, 2))
				{
					flag = true;
				}

				if (flag || IsBlockSolid(par1IBlockAccess, par2, par3, par4 + 1, 3))
				{
					flag = true;
				}

				if (flag || IsBlockSolid(par1IBlockAccess, par2 - 1, par3, par4, 4))
				{
					flag = true;
				}

				if (flag || IsBlockSolid(par1IBlockAccess, par2 + 1, par3, par4, 5))
				{
					flag = true;
				}

				if (flag || IsBlockSolid(par1IBlockAccess, par2, par3 + 1, par4 - 1, 2))
				{
					flag = true;
				}

				if (flag || IsBlockSolid(par1IBlockAccess, par2, par3 + 1, par4 + 1, 3))
				{
					flag = true;
				}

				if (flag || IsBlockSolid(par1IBlockAccess, par2 - 1, par3 + 1, par4, 4))
				{
					flag = true;
				}

				if (flag || IsBlockSolid(par1IBlockAccess, par2 + 1, par3 + 1, par4, 5))
				{
					flag = true;
				}

				if (flag)
				{
					vec3d = vec3d.Normalize().AddVector(0.0F, -6D, 0.0F);
				}
			}

			vec3d = vec3d.Normalize();
			return vec3d;
		}

		/// <summary>
		/// Can add to the passed in vector for a movement vector to be applied to the entity. Args: x, y, z, entity, vec3d
		/// </summary>
		public override void VelocityToAddToEntity(World par1World, int par2, int par3, int par4, Entity par5Entity, Vec3D par6Vec3D)
		{
			Vec3D vec3d = GetFlowVector(par1World, par2, par3, par4);
			par6Vec3D.XCoord += vec3d.XCoord;
			par6Vec3D.YCoord += vec3d.YCoord;
			par6Vec3D.ZCoord += vec3d.ZCoord;
		}

		/// <summary>
		/// How many world ticks before ticking
		/// </summary>
		public override int TickRate()
		{
			if (BlockMaterial == Material.Water)
			{
				return 5;
			}

			return BlockMaterial != Material.Lava ? 0 : 30;
		}

		/// <summary>
		/// 'Goes straight to getLightBrightnessForSkyBlocks for Blocks, does some fancy computing for Fluids'
		/// </summary>
		public override int GetMixedBrightnessForBlock(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = par1IBlockAccess.GetLightBrightnessForSkyBlocks(par2, par3, par4, 0);
			int j = par1IBlockAccess.GetLightBrightnessForSkyBlocks(par2, par3 + 1, par4, 0);
			int k = i & 0xff;
			int l = j & 0xff;
			int i1 = i >> 16 & 0xff;
			int j1 = j >> 16 & 0xff;
			return (k <= l ? l : k) | (i1 <= j1 ? j1 : i1) << 16;
		}

		/// <summary>
		/// How bright to render this block based on the light its receiving. Args: iBlockAccess, x, y, z
		/// </summary>
		public override float GetBlockBrightness(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			float f = par1IBlockAccess.GetLightBrightness(par2, par3, par4);
			float f1 = par1IBlockAccess.GetLightBrightness(par2, par3 + 1, par4);
			return f <= f1 ? f1 : f;
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			base.UpdateTick(par1World, par2, par3, par4, par5Random);
		}

		/// <summary>
		/// Returns which pass should this block be rendered on. 0 for solids and 1 for alpha
		/// </summary>
		public override int GetRenderBlockPass()
		{
			return BlockMaterial != Material.Water ? 0 : 1;
		}

		/// <summary>
		/// A randomly called display update to be able to add particles or other items for display
		/// </summary>
		public override void RandomDisplayTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (BlockMaterial == Material.Water)
			{
				if (par5Random.Next(10) == 0)
				{
					int i = par1World.GetBlockMetadata(par2, par3, par4);

					if (i <= 0 || i >= 8)
					{
						par1World.SpawnParticle("suspended", (float)par2 + par5Random.NextFloat(), (float)par3 + par5Random.NextFloat(), (float)par4 + par5Random.NextFloat(), 0.0F, 0.0F, 0.0F);
					}
				}

				for (int j = 0; j < 0; j++)
				{
					int l = par5Random.Next(4);
					int i1 = par2;
					int j1 = par4;

					if (l == 0)
					{
						i1--;
					}

					if (l == 1)
					{
						i1++;
					}

					if (l == 2)
					{
						j1--;
					}

					if (l == 3)
					{
						j1++;
					}

					if (par1World.GetBlockMaterial(i1, par3, j1) != Material.Air || !par1World.GetBlockMaterial(i1, par3 - 1, j1).BlocksMovement() && !par1World.GetBlockMaterial(i1, par3 - 1, j1).IsLiquid())
					{
						continue;
					}

					float f = 0.0625F;
					double d6 = (float)par2 + par5Random.NextFloat();
					double d7 = (float)par3 + par5Random.NextFloat();
					double d8 = (float)par4 + par5Random.NextFloat();

					if (l == 0)
					{
						d6 = (float)par2 - f;
					}

					if (l == 1)
					{
						d6 = (float)(par2 + 1) + f;
					}

					if (l == 2)
					{
						d8 = (float)par4 - f;
					}

					if (l == 3)
					{
						d8 = (float)(par4 + 1) + f;
					}

					double d9 = 0.0F;
					double d10 = 0.0F;

					if (l == 0)
					{
						d9 = -f;
					}

					if (l == 1)
					{
						d9 = f;
					}

					if (l == 2)
					{
						d10 = -f;
					}

					if (l == 3)
					{
						d10 = f;
					}

					par1World.SpawnParticle("splash", d6, d7, d8, d9, 0.0F, d10);
				}
			}

			if (BlockMaterial == Material.Water && par5Random.Next(64) == 0)
			{
				int k = par1World.GetBlockMetadata(par2, par3, par4);

				if (k > 0 && k < 8)
				{
					par1World.PlaySoundEffect((float)par2 + 0.5F, (float)par3 + 0.5F, (float)par4 + 0.5F, "liquid.water", par5Random.NextFloat() * 0.25F + 0.75F, par5Random.NextFloat() * 1.0F + 0.5F);
				}
			}

			if (BlockMaterial == Material.Lava && par1World.GetBlockMaterial(par2, par3 + 1, par4) == Material.Air && !par1World.IsBlockOpaqueCube(par2, par3 + 1, par4))
			{
				if (par5Random.Next(100) == 0)
				{
					double d = (float)par2 + par5Random.NextFloat();
					double d2 = (double)par3 + MaxY;
					double d4 = (float)par4 + par5Random.NextFloat();
					par1World.SpawnParticle("lava", d, d2, d4, 0.0F, 0.0F, 0.0F);
					par1World.PlaySoundEffect(d, d2, d4, "liquid.lavapop", 0.2F + par5Random.NextFloat() * 0.2F, 0.9F + par5Random.NextFloat() * 0.15F);
				}

				if (par5Random.Next(200) == 0)
				{
					par1World.PlaySoundEffect(par2, par3, par4, "liquid.lava", 0.2F + par5Random.NextFloat() * 0.2F, 0.9F + par5Random.NextFloat() * 0.15F);
				}
			}

			if (par5Random.Next(10) == 0 && par1World.IsBlockNormalCube(par2, par3 - 1, par4) && !par1World.GetBlockMaterial(par2, par3 - 2, par4).BlocksMovement())
			{
				double d1 = (float)par2 + par5Random.NextFloat();
				double d3 = (double)par3 - 1.05D;
				double d5 = (float)par4 + par5Random.NextFloat();

				if (BlockMaterial == Material.Water)
				{
					par1World.SpawnParticle("dripWater", d1, d3, d5, 0.0F, 0.0F, 0.0F);
				}
				else
				{
					par1World.SpawnParticle("dripLava", d1, d3, d5, 0.0F, 0.0F, 0.0F);
				}
			}
		}

		public static double Func_293_a(IBlockAccess par0IBlockAccess, int par1, int par2, int par3, Material par4Material)
		{
			Vec3D vec3d = null;

			if (par4Material == Material.Water)
			{
				vec3d = ((BlockFluid)Block.WaterMoving).GetFlowVector(par0IBlockAccess, par1, par2, par3);
			}

			if (par4Material == Material.Lava)
			{
				vec3d = ((BlockFluid)Block.LavaMoving).GetFlowVector(par0IBlockAccess, par1, par2, par3);
			}

			if (vec3d.XCoord == 0.0F && vec3d.ZCoord == 0.0F)
			{
				return -1000D;
			}
			else
			{
				return Math.Atan2(vec3d.ZCoord, vec3d.XCoord) - (Math.PI / 2D);
			}
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			CheckForHarden(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			CheckForHarden(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Forces lava to check to see if it is colliding with water, and then decide what it should harden to.
		/// </summary>
		private void CheckForHarden(World par1World, int par2, int par3, int par4)
		{
			if (par1World.GetBlockId(par2, par3, par4) != BlockID)
			{
				return;
			}

			if (BlockMaterial == Material.Lava)
			{
				bool flag = false;

				if (flag || par1World.GetBlockMaterial(par2, par3, par4 - 1) == Material.Water)
				{
					flag = true;
				}

				if (flag || par1World.GetBlockMaterial(par2, par3, par4 + 1) == Material.Water)
				{
					flag = true;
				}

				if (flag || par1World.GetBlockMaterial(par2 - 1, par3, par4) == Material.Water)
				{
					flag = true;
				}

				if (flag || par1World.GetBlockMaterial(par2 + 1, par3, par4) == Material.Water)
				{
					flag = true;
				}

				if (flag || par1World.GetBlockMaterial(par2, par3 + 1, par4) == Material.Water)
				{
					flag = true;
				}

				if (flag)
				{
					int i = par1World.GetBlockMetadata(par2, par3, par4);

					if (i == 0)
					{
						par1World.SetBlockWithNotify(par2, par3, par4, Block.Obsidian.BlockID);
					}
					else if (i <= 4)
					{
						par1World.SetBlockWithNotify(par2, par3, par4, Block.Cobblestone.BlockID);
					}

					TriggerLavaMixEffects(par1World, par2, par3, par4);
				}
			}
		}

		/// <summary>
		/// Creates fizzing sound and smoke. Used when lava flows over block or mixes with water.
		/// </summary>
		protected virtual void TriggerLavaMixEffects(World par1World, int par2, int par3, int par4)
		{
			par1World.PlaySoundEffect((float)par2 + 0.5F, (float)par3 + 0.5F, (float)par4 + 0.5F, "random.fizz", 0.5F, 2.6F + (par1World.Rand.NextFloat() - par1World.Rand.NextFloat()) * 0.8F);

			for (int i = 0; i < 8; i++)
			{
				par1World.SpawnParticle("largesmoke", (double)par2 + (new Random(1)).NextDouble(), (double)par3 + 1.2D, (double)par4 + new Random(2).NextDouble(), 0.0F, 0.0F, 0.0F);
			}
		}
	}
}