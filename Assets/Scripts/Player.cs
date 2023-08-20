using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] int Health;
    [SerializeField] int Exp;
    [SerializeField] TextMeshProUGUI HealthText;
    [SerializeField] TextMeshProUGUI ExpText;

    private void Awake()
    {
        Instance = this;
    }

    public void IncreaseHealth(int value)
    {
        Health += value;
        HealthText.text = $"HP: {Health}";
    }

    public void IncreaseExp(int value)
    {
        Exp += value;
        ExpText.text = $"EXP: {Exp}";
    }
}
