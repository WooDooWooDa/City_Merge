using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Events
{
    public static class Screen
    {
        public static Action<ScreenType> OnScreenOpened;
        public static Action<ScreenType> OnScreenClosed;

        public static void FireScreenOpened(ScreenType type) { OnScreenOpened?.Invoke(type); }
        public static void FireScreenClosed(ScreenType type) { OnScreenClosed?.Invoke(type); }
    }
    public static class Loading
    {
        public static Action OnFinishLoading;
        public static Action<Manager> OnStartLoadingManager;
        public static Action OnManagerFinishLoading;
        public static Action OnLoadingPageClose;

        public static void FireFinishLoading() { OnFinishLoading?.Invoke();  }
        public static void FireStartLoadingManager(Manager m) { OnStartLoadingManager?.Invoke(m); }
        public static void FireManagerFinishLoading() { OnManagerFinishLoading?.Invoke(); }
        public static void FireLoadingPageClose() { OnLoadingPageClose?.Invoke(); }
    }

    public static class Profile
    {
        public static Action<int> OnLevelUp;
        public static Action OnMoneyChanged;

        public static void FireLevelUp(int newLevel) { OnLevelUp?.Invoke(newLevel); }
        public static void FireMoneyChanged() { OnMoneyChanged?.Invoke(); }
    }

    public static class Building
    {
        public static Action<BuildingKey> OnMerge;
        public static Action<BuildingKey> OnBought;
        public static void FireMerge(BuildingKey mergeKey) { OnMerge?.Invoke(mergeKey); }
        public static void FireBought(BuildingKey boughtKey) { OnBought?.Invoke(boughtKey); }
    }

    public static class Upgrade
    {
        public static Action<UpgradeType> OnUpgradeBought;
        public static Action<UpgradeType, int> OnUpgradeLeveledUp;

        public static void FireUpgradeBought(UpgradeType type) { OnUpgradeBought?.Invoke(type); }
        public static void FireUpgradeLeveledUp(UpgradeType type, int newLevel) { OnUpgradeLeveledUp?.Invoke(type, newLevel); }
    }

    public static class Collection
    {
        public static Action<CollectionType, int> OnNewInCollection;

        public static void FireNewInCollection(CollectionType collection, int value) { OnNewInCollection?.Invoke(collection, value); }
    }
    //TODO Events to add
    //island level up

}
