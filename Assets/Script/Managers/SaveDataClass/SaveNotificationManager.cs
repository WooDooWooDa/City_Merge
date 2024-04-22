using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class NotificationManager : Manager
{
    private NotificationSaveData m_data = new NotificationSaveData();
    public NotificationSaveData Data => m_data;
    public override bool Load(ref SaveData data)
    {
        m_data = data.NotificationSaveData;

        return true;
    }

    public override bool Save(ref SaveData data)
    {
        data.NotificationSaveData = m_data;
        return true;
    }
}

[Serializable]
public class NotificationSaveData
{

}