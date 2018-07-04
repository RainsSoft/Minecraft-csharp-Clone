namespace net.minecraft.src
{    
	class AnvilSaveConverterFileFilter
	{
		readonly AnvilSaveConverter Parent;

        public static string SearchString = "*.mcr";

		AnvilSaveConverterFileFilter(AnvilSaveConverter par1AnvilSaveConverter)
		{
			Parent = par1AnvilSaveConverter;
		}

		public virtual bool Accept(string par1File, string par2Str)
		{
			return par2Str.EndsWith(".mcr");
		}
	}
}