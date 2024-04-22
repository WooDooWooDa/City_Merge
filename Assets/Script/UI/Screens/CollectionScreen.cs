using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CollectionScreen : UIScreenBase
{
    [Header("Specific")]
    [SerializeField] private CollectionItem m_collectionItem;
    [SerializeField] private Button m_leftArrow;
    [SerializeField] private Button m_rightArrow;

    private CollectionManager m_collectionManager;
    private int m_currentPosition = 0;

    private BuildingInformations[] m_buildingInformations;

    protected override void Awake()
    {
        base.Awake();
    }

    public override UIScreenBase Open(OpenInfo openInfo)
    {
        base.Open(openInfo);

        m_buildingInformations = Main.Instance.GlobalConfig.BuildingsConfig.BuildingInformations;

        m_leftArrow.onClick.AddListener(() => {
            --m_currentPosition;
            if (m_currentPosition < 0) m_currentPosition = 0;
            SetData();
        });
        m_rightArrow.onClick.AddListener(() => {
            ++m_currentPosition;
            if (m_currentPosition > m_buildingInformations.Length - 1) m_currentPosition = m_buildingInformations.Length - 1;
            SetData();
        });

        m_collectionManager = Main.Instance.GetManager<CollectionManager>();
        SetData();

        return this;
    }

    private void SetData()
    {
        m_leftArrow.gameObject.SetActive(m_currentPosition != 0);
        m_rightArrow.gameObject.SetActive(m_currentPosition != m_buildingInformations.Length - 1);

        BuildingInformations info = m_buildingInformations[m_currentPosition];
        var data = new CollectionData { Title = info.Name };
        data.Lock = !m_collectionManager.HasInCollection(CollectionType.Building, (int)info.Key.Level);
        m_collectionItem.SetData(data);
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
