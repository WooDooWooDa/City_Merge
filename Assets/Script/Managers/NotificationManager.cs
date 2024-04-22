using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum NotificationType
{
    New,
    Number,
    Info
}

public partial class NotificationManager : Manager
{
    public override string LoadingInfo => "";

    
}
