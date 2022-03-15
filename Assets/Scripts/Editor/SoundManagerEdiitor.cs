using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

[CustomEditor(typeof(SoundManager)), CanEditMultipleObjects]
public class SoundManagerEdiitor : Editor
{
    private SoundManager sm;

    private GUIStyle bigHeader;
    private GUIStyle header;

    private void OnEnable()
    {
        sm = (SoundManager)target;
        SetStyles();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("♪ Sound Manager ♪", bigHeader);

        EditorGUILayout.Space(8);
        sm.mode = (SoundManager.SoundManagerMode)EditorGUILayout.Popup("Режим:", (int)sm.mode, new string[] { "Простой", "Составной", "Кнопка интерфейса" });
        sm.mixer = (AudioMixerGroup)EditorGUILayout.ObjectField("Микшер:", sm.mixer, typeof(AudioMixerGroup), false);

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Основное:", header);
        if (sm.mode == SoundManager.SoundManagerMode.composite)
        {
            EditorGUILayout.LabelField("Клипы: (Начальный, Основной, Окончательный)");
            EditorGUILayout.BeginHorizontal();
            sm.startClip = (AudioClip)EditorGUILayout.ObjectField(sm.startClip, typeof(AudioClip), false);
            sm.clip = (AudioClip)EditorGUILayout.ObjectField(sm.clip, typeof(AudioClip), false);
            sm.endClip = (AudioClip)EditorGUILayout.ObjectField(sm.endClip, typeof(AudioClip), false);
            EditorGUILayout.EndHorizontal();
            sm.volume = EditorGUILayout.Slider("Громкость:", sm.volume, 0f, 1f);
        }
        else if(sm.mode == SoundManager.SoundManagerMode.simple)
        {
            sm.clip = (AudioClip)EditorGUILayout.ObjectField("Клип:",sm.clip, typeof(AudioClip), false);
            sm.volume = EditorGUILayout.Slider("Громкость:", sm.volume, 0f, 1f);
            sm.playOnAwake = EditorGUILayout.Toggle("Проиграть при старте:", sm.playOnAwake);
            sm.loop = EditorGUILayout.Toggle("Повторять:", sm.loop);
            sm.dontPlayWhileSoundEnd = EditorGUILayout.Toggle("Играть только если клип проигрался:", sm.dontPlayWhileSoundEnd);
            sm.playInNewObject = EditorGUILayout.Toggle("Играть в новом объекте:", sm.playInNewObject);
        }
        else
        {
            sm.clip = (AudioClip)EditorGUILayout.ObjectField("Клип:", sm.clip, typeof(AudioClip), false);
            sm.volume = EditorGUILayout.Slider("Громкость:", sm.volume, 0f, 1f);
            sm.dontPlayWhileSoundEnd = EditorGUILayout.Toggle("Играть только если клип проигрался:", sm.dontPlayWhileSoundEnd);
            sm.playInNewObject = EditorGUILayout.Toggle("Играть в новом объекте:", sm.playInNewObject);
        }

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Дополнительное:", header);
        sm.randomPitch = EditorGUILayout.Toggle("Случайная высота звука:", sm.randomPitch);
        if (sm.randomPitch)
        {
            EditorGUILayout.MinMaxSlider($"Границы высоты: ({sm.pitchMin.ToString("0.0")} - {sm.pitchMax.ToString("0.0")})",ref sm.pitchMin, ref sm.pitchMax, -1f,3f);
        }

        if (GUI.changed) EditorUtility.SetDirty(sm);

        if (targets.Length > 1)
        {
            EditorGUILayout.Space(20);
            if (GUILayout.Button("Установить эти настройки для всех", GUILayout.Height(30)))
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    SoundManager obj = (SoundManager)targets[i];

                    obj.mode = sm.mode;
                    obj.startClip = sm.startClip;
                    obj.clip = sm.clip;
                    obj.endClip = sm.endClip;
                    obj.mixer = sm.mixer;
                    obj.loop = sm.loop;
                    obj.playOnAwake = sm.playOnAwake;
                    obj.dontPlayWhileSoundEnd = sm.dontPlayWhileSoundEnd;
                    obj.itButton = sm.itButton;
                    obj.playInNewObject = sm.playInNewObject;
                    obj.volume = sm.volume;
                    obj.randomPitch = sm.randomPitch;
                    obj.pitchMin = sm.pitchMin;
                    obj.pitchMax = sm.pitchMax;

                    EditorUtility.SetDirty(obj);
                }
            }
            EditorGUILayout.Space(10);
        }
    }

    private void SetStyles()
    {
        bigHeader = new GUIStyle();
        bigHeader.fontSize = 22;
        bigHeader.alignment = TextAnchor.MiddleCenter;
        bigHeader.normal.textColor = Color.cyan;

        header = new GUIStyle();
        header.fontStyle = FontStyle.Bold;
        header.normal.textColor = Color.white;
    }
}
