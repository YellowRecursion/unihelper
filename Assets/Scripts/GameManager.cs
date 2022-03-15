using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameData gameData;

    [Header("Theme setuping")]
    public Camera cam;
    public TextMeshProUGUI[] texts;
    public Image[] images;

    [Header("Info")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI timerText;
    public Image timerBar;

    [Header("Game")]
    public QuestionBox question;
    public AnswerButton[] answers;
    public Color green = Color.green;
    public Color red = Color.red;
    public float switchSpeed = 5f;
    public RectTransform qaBox;
    public Vector2 center = Vector2.zero, left = Vector2.left * -900f, right = Vector2.right * 900f;

    [Header("Level complete")]
    public GameObject levelCompleteMenu;
    public GameObject levelCompleteMenuAnimation;
    public GameObject[] stars;
    public TextMeshProUGUI correctAnswersText;
    public GameObject nextLevelButton;

    [Header("Sound FX")]
    public SoundManager correctAnswerSound;
    public SoundManager incorrectAnswerSound;
    public SoundManager timerSound;

    // private vars
    private int currentQuestion;
    private float timer;
    private Vector2 targetPosition;
    private bool timerStopped;
    private int correctAnswer;
    private int givedAnswer;
    private int correctAnswersCount;
    private bool paused;
    private List<int> mixedAnswers = new List<int>();
    private List<int> mistakes = new List<int>();
    private float levelPlayTime;

    private void Start()
    {
        if (gameData.SelectedLevel == null)
        {
            Debug.LogError("Level not selected. Please open level by main menu");
            return;
        }

        SetTheme(gameData.difficultyColors[(int)gameData.SelectedLevel.difficulty]);
        SetLevelInfo();

        StartCoroutine( StartQuestion(0));
    }

    // Установка цвета интерфейса
    private void SetTheme(Color color)
    {
        cam.backgroundColor = color;
        foreach (var item in texts)
        {
            item.color = color;
        }
        foreach (var item in images)
        {
            item.color = color;
        }
    }

    // Установка информации об уровни 
    private void SetLevelInfo()
    {
        levelText.text = "Уровень " + (gameData.SelectedLevel.levelIndex + 1);
    }

    // Начать новый вопрос
    private IEnumerator StartQuestion(int q)
    {
        if (q < 0) q = 0;

        timerStopped = true;
        if (q > 0) // переключение с предыдущего вопроса к следующему
        {
            var givedAnswer1 = GetIndexFromMixedAnswers(givedAnswer);
            var correctAnswer1 = GetIndexFromMixedAnswers(correctAnswer);

            if (givedAnswer1 != correctAnswer1) answers[givedAnswer1].backgroundAnimator.Begin(red);
            answers[correctAnswer1].backgroundAnimator.Begin(green);

            yield return new WaitForSeconds(2f);

            while (Vector2.Distance(qaBox.anchoredPosition, left) > 1f)
            {
                qaBox.anchoredPosition = Vector2.Lerp(qaBox.anchoredPosition, left, Time.deltaTime * switchSpeed);
                yield return new WaitForEndOfFrame();
            }

            answers[correctAnswer1].backgroundAnimator.End();
            if (givedAnswer1 != correctAnswer1) answers[givedAnswer1].backgroundAnimator.End();
        }

        if (q >= gameData.SelectedLevel.questions.Count)
        {
            Debug.Log("Level completed");
            levelCompleteMenuAnimation.SetActive(true);
            Invoke("EnableLevelComplete", 0.5f);
            yield break;
        }

        qaBox.anchoredPosition = right;

        currentQuestion = q;
        timer = gameData.SelectedLevel.questions[q].time;
        questionText.text = (q+1) + "/" + gameData.SelectedLevel.questions.Count;

        // Setup the question:
        question.Setup(gameData.SelectedLevel.questions[q]);

        // Setup answers:
        int answersCount = gameData.SelectedLevel.questions[q].answers.Count;
        for (int i = 0; i < answers.Length; i++) answers[i].gameObject.SetActive(false);
        mixedAnswers.Clear();
        for (int i = 0; i < answersCount; i++) mixedAnswers.Add(i); 
        Shuffle(ref mixedAnswers); // Mix answers

        // Setup answers UI
        for (int i = 0; i < answersCount; i++)
        {
            answers[i].gameObject.SetActive(true);
            answers[i].Setup(gameData.SelectedLevel.questions[q], mixedAnswers[i]);
        }

        while (Vector2.Distance(qaBox.anchoredPosition, center) > 1f)
        {
            qaBox.anchoredPosition = Vector2.Lerp(qaBox.anchoredPosition, center, Time.deltaTime * switchSpeed);
            yield return new WaitForEndOfFrame();
        }
        timerStopped = false;
    }

    // Перемешать список
    public void Shuffle<T>(ref List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // Функция необходимо чтобы взять реальный индекс перемешанного списка ответов
    private int GetIndexFromMixedAnswers(int mixedIndex)
    {
        for (int i = 0; i < mixedAnswers.Count; i++)
        {
            if (mixedAnswers[i] == mixedIndex) return i;
        }
        return 0;
    }

    // Каждый кадр выполняем
    private void Update()
    {
        if (!timerStopped)
        {
            // управляем таймеров и запускаем следующий вопрос

            if (!paused && timer > 0)
            {
                timer -= Time.deltaTime;
                string oldText = timerText.text;
                timerText.text = Mathf.Floor(timer).ToString();
                if (oldText != timerText.text) timerSound.Play();
                timerBar.fillAmount = timer / gameData.SelectedLevel.questions[currentQuestion].time;
                if (timer <= 0)
                {
                    givedAnswer = correctAnswer;
                    StartCoroutine(StartQuestion(currentQuestion + 1));
                }
            }
        }
        else
        {
            timerText.text = "0";
            timerBar.fillAmount = 0f;
        }

        if (!paused)
        {
            levelPlayTime += Time.deltaTime;
        }
    }

    // Срабатывает при нажатии на кнопку с ответом
    public void GiveAnswer(int index)
    {
        if (timerStopped) return;
        givedAnswer = index;
        if (GetIndexFromMixedAnswers(givedAnswer) == GetIndexFromMixedAnswers(correctAnswer))
        {
            correctAnswersCount++;
            correctAnswerSound.Play();
        }
        else
        {
            mistakes.Add(currentQuestion + 1);
            //incorrectAnswerSound.Play();
            Handheld.Vibrate();
        }
        Debug.Log("correctAnswersCount = " + correctAnswersCount);
        StartCoroutine(StartQuestion(currentQuestion + 1));
    }

    // При завершении уровня вызывается этот метод
    public void EnableLevelComplete()
    {
        levelCompleteMenu.SetActive(true);

        float questionsCount = gameData.SelectedLevel.questions.Count;
        correctAnswersText.text = "Верные ответы: " + correctAnswersCount + "/" + questionsCount;

        int countOfStars = 0;
        if (IsCorrectAnswersSatisfyTreshold(1f)) countOfStars = 3;
        else if (IsCorrectAnswersSatisfyTreshold(0.9f)) countOfStars = 2;
        else if (IsCorrectAnswersSatisfyTreshold(0.8f)) countOfStars = 1;

        for (int i = 0; i < stars.Length; i++)
        {
            if (i < countOfStars)
            {
                stars[i].SetActive(true);
            }
            else
            {
                stars[i].SetActive(false);
                stars[i].transform.parent.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.3f);
            }
        }

        if (correctAnswersCount > gameData.SelectedLevel.CorrectAnswersCount)
        {
            gameData.SelectedLevel.Stars = countOfStars;
            gameData.SelectedLevel.CorrectAnswersCount = correctAnswersCount;
            gameData.SelectedLevel.MistakeIndexes = mistakes.ToArray();
        }

        if (countOfStars == 0) nextLevelButton.SetActive(false);
        if (gameData.SelectedLevel.levelIndex >= gameData.SelectedLevel.parentBase.levels.Count - 1) nextLevelButton.SetActive(false);

        GameData.TotalGameplayTime += levelPlayTime;
    }

    private bool IsCorrectAnswersSatisfyTreshold(float treshold)
    {
        float questionsCount = gameData.SelectedLevel.questions.Count;
        return Mathf.RoundToInt(Mathf.Lerp(0f, questionsCount, treshold)) <= correctAnswersCount;
    }

    public void NextLevel()
    {
        gameData.SelectedLevel = gameData.SelectedLevel.parentBase.levels[gameData.SelectedLevel.levelIndex + 1];
    }

    public void SkipQuestion()
    {
        if (!timerStopped)
            timer = 0.1f;
    }

    public void PauseChange()
    {
        paused = !paused;
    }
}
