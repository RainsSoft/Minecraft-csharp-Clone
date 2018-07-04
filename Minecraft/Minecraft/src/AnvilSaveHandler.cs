using System.Collections.Generic;
using System.IO;

namespace net.minecraft.src
{
    using IOPath = System.IO.Path;

	public class AnvilSaveHandler : SaveHandler
	{
		public AnvilSaveHandler(string par1File, string par2Str, bool par3)
            : base(par1File, par2Str, par3)
		{
		}

		/// <summary>
		/// Returns the chunk loader with the provided world provider
		/// </summary>
		public override IChunkLoader GetChunkLoader(WorldProvider par1WorldProvider)
		{
			string directory = GetSaveDirectory();

			if (par1WorldProvider is WorldProviderHell)
			{
                string directory1 = IOPath.Combine(directory, "DIM-1");
				Directory.CreateDirectory(directory1);
				return new AnvilChunkLoader(directory1);
			}

			if (par1WorldProvider is WorldProviderEnd)
			{
                string directory2 = IOPath.Combine(directory, "DIM1");
                Directory.CreateDirectory(directory2);
				return new AnvilChunkLoader(directory2);
			}
			else
			{
                return new AnvilChunkLoader(directory);
			}
		}

		/// <summary>
		/// saves level.dat and backs up the existing one to level.dat_old
		/// </summary>
		public override void SaveWorldInfoAndPlayer(WorldInfo par1WorldInfo, List<EntityPlayer> par2List)
		{
			par1WorldInfo.SetSaveVersion(19133);
			base.SaveWorldInfoAndPlayer(par1WorldInfo, par2List);
		}
	}
}