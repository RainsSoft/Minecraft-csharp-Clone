namespace net.minecraft.src
{

	public class BlockSoulSand : Block
	{
		public BlockSoulSand(int par1, int par2) : base(par1, par2, Material.Sand)
		{
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			float f = 0.125F;
			return AxisAlignedBB.GetBoundingBoxFromPool(par2, par3, par4, par2 + 1, (float)(par3 + 1) - f, par4 + 1);
		}

		/// <summary>
		/// Triggered whenever an entity collides with this block (enters into the block). Args: world, x, y, z, entity
		/// </summary>
		public override void OnEntityCollidedWithBlock(World par1World, int par2, int par3, int par4, Entity par5Entity)
		{
			par5Entity.MotionX *= 0.40000000000000002F;
			par5Entity.MotionZ *= 0.40000000000000002F;
		}
	}
}