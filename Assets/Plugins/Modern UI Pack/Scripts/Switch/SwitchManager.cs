using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Michsky.MUIP
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Button))]
    public class SwitchManager : MonoBehaviour, IPointerEnterHandler
    {
        // Events
        [SerializeField] public SwitchEvent onValueChanged = new SwitchEvent();
        public UnityEvent OnEvents = new UnityEvent();
        public UnityEvent OffEvents = new UnityEvent();

        // Saving
        public bool saveValue = true;
        public string switchTag = "Switch";

        // Settings
        public bool isOn = true;
        public bool invokeAtStart = true;
        public bool enableSwitchSounds = false;
        public bool useHoverSound = true;
        public bool useClickSound = true;

        public Animator SwitchAnimator
        {
            get
            {
                if (switchAnimator == null)
                {
                    switchAnimator = GetComponent<Animator>();
                }

                return switchAnimator;
            }
        }

        // Resources
        public Animator switchAnimator;
        public Button switchButton;
        public AudioSource soundSource;

        // Audio
        public AudioClip hoverSound;
        public AudioClip clickSound;

        [System.Serializable]
        public class SwitchEvent : UnityEvent<bool> { }

        bool isInitialized = false;

        void Awake()
        {
            if (switchAnimator == null) { switchAnimator = gameObject.GetComponent<Animator>(); }
            if (switchButton == null)
            {
                switchButton = gameObject.GetComponent<Button>();
                switchButton.onClick.AddListener(AnimateSwitch);

                if (enableSwitchSounds && useClickSound)
                {
                    switchButton.onClick.AddListener(delegate
                    {
                        soundSource.PlayOneShot(clickSound);
                    });
                }
            }

            if (saveValue) { GetSavedData(); }
            else
            {
                if (gameObject.activeInHierarchy) { StopCoroutine("DisableAnimator"); }
                if (gameObject.activeInHierarchy) { StartCoroutine("DisableAnimator"); }

                SwitchAnimator.enabled = true;

                if (isOn) { SwitchAnimator.Play("On Instant"); }
                else { SwitchAnimator.Play("Off Instant"); }
            }

            if (invokeAtStart && isOn ) { OnEvents.Invoke(); }
            else if (invokeAtStart && !isOn) { OffEvents.Invoke(); }

            isInitialized = true;
        }

        void OnEnable() 
        {
            if (isInitialized) 
            {
                UpdateUI(); 
            } 
        }

        void GetSavedData()
        {
            if (gameObject.activeInHierarchy) 
            {
                StopCoroutine("DisableAnimator");
                StartCoroutine("DisableAnimator");
            }

            SwitchAnimator.enabled = true;

            if (PlayerPrefs.GetString(switchTag + "Switch") == "" || !PlayerPrefs.HasKey(switchTag + "Switch"))
            {
                if (isOn) { SwitchAnimator.Play("Switch On"); PlayerPrefs.SetString(switchTag + "Switch", "true"); }
                else { SwitchAnimator.Play("Switch Off"); PlayerPrefs.SetString(switchTag + "Switch", "false"); }
            }
            else if (PlayerPrefs.GetString(switchTag + "Switch") == "true") { SwitchAnimator.Play("Switch On"); isOn = true; }
            else if (PlayerPrefs.GetString(switchTag + "Switch") == "false") { SwitchAnimator.Play("Switch Off"); isOn = false; }
        }

        public void AnimateSwitch()
        {
            if (gameObject.activeInHierarchy)
            {
                StopCoroutine("DisableAnimator");
                StartCoroutine("DisableAnimator");
            }

            SwitchAnimator.enabled = true;

            if (isOn)
            {
                isOn = false;
                SwitchAnimator.Play("Switch Off");
                OffEvents.Invoke();

                if (saveValue) { PlayerPrefs.SetString(switchTag + "Switch", "false"); }
            }

            else
            {
                isOn = true;
                SwitchAnimator.Play("Switch On");
                OnEvents.Invoke();

                if (saveValue) { PlayerPrefs.SetString(switchTag + "Switch", "true"); }
            }

            onValueChanged.Invoke(isOn);
        }

        public void Set(bool setOn, bool notify)
        {
            if (setOn)
            {
                SetOn(notify);
            }
            else
            {
                SetOff(notify);
            }
        }

        public void SetOn(bool notify)
        {
            if (saveValue) { PlayerPrefs.SetString(switchTag + "Switch", "true"); }
            if (gameObject.activeInHierarchy)
            {
                StopCoroutine("DisableAnimator");
                StartCoroutine("DisableAnimator");
            }

            isOn = true;

            SwitchAnimator.enabled = true;
            SwitchAnimator.Play("Switch On");

            if (notify)
            {
                OnEvents.Invoke();
                onValueChanged.Invoke(true);   
            }
        }

        public void SetOff(bool notify)
        {
            if (saveValue) { PlayerPrefs.SetString(switchTag + "Switch", "false"); }
            if (gameObject.activeInHierarchy)
            {
                StopCoroutine("DisableAnimator");
                StartCoroutine("DisableAnimator");
            }

            isOn = false;

            SwitchAnimator.enabled = true;
            SwitchAnimator.Play("Switch Off");

            if (notify)
            {
                OffEvents.Invoke();
                onValueChanged.Invoke(false);   
            }
        }

        public void UpdateUI()
        {
            if (gameObject.activeInHierarchy)
            {
                StopCoroutine("DisableAnimator");
                StartCoroutine("DisableAnimator");
            }

            SwitchAnimator.enabled = true;

            if (isOn && SwitchAnimator.gameObject.activeInHierarchy) { SwitchAnimator.Play("On Instant"); }
            else if (!isOn && SwitchAnimator.gameObject.activeInHierarchy) { SwitchAnimator.Play("Off Instant"); }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableSwitchSounds && useHoverSound && switchButton.interactable)
            {
                soundSource.PlayOneShot(hoverSound);
            }
        }

        IEnumerator DisableAnimator()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            SwitchAnimator.enabled = false;
        }
    }
}