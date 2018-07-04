namespace net.minecraft.src
{
	public class GuiSmallButton : GuiButton
	{
        private readonly Options Options;

		public GuiSmallButton(int par1, int par2, int par3, string par4Str)
            : this(par1, par2, par3, null, par4Str)
		{
		}

		public GuiSmallButton(int par1, int par2, int par3, int par4, int par5, string par6Str)
            : base(par1, par2, par3, par4, par5, par6Str)
		{
            Options = null;
		}

        public GuiSmallButton(int par1, int par2, int par3, Options par4Options, string par5Str)
            : base(par1, par2, par3, 150, 20, par5Str)
		{
            Options = par4Options;
		}

        public virtual Options ReturnOptions()
		{
            return Options;
		}
	}
}