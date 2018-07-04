using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class GuiInventory : GuiContainer
	{
		/// <summary>
		/// x size of the inventory window in pixels. Defined as float, passed as int
		/// </summary>
		private float XSize_lo;

		/// <summary>
		/// y size of the inventory window in pixels. Defined as float, passed as int.
		/// </summary>
		private float YSize_lo;

		public GuiInventory(EntityPlayer par1EntityPlayer) : base(par1EntityPlayer.InventorySlots)
		{
			AllowUserInput = true;
			par1EntityPlayer.AddStat(AchievementList.OpenInventory, 1);
		}

		/// <summary>
		/// Called from the main game loop to update the screen.
		/// </summary>
		public override void UpdateScreen()
		{
			if (Mc.PlayerController.IsInCreativeMode())
			{
				Mc.DisplayGuiScreen(new GuiContainerCreative(Mc.ThePlayer));
			}
		}

		/// <summary>
		/// Adds the buttons (and other controls) to the screen in question.
		/// </summary>
		public override void InitGui()
		{
			ControlList.Clear();

			if (Mc.PlayerController.IsInCreativeMode())
			{
				Mc.DisplayGuiScreen(new GuiContainerCreative(Mc.ThePlayer));
			}
			else
			{
				base.InitGui();

				if (Mc.ThePlayer.GetActivePotionEffects().Count != 0)
				{
					GuiLeft = 160 + (Width - XSize - 200) / 2;
				}
			}
		}

		/// <summary>
		/// Draw the foreground layer for the GuiContainer (everythin in front of the items)
		/// </summary>
		protected override void DrawGuiContainerForegroundLayer()
		{
			FontRenderer.DrawString(StatCollector.TranslateToLocal("container.crafting"), 86, 16, 0x404040);
		}

		/// <summary>
		/// Draws the screen and all the components in it.
		/// </summary>
		public override void DrawScreen(int par1, int par2, float par3)
		{
			base.DrawScreen(par1, par2, par3);
			XSize_lo = par1;
			YSize_lo = par2;
		}

		/// <summary>
		/// Draw the background layer for the GuiContainer (everything behind the items)
		/// </summary>
		protected override void DrawGuiContainerBackgroundLayer(float par1, int par2, int par3)
		{
			int i = Mc.RenderEngineOld.GetTexture("/gui/inventory.png");
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			Mc.RenderEngineOld.BindTexture(i);
			int j = GuiLeft;
			int k = GuiTop;
			DrawTexturedModalRect(j, k, 0, 0, XSize, YSize);
			DisplayDebuffEffects();
			//GL.Enable(EnableCap.RescaleNormal);
			//GL.Enable(EnableCap.ColorMaterial);
			//GL.PushMatrix();
			//GL.Translate(j + 51, k + 75, 50F);
			float f = 30F;
			//GL.Scale(-f, f, f);
			//GL.Rotate(180F, 0.0F, 0.0F, 1.0F);
			float f1 = Mc.ThePlayer.RenderYawOffset;
			float f2 = Mc.ThePlayer.RotationYaw;
			float f3 = Mc.ThePlayer.RotationPitch;
			float f4 = (float)(j + 51) - XSize_lo;
			float f5 = (float)((k + 75) - 50) - YSize_lo;
			//GL.Rotate(135F, 0.0F, 1.0F, 0.0F);
			RenderHelper.EnableStandardItemLighting();
			//GL.Rotate(-135F, 0.0F, 1.0F, 0.0F);
			//GL.Rotate(-(float)Math.Atan(f5 / 40F) * 20F, 1.0F, 0.0F, 0.0F);
			Mc.ThePlayer.RenderYawOffset = (float)Math.Atan(f4 / 40F) * 20F;
			Mc.ThePlayer.RotationYaw = (float)Math.Atan(f4 / 40F) * 40F;
			Mc.ThePlayer.RotationPitch = -(float)Math.Atan(f5 / 40F) * 20F;
			Mc.ThePlayer.RotationYawHead = Mc.ThePlayer.RotationYaw;
			//GL.Translate(0.0F, Mc.ThePlayer.YOffset, 0.0F);
			RenderManager.Instance.PlayerViewY = 180F;
			RenderManager.Instance.RenderEntityWithPosYaw(Mc.ThePlayer, 0.0F, 0.0F, 0.0F, 0.0F, 1.0F);
			Mc.ThePlayer.RenderYawOffset = f1;
			Mc.ThePlayer.RotationYaw = f2;
			Mc.ThePlayer.RotationPitch = f3;
			//GL.PopMatrix();
			RenderHelper.DisableStandardItemLighting();
			//GL.Disable(EnableCap.RescaleNormal);
		}

		/// <summary>
		/// Fired when a control is clicked. This is the equivalent of ActionListener.actionPerformed(ActionEvent e).
		/// </summary>
		protected override void ActionPerformed(GuiButton par1GuiButton)
		{
			if (par1GuiButton.Id == 0)
			{
				Mc.DisplayGuiScreen(new GuiAchievements(Mc.StatFileWriter));
			}

			if (par1GuiButton.Id == 1)
			{
				Mc.DisplayGuiScreen(new GuiStats(this, Mc.StatFileWriter));
			}
		}

		/// <summary>
		/// Displays debuff/potion effects that are currently being applied to the player
		/// </summary>
		private void DisplayDebuffEffects()
		{
			int i = GuiLeft - 124;
			int j = GuiTop;
			int k = Mc.RenderEngineOld.GetTexture("/gui/inventory.png");
			ICollection<PotionEffect> collection = Mc.ThePlayer.GetActivePotionEffects();

			if (collection.Count == 0)
			{
				return;
			}

			int l = 33;

			if (collection.Count > 5)
			{
				l = 132 / (collection.Count - 1);
			}

			for (IEnumerator<PotionEffect> iterator = Mc.ThePlayer.GetActivePotionEffects().GetEnumerator(); iterator.MoveNext();)
			{
				PotionEffect potioneffect = iterator.Current;
				Potion potion = Potion.PotionTypes[potioneffect.GetPotionID()];
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
				Mc.RenderEngineOld.BindTexture(k);
				DrawTexturedModalRect(i, j, 0, YSize, 140, 32);

				if (potion.HasStatusIcon())
				{
					int i1 = potion.GetStatusIconIndex();
					DrawTexturedModalRect(i + 6, j + 7, 0 + (i1 % 8) * 18, YSize + 32 + (i1 / 8) * 18, 18, 18);
				}

				string s = StatCollector.TranslateToLocal(potion.GetName());

				if (potioneffect.GetAmplifier() == 1)
				{
					s = (new StringBuilder()).Append(s).Append(" II").ToString();
				}
				else if (potioneffect.GetAmplifier() == 2)
				{
					s = (new StringBuilder()).Append(s).Append(" III").ToString();
				}
				else if (potioneffect.GetAmplifier() == 3)
				{
					s = (new StringBuilder()).Append(s).Append(" IV").ToString();
				}

				FontRenderer.DrawStringWithShadow(s, i + 10 + 18, j + 6, 0xffffff);
				string s1 = Potion.GetDurationString(potioneffect);
				FontRenderer.DrawStringWithShadow(s1, i + 10 + 18, j + 6 + 10, 0x7f7f7f);
				j += l;
			}
		}
	}
}