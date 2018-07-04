using System;

namespace net.minecraft.src
{
	public class StructureMineshaftStart : StructureStart
	{
		public StructureMineshaftStart(World par1World, Random par2Random, int par3, int par4)
		{
			ComponentMineshaftRoom componentmineshaftroom = new ComponentMineshaftRoom(0, par2Random, (par3 << 4) + 2, (par4 << 4) + 2);
			Components.Add(componentmineshaftroom);
			componentmineshaftroom.BuildComponent(componentmineshaftroom, Components, par2Random);
			UpdateBoundingBox();
			MarkAvailableHeight(par1World, par2Random, 10);
		}
	}
}