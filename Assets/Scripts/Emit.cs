using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Emit : MonoBehaviour
{
    [SerializeField] ParticleSystem[] ps = null;
    [SerializeField] GameObject text = null;
    [SerializeField] Vector2 textScale = new Vector2(0, 0);
    [SerializeField] float easingDuration = 0.0f;
    bool isEmit = false;
    [SerializeField] KeyCode key = KeyCode.A;

    #region accessor
    public bool IsEmit{
        get{return isEmit;}
        set{isEmit = value;}
    }
    #endregion

    private void Awake() {
        for(int i = 0; i < ps.Length; i++){
            ParticleSystem.MainModule main = ps[i].GetComponent<ParticleSystem>().main;
            main.playOnAwake = false;
            main.loop = false;
            // ps[i].playOnAwake = false;
            // ps[i].loop = false;
        }
    }
    
    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(key)){
            isEmit = true;
        }
        if(isEmit){
            foreach(var p in ps){
                p.Play();
                //text.transform.DOScale(new Vector3(textScale.x, textScale.y, 1.0f), 1.0f);
                var seq = DOTween.Sequence();
                seq.Append(text.transform.DOScale(new Vector3(textScale.x, textScale.y, 1.0f), easingDuration));
                seq.Append(text.transform.DOScale(new Vector3(0.0f, 0.0f, 1.0f), easingDuration));
            }
        }
        isEmit = false;
    }
}
