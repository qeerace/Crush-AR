using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class SurfaceChecker : MonoBehaviour
{
    Animator yesAnimator;
    Animator NoAnimator;
    Animator letterCloseAnimator;
    Animator letterOpenAnimator;
    public AudioSource audioPlayer;
    public AudioClip yesOrNo;
    public Canvas canvas;
    public Animator panel;
    private Vector3[] offsetPosition = new Vector3[4];
    public ARRaycastManager arRayCastMng;
    public bool canPlace;
    private Pose placementPose;
    private Pose playerPOS;
    public GameObject cameraPhoneImage;
    public GameObject placementIndicator;
    public GameObject spawningObject;
    public GameObject dialogBox;
    Button yesButton;
    Button noButton;
    public GameObject resetObject;
    public GameObject yesObject;
    public GameObject noObject;
    public GameObject letterClose;
    public GameObject letterOpen;
    public GameObject heart;
    public GameObject brokenHeart;
    public string[] dialogList = new string[6];
    public int dialogIndex;

    private float scale =1;

    private bool isStart;
    // Start is called before the first frame update
    void Start()
    {
        if (Screen.width > 800 && Screen.height > 800) {
            canvas.GetComponent<CanvasScaler>().scaleFactor = 2;
            scale = 2;
        }
        yesAnimator = yesObject.GetComponent<Animator>();
        NoAnimator = noObject.GetComponent<Animator>();
        letterCloseAnimator = letterClose.GetComponent<Animator>();
        letterOpenAnimator = letterOpen.GetComponent<Animator>();
        yesButton = yesObject.GetComponent<Button>();
        noButton = noObject.GetComponent<Button>();
        offsetPosition[0] = new Vector3(0, -2f, 3f);
        offsetPosition[1] = new Vector3(-3f, -2f, 0f);
        offsetPosition[2] = new Vector3(3f, -2f, 0f);
        offsetPosition[3] = new Vector3(0f, -2f, -3f);
        dialogIndex = -1;
        dialogList[0] = "This is awkward but bare with me …";
        dialogList[1] = "I have got something to tell you …";
        dialogList[2] = "You are the only person I want to be with …";
        dialogList[3] = "Every single day until the end of time";
        dialogList[4] = "So …";
        dialogList[5] = "Will you be my Valentine?";
        cameraPhoneImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width/scale,(Screen.height/6f)/scale);
        cameraPhoneImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, ((Screen.height / 12f) - (Screen.height / 2f))/scale, 0);
        resetObject.GetComponent<RectTransform>().anchoredPosition = new Vector3((Screen.width / 2 -100)/scale, ((Screen.height / 2) -100)/scale, 0);
        dialogBox.GetComponent<DialogBox>().textMeshPro.GetComponent<TMPro.TextMeshPro>().text = "Hello";
        dialogBox.SetActive(false);
        playerPOS.rotation = new Quaternion(0,0,0,0) ;
        letterClose.SetActive(false);
        letterOpen.SetActive(false);
        yesObject.SetActive(false);
        noObject.SetActive(false);
        resetObject.SetActive(false);
        heart.SetActive(false);
        brokenHeart.SetActive(false);
        isStart = false;
        yesButton.onClick.AddListener(delegate ()
        {
            //letterClose.SetActive(false);
            //letterOpen.SetActive(false);
            
            heart.SetActive(true);
            brokenHeart.SetActive(false);
            //heart.transform.position = playerPOS.position + new Vector3(0, 0f, 5f);
            //heart.transform.localScale = new Vector3(3, 3, 3);
            StartCoroutine("FadeOut", "yes");
            //heart.transform.eulerAngles = GameObject.Find("AR Camera").transform.eulerAngles;
            //StartCoroutine("YesHeart");
            // คลิกแล้วเปลี่ยนไปหน้า home
        });
        noButton.onClick.AddListener(delegate ()
        {
            //letterClose.SetActive(false);
            //letterOpen.SetActive(false);
            
            brokenHeart.SetActive(true);
            heart.SetActive(false);
            //brokenHeart.transform.position = playerPOS.position + new Vector3(0, 0f, 5f);
            //brokenHeart.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            StartCoroutine("FadeOut", "no");
            //brokenHeart.transform.eulerAngles = GameObject.Find("AR Camera").transform.eulerAngles;
            //StartCoroutine("NoHeart");
            // คลิกแล้วเปลี่ยนไปหน้า home
        });

    }
    private void Reset()
    {
        if(Screen.width > 800 && Screen.height > 800) {
            canvas.GetComponent<CanvasScaler>().scaleFactor = 2;
            scale = 2;
        }
        dialogIndex = -1;
        cameraPhoneImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / scale, (Screen.height / 6f) / scale);
        cameraPhoneImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, ((Screen.height / 12f) - (Screen.height / 2f)) / scale, 0);
        resetObject.GetComponent<RectTransform>().anchoredPosition = new Vector3((Screen.width / 2 - 100) / scale, ((Screen.height / 2) - 100) / scale, 0);
        dialogBox.GetComponent<DialogBox>().textMeshPro.GetComponent<TMPro.TextMeshPro>().text = "Hello";
        dialogBox.SetActive(false);
        playerPOS.rotation = new Quaternion(0, 0, 0, 0);
        letterClose.SetActive(false);
        letterOpen.SetActive(false);
        yesObject.SetActive(false);
        noObject.SetActive(false);
        resetObject.SetActive(false);
        heart.SetActive(false);
        brokenHeart.SetActive(false);
        isStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        letterClose.transform.LookAt(GameObject.Find("AR Camera").transform);
        //letterClose.transform.rotation *= Quaternion.Euler(0, 180, 0);
        letterOpen.transform.LookAt(GameObject.Find("AR Camera").transform);
        //letterOpen.transform.rotation *= Quaternion.Euler(0, 180, 0);
        heart.transform.LookAt(GameObject.Find("AR Camera").transform);
        brokenHeart.transform.LookAt(GameObject.Find("AR Camera").transform);
        playerPOS.position = GameObject.Find("AR Camera").transform.position;
        playerPOS.rotation = GameObject.Find("AR Camera").transform.rotation;
        //if (-90 < playerPOS.rotation.y && 90 > playerPOS.rotation.y) { dialogBox.transform.position = playerPOS.position + offsetPosition[0]; }
        //if (-180 < playerPOS.rotation.y && 0 > playerPOS.rotation.y) { dialogBox.transform.position = playerPOS.position + offsetPosition[1]; }
        //if (0 < playerPOS.rotation.y && 180 > playerPOS.rotation.y) { dialogBox.transform.position = playerPOS.position + offsetPosition[2]; }
        //if (180 < Mathf.Abs(playerPOS.rotation.y) && 270 > Mathf.Abs(playerPOS.rotation.y)) { dialogBox.transform.position = playerPOS.position + offsetPosition[3]; }

        //dialogBox.transform.position = GameObject.Find("AR Camera").transform.TransformPoint(new Vector3(0f, -2f, 3f));
        if (dialogBox.activeInHierarchy&& isStart) {
            dialogBox.transform.LookAt(GameObject.Find("AR Camera").transform);
            dialogBox.transform.rotation *= Quaternion.Euler(0, 180, 0);
            //dialogBox.transform.position = dialogBox.transform.position + offsetPosition[0];
            //dialogBox.transform.eulerAngles = GameObject.Find("AR Camera").transform.eulerAngles;
            //dialogBox.transform.rotation = Quaternion.Euler(playerPOS.position.x, playerPOS.position.y, playerPOS.position.z);
        }
        //dialogBox.transform.position = playerPOS.position + offsetPosition[0];
        //dialogBox.transform.eulerAngles = GameObject.Find("AR Camera").transform.eulerAngles;

        //dialogBox.GetComponent<DialogBox>().textMeshPro.GetComponent<TMPro.TextMeshPro>().text = "Hello";
        //UpdatePlacementPose();
        //UpdateTargetIndicator();
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (dialogIndex == -1)
            {
                if (((Screen.width / 2f) - (Screen.width / 4)) < Input.GetTouch(0).position.x && ((Screen.width / 2f) + (Screen.width / 4)) > Input.GetTouch(0).position.x && ((Screen.height / 24f) - (Screen.height / 2f)) < Input.GetTouch(0).position.y && ((3 * 6) * (Screen.height / 24f) - (Screen.height / 2f)) > Input.GetTouch(0).position.y)
                {
                    Destroy(cameraPhoneImage);
                    playerPOS.rotation = GameObject.Find("AR Camera").transform.rotation;
                    playerPOS.rotation = GameObject.Find("AR Camera").transform.rotation;
                    isStart = true;
                    dialogBox.SetActive(true);
                    resetObject.SetActive(true);
                    Debug.Log(Input.GetTouch(0).position);
                }
            }
            if (dialogIndex == 5)
            {
                dialogBox.SetActive(false);
                Vector3 letterPlacement = playerPOS.position;
                StartCoroutine("LetterClose");
                dialogIndex = 6;
                //letterClose.transform.position = playerPOS.position + new Vector3(2f, 3f,15f);
                //letterClose.transform.eulerAngles = GameObject.Find("AR Camera").transform.eulerAngles;
                //letterCloseAnimator.SetBool("play", true);
                //letterOpen.transform.position = playerPOS.position + new Vector3(0, 0.01f, 10f);
                //letterOpen.transform.eulerAngles = GameObject.Find("AR Camera").transform.eulerAngles;
                //dialogIndex = -1;
                //dialogBox.GetComponent<DialogBox>().textMeshPro.GetComponent<TMPro.TextMeshPro>().text = "";
                //yesAnimator.SetBool("Start", true);
                //NoAnimator.SetBool("Start", true);
                //UserPressYes();
            }
            if(dialogIndex<5)
            {
                dialogIndex++;
                dialogBox.GetComponent<DialogBox>().textMeshPro.GetComponent<TMPro.TextMeshPro>().text = dialogList[dialogIndex];
            }
            
        }
        //if (canPlace && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    // now the player touches the screen, spawn the prefab
        //    Instantiate(spawningObject, placementPose.position, placementPose.rotation);
        //}
    }

    private void UpdateTargetIndicator()
    {
        if (canPlace)
        {
            // show the placement indicator
            // update the pos/rotation base on the placementPose
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            // hide the placement indicator
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {

        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        arRayCastMng.Raycast(screenCenter, hits, TrackableType.Planes);

        canPlace = hits.Count > 0;

        if (canPlace)
        {
            placementPose = hits[0].pose;

            Vector3 cameraForward = Camera.current.transform.forward;
            Vector3 cameraBearing = new Vector3(cameraForward.x, 0f, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }

    }
    IEnumerator LetterOpen() {
        letterOpenAnimator.SetBool("play", true);
        yield return new WaitForSeconds(1f);
        //letterOpenAnimator.SetBool("play", false);
        //letterOpen.SetActive(false);
        StartCoroutine("FadeIn");
    }

    IEnumerator LetterClose(){
        letterClose.SetActive(true);
        letterOpen.SetActive(true);
        //letterClose.transform.position = playerPOS.position + new Vector3(2f, 3f, 15f);
        //letterClose.transform.eulerAngles = GameObject.Find("AR Camera").transform.eulerAngles;
        letterCloseAnimator.SetBool("play", true);
        yield return new WaitForSeconds(1.833f);
        //letterCloseAnimator.SetBool("play", false);
        StartCoroutine("LetterOpen");
    }
    IEnumerator FadeIn() {
        panel.SetBool("isLove", true);
        yield return new WaitForSeconds(0.333f);
        //panel.SetBool("isLove", false);
        yesObject.SetActive(true);
        noObject.SetActive(true);
        
        yesAnimator.SetBool("Start", true);
        NoAnimator.SetBool("Start", true);
        audioPlayer.PlayOneShot(yesOrNo);
        dialogBox.GetComponent<AudioSource>().Stop();
        yield return new WaitForSeconds(1f);
        letterOpen.SetActive(false);
        //yesAnimator.SetBool("Start", false);
        //NoAnimator.SetBool("Start", false);
    }
    IEnumerator FadeOut(string status) {
        panel.SetBool("finish", true);
        yesObject.SetActive(false);
        noObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        //panel.SetBool("finish", false);
        if (status == "yes") { StartCoroutine("YesHeart"); }
        else { StartCoroutine("NoHeart"); }
    }
    IEnumerator YesHeart() {
        yield return new WaitForSeconds(1f);
        heart.GetComponent<Animator>().SetBool("sayYes", true);
    }
    IEnumerator NoHeart() {
        yield return new WaitForSeconds(1f);
        brokenHeart.GetComponent<Animator>().SetBool("sayNo", true);
    }
}

