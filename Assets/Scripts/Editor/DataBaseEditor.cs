using System.Collections;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Net;

[CustomEditor(typeof(DataBase))]
public class DataBaseEditor : Editor // жаль того, кто сюда залез...
{
    public GameData gameData;

    public static int j;
    public DataBase db;
    public GUIStyle bigFont;
    public Sprite timerSprite;

    private bool dirty;

    private GUIStyle header;
    private GUIStyle header2;
    private GUIStyle headerC;
    private GUIStyle title;
    private GUIStyle big;
    private GUIStyle red;

    private bool notDirty;
    private static bool MainMenu = false;

    float defaultTime = 10f;

    private void SetStyles()
    {
        bigFont = new GUIStyle
        {
            fontSize = 24,
            alignment = TextAnchor.MiddleCenter
        };

        header = new GUIStyle();
        header.fontStyle = FontStyle.Bold;
        header.normal.textColor = Color.white;
        header.alignment = TextAnchor.MiddleLeft;

        header2 = new GUIStyle();
        header2.fontStyle = FontStyle.Bold;
        header2.normal.textColor = Color.white;
        header2.alignment = TextAnchor.MiddleCenter;

        headerC = new GUIStyle();
        headerC.fontStyle = FontStyle.Bold;
        headerC.normal.textColor = Color.white;
        headerC.alignment = TextAnchor.MiddleLeft;

        title = new GUIStyle();
        title.normal.textColor = new Color(0.4f, 0.7f, 1f);

        big = new GUIStyle();
        big.fontStyle = FontStyle.Bold;
        big.alignment = TextAnchor.MiddleCenter;
        big.fontSize = 20;
        big.normal.textColor = new Color(0.4f, 0.7f, 1f);

        red = new GUIStyle();
        red.fontStyle = FontStyle.Bold;
        red.alignment = TextAnchor.MiddleCenter;
        red.fontSize = 28;
        red.normal.textColor = Color.red;
    }
   
    private void OnEnable()
    {
        db = (DataBase)target;
        SetStyles();
    }
    public override void OnInspectorGUI()
    {
        notDirty = false;

        EditorGUILayout.LabelField("Файл с уровнями", big);
       
        DrawSaveLine();
        //GUI.changed = false;
        if (MainMenu == false)
        {
            DrawLevelsList();
        }
        else
        {
            if (j >= db.levels.Count)
            {
                MainMenu = false;
            }
            else
            {
                DrawLevel();
            }
        }
        if (GUI.changed && !notDirty) dirty = true;
        DrawSaveLine();
        
        if (dirty)
        {
            if (GUILayout.Button("Сохранить данные", GUILayout.Height(30)))
            {
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                dirty = false;
            }
            GUILayout.Space(20);
        }
    }

    private void DrawLevelsList()
    {
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Настройки раздела:");
        db.sectionName = EditorGUILayout.TextField("Название раздела", db.sectionName);
        GUILayout.Space(20);

        if (db.levels.Count == 0)
        {
            EditorGUILayout.LabelField("Уровней нет");
        }
        else
        {
            for (int i = 0; i < db.levels.Count; i++)
            {

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                db.levels[i].levelIndex = i;
                db.levels[i].parentBase = db;
                EditorGUILayout.LabelField((db.levels[i].levelIndex + 1).ToString(), header, GUILayout.Width(25));
                db.levels[i].difficulty = (Level.Difficulty)EditorGUILayout.EnumPopup(db.levels[i].difficulty, GUILayout.Width(120), GUILayout.Height(10));
                //db.levels[i].stars = EditorGUILayout.IntField(db.levels[i].stars, GUILayout.Width(60));
                EditorGUILayout.LabelField(" ");
                if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    db.levels.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    dirty = true;
                    break;
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.LabelField("Количество вопросов: " + db.levels[i].questions.Count);

                if (db.levels[i].questions.Count > 0)
                {
                    float allTime = 0f;
                    for (int j = 0; j < db.levels[i].questions.Count; j++)
                    {
                        allTime += db.levels[i].questions[j].time;
                    }
                    EditorGUILayout.LabelField("Среднее время: " + Mathf.Round(allTime / db.levels[i].questions.Count) + " c.");
                    GUILayout.Space(10);

                    EditorGUILayout.LabelField("Содержание уровня:");
                    EditorGUILayout.BeginHorizontal();
                    for (int j = 0; j < db.levels[i].questions.Count; j++)
                    {
                        if (j == 4) break;

                        if (db.levels[i].questions[j].questionImage != null)
                        {
                            GUILayout.Box(db.levels[i].questions[j].questionImage, GUILayout.Width(90), GUILayout.Height(40));
                        }
                        else
                        {
                            db.levels[i].questions[j].questionImage = TryGetFormulaImageFromFile(db.levels[i].questions[j].question);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                GUILayout.Space(10);
                if (GUILayout.Button("Редактировать уровень"))
                {
                    j = i;
                    MainMenu = true;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    dirty = false;
                    notDirty = true;
                    break;
                }
                EditorGUILayout.EndVertical();
                GUILayout.Space(20);
            }

        }

        if (GUILayout.Button("Добавить уровень"))
        {
            db.levels.Add(new Level());
            dirty = true;
        }
    }

    private void DrawLevel()
    {
        GUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("◄", GUILayout.Width(40), GUILayout.Height(40)))
        {
            MainMenu = false;
            notDirty = true;
        }
        EditorGUILayout.LabelField("Уровень " + (j + 1), big);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20);
        /*if (GUILayout.Button("NULL all"))
        {
            for (int k = 0; k < db.levels[j].questions.Count; k++)
            {
                db.levels[j].questions[k].questionImage = null;
                for (int o = 0; o < db.levels[j].questions[k].answers.Count; o++)
                {
                    db.levels[j].questions[k].answersImage[o] = null;
                }
            }
        }*/
        EditorGUILayout.BeginHorizontal();
        defaultTime = EditorGUILayout.FloatField("Время (с.)", defaultTime);
        if (GUILayout.Button("Применить ко всем вопросам"))
        {
            for (int k = 0; k < db.levels[j].questions.Count; k++)
            {
                db.levels[j].questions[k].time = defaultTime;
            }
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(6);
        if (GUILayout.Button("Прожать все кнопки UPD"))
        {
            for (int k = 0; k < db.levels[j].questions.Count; k++)
            {
                db.levels[j].questions[k].questionImage = GetFormulaImage(db.levels[j].questions[k].question);
                for (int o = 0; o < db.levels[j].questions[k].answers.Count; o++)
                {
                    db.levels[j].questions[k].answersImage[o] = GetFormulaImage(db.levels[j].questions[k].answers[o]);
                }
            }
        }
        GUILayout.Space(6);
        if (GUILayout.Button("Запустить этот уровень"))
        {
            int levelToStart = 1;

            if (gameData) gameData.SelectedLevel = db.levels[j];

            if (UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().buildIndex == levelToStart)
                EditorApplication.EnterPlaymode();
            else
            {
                if (!UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().isDirty)
                {
                    try
                    {
                        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(levelToStart), UnityEditor.SceneManagement.OpenSceneMode.Single);
                        EditorApplication.EnterPlaymode();
                    }
                    catch
                    {
                        Debug.LogError("Ошибка при запуске нужной сцены");
                    }
                }
                else
                {
                    Debug.LogWarning("Сцена не сохранена!");
                }
            }
        }
        GUILayout.Space(20);

        if (db.levels[j].questions.Count == 0)
        {
            EditorGUILayout.LabelField("Вопросов нет");
        }
        else
        {
            for (int k = 0; k < db.levels[j].questions.Count; k++)
            {
                if (!PrintQuestion(db.levels[j].questions[k], k)) break;
            }
        }

        if (GUILayout.Button("Добавить вопрос"))
        {
            db.levels[j].questions.Add(new Question());
            if (db.levels[j].questions.Count >= 2) db.levels[j].questions[db.levels[j].questions.Count - 1].text = db.levels[j].questions[db.levels[j].questions.Count - 2].text;
            dirty = true;
        }
    }

    private bool PrintQuestion(Question question, int k)
    {
        EditorGUILayout.BeginVertical("box");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Вопрос " + (k + 1), big);
        if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
        {
            db.levels[j].questions.RemoveAt(k);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            return false;
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Вопрос:");
        question.text = EditorGUILayout.TextField(question.text);
        EditorGUILayout.BeginHorizontal();
        if (question.questionImage != null)
        {
            GUILayout.Box(question.questionImage, GUILayout.Width(90), GUILayout.Height(40));
        }
        else
        {
            question.questionImage = TryGetFormulaImageFromFile(question.question);
        }
        if (GUILayout.Button("UPD", GUILayout.Width(40), GUILayout.Height(40)))
        {
            question.questionImage = GetFormulaImage(question.question);
        }
        question.question = EditorGUILayout.TextField(question.question, GUILayout.Height(40));
        GUILayout.Box(timerSprite.texture, GUILayout.Width(40), GUILayout.Height(40));
        question.time = EditorGUILayout.FloatField(question.time, GUILayout.Width(40), GUILayout.Height(40));
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        if (question.answers.Count > 1)
        {
            while (question.answersImage.Count < question.answers.Count)
            {
                question.answersImage.Add(null);
            }
            EditorGUILayout.LabelField("Ответы:");
            for (int i = 0; i < question.answers.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (question.answersImage[i] != null)
                {
                    GUILayout.Box(question.answersImage[i], GUILayout.Width(90), GUILayout.Height(40));
                }
                else
                {
                    question.answersImage[i] = TryGetFormulaImageFromFile(question.answers[i]);
                }
                if (GUILayout.Button("UPD", GUILayout.Width(40), GUILayout.Height(40)))
                {
                    question.answersImage[i] = GetFormulaImage(question.answers[i]);
                }
                question.answers[i] = EditorGUILayout.TextField(question.answers[i], GUILayout.Height(40));
                if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    question.answers.RemoveAt(i);
                    question.answersImage.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            question.answers.Add("empty");
        }
        if (question.answers.Count < 4 && GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20))) question.answers.Add("empty");
        EditorGUILayout.EndVertical();
        EditorGUILayout.Separator();
        GUILayout.Space(20);
        return true;
    }

    public Texture2D TryGetFormulaImageFromFile(string formula)
    {
        var path = Application.dataPath + "/ExpressionImages/";
        Texture2D texture = new Texture2D(2, 2);
        if (File.Exists(path + Encryption.SimpleEncrypt(formula) + ".png"))
        {
            var bytes = File.ReadAllBytes(path + Encryption.SimpleEncrypt(formula) + ".png");
            texture.LoadImage(bytes);
        }
        return texture;
    }

    public Texture2D GetFormulaImage(string formula)
    {
        var path = Application.dataPath + "/ExpressionImages/";

        formula = formula.Replace("%2b", "%+");
        var url = "http://chart.apis.google.com/chart?cht=tx&chl=" + WebUtility.UrlEncode(formula);

        Debug.Log(url);
        WWW www = new WWW(url);
        while (!www.isDone) ;

        Texture2D temp = www.texture;
        Texture2D texture = new Texture2D(temp.width, temp.height);
        for (int x = 0; x < temp.width; x++)
        {
            for (int y = 0; y < temp.height; y++)
            {
                float r = (1f - temp.GetPixel(x, y).r);
                texture.SetPixel(x, y, new Color(1f, 1f, 1f, r * 1.6f));
            }
        }
        texture.Apply();

        var bytes = texture.EncodeToPNG();
        if (Directory.Exists(path))
        {
            File.WriteAllBytes(path + Encryption.SimpleEncrypt(formula) + ".png", bytes);
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("ExpressionImages folder not found");
        }

        return texture;
    }

    private void DrawSaveLine()
    {
        if (dirty) big.normal.textColor = Color.red;
        else big.normal.textColor = new Color(0.4f, 0.7f, 0.4f);
        EditorGUILayout.LabelField("___________________________________________________________________________", big);
    }
}
