using System;

namespace net.minecraft.src
{
	public class BlockRedstoneOre : Block
	{
		private bool Glowing;

		public BlockRedstoneOre(int par1, int par2, bool par3) : base(par1, par2, Material.Rock)
		{

			if (par3)
			{
				SetTickRandomly(true);
			}

			Glowing = par3;
		}

		/// <summary>
		/// How many world ticks before ticking
		/// </summary>
		public override int TickRate()
		{
			return 30;
		}

		/// <summary>
		/// Called when the block is clicked by a player. Args: x, y, z, entityPlayer
		/// </summary>
		public override void OnBlockClicked(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			Glow(par1World, par2, par3, par4);
			base.OnBlockClicked(par1World, par2, par3, par4, par5EntityPlayer);
		}

		/// <summary>
		/// Called whenever an entity is walking on top of this block. Args: world, x, y, z, entity
		/// </summary>
		public override void OnEntityWalking(World par1World, int par2, int par3, int par4, Entity par5Entity)
		{
			Glow(par1World, par2, par3, par4);
			base.OnEntityWalking(par1World, par2, par3, par4, par5Entity);
		}

		/// <summary>
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			Glow(par1World, par2, par3, par4);
			return base.BlockActivated(par1World, par2, par3, par4, par5EntityPlayer);
		}

		/// <summary>
		/// The redstone ore glows.
		/// </summary>
		private void Glow(World par1World, int par2, int par3, int par4)
		{
			Sparkle(par1World, par2, par3, par4);

			if (BlockID == Block.OreRedstone.BlockID)
			{
				par1World.SetBlockWithNotify(par2, par3, par4, Block.OreRedstoneGlowing.BlockID);
			}
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (BlockID == Block.OreRedstoneGlowing.BlockID)
			{
				par1World.SetBlockWithNotify(par2, par3, par4, Block.OreRedstone.BlockID);
			}
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Item.Redstone.ShiftedIndex;
		}

		/// <summary>
		/// Returns the usual quantity dropped by the block plus a bonus of 1 to 'i' (inclusive).
		/// </summary>
		public override int QuantityDroppedWithBonus(int par1, Random par2Random)
		{
			return QuantityDropped(par2Random) + par2Random.Next(par1 + 1);
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 4 + par1Random.Next(2);
		}

		/// <summary>
		/// A randomly called display update to be able to add particles or other items for display
		/// </summary>
		public override void RandomDisplayTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (Glowing)
			{
				Sparkle(par1World, par2, par3, par4);
			}
		}

		/// <summary>
		/// The redstone ore sparkles.
		/// </summary>
		private void Sparkle(World par1World, int par2, int par3, int par4)
		{
			Random random = par1World.Rand;
			double d = 0.0625D;

			for (int i = 0; i < 6; i++)
			{
				double d1 = (float)par2 + random.NextFloat();
				double d2 = (float)par3 + random.NextFloat();
				double d3 = (float)par4 + random.NextFloat();

				if (i == 0 && !par1World.IsBlockOpaqueCube(par2, par3 + 1, par4))
				{
					d2 = (double)(par3 + 1) + d;
				}

				if (i == 1 && !par1World.IsBlockOpaqueCube(par2, par3 - 1, par4))
				{
					d2 = (double)(par3 + 0) - d;
				}

				if (i == 2 && !par1World.IsBlockOpaqueCube(par2, par3, par4 + 1))
				{
					d3 = (double)(par4 + 1) + d;
				}

				if (i == 3 && !par1World.IsBlockOpaqueCube(par2, par3, par4 - 1))
				{
					d3 = (double)(par4 + 0) - d;
				}

				if (i == 4 && !par1World.IsBlockOpaqueCube(par2 + 1, par3, par4))
				{
					d1 = (double)(par2 + 1) + d;
				}

				if (i == 5 && !par1World.IsBlockOpaqueCube(par2 - 1, par3, par4))
				{
					d1 = (double)(par2 + 0) - d;
				}

				if (d1 < (double)par2 || d1 > (double)(par2 + 1) || d2 < 0.0F || d2 > (double)(par3 + 1) || d3 < (double)par4 || d3 > (double)(par4 + 1))
				{
					par1World.SpawnParticle("reddust", d1, d2, d3, 0.0F, 0.0F, 0.0F);
				}
			}
		}

		/// <summary>
		/// Returns an item stack containing a single instance of the current block type. 'i' is the block's subtype/damage
		/// and is ignored for blocks which do not support subtypes. Blocks which cannot be harvested should return null.
		/// </summary>
		protected override ItemStack CreateStackedBlock(int par1)
		{
			return new ItemStack(Block.OreRedstone);
		}
	}
}