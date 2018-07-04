using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BlockStairs : Block
	{
		/// <summary>
		/// The block that is used as model for the stair. </summary>
		private Block ModelBlock;

        public BlockStairs(int par1, Block par2Block)
            : base(par1, par2Block.BlockIndexInTexture, par2Block.BlockMaterial)
		{
			ModelBlock = par2Block;
			SetHardness(par2Block.BlockHardness);
			SetResistance(par2Block.BlockResistance / 3F);
			SetStepSound(par2Block.StepSound);
			SetLightOpacity(255);
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			return base.GetCollisionBoundingBoxFromPool(par1World, par2, par3, par4);
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
		/// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
		/// </summary>
		public override bool RenderAsNormalBlock()
		{
			return false;
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 10;
		}

		/// <summary>
		/// Returns true if the given side of this block type should be rendered, if the adjacent block is at the given
		/// coordinates.  Args: blockAccess, x, y, z, side
		/// </summary>
		public override bool ShouldSideBeRendered(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			return base.ShouldSideBeRendered(par1IBlockAccess, par2, par3, par4, par5);
		}

		/// <summary>
		/// Adds to the supplied array any colliding bounding boxes with the passed in bounding box. Args: world, x, y, z,
		/// axisAlignedBB, arrayList
		/// </summary>
        public override void GetCollidingBoundingBoxes(World par1World, int par2, int par3, int par4, AxisAlignedBB par5AxisAlignedBB, List<AxisAlignedBB> par6ArrayList)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			int j = i & 3;
			float f = 0.0F;
			float f1 = 0.5F;
			float f2 = 0.5F;
			float f3 = 1.0F;

			if ((i & 4) != 0)
			{
				f = 0.5F;
				f1 = 1.0F;
				f2 = 0.0F;
				f3 = 0.5F;
			}

			SetBlockBounds(0.0F, f, 0.0F, 1.0F, f1, 1.0F);
			base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);

			if (j == 0)
			{
				SetBlockBounds(0.5F, f2, 0.0F, 1.0F, f3, 1.0F);
				base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			}
			else if (j == 1)
			{
				SetBlockBounds(0.0F, f2, 0.0F, 0.5F, f3, 1.0F);
				base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			}
			else if (j == 2)
			{
				SetBlockBounds(0.0F, f2, 0.5F, 1.0F, f3, 1.0F);
				base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			}
			else if (j == 3)
			{
				SetBlockBounds(0.0F, f2, 0.0F, 1.0F, f3, 0.5F);
				base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
			}

			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
		}

		/// <summary>
		/// A randomly called display update to be able to add particles or other items for display
		/// </summary>
		public override void RandomDisplayTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			ModelBlock.RandomDisplayTick(par1World, par2, par3, par4, par5Random);
		}

		/// <summary>
		/// Called when the block is clicked by a player. Args: x, y, z, entityPlayer
		/// </summary>
		public override void OnBlockClicked(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			ModelBlock.OnBlockClicked(par1World, par2, par3, par4, par5EntityPlayer);
		}

		/// <summary>
		/// Called right before the block is destroyed by a player.  Args: world, x, y, z, metaData
		/// </summary>
		public override void OnBlockDestroyedByPlayer(World par1World, int par2, int par3, int par4, int par5)
		{
			ModelBlock.OnBlockDestroyedByPlayer(par1World, par2, par3, par4, par5);
		}

		/// <summary>
		/// 'Goes straight to getLightBrightnessForSkyBlocks for Blocks, does some fancy computing for Fluids'
		/// </summary>
		public override int GetMixedBrightnessForBlock(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return ModelBlock.GetMixedBrightnessForBlock(par1IBlockAccess, par2, par3, par4);
		}

		/// <summary>
		/// How bright to render this block based on the light its receiving. Args: iBlockAccess, x, y, z
		/// </summary>
		public override float GetBlockBrightness(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return ModelBlock.GetBlockBrightness(par1IBlockAccess, par2, par3, par4);
		}

		/// <summary>
		/// Returns how much this block can resist explosions from the passed in entity.
		/// </summary>
		public override float GetExplosionResistance(Entity par1Entity)
		{
			return ModelBlock.GetExplosionResistance(par1Entity);
		}

		/// <summary>
		/// Returns which pass should this block be rendered on. 0 for solids and 1 for alpha
		/// </summary>
		public override int GetRenderBlockPass()
		{
			return ModelBlock.GetRenderBlockPass();
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			return ModelBlock.GetBlockTextureFromSideAndMetadata(par1, 0);
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			return ModelBlock.GetBlockTextureFromSideAndMetadata(par1, 0);
		}

		/// <summary>
		/// How many world ticks before ticking
		/// </summary>
		public override int TickRate()
		{
			return ModelBlock.TickRate();
		}

		/// <summary>
		/// Returns the bounding box of the wired rectangular prism to render.
		/// </summary>
		public override AxisAlignedBB GetSelectedBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			return ModelBlock.GetSelectedBoundingBoxFromPool(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Can add to the passed in vector for a movement vector to be applied to the entity. Args: x, y, z, entity, vec3d
		/// </summary>
		public override void VelocityToAddToEntity(World par1World, int par2, int par3, int par4, Entity par5Entity, Vec3D par6Vec3D)
		{
			ModelBlock.VelocityToAddToEntity(par1World, par2, par3, par4, par5Entity, par6Vec3D);
		}

		/// <summary>
		/// Returns if this block is collidable (only used by Fire). Args: x, y, z
		/// </summary>
		public override bool IsCollidable()
		{
			return ModelBlock.IsCollidable();
		}

		/// <summary>
		/// Returns whether this block is collideable based on the arguments passed in Args: blockMetaData, unknownFlag
		/// </summary>
		public override bool CanCollideCheck(int par1, bool par2)
		{
			return ModelBlock.CanCollideCheck(par1, par2);
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			return ModelBlock.CanPlaceBlockAt(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			OnNeighborBlockChange(par1World, par2, par3, par4, 0);
			ModelBlock.OnBlockAdded(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public override void OnBlockRemoval(World par1World, int par2, int par3, int par4)
		{
			ModelBlock.OnBlockRemoval(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Called whenever an entity is walking on top of this block. Args: world, x, y, z, entity
		/// </summary>
		public override void OnEntityWalking(World par1World, int par2, int par3, int par4, Entity par5Entity)
		{
			ModelBlock.OnEntityWalking(par1World, par2, par3, par4, par5Entity);
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			ModelBlock.UpdateTick(par1World, par2, par3, par4, par5Random);
		}

		/// <summary>
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			return ModelBlock.BlockActivated(par1World, par2, par3, par4, par5EntityPlayer);
		}

		/// <summary>
		/// Called upon the block being destroyed by an explosion
		/// </summary>
		public override void OnBlockDestroyedByExplosion(World par1World, int par2, int par3, int par4)
		{
			ModelBlock.OnBlockDestroyedByExplosion(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Called when the block is placed in the world.
		/// </summary>
		public override void OnBlockPlacedBy(World par1World, int par2, int par3, int par4, EntityLiving par5EntityLiving)
		{
			int i = MathHelper2.Floor_double((double)((par5EntityLiving.RotationYaw * 4F) / 360F) + 0.5D) & 3;
			int j = par1World.GetBlockMetadata(par2, par3, par4) & 4;

			if (i == 0)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 2 | j);
			}

			if (i == 1)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 1 | j);
			}

			if (i == 2)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 3 | j);
			}

			if (i == 3)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 0 | j);
			}
		}

		/// <summary>
		/// Called when a block is placed using an item. Used often for taking the facing and figuring out how to position
		/// the item. Args: x, y, z, facing
		/// </summary>
		public override void OnBlockPlaced(World par1World, int par2, int par3, int par4, int par5)
		{
			if (par5 == 0)
			{
				int i = par1World.GetBlockMetadata(par2, par3, par4);
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, i | 4);
			}
		}
	}

}