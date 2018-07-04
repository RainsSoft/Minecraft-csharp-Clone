namespace net.minecraft.src
{
	public class EntityMooshroom : EntityCow
	{
		public EntityMooshroom(World par1World) : base(par1World)
		{
			Texture = "/mob/redcow.png";
			SetSize(0.9F, 1.3F);
		}

		/// <summary>
		/// Called when a player interacts with a mob. e.g. gets milk from a cow, gets into the saddle on a pig.
		/// </summary>
		public override bool Interact(EntityPlayer par1EntityPlayer)
		{
			ItemStack itemstack = par1EntityPlayer.Inventory.GetCurrentItem();

			if (itemstack != null && itemstack.ItemID == Item.BowlEmpty.ShiftedIndex && GetGrowingAge() >= 0)
			{
				if (itemstack.StackSize == 1)
				{
					par1EntityPlayer.Inventory.SetInventorySlotContents(par1EntityPlayer.Inventory.CurrentItem, new ItemStack(Item.BowlSoup));
					return true;
				}

				if (par1EntityPlayer.Inventory.AddItemStackToInventory(new ItemStack(Item.BowlSoup)) && !par1EntityPlayer.Capabilities.IsCreativeMode)
				{
					par1EntityPlayer.Inventory.DecrStackSize(par1EntityPlayer.Inventory.CurrentItem, 1);
					return true;
				}
			}

			if (itemstack != null && itemstack.ItemID == Item.Shears.ShiftedIndex && GetGrowingAge() >= 0)
			{
				SetDead();
				WorldObj.SpawnParticle("largeexplode", PosX, PosY + (Height / 2.0F), PosZ, 0.0F, 0.0F, 0.0F);

				if (!WorldObj.IsRemote)
				{
					EntityCow entitycow = new EntityCow(WorldObj);
					entitycow.SetLocationAndAngles(PosX, PosY, PosZ, RotationYaw, RotationPitch);
					entitycow.SetEntityHealth(GetHealth());
					entitycow.RenderYawOffset = RenderYawOffset;
					WorldObj.SpawnEntityInWorld(entitycow);

					for (int i = 0; i < 5; i++)
					{
						WorldObj.SpawnEntityInWorld(new EntityItem(WorldObj, PosX, PosY + Height, PosZ, new ItemStack(Block.MushroomRed)));
					}
				}

				return true;
			}
			else
			{
				return base.Interact(par1EntityPlayer);
			}
		}

		/// <summary>
		/// This function is used when two same-species animals in 'love mode' breed to generate the new baby animal.
		/// </summary>
		public override EntityAnimal SpawnBabyAnimal(EntityAnimal par1EntityAnimal)
		{
			return new EntityMooshroom(WorldObj);
		}
	}
}