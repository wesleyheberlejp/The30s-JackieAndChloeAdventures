using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaArmaFlecha : BalaArma
{
    private void Start()
    {
        base.rb = this.rb;

        IniciaTiro(transform);
    }
}
