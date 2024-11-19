using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactible_v2
{

    public abstract bool Interact(KeyCode key);

    public virtual void BehaveOnTargeted() { }

    public virtual void BehaveOnUnTargeted() { }


}
