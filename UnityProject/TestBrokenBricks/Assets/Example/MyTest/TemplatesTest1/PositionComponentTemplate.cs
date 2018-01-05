using MyTest.Components;

public class PositionComponentTemplate : GenericEntityTemplate<PositionComponent> {

	public override void Apply (ECS.Entity e)
	{
		base.Apply (e);

		var positionComponent = _entityManager.GetComponent<PositionComponent>(e);

		var p = positionComponent.position;
		p.x = transform.position.x;
		p.y = transform.position.z;
		positionComponent.position = p;
	}

}
