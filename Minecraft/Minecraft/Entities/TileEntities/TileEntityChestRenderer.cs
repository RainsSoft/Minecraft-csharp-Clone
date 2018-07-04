using System;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class TileEntityChestRenderer : TileEntitySpecialRenderer
	{
		/// <summary>
		/// The normal small chest model. </summary>
		private ModelChest ChestModel;

		/// <summary>
		/// The large double chest model. </summary>
		private ModelChest LargeChestModel;

		public TileEntityChestRenderer()
		{
			ChestModel = new ModelChest();
			LargeChestModel = new ModelLargeChest();
		}

		/// <summary>
		/// Renders the TileEntity for the chest at a position.
		/// </summary>
		public virtual void RenderTileEntityChestAt(TileEntityChest par1TileEntityChest, double par2, double par4, double par6, float par8)
		{
			int i;

			if (par1TileEntityChest.WorldObj == null)
			{
				i = 0;
			}
			else
			{
				Block block = par1TileEntityChest.GetBlockType();
				i = par1TileEntityChest.GetBlockMetadata();

				if (block != null && i == 0)
				{
					((BlockChest)block).UnifyAdjacentChests(par1TileEntityChest.WorldObj, par1TileEntityChest.XCoord, par1TileEntityChest.YCoord, par1TileEntityChest.ZCoord);
					i = par1TileEntityChest.GetBlockMetadata();
				}

				par1TileEntityChest.CheckForAdjacentChests();
			}

			if (par1TileEntityChest.AdjacentChestZNeg != null || par1TileEntityChest.AdjacentChestXNeg != null)
			{
				return;
			}

			ModelChest modelchest;

			if (par1TileEntityChest.AdjacentChestXPos != null || par1TileEntityChest.AdjacentChestZPos != null)
			{
				modelchest = LargeChestModel;
				BindTextureByName("/item/largechest.png");
			}
			else
			{
				modelchest = ChestModel;
				BindTextureByName("/item/chest.png");
			}

			//GL.PushMatrix();
			//GL.Enable(EnableCap.RescaleNormal);
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			//GL.Translate((float)par2, (float)par4 + 1.0F, (float)par6 + 1.0F);
			//GL.Scale(1.0F, -1F, -1F);
			//GL.Translate(0.5F, 0.5F, 0.5F);
			int j = 0;

			if (i == 2)
			{
				j = 180;
			}

			if (i == 3)
			{
				j = 0;
			}

			if (i == 4)
			{
				j = 90;
			}

			if (i == 5)
			{
				j = -90;
			}

			if (i == 2 && par1TileEntityChest.AdjacentChestXPos != null)
			{
				//GL.Translate(1.0F, 0.0F, 0.0F);
			}

			if (i == 5 && par1TileEntityChest.AdjacentChestZPos != null)
			{
				//GL.Translate(0.0F, 0.0F, -1F);
			}

			//GL.Rotate(j, 0.0F, 1.0F, 0.0F);
			//GL.Translate(-0.5F, -0.5F, -0.5F);
			float f = par1TileEntityChest.PrevLidAngle + (par1TileEntityChest.LidAngle - par1TileEntityChest.PrevLidAngle) * par8;

			if (par1TileEntityChest.AdjacentChestZNeg != null)
			{
				float f1 = par1TileEntityChest.AdjacentChestZNeg.PrevLidAngle + (par1TileEntityChest.AdjacentChestZNeg.LidAngle - par1TileEntityChest.AdjacentChestZNeg.PrevLidAngle) * par8;

				if (f1 > f)
				{
					f = f1;
				}
			}

			if (par1TileEntityChest.AdjacentChestXNeg != null)
			{
				float f2 = par1TileEntityChest.AdjacentChestXNeg.PrevLidAngle + (par1TileEntityChest.AdjacentChestXNeg.LidAngle - par1TileEntityChest.AdjacentChestXNeg.PrevLidAngle) * par8;

				if (f2 > f)
				{
					f = f2;
				}
			}

			f = 1.0F - f;
			f = 1.0F - f * f * f;
			modelchest.ChestLid.RotateAngleX = -((f * (float)Math.PI) / 2.0F);
			modelchest.RenderAll();
			//GL.Disable(EnableCap.RescaleNormal);
			//GL.PopMatrix();
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
		}

        public override void RenderTileEntityAt(TileEntity par1TileEntity, float par2, float par4, float par6, float par8)
		{
			RenderTileEntityChestAt((TileEntityChest)par1TileEntity, par2, par4, par6, par8);
		}
	}
}