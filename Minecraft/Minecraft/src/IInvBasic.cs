namespace net.minecraft.src
{

	public interface IInvBasic
	{
		/// <summary>
		/// Called by InventoryBasic.onInventoryChanged() on a array that is never filled.
		/// </summary>
		void OnInventoryChanged(InventoryBasic inventorybasic);
	}

}