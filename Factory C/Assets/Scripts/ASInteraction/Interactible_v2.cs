using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactible_v2
{

    public abstract object Interact(KeyCode key, object data);

    public virtual void BehaveOnTargeted(object targetedData) { }

    public virtual void BehaveOnUnTargeted(object targetedData) { }


}
