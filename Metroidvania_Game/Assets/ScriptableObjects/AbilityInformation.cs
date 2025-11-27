using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemInformation", menuName = "Scriptable Objects/ItemInformation")]
public class AbilityInformation : ScriptableObject
{
    public string abilityName;
    public Image abilityImage;
}
