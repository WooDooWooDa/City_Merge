using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ScreenManager : Manager
{
    public override bool Load(ref SaveData data)
    {
        return true;
    }

    public override bool Save(ref SaveData data)
    {
        return true;
    }
}
