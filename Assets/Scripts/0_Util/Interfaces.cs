using System.Collections;

public interface IAttackable
{
    bool Attack(IDamagable target, float power, int numberOfAttacks);
}
public interface IDamagable
{

    void ApplyDamage(float dmg);
}

public interface IState
{
    public void Enter();
    public void Exit();

    public bool CanTransitState(IState prevState);
}