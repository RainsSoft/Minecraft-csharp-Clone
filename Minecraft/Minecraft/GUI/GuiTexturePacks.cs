using System;
using System.Text;

namespace net.minecraft.src
{
	public class GuiTexturePacks : GuiScreen
	{
		protected GuiScreen GuiScreen;
		private int RefreshTimer;

		/// <summary>
		/// the absolute location of this texture pack </summary>
		private string FileLocation;

		/// <summary>
		/// the GuiTexturePackSlot that Contains all the texture packs and their descriptions
		/// </summary>
		private GuiTexturePackSlot GuiTexturePackSlot;

		public GuiTexturePacks(GuiScreen par1GuiScreen)
		{
			RefreshTimer = -1;
			FileLocation = "";
			GuiScreen = par1GuiScreen;
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			StringTranslate stringtranslate = StringTranslate.GetInstance();
			ControlList.Add(new GuiSmallButton(5, Width / 2 - 154, Height - 48, stringtranslate.TranslateKey("texturePack.openFolder")));
			ControlList.Add(new GuiSmallButton(6, Width / 2 + 4, Height - 48, stringtranslate.TranslateKey("gui.done")));
			Mc.TexturePackList.UpdateAvaliableTexturePacks();
			FileLocation = (System.IO.Path.Combine(Minecraft.GetMinecraftDir(), "Texturepacks"));
			GuiTexturePackSlot = new GuiTexturePackSlot(this);
			GuiTexturePackSlot.RegisterScrollButtons(ControlList, 7, 8);
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton par1GuiButton)
		{
			if (!par1GuiButton.Enabled)
			{
				return;
			}

			if (par1GuiButton.Id == 5)
			{/*
				bool flag = false;

				try
				{
					Type class1 = Type.GetType("java.awt.Desktop");
					object obj = class1.GetMethod("getDesktop", new Type[0]).Invoke(null, new object[0]);
					class1.GetMethod("browse", new Type[] { typeof(URI) }).Invoke(obj, new object[] { (new File(Minecraft.GetMinecraftDir(), "texturepacks")).toURI() });
				}
				catch (Exception throwable)
				{
					Console.WriteLine(throwable.ToString());
					Console.Write(throwable.StackTrace);
					flag = true;
				}

				if (flag)
				{
					Console.WriteLine("Opening via Sys class!");
					Sys.openURL((new StringBuilder()).Append("file://").Append(FileLocation).ToString());
				}*/
			}
			else if (par1GuiButton.Id == 6)
			{
				//Mc.RenderEngineOld.RefreshTextures();
				Mc.DisplayGuiScreen(GuiScreen);
			}
			else
			{
				GuiTexturePackSlot.ActionPerformed(par1GuiButton);
			}
		}

		/// <summary>
		/// Called when the mouse is clicked.
		/// </summary>
		protected override void MouseClicked(int par1, int par2, int par3)
		{
			base.MouseClicked(par1, par2, par3);
		}

		/// <summary>
		/// Called when the mouse is moved or a mouse button is released.  Signature: (mouseX, mouseY, which) which==-1 is
		/// mouseMove, which==0 or which==1 is mouseUp
		/// </summary>
		protected override void MouseMovedOrUp(int par1, int par2, int par3)
		{
			base.MouseMovedOrUp(par1, par2, par3);
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			GuiTexturePackSlot.DrawScreen(par1, par2, par3);

			if (RefreshTimer <= 0)
			{
				Mc.TexturePackList.UpdateAvaliableTexturePacks();
				RefreshTimer += 20;
			}

			StringTranslate stringtranslate = StringTranslate.GetInstance();
			DrawCenteredString(FontRenderer, stringtranslate.TranslateKey("texturePack.title"), Width / 2, 16, 0xffffff);
			DrawCenteredString(FontRenderer, stringtranslate.TranslateKey("texturePack.folderInfo"), Width / 2 - 77, Height - 26, 0x808080);
			base.DrawScreen(par1, par2, par3);
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			base.UpdateScreen();
			RefreshTimer--;
		}

        public static Minecraft GetMinecraft(GuiTexturePacks gui)
        {
            return gui.Mc;
        }
	}
}