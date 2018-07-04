using System;

namespace net.minecraft.src
{
	public class BlockMobSpawner : BlockContainer
	{
        public BlockMobSpawner(int par1, int par2)
            : base(par1, par2, Material.Rock)
		{
		}

		/// <summary>
		/// Returns the TileEntity used by this block.
		/// </summary>
		public override TileEntity GetBlockEntity()
		{
			return new TileEntityMobSpawner();
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
		/// Is this block (a) opaque and (b) a full 1m cube?  This determines whether or not to render the shared face of two
		/// adjacent blocks and also whether the player can attach torches, redstone wire, etc to this block.
		/// </summary>
		public override bool IsOpaqueCube()
		{
			return false;
		}
	}

}