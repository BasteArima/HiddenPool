using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonoSystem : MonoBehaviour
{
    protected AppData data;

    public virtual void Init(AppData data)
    {
        this.data = data;
    }
}

