using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using Unity.Burst.CompilerServices;

public class FunctionLever : MonoBehaviour
{
    public Button lever, settingsButton, creditsButton, exitButton;
    public AudioSource leverSound, buttonClick;
    public GameObject FadeOutAnim, creditsGO, settingsGO, bossSelectionGO, SettinsPanel1;
    public Animator leverAnim, creditsButtonAnim, settingsButtonAnim, exitButtonAnim, bossSelectionAnim;
    public AudioManager am;
    
    bool creditsOn, settingsOn, bossSelectionOn;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider, sfxSlider;


    public KeyCode[] combo;
    public int currentIndex = 0;

    Scene activeScene;

    public GameObject difficultyChanger;
    public bool difficultychanged = false;
    public int difficultyChanedIntIndicator = 0;
    public bool hasPressed = false;
    public int hasPressedInt = 0;

    public GameObject DifficultySwitcherI;
    public GameObject DifficultySwitcherO;

    public GameObject ThawPanel, ThawPanel1, AintWorkingPanel;
    public bool ThawPanelOn;
    public bool AintWorkingBoolean;

    [Header("Lever")]
    public Sprite[] leverSprites;
    public Slider leverSlider;
    public Image leverRenderer;

    public Animator slotsAnim;
    public AudioSource jackpotSound;

    void Start()
    {
        activeScene = SceneManager.GetActiveScene();

        /*
        if (activeScene.name == "EndScreen")
        {
            difficultyChanedIntIndicator = 1;
            save();
        }
        */
        
        //save();
        load();

        leverSound = GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            loadMusicVolume();
        }
        else setSFXVolume();

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            loadSFXVolume();
        }
        else setMusicVolume();

        difficultyChanger.SetActive(true);

        /*
        if (difficultyChanedIntIndicator == 1)
        {
            difficultyChanger.SetActive(true);
        }
        */

        //print("Loaded hasPressedInt: " + hasPressedInt);
    }
    
    private void Update()
    {
        print(difficultychanged);

        if (creditsOn && Input.anyKeyDown) StartCoroutine(exitCredits());
        if (settingsOn && (Input.anyKeyDown) && (!EventSystem.current.IsPointerOverGameObject()) || (!EventSystem.current.IsPointerOverGameObject()) && Input.GetMouseButtonDown(0) || (Input.GetKeyDown(KeyCode.Escape))) StartCoroutine(exitSettings());
        if (ThawPanelOn && (Input.anyKeyDown) && (!EventSystem.current.IsPointerOverGameObject()) || (!EventSystem.current.IsPointerOverGameObject()) && Input.GetMouseButtonDown(0) || (Input.GetKeyDown(KeyCode.Escape))) StartCoroutine(exitThawPanel());
        if (bossSelectionOn && Input.anyKeyDown) StartCoroutine(exitBossSelection());
        if (!bossSelectionOn)
        {
            if (currentIndex < combo.Length)
            {
                if (Input.GetKeyDown(combo[currentIndex]))
                {
                    currentIndex++;
                }
            }
            else
            {
                print("combo");
                bossSelection();
            }
        }  
    }

    public void save()
    {
        //PlayerPrefs.SetInt("Difficulty Changed", difficultyChanedIntIndicator);
        PlayerPrefs.SetInt("HasPressedInt", hasPressedInt);
        PlayerPrefs.Save();
    }

    public void load()
    {
        //difficultyChanedIntIndicator = PlayerPrefs.GetInt("Difficulty Changed");
        hasPressedInt = PlayerPrefs.GetInt("HasPressedInt");

        if (hasPressedInt == 1)
        {
            //difficultychanged = true;
            //hasPressed = true;
            DifficultySwitcherI.SetActive(true);
            DifficultySwitcherO.SetActive(false);
        }
        else
        {
            //difficultychanged = false;
            //hasPressed = false;
            DifficultySwitcherI.SetActive(false);
            DifficultySwitcherO.SetActive(true);
        }
    }

    public void StartGame()
    {
        lever.interactable = false;
        leverAnim.Play("leverPull");
        StartCoroutine(WaitAndLoadScene());
        am.playAudio(leverSound);
    }

    public void settings()
    {
        settingsOn = true;
        settingsButtonAnim.Play("settingsButtonPress");
        am.playAudio(buttonClick);
        settingsGO.SetActive(true);
        settingsGO.GetComponent<Animator>().Play("settingsRollIn");
    }

    public void DifficultyIncrease()
    {
        if (AintWorkingBoolean == false) StartCoroutine(AintWorker());

        if (DifficultySwitcherI.activeInHierarchy)
        {
            DifficultySwitcherI.SetActive(false);
            DifficultySwitcherO.SetActive(true);
            hasPressedInt = 0;
        }
        else
        {
            DifficultySwitcherI.SetActive(true);
            DifficultySwitcherO.SetActive(false);
            hasPressedInt = 1;
        }
        save();

        /*
        if (DifficultySwitcherI.activeInHierarchy)
        {
            DifficultySwitcherI.SetActive(false);
            DifficultySwitcherO.SetActive(true);
            hasPressedInt = 1;
            save();
        }
        else
        {
            DifficultySwitcherI.SetActive(true);
            DifficultySwitcherO.SetActive(false);
            hasPressedInt = 0;
            save();
        }
        */

        /*
        if (difficultyChanedIntIndicator == 1)
        {
            if (hasPressed == false)
            {
                difficultychanged = true;
                hasPressed = true;
                hasPressedInt = 1;

                save();

                DifficultySwitcherI.SetActive(true);
                DifficultySwitcherO.SetActive(false);
                difficultyChanedIntIndicator = 0;
            }
            else
            {
                difficultychanged = false;
                hasPressed = false;
                hasPressedInt = 0;

                save();

                DifficultySwitcherI.SetActive(false);
                DifficultySwitcherO.SetActive(true);
                difficultyChanedIntIndicator = 0;
            }
        }
        else
        {
            if (AintWorkingBoolean == false) StartCoroutine(AintWorker());

            if (DifficultySwitcherI.activeInHierarchy)
            {
                DifficultySwitcherI.SetActive(false);
                DifficultySwitcherO.SetActive(true);
            }
            else
            {
                DifficultySwitcherI.SetActive(true);
                DifficultySwitcherO.SetActive(false);
            }
        }
        */
    }

    public void leverChange()
    {
        leverRenderer.sprite = leverSprites[(int)leverSlider.value];
    }

    public void leverRelease()
    {
        if (leverSlider.value <= 3)
        {
            StartCoroutine(leverPullUp());
        }
        else
        {
            leverSlider.interactable = false;
            StartCoroutine(leverPullUp());
            if (activeScene.name == "MainMenu") slotsAnim.Play("slotSpinStart");
            StartCoroutine(WaitAndLoadScene());
            am.playAudio(leverSound);
        }
    }

    public IEnumerator leverPullUp()
    {
        while (leverSlider.value > 0)
        {
            leverSlider.value--;
            leverRenderer.sprite = leverSprites[(int)leverSlider.value];
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator exitSettings()
    {
        settingsOn = false;
        SettinsPanel1.GetComponent<Animator>().Play("settingsRollOut");
        settingsGO.GetComponent<Animator>().Play("creditsRollOut");
        yield return new WaitForSeconds(0.4f);
        settingsGO.SetActive(false);
    }

    private IEnumerator exitThawPanel()
    {
        ThawPanelOn = false;
        ThawPanel1.GetComponent<Animator>().Play("settingsRollOut");
        ThawPanel.GetComponent<Animator>().Play("creditsRollOut");
        yield return new WaitForSeconds(0.4f);
        ThawPanel.SetActive(false);
    }


    public void setMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void setSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void loadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");

        setMusicVolume();
    }
    public void loadSFXVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        setSFXVolume();
    }

    public void ThawPanelActivator()
    {
        ThawPanel.SetActive(true);
    }

    public void credits()
    {
        creditsOn = true;
        creditsButtonAnim.Play("creditsButtonPress");
        am.playAudio(buttonClick);
        creditsGO.SetActive(true);
        creditsGO.GetComponent<Animator>().Play("creditsRollIn");
    }

    private IEnumerator exitCredits()
    {
        creditsOn = false;
        creditsGO.GetComponent<Animator>().Play("creditsRollOut");
        yield return new WaitForSeconds(0.4f);
        creditsGO.SetActive(false);
    }

    public void exitGame()
    {
        am.playAudio(buttonClick);
        Application.Quit();
    }

    public void bossSelection()
    {
        currentIndex = 0;
        bossSelectionOn = true;
        bossSelectionGO.SetActive(true);
        bossSelectionGO.GetComponent<Animator>().Play("bossSelectionRollIn");
    }
    private IEnumerator exitBossSelection()
    {
        bossSelectionGO.GetComponent<Animator>().Play("bossSelectionRollOut");
        yield return new WaitForSeconds(0.4f);
        bossSelectionGO.SetActive(false);
        bossSelectionOn = false;
    }

    public void diceBoss()
    {
        StartCoroutine(diceBossCRT());
    }
    public void plinkoBoss()
    {
        StartCoroutine(plinkoBossCRT());
    }
    public void slotBoss()
    {
        StartCoroutine(slotBossCRT());
    }

    public IEnumerator diceBossCRT()
    {
        FadeOutAnim.gameObject.SetActive(true);
        FadeOutAnim.GetComponent<Animator>().Play("sceneFadeOut");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }

    public IEnumerator plinkoBossCRT()
    {
        FadeOutAnim.gameObject.SetActive(true);
        FadeOutAnim.GetComponent<Animator>().Play("sceneFadeOut");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(3);
    }

    public IEnumerator slotBossCRT()
    {
        FadeOutAnim.gameObject.SetActive(true);
        FadeOutAnim.GetComponent<Animator>().Play("sceneFadeOut");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(4);
    }

    IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(1.5f);
        am.playAudio(jackpotSound);
        yield return new WaitForSeconds(0.5f);
        FadeOutAnim.gameObject.SetActive(true);
        FadeOutAnim.GetComponent<Animator>().Play("sceneFadeOut");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(4);
        /*
        if (difficultychanged == false)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(6);
        }
        */
    }

    public IEnumerator AintWorker()
    {
        AintWorkingBoolean = true;
        AintWorkingPanel.SetActive(true);
        yield return new WaitForSeconds(3);
        AintWorkingPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        AintWorkingBoolean = false;
    }
}
