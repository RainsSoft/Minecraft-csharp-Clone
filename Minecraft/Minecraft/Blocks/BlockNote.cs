using System;
using System.Text;

namespace net.minecraft.src
{
	public class BlockNote : BlockContainer
	{
		public BlockNote(int par1) : base(par1, 74, Material.Wood)
		{
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			return BlockIndexInTexture;
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			if (par5 > 0)
			{
				bool flag = par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4);
				TileEntityNote tileentitynote = (TileEntityNote)par1World.GetBlockTileEntity(par2, par3, par4);

				if (tileentitynote != null && tileentitynote.PreviousRedstoneState != flag)
				{
					if (flag)
					{
						tileentitynote.TriggerNote(par1World, par2, par3, par4);
					}

					tileentitynote.PreviousRedstoneState = flag;
				}
			}
		}

		/// <summary>
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			if (par1World.IsRemote)
			{
				return true;
			}

			TileEntityNote tileentitynote = (TileEntityNote)par1World.GetBlockTileEntity(par2, par3, par4);

			if (tileentitynote != null)
			{
				tileentitynote.ChangePitch();
				tileentitynote.TriggerNote(par1World, par2, par3, par4);
			}

			return true;
		}

		/// <summary>
		/// Called when the block is clicked by a player. Args: x, y, z, entityPlayer
		/// </summary>
		public override void OnBlockClicked(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			if (par1World.IsRemote)
			{
				return;
			}

			TileEntityNote tileentitynote = (TileEntityNote)par1World.GetBlockTileEntity(par2, par3, par4);

			if (tileentitynote != null)
			{
				tileentitynote.TriggerNote(par1World, par2, par3, par4);
			}
		}

		/// <summary>
		/// Returns the TileEntity used by this block.
		/// </summary>
		public override TileEntity GetBlockEntity()
		{
			return new TileEntityNote();
		}

		public override void PowerBlock(World par1World, int par2, int par3, int par4, int par5, int par6)
		{
			float f = (float)Math.Pow(2D, (double)(par6 - 12) / 12D);
			string s = "harp";

			if (par5 == 1)
			{
				s = "bd";
			}

			if (par5 == 2)
			{
				s = "snare";
			}

			if (par5 == 3)
			{
				s = "hat";
			}

			if (par5 == 4)
			{
				s = "bassattack";
			}

			par1World.PlaySoundEffect((double)par2 + 0.5D, (double)par3 + 0.5D, (double)par4 + 0.5D, (new StringBuilder()).Append("note.").Append(s).ToString(), 3F, f);
			par1World.SpawnParticle("note", (double)par2 + 0.5D, (double)par3 + 1.2D, (double)par4 + 0.5D, (double)par6 / 24D, 0.0F, 0.0F);
		}
	}
}