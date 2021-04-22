using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Button startButton;
    private float speed = 0;
    public float maxSpeed = 10;
    public float increment = 1;

    public float fadeoutSpeed = 0.5f;
    public GameObject spaceship;
    public GameObject blackoutSquare;

    private Vector3 position;
    void Start()
    {
        position = spaceship.transform.position;
        startButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        StartCoroutine(MoveSpaceship());
        StartCoroutine(FadeToBlack());
    }

    private IEnumerator MoveSpaceship()
    {
        while (spaceship.GetComponent<Renderer>().isVisible)
        {
            if (speed > -maxSpeed)
            {
                speed = speed + increment * Time.deltaTime;
            }
            position.y = spaceship.transform.position.y + speed * Time.deltaTime;
            spaceship.transform.position = position;
            yield return null;
        }
    }

    private IEnumerator FadeToBlack()
    {
        Color objectColor = blackoutSquare.GetComponent<Image>().color;
        float fadeAmount;
        while (blackoutSquare.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = objectColor.a + (fadeoutSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackoutSquare.GetComponent<Image>().color = objectColor;
            yield return null;
        }
        SceneManager.LoadScene(1);
    }
}