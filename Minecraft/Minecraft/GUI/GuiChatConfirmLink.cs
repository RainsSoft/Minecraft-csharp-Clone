namespace net.minecraft.src
{
	class GuiChatConfirmLink : GuiConfirmOpenLink
	{
		readonly ChatClickData Field_50056_a;
		readonly GuiChat Field_50055_b;

		public GuiChatConfirmLink(GuiChat par1GuiChat, GuiScreen par2GuiScreen, string par3Str, int par4, ChatClickData par5ChatClickData) : base(par2GuiScreen, par3Str, par4)
		{
			Field_50055_b = par1GuiChat;
			Field_50056_a = par5ChatClickData;
		}

		public override void Func_50052_d()
		{
			WriteToClipboard(Field_50056_a.Func_50088_a());
		}
	}
}