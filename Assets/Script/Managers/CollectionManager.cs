using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CollectionManager : Manager
{
    public override void Initialize()
    {
        base.Initialize();

        IsInitialize = true;
    }

    //Maybe move this region to another manager
    #region Collection 

    public bool HasInCollection(CollectionType collection, int flag)
    {
        return (m_data.Collections[(int)collection] & (1 << flag)) > 0;
    }

    public bool AddToCollection(CollectionType collection, int flag)
    {
        if (HasInCollection(collection, flag)) return false;
        m_data.Collections[(int)collection] |= (1 << flag);
        Events.Collection.FireNewInCollection(collection, m_data.Collections[(int)collection]);
        return true;
    }

    #endregion
}

