namespace net.minecraft.src
{

	public interface ICrafting
	{
		/// <summary>
		/// inform the player of a change in a single slot
		/// </summary>
		void UpdateCraftingInventorySlot(Container container, int i, ItemStack itemstack);

		/// <summary>
		/// send information about the crafting inventory to the client(currently only for furnace times)
		/// </summary>
		void UpdateCraftingInventoryInfo(Container container, int i, int j);
	}

}