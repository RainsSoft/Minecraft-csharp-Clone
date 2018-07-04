namespace net.minecraft.src
{

	public class ItemShears : Item
	{
		public ItemShears(int par1) : base(par1)
		{
			SetMaxStackSize(1);
			SetMaxDamage(238);
		}

		public override bool OnBlockDestroyed(ItemStack par1ItemStack, int par2, int par3, int par4, int par5, EntityLiving par6EntityLiving)
		{
			if (par2 == Block.Leaves.BlockID || par2 == Block.Web.BlockID || par2 == Block.TallGrass.BlockID || par2 == Block.Vine.BlockID)
			{
				par1ItemStack.DamageItem(1, par6EntityLiving);
				return true;
			}
			else
			{
				return base.OnBlockDestroyed(par1ItemStack, par2, par3, par4, par5, par6EntityLiving);
			}
		}

		/// <summary>
		/// Returns if the item (tool) can harvest results from the block type.
		/// </summary>
		public override bool CanHarvestBlock(Block par1Block)
		{
			return par1Block.BlockID == Block.Web.BlockID;
		}

		/// <summary>
		/// Returns the strength of the stack against a given block. 1.0F base, (Quality+1)*2 if correct blocktype, 1.5F if
		/// sword
		/// </summary>
		public override float GetStrVsBlock(ItemStack par1ItemStack, Block par2Block)
		{
			if (par2Block.BlockID == Block.Web.BlockID || par2Block.BlockID == Block.Leaves.BlockID)
			{
				return 15F;
			}

			if (par2Block.BlockID == Block.Cloth.BlockID)
			{
				return 5F;
			}
			else
			{
				return base.GetStrVsBlock(par1ItemStack, par2Block);
			}
		}
	}

}