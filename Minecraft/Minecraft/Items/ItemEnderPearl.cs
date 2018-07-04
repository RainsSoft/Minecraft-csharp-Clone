namespace net.minecraft.src
{
	public class ItemEnderPearl : Item
	{
		public ItemEnderPearl(int par1) : base(par1)
		{
			MaxStackSize = 16;
		}

		/// <summary>
		/// Called whenever this item is equipped and the right mouse button is pressed. Args: itemStack, world, entityPlayer
		/// </summary>
		public override ItemStack OnItemRightClick(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			if (par3EntityPlayer.Capabilities.IsCreativeMode)
			{
				return par1ItemStack;
			}

			if (par3EntityPlayer.RidingEntity != null)
			{
				return par1ItemStack;
			}

			par1ItemStack.StackSize--;
			par2World.PlaySoundAtEntity(par3EntityPlayer, "random.bow", 0.5F, 0.4F / (ItemRand.NextFloat() * 0.4F + 0.8F));

			if (!par2World.IsRemote)
			{
				par2World.SpawnEntityInWorld(new EntityEnderPearl(par2World, par3EntityPlayer));
			}

			return par1ItemStack;
		}
	}
}