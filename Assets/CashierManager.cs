using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierManager : Singleton<CashierManager>
{
    public Cashier Cashier;
    public Transform customerExitPosition;
}
