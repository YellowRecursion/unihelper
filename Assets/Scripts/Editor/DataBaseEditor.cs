using System.Collections;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Net;

[CustomEditor(typeof(DataBase))]
public class DataBaseEditor : Editor // жаль того, кто сюда залез...
{
    public int j;
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
    public bool MainMenu = false;
    private void OnEnable()
    {
        db = (DataBase)target;
        SetStyles();
    }
    public override void OnInspectorGUI()
    {
        bool notDirty = false;
        

        EditorGUILayout.LabelField("База с данными об уровнях", big);
        GUILayout.Space(10);
        db.sectionName = EditorGUILayout.TextField("Название раздела", db.sectionName);
        GUILayout.Space(10);
        DrawSaveLine();
        GUI.changed = false;
        if (MainMenu == false)
        {
            if (db.levels.Count == 0)
            {
                EditorGUILayout.LabelField("Уровней нет");
            }
            else
            {
                for (int i = 0; i < db.levels.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal("box");
                    db.levels[i].levelIndex = i;
                    db.levels[i].parentBase = db;
                    EditorGUILayout.LabelField((db.levels[i].levelIndex + 1).ToString(), header, GUILayout.Width(25));
                    db.levels[i].difficulty = (Level.Difficulty)EditorGUILayout.EnumPopup(db.levels[i].difficulty, GUILayout.Width(120), GUILayout.Height(10));
                    //db.levels[i].stars = EditorGUILayout.IntField(db.levels[i].stars, GUILayout.Width(60));
                    if (GUILayout.Button("Редактировать уровень"))
                    {
                        j = i;
                        MainMenu = true;
                        EditorGUILayout.EndHorizontal();
                        dirty = false;
                        notDirty = true;
                        break;
                    }
                    if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        db.levels.RemoveAt(i);
                        EditorGUILayout.EndHorizontal();
                        dirty = true;
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(10);
                }

            }

            if (GUILayout.Button("Добавить уровень"))
            {
                db.levels.Add(new Level());
                dirty = true;
            }
        }
        else
        {
            if (GUILayout.Button("◄", GUILayout.Width(20), GUILayout.Height(20)))
            {
                MainMenu = false;
                notDirty = true;
            }

            GUILayout.Space(10);
            if (GUILayout.Button("NULL all"))
            {
                for (int k = 0; k < db.levels[j].questions.Count; k++)
                {
                    db.levels[j].questions[k].questionImage = null;
                    for (int o = 0; o < db.levels[j].questions[k].answers.Count; o++)
                    {
                        db.levels[j].questions[k].answersImage[o] = null;
                    }
                }
            }
            if (GUILayout.Button("Update all"))
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
            GUILayout.Space(10);

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
