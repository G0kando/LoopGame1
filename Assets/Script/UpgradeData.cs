[System.Serializable]
public class UpgradeData
{
    public string upgradeName;
    public string description;

    public UpgradeData(string name, string desc)
    {
        upgradeName = name;
        description = desc;
    }
}