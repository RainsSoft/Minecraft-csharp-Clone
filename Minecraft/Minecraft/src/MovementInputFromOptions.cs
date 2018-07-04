namespace net.minecraft.src
{
	public class MovementInputFromOptions : MovementInput
	{
		private GameSettings GameSettings;

		public MovementInputFromOptions(GameSettings par1GameSettings)
		{
			GameSettings = par1GameSettings;
		}

		public override void Func_52013_a()
		{
			MoveStrafe = 0.0F;
			MoveForward = 0.0F;

			if (GameSettings.KeyBindForward.Pressed)
			{
				MoveForward++;
			}

            if (GameSettings.KeyBindBack.Pressed)
			{
				MoveForward--;
			}

            if (GameSettings.KeyBindLeft.Pressed)
			{
				MoveStrafe++;
			}

            if (GameSettings.KeyBindRight.Pressed)
			{
				MoveStrafe--;
			}

            Jump = GameSettings.KeyBindJump.Pressed;
            Sneak = GameSettings.KeyBindSneak.Pressed;

			if (Sneak)
			{
				MoveStrafe *= 0.29999999999999999F;
				MoveForward *= 0.29999999999999999F;
			}
		}
	}
}