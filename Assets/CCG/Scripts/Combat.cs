using UnityEngine;
using Mirror;

public class Combat : NetworkBehaviour
{
    [Header("Entity")]
    public Entity entity;
    
    [Command(ignoreAuthority = true)]
    public void CmdChangeMana(int amount)
    {
        // Increase mana by amount. If 3, increase by 3. If -3, reduce by 3.
        if (entity is Player) entity.GetComponent<Player>().mana += amount;
    }

    [Command(ignoreAuthority = true)]
    public void CmdChangeStrength(int amount)
    {
        // Increase dame by amount. If 3, increase by 3. If -3, reduce by 3.
        entity.strength += amount;
    }

    [Command(ignoreAuthority = true)]
    public void CmdChangeHealth(int amount)
    {
        // Increase health by amount. If 3, increase by 3. If -3, reduce by 3.
        entity.health += amount;
        if (entity.health > 30) entity.health = 30;
        // if (entity.IsDead() && entity is not Player)
        // {
        //     //Destroy(entity.gameObject);
        //     entity.transform.SetParent(GameManager.graveyard.transform);
        //     entity.transform.position = new Vector3(entity.transform.position.x + 4000, entity.transform.position.y,
        //         entity.transform.position.z);
        // }
    }

    [Command(ignoreAuthority = true)]
    public void CmdIncreaseWaitTurn()
    {
        entity.waitTurn++;
    }
}
