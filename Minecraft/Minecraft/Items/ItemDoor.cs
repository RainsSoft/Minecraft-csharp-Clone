namespace net.minecraft.src
{

	public class ItemDoor : Item
	{
		private Material DoorMaterial;

		public ItemDoor(int par1, Material par2Material) : base(par1)
		{
			DoorMaterial = par2Material;
			MaxStackSize = 1;
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public override bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int par4, int par5, int par6, int par7)
		{
			if (par7 != 1)
			{
				return false;
			}

			par5++;
			Block block;

			if (DoorMaterial == Material.Wood)
			{
				block = Block.DoorWood;
			}
			else
			{
				block = Block.DoorSteel;
			}

			if (!par2EntityPlayer.CanPlayerEdit(par4, par5, par6) || !par2EntityPlayer.CanPlayerEdit(par4, par5 + 1, par6))
			{
				return false;
			}

			if (!block.CanPlaceBlockAt(par3World, par4, par5, par6))
			{
				return false;
			}
			else
			{
				int i = MathHelper2.Floor_double((double)(((par2EntityPlayer.RotationYaw + 180F) * 4F) / 360F) - 0.5D) & 3;
				PlaceDoorBlock(par3World, par4, par5, par6, i, block);
				par1ItemStack.StackSize--;
				return true;
			}
		}

		public static void PlaceDoorBlock(World par0World, int par1, int par2, int par3, int par4, Block par5Block)
		{
			sbyte byte0 = 0;
			sbyte byte1 = 0;

			if (par4 == 0)
			{
				byte1 = 1;
			}

			if (par4 == 1)
			{
				byte0 = -1;
			}

			if (par4 == 2)
			{
				byte1 = -1;
			}

			if (par4 == 3)
			{
				byte0 = 1;
			}

			int i = (par0World.IsBlockNormalCube(par1 - byte0, par2, par3 - byte1) ? 1 : 0) + (par0World.IsBlockNormalCube(par1 - byte0, par2 + 1, par3 - byte1) ? 1 : 0);
			int j = (par0World.IsBlockNormalCube(par1 + byte0, par2, par3 + byte1) ? 1 : 0) + (par0World.IsBlockNormalCube(par1 + byte0, par2 + 1, par3 + byte1) ? 1 : 0);
			bool flag = par0World.GetBlockId(par1 - byte0, par2, par3 - byte1) == par5Block.BlockID || par0World.GetBlockId(par1 - byte0, par2 + 1, par3 - byte1) == par5Block.BlockID;
			bool flag1 = par0World.GetBlockId(par1 + byte0, par2, par3 + byte1) == par5Block.BlockID || par0World.GetBlockId(par1 + byte0, par2 + 1, par3 + byte1) == par5Block.BlockID;
			bool flag2 = false;

			if (flag && !flag1)
			{
				flag2 = true;
			}
			else if (j > i)
			{
				flag2 = true;
			}

			par0World.EditingBlocks = true;
			par0World.SetBlockAndMetadataWithNotify(par1, par2, par3, par5Block.BlockID, par4);
			par0World.SetBlockAndMetadataWithNotify(par1, par2 + 1, par3, par5Block.BlockID, 8 | (flag2 ? 1 : 0));
			par0World.EditingBlocks = false;
			par0World.NotifyBlocksOfNeighborChange(par1, par2, par3, par5Block.BlockID);
			par0World.NotifyBlocksOfNeighborChange(par1, par2 + 1, par3, par5Block.BlockID);
		}
	}

}