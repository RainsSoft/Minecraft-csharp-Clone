using System.Collections.Generic;

namespace net.minecraft.src
{
	public class Session
	{
		public static List<Block> RegisteredBlocksList;
		public string Username;
		public string SessionId;
		public string MpPassParameter;

		public Session(string par1Str, string par2Str)
		{
			Username = par1Str;
			SessionId = par2Str;
		}

		static Session()
		{
            RegisteredBlocksList = new List<Block>();
			RegisteredBlocksList.Add(Block.Stone);
			RegisteredBlocksList.Add(Block.Cobblestone);
			RegisteredBlocksList.Add(Block.Brick);
			RegisteredBlocksList.Add(Block.Dirt);
			RegisteredBlocksList.Add(Block.Planks);
			RegisteredBlocksList.Add(Block.Wood);
			RegisteredBlocksList.Add(Block.Leaves);
			RegisteredBlocksList.Add(Block.TorchWood);
			RegisteredBlocksList.Add(Block.StairSingle);
			RegisteredBlocksList.Add(Block.Glass);
			RegisteredBlocksList.Add(Block.CobblestoneMossy);
			RegisteredBlocksList.Add(Block.Sapling);
			RegisteredBlocksList.Add(Block.PlantYellow);
			RegisteredBlocksList.Add(Block.PlantRed);
			RegisteredBlocksList.Add(Block.MushroomBrown);
			RegisteredBlocksList.Add(Block.MushroomRed);
			RegisteredBlocksList.Add(Block.Sand);
			RegisteredBlocksList.Add(Block.Gravel);
			RegisteredBlocksList.Add(Block.Sponge);
			RegisteredBlocksList.Add(Block.Cloth);
			RegisteredBlocksList.Add(Block.OreCoal);
			RegisteredBlocksList.Add(Block.OreIron);
			RegisteredBlocksList.Add(Block.OreGold);
			RegisteredBlocksList.Add(Block.BlockSteel);
			RegisteredBlocksList.Add(Block.BlockGold);
			RegisteredBlocksList.Add(Block.BookShelf);
			RegisteredBlocksList.Add(Block.Tnt);
			RegisteredBlocksList.Add(Block.Obsidian);
		}
	}
}