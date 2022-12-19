using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ESoundType
{
    BGM,
    SFX,
    End
}
public class AudioSourceClass
{
    public AudioSource audiosource;
    public float audioVolume;
}
public class SoundManager : Singleton<SoundManager>
{
    [Header("Sounds")]
    [SerializeField]
    private List<AudioClip> BGM = new List<AudioClip>();
    [SerializeField]
    private List<AudioClip> SFX = new List<AudioClip>();

    public bool bgmmute;
    public bool sfxmute;

    //오디오를 이름별로 분류해 있는 Dictionary
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    //오디오실행 타입에 따른 AudioSource가 있는 Dictionary
    private Dictionary<ESoundType, AudioSourceClass> audioSourceClasses = new Dictionary<ESoundType, AudioSourceClass>();

    //시작할때 사운드를 딕셔너리에 추가해줌
    protected override void Awake()
    {
        base.Awake();
        //Resources폴더 안에 Sounds폴더 안에 있는  AudioClip을 전부 가져온다
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds/");
        //가져온 AudioClips를 Dictionary에 추가
        foreach (AudioClip clip in clips)
        {
            audioClips[clip.name] = clip;
        }
        //Enum형 이름을 string으로 변환해줌
        string[] enumNames = Enum.GetNames(typeof(ESoundType));

        //게임오브젝트를 오디오 클립종류만큼 생성하고 오디오 클립들을 그에 맞는 자식 오브젝트로 생성한다
        for (int i = 0; i < (int)ESoundType.End; i++)
        {
            GameObject AudioSourceobj = new GameObject(enumNames[i]);
            AudioSourceobj.transform.SetParent(transform);
            AudioSourceClass sourceClass = new AudioSourceClass
                { audiosource = AudioSourceobj.AddComponent<AudioSource>(), audioVolume = 0.5f };
            audioSourceClasses[(ESoundType)i] = sourceClass;
        }

        audioSourceClasses[ESoundType.BGM].audiosource.loop = true;
    }

    /// <summary>
    /// 사운드 사져오는데 여러가지 요소를 넣어서 가져온다
    /// </summary>
    /// <param name="soundtype">BGM , SFX</param>
    /// <param name="name">무슨 사운드</param>
    /// <param name="volume">볼륨</param>
    /// <param name="pitch">피치</param>
    /// <returns></returns>
    public AudioClip PlaySoundClip(string clipname, ESoundType type, float volume = 0.5f, float pitch = 1)
    {
        AudioClip clip = audioClips[clipname];

        audioSourceClasses[type].audiosource.clip = clip;
        audioSourceClasses[type].audiosource.volume = volume;
        audioSourceClasses[type].audiosource.pitch = pitch;

        if (type == ESoundType.BGM)//BGM
            audioSourceClasses[type].audiosource.Play();
        else//SFX
            audioSourceClasses[type].audiosource.PlayOneShot(clip);

        return clip;
    }
}