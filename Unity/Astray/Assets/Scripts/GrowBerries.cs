using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Starts the growth of berries once enabled.
public class GrowBerries : MonoBehaviour
{
    private BerryTimer _berryTimer_script;

    private void OnEnable()
    {
        _berryTimer_script = GetComponentInParent<BerryTimer>();
        _berryTimer_script.GrowBerries();
    }
}
