using System;

namespace net.minecraft.src
{
	public class BlockStone : Block
	{
		public BlockStone(int par1, int par2) : base(par1, par2, Material.Rock)
		{
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Block.Cobblestone.BlockID;
		}
	}

}