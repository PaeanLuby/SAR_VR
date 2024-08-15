using sxr_internal;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VisibleObject : MonoBehaviour
{

    private MeshRenderer meshrenderer = null;
    public XRRayInteractor leftray;
    public XRRayInteractor rightray;
    public Camera inactiveCamera;
    private int randIndexSearch = 0;
    private RaycastHit hit;
    private string numTrials;
    private string radius;
    [SerializeField] GameObject[] locations;
    [SerializeField] GameObject[] searchObjects;
    [SerializeField] GameObject player;
    [SerializeField] Camera playerHead;
    [SerializeField] Camera objectHead;
    private int resWidth;
    private int resHeight;
    private bool notFoundRecorded = false;
    private enum ObjectState { found, notFound };
    private bool isTransitionHappening = false;
    private bool objectDropped = false;
    private ObjectState state;
    private GameObject objectCopy;
    private Color[] pixelColors1;
    private Color[] pixelColors2;
    private float mSE;
    private Vector3 originPoint;

    void Awake()
    {
        meshrenderer = GetComponent<MeshRenderer>();
        state = ObjectState.found;
        numTrials = "-5";
        radius = "0";
        resWidth = 1080;
        resHeight = 1080;
        objectHead.gameObject.SetActive(false);
    }

    public void ReadStringInputTrials(string s)
    {
        numTrials = s;
        Debug.Log(numTrials);
    }

    public void ReadStringInputRad(string r)
    {
        radius = r;
        Debug.Log(radius);
    }

    private void Start()
    {
        //Spawn random location and target of player
        int randIndexSpawn = UnityEngine.Random.Range(0, locations.Length);
        int randIndexSearch = UnityEngine.Random.Range(0, searchObjects.Length);
    }

    void Update()
    {
        switch (sxr.GetPhase())
        {
            case 1: // Instruction Phase
                switch (sxr.GetStepInTrial())
                {
                    case 0: // Hit trigger to start
                        sxr.DisplayImage("triggercont");
                        if (sxr.CheckController(ControllerButton.Trigger))
                        {
                            sxr.HideImagesUI();
                            sxr.NextStep();
                            sxr.RestartTimer();
                        }
                        break;
                    case 1:
                        sxr.DisplayImage("practiceStart");
                        if (sxr.CheckController(ControllerButton.Trigger))
                        {
                            sxr.HideImagesUI();
                            sxr.NextPhase();
                        }
                        break;
                }
                break;
            case 2: // Testing Round  
                switch (sxr.GetBlock())
                {
                    case 0: // Hit trigger to start
                        sxr.DisplayImage("triggerSelect2_result");
                        if (sxr.CheckController(ControllerButton.Trigger))
                        {
                            sxr.HideImagesUI();
                            sxr.NextBlock();
                        }
                        break;
                    case 1:
                        sxr.DisplayImage("Start experiment");
                        if (sxr.CheckController(ControllerButton.ButtonA))
                        {
                            sxr.HideImagesUI();
                            sxr.NextBlock();
                            sxr.RestartTimer();
                        }
                        break;
                    case 2:
                        if (sxr.GetTrial() == int.Parse(numTrials) + 1)
                        {
                            sxr.NextPhase();
                        }
                        switch (state)
                        {
                            //If player doesn't find object after 30 seconds, transition to next trial.
                            case ObjectState.notFound:
                                StartCoroutine(findObject(0));
                                if (sxr.TimePassed() > 30)
                                {
                                    objectDisappear();
                                    inactiveCamera.enabled = true;
                                    playerHead.enabled = false;
                                    StartCoroutine(trialTransition(0));

                                }
                                break;
                            //If player finds object, transition to next trial.
                            case ObjectState.found:
                                if (sxr.CheckController(ControllerButton.ButtonA))
                                {
                                    inactiveCamera.enabled = true;
                                    playerHead.enabled = false;
                                    StartCoroutine(trialTransition(0));
                                }

                                break;
                        }
                        break;
                }
                break;
            case 3:
                sxr.DisplayImage("finished");
                break;
        }
    }

    IEnumerator findObject(float time)
    {
        if (sxr.CheckController(ControllerButton.Trigger) && (leftray.TryGetCurrent3DRaycastHit(out hit) || rightray.TryGetCurrent3DRaycastHit(out hit)))
        {
            hit.transform.gameObject.SetActive(false);
            sxr.WriteToTaggedFile("mainFile", "");
            state = ObjectState.found;
            objectDropped = false;
            yield return new WaitForSeconds(0);
        }
    }

    void objectDisappear()
    {
        if (notFoundRecorded == false)
        {
            notFoundRecorded = true;
            objectCopy.SetActive(false);
            sxr.WriteToTaggedFile("mainFile", "");
            objectDropped = false;
        }
        
    }

    IEnumerator trialTransition(float time) {
        if (isTransitionHappening) {
            yield break;
        }
        isTransitionHappening = true;

        if (!objectDropped) {
            dropObject();
        }

        
        
        yield return new WaitForSeconds(4);
        
        notFoundRecorded = false;
        StartCoroutine(takeSnapshot(2));
        sxr.WriteHeaderToTaggedFile("mainFile", "trial number " + sxr.GetTrial() + searchObjects[randIndexSearch].name.ToString() + "item at " + originPoint.ToString() + "player at " + player.transform.position.ToString() + "intensity ratio " + mSE);
        sxr.NextTrial();
        sxr.RestartTimer();
        playerHead.enabled = true;
        isTransitionHappening = false;
        
    }

    IEnumerator takeSnapshot(int imageNum) {
        leftray.GetComponent<LineRenderer>().enabled = false;
        rightray.GetComponent<LineRenderer>().enabled = false;
        string fileName = string.Format("{0}/Experiments/trial_{1}_{2}x{3}_{4}_{5}.png", 
            Application.dataPath,
            sxr.GetTrial(),
            resWidth,
            resHeight,
            searchObjects[randIndexSearch].name.ToString(),
            DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
        objectHead.gameObject.SetActive(true);
        Texture2D snapshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        objectHead.Render();
        RenderTexture.active = objectHead.targetTexture;
        snapshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        switch(imageNum) {
            case 1: 
                pixelColors1 = snapshot.GetPixels();
                break;
            case 2:
                pixelColors2 = snapshot.GetPixels();
                calculateMSE();
                break;
        }
        byte[] bytes = snapshot.EncodeToPNG();
        System.IO.File.WriteAllBytes(fileName, bytes);
        objectHead.gameObject.SetActive(false);
        leftray.GetComponent<LineRenderer>().enabled = true;
        rightray.GetComponent<LineRenderer>().enabled = true;
        yield return new WaitForSeconds(0);
    }

    void calculateMSE() {
        float sumOfSquares = 0;
        for (int i = 0; i < pixelColors1.Length; i++) {
            float squaredDiffR = (pixelColors1[i].r - pixelColors2[i].r) * (pixelColors1[i].r - pixelColors2[i].r);
            float squaredDiffG = (pixelColors1[i].g - pixelColors2[i].g) * (pixelColors1[i].g - pixelColors2[i].g);
            float squaredDiffB = (pixelColors1[i].b - pixelColors2[i].b) * (pixelColors1[i].b - pixelColors2[i].b);
            sumOfSquares += squaredDiffR + squaredDiffG + squaredDiffB;
        }
        mSE = sumOfSquares/pixelColors1.Length;
    }

    void dropObject() {
        if (!objectDropped) {
            objectDropped = true;
            state = ObjectState.notFound;
            int randIndexSpawn = UnityEngine.Random.Range(0, locations.Length);
            randIndexSearch = UnityEngine.Random.Range(0, searchObjects.Length);
            float rotationAnglePlayerY = locations[randIndexSpawn].transform.rotation.eulerAngles.y -
                playerHead.transform.rotation.eulerAngles.y;
            
            player.transform.Rotate(0, rotationAnglePlayerY, 0);

            Vector3 distanceDiffPlayer = locations[randIndexSpawn].transform.position -
                playerHead.transform.position;
            Vector3 playerStart = player.transform.position + distanceDiffPlayer;

            //Spawn 1 random object
            float defrad = UnityEngine.Random.Range(0, float.Parse(radius));
            originPoint = playerStart;
            originPoint.x += UnityEngine.Random.Range(-defrad, defrad);
            originPoint.z += UnityEngine.Random.Range(-defrad, defrad);
            objectCopy = Instantiate(searchObjects[randIndexSearch], originPoint, transform.rotation);
            Vector3 distanceDiffObject = objectCopy.transform.position -
                objectHead.transform.position;
            objectHead.transform.position += distanceDiffObject;
            player.transform.position = playerStart;
            StartCoroutine(takeSnapshot(1));
        }
    }
}
