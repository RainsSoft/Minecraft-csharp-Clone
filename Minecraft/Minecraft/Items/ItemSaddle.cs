namespace net.minecraft.src
{

	public class ItemSaddle : Item
	{
		public ItemSaddle(int par1) : base(par1)
		{
			MaxStackSize = 1;
		}

		/// <summary>
		/// Called when a player right clicks a entity with a item.
		/// </summary>
		public override void UseItemOnEntity(ItemStack par1ItemStack, EntityLiving par2EntityLiving)
		{
			if (par2EntityLiving is EntityPig)
			{
				EntityPig entitypig = (EntityPig)par2EntityLiving;

				if (!entitypig.GetSaddled() && !entitypig.IsChild())
				{
					entitypig.SetSaddled(true);
					par1ItemStack.StackSize--;
				}
			}
		}

		/// <summary>
		/// Current implementations of this method in child classes do not use the entry argument beside ev. They just raise
		/// the damage on the stack.
		/// </summary>
		public override bool HitEntity(ItemStack par1ItemStack, EntityLiving par2EntityLiving, EntityLiving par3EntityLiving)
		{
			UseItemOnEntity(par1ItemStack, par2EntityLiving);
			return true;
		}
	}

}