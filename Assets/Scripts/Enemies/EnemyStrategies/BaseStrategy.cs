using Unity.Netcode;

public abstract class BaseStrategy : NetworkBehaviour
{
    public abstract void UpdateStrategy();
}