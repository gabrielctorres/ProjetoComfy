using UnityEngine;
using TMPro; // Não esqueça do namespace do TextMeshPro!

public class DayUI : MonoBehaviour
{
    [Header("Componentes de UI")]
    public GameObject container;
    public TextMeshProUGUI dayText;   // Arraste o texto do Dia aqui
    public TextMeshProUGUI pointsText;// Arraste o texto dos Pontos aqui

    [Header("Dados do Jogo")]
    public GameData gameData;

    private void Start()
    {
        if (container != null) container.SetActive(false);
    }

    private void OnEnable()
    {
        DayManager.OnDayEnded += EnableUI;
    }

    private void OnDisable()
    {
        DayManager.OnDayEnded -= EnableUI;
    }

    public void EnableUI()
    {
        if (gameData == null)
        {
            return;
        }
        dayText.text = $"Day {gameData.currenteDay - 1} is over!";
        pointsText.text = $"Points: {gameData.point}";

        container.SetActive(true);
    }
}