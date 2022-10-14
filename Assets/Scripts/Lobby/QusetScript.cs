using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QusetScript : MonoBehaviour
{
    public int[,] qusetIdx;
    [SerializeField] Image QusetImage;//퀘스트 보상 이미지
    [SerializeField] Text Qusetrewards;//퀘스트 보상 텍스트
    [SerializeField] Text QusetText; //퀘스트 내용 텍스트
    [SerializeField] Slider QusetSliders;//퀘스트 진행도 슬라이더
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
