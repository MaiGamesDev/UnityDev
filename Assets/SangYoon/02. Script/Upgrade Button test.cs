using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtontest : MonoBehaviour
{
    public Button button;

    UpgradeStats upgradeStats;

    private void Update()
    {
        button.onClick.AddListener(OnClick);

    }
    public void OnClick()
    {
        upgradeStats.isAttackSpeedUpgrade = true;
        upgradeStats.Upgrade();
        upgradeStats.isAttackSpeedUpgrade = false;
    }
}
