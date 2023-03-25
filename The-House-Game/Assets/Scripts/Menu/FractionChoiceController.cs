using UnityEngine;
using UnityEngine.SceneManagement;
using Units.Settings;

public class FractionChoiceController : MonoBehaviour
{
    public void ChooseRats()
    {
        LoadScene(Fraction.Name.RATS);
    }

    public void ChooseFourth()
    {
        LoadScene(Fraction.Name.FOURTH);
    }

    private void LoadScene(Fraction.Name fraction)
    {
        GameManager.gamerFractionName = fraction;
        SceneManager.LoadScene("GameMap");
        Time.timeScale = 1f;
    }
}
