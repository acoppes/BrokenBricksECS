using ECS;
using UnityEngine;

public class PositionComponent : IComponent
{
	public Vector2 position;
}

public class MovementComponent : IComponent
{
	public Vector2 velocity;
}