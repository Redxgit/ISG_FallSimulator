using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [SerializeField] private Text fileName;
    [SerializeField] private AccelerometerFromDeltaPosition filesaver;
    [SerializeField] private FromAnimToRagdoll anim;
    [SerializeField] private Text animatorName;
    [SerializeField] private animation_player aPlayer;
    

    private void Start()
    {
        aPlayer.AnimatorChanged += UpdateAnimatorTextInfo;
    }

    public void UpdateAnimatorTextInfo(string newName)
    {
        animatorName.text = newName;
    }

    public void UISaveToFile() {
        filesaver.WriteInfoToFile(fileName.text);
    }

    public void UIReset() {
        anim.ResetThings();
    }

    public void UIWalk() {
        anim.StartWalking();
    }
}
