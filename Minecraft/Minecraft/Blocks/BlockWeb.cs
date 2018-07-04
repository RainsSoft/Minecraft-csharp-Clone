using System;

namespace net.minecraft.src
{
	public class BlockWeb : Block
	{
		public BlockWeb(int par1, int par2) : base(par1, par2, Material.Web)
		{
		}

		/// <summary>
		/// Triggered whenever an entity collides with this block (enters into the block). Args: world, x, y, z, entity
		/// </summary>
		public override void OnEntityCollidedWithBlock(World par1World, int par2, int par3, int par4, Entity par5Entity)
		{
			par5Entity.SetInWeb();
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
			return 1;
		}

		/// <summary>
		/// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
		/// </summary>
		public override bool RenderAsNormalBlock()
		{
			return false;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Item.Silk.ShiftedIndex;
		}
	}

}