using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D
{
	private const float _moveSpeed = 150.0f;
	private const float _jumpVelocity = 369.0f;
	private readonly float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public Sprite2D IdleSprite { get; private set; }
	public AnimatedSprite2D WalkingSprite { get; private set; }
	public override void _Ready()
	{
		IdleSprite = GetNode<Sprite2D>("IdleSprite");
		WalkingSprite = GetNode<AnimatedSprite2D>("WalkingSprite");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// apply gravity if in air
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
		}

		// handle horizontal movement
		velocity.X = 0;

		if (Input.IsKeyPressed(Key.A))
			velocity.X = -_moveSpeed;
		else if (Input.IsKeyPressed(Key.D))
			velocity.X = _moveSpeed;

		UpdateSpriteRenderer(velocity.X);

		// handle jump
		if (Input.IsKeyPressed(Key.Space) && IsOnFloor())
			velocity.Y = -_jumpVelocity;

		Velocity = velocity;
		MoveAndSlide();
	}

	private void UpdateSpriteRenderer(float velocityX)
	{
		bool walking = velocityX != 0;
		IdleSprite.Visible = !walking;
		WalkingSprite.Visible = walking;

		if (walking)
		{
			WalkingSprite.Play();
			WalkingSprite.FlipH = velocityX < 0;
		}
	}
}
