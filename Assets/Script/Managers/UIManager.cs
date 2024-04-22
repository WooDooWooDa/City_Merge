using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class UIManager : Manager
{
    public override string LoadingInfo => "Preparing the UI";

    public override void Initialize()
    {
        base.Initialize();

        IsInitialize = true;
    }
}
