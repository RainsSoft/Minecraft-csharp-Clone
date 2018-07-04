namespace net.minecraft.src
{
	public class BlockJukeBox : BlockContainer
	{
        public BlockJukeBox(int par1, int par2)
            : base(par1, par2, Material.Wood)
		{
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			return BlockIndexInTexture + (par1 != 1 ? 0 : 1);
		}

		/// <summary>
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			if (par1World.GetBlockMetadata(par2, par3, par4) == 0)
			{
				return false;
			}
			else
			{
				EjectRecord(par1World, par2, par3, par4);
				return true;
			}
		}

		/// <summary>
		/// Inserts the given record into the JukeBox.
		/// </summary>
		public virtual void InsertRecord(World par1World, int par2, int par3, int par4, int par5)
		{
			if (par1World.IsRemote)
			{
				return;
			}

			TileEntityRecordPlayer tileentityrecordplayer = (TileEntityRecordPlayer)par1World.GetBlockTileEntity(par2, par3, par4);

			if (tileentityrecordplayer == null)
			{
				return;
			}
			else
			{
				tileentityrecordplayer.Record = par5;
				tileentityrecordplayer.OnInventoryChanged();
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 1);
				return;
			}
		}

		/// <summary>
		/// Ejects the current record inside of the jukebox.
		/// </summary>
		public virtual void EjectRecord(World par1World, int par2, int par3, int par4)
		{
			if (par1World.IsRemote)
			{
				return;
			}

			TileEntityRecordPlayer tileentityrecordplayer = (TileEntityRecordPlayer)par1World.GetBlockTileEntity(par2, par3, par4);

			if (tileentityrecordplayer == null)
			{
				return;
			}

			int i = tileentityrecordplayer.Record;

			if (i == 0)
			{
				return;
			}
			else
			{
				par1World.PlayAuxSFX(1005, par2, par3, par4, 0);
				par1World.PlayRecord(null, par2, par3, par4);
				tileentityrecordplayer.Record = 0;
				tileentityrecordplayer.OnInventoryChanged();
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 0);
				int j = i;
				float f = 0.7F;
                float d = (par1World.Rand.NextFloat() * f) + (1.0F - f) * 0.5F;
                float d1 = (par1World.Rand.NextFloat() * f) + (1.0F - f) * 0.20000000000000001F + 0.59999999999999998F;
                float d2 = (par1World.Rand.NextFloat() * f) + (1.0F - f) * 0.5F;
				EntityItem entityitem = new EntityItem(par1World, par2 + d, par3 + d1, par4 + d2, new ItemStack(j, 1, 0));
				entityitem.DelayBeforeCanPickup = 10;
				par1World.SpawnEntityInWorld(entityitem);
				return;
			}
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public override void OnBlockRemoval(World par1World, int par2, int par3, int par4)
		{
			EjectRecord(par1World, par2, par3, par4);
			base.OnBlockRemoval(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Drops the block items with a specified chance of dropping the specified items
		/// </summary>
		public override void DropBlockAsItemWithChance(World par1World, int par2, int par3, int par4, int par5, float par6, int par7)
		{
			if (par1World.IsRemote)
			{
				return;
			}
			else
			{
				base.DropBlockAsItemWithChance(par1World, par2, par3, par4, par5, par6, 0);
				return;
			}
		}

		/// <summary>
		/// Returns the TileEntity used by this block.
		/// </summary>
		public override TileEntity GetBlockEntity()
		{
			return new TileEntityRecordPlayer();
		}
	}

}