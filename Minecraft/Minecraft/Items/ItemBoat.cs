using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ItemBoat : Item
	{
		public ItemBoat(int par1) : base(par1)
		{
			MaxStackSize = 1;
		}

		/// <summary>
		/// Called whenever this item is equipped and the right mouse button is pressed. Args: itemStack, world, entityPlayer
		/// </summary>
		public override ItemStack OnItemRightClick(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			float f = 1.0F;
			float f1 = par3EntityPlayer.PrevRotationPitch + (par3EntityPlayer.RotationPitch - par3EntityPlayer.PrevRotationPitch) * f;
			float f2 = par3EntityPlayer.PrevRotationYaw + (par3EntityPlayer.RotationYaw - par3EntityPlayer.PrevRotationYaw) * f;
			double d = par3EntityPlayer.PrevPosX + (par3EntityPlayer.PosX - par3EntityPlayer.PrevPosX) * (double)f;
			double d1 = (par3EntityPlayer.PrevPosY + (par3EntityPlayer.PosY - par3EntityPlayer.PrevPosY) * (double)f + 1.6200000000000001D) - (double)par3EntityPlayer.YOffset;
			double d2 = par3EntityPlayer.PrevPosZ + (par3EntityPlayer.PosZ - par3EntityPlayer.PrevPosZ) * (double)f;
			Vec3D vec3d = Vec3D.CreateVector(d, d1, d2);
			float f3 = MathHelper2.Cos(-f2 * 0.01745329F - (float)Math.PI);
			float f4 = MathHelper2.Sin(-f2 * 0.01745329F - (float)Math.PI);
			float f5 = -MathHelper2.Cos(-f1 * 0.01745329F);
			float f6 = MathHelper2.Sin(-f1 * 0.01745329F);
			float f7 = f4 * f5;
			float f8 = f6;
			float f9 = f3 * f5;
            float d3 = 5;
			Vec3D vec3d1 = vec3d.AddVector((double)f7 * d3, (double)f8 * d3, (double)f9 * d3);
			MovingObjectPosition movingobjectposition = par2World.RayTraceBlocks_do(vec3d, vec3d1, true);

			if (movingobjectposition == null)
			{
				return par1ItemStack;
			}

			Vec3D vec3d2 = par3EntityPlayer.GetLook(f);
			bool flag = false;
			float f10 = 1.0F;
            List<Entity> list = par2World.GetEntitiesWithinAABBExcludingEntity(par3EntityPlayer, par3EntityPlayer.BoundingBox.AddCoord((float)vec3d2.XCoord * d3, (float)vec3d2.YCoord * d3, (float)vec3d2.ZCoord * d3).Expand(f10, f10, f10));

			for (int l = 0; l < list.Count; l++)
			{
				Entity entity = list[l];

				if (!entity.CanBeCollidedWith())
				{
					continue;
				}

				float f11 = entity.GetCollisionBorderSize();
				AxisAlignedBB axisalignedbb = entity.BoundingBox.Expand(f11, f11, f11);

				if (axisalignedbb.IsVecInside(vec3d))
				{
					flag = true;
				}
			}

			if (flag)
			{
				return par1ItemStack;
			}

			if (movingobjectposition.TypeOfHit == EnumMovingObjectType.TILE)
			{
				int i = movingobjectposition.BlockX;
				int j = movingobjectposition.BlockY;
				int k = movingobjectposition.BlockZ;

				if (!par2World.IsRemote)
				{
					if (par2World.GetBlockId(i, j, k) == Block.Snow.BlockID)
					{
						j--;
					}

					par2World.SpawnEntityInWorld(new EntityBoat(par2World, (float)i + 0.5F, (float)j + 1.0F, (float)k + 0.5F));
				}

				if (!par3EntityPlayer.Capabilities.IsCreativeMode)
				{
					par1ItemStack.StackSize--;
				}
			}

			return par1ItemStack;
		}
	}
}