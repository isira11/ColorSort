using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move 
{
    public Tube from;
    public Tube to;

    public Move(Tube from_tube, Tube to_tube)
    {
        from = from_tube;
        to = to_tube;
    }
}
