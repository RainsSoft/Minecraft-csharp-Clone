namespace net.minecraft.src
{

	public class MovementInput
	{
		/// <summary>
		/// The speed at which the player is strafing. Postive numbers to the left and negative to the right.
		/// </summary>
		public float MoveStrafe;

		/// <summary>
		/// The speed at which the player is moving forward. Negative numbers will move backwards.
		/// </summary>
		public float MoveForward;
		public bool Jump;
		public bool Sneak;

		public MovementInput()
		{
			MoveStrafe = 0.0F;
			MoveForward = 0.0F;
			Jump = false;
			Sneak = false;
		}

		public virtual void Func_52013_a()
		{
		}
	}

}