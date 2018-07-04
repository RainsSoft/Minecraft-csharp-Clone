namespace net.minecraft.src
{
	public class BlockSponge : Block
	{
        public BlockSponge(int par1)
            : base(par1, Material.Sponge)
		{
			BlockIndexInTexture = 48;
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World world, int i, int j, int k)
		{
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public override void OnBlockRemoval(World world, int i, int j, int k)
		{
		}
	}
}