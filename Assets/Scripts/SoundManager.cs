using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public enum SoundManagerMode
    {
        simple,
        composite,
        uiButton
    }
    public SoundManagerMode mode;

    public AudioClip startClip;
    public AudioClip clip;
    public AudioClip endClip;

    public AudioMixerGroup mixer;

    public bool loop = false;
    public bool playOnAwake = false;
    public bool dontPlayWhileSoundEnd = false;
    public bool itButton;
    public bool playInNewObject;

    [Range(0, 1)]
    public float volume = 0.6f;

    public bool randomPitch = false;
    public float pitchMin = 0.7f, pitchMax = 1.3f;

    [HideInInspector] public AudioSource au;
    private byte compositeState = 2;
    private float rp;

    private void Start()
    {
        if (mode == SoundManagerMode.uiButton) itButton = true;

        if (itButton)
        {
            if (GetComponent<Button>() == null)
            {
                itButton = false;
                mode = SoundManagerMode.simple;
            }
            else
            {
                GetComponent<Button>().onClick.AddListener(Play);
            }
        }

        if (playOnAwake) Play();
    }

    private void OnEnable()
    {
        if (playOnAwake) Play();
    }

    private void Update()
    {
        if (mode != SoundManagerMode.composite) return;

        /*if (compositeState == 0 && !au.isPlaying && clip != null)
        {
            compositeState = 1;
            if (randomPitch)
                au.pitch = rp;
            else
                au.pitch = 1f;
            au.clip = clip;
            au.loop = true;
            au.Play();
        }*/
    }

    public void Play()
    {
        if (dontPlayWhileSoundEnd && au != null && au.isPlaying)
        {
            return;
        }

        rp = Random.Range(pitchMin, pitchMax);

        if (mode == SoundManagerMode.composite)
        {
            au = new GameObject().AddComponent<AudioSource>();
            au.name = "Sound player";
            au.volume = volume;
            au.playOnAwake = false;
            au.outputAudioMixerGroup = mixer;
            au.clip = endClip;
            Destroy(au.gameObject, au.clip.length); // TODO: тут возникает исключение, я думаю из-за new GameObject()
            au.loop = false;
            if (randomPitch)
                au.pitch = rp;
            else
                au.pitch = 1f;
            au.Play();
            au = null;
        }

        if (playInNewObject)
        {
            au = new GameObject().AddComponent<AudioSource>();
            au.name = "Sound player";
            au.volume = volume;
            au.loop = loop;
            au.playOnAwake = false;
            au.outputAudioMixerGroup = mixer;
            /*if (mode == SoundManagerMode.composite)
            {
                au.clip = startClip;
                au.loop = false;
            }
            else*/
            {
                au.clip = clip;
            }
            if (clip != null)
            Destroy(au.gameObject, au.clip.length);
        }
        else if (au == null)
        {
            au = gameObject.AddComponent<AudioSource>();
            au.volume = volume;
            au.loop = loop;
            au.playOnAwake = false;
            au.outputAudioMixerGroup = mixer;
            /*if (mode == SoundManagerMode.composite)
            {
                au.clip = startClip;
                au.loop = false;
            }
            else*/
            {
                au.clip = clip;
            }
        }

        if (randomPitch)
        {
            au.pitch = rp;
        }
        else
            au.pitch = 1f;


        if (mode == SoundManagerMode.composite)
        {
            au.loop = true;
        }

        compositeState = 0;
        au.Play();
    }

    public void Stop()
    {
        compositeState = 2;

        if (au == null) return;

        if (mode == SoundManagerMode.composite && endClip != null)
        {
            au.Stop();
            Destroy(au);
            au = new GameObject().AddComponent<AudioSource>();
            au.name = "Sound player";
            au.volume = volume;
            au.playOnAwake = false;
            au.outputAudioMixerGroup = mixer;
            au.clip = endClip;
            Destroy(au.gameObject, au.clip.length);
            au.loop = false;
            if (randomPitch)
                au.pitch = rp;
            else
                au.pitch = 1f;
            au.Play();
            au = null;
        }
        if (au != null)
            au.Stop();
    }
}
