namespace net.minecraft.src
{
	public class ItemHoe : Item
	{
		public ItemHoe(int par1, ToolMaterial par2EnumToolMaterial) : base(par1)
		{
			MaxStackSize = 1;
			SetMaxDamage(par2EnumToolMaterial.MaxUses);
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public override bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int par4, int par5, int par6, int par7)
		{
			if (!par2EntityPlayer.CanPlayerEdit(par4, par5, par6))
			{
				return false;
			}

			int i = par3World.GetBlockId(par4, par5, par6);
			int j = par3World.GetBlockId(par4, par5 + 1, par6);

			if (par7 != 0 && j == 0 && i == Block.Grass.BlockID || i == Block.Dirt.BlockID)
			{
				Block block = Block.TilledField;
				par3World.PlaySoundEffect((float)par4 + 0.5F, (float)par5 + 0.5F, (float)par6 + 0.5F, block.StepSound.GetStepSound(), (block.StepSound.GetVolume() + 1.0F) / 2.0F, block.StepSound.GetPitch() * 0.8F);

				if (par3World.IsRemote)
				{
					return true;
				}
				else
				{
					par3World.SetBlockWithNotify(par4, par5, par6, block.BlockID);
					par1ItemStack.DamageItem(1, par2EntityPlayer);
					return true;
				}
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Returns True is the item is renderer in full 3D when hold.
		/// </summary>
		public override bool IsFull3D()
		{
			return true;
		}
	}
}