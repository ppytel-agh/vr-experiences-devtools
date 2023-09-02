using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LectorPanelUI : MonoBehaviour
{
    /*
    [TextArea]
    public String panelText;
    */
    public AudioSource transcription;
    public Slider transcriptionProgress;
    public TextMeshProUGUI transcriptionProgressTimingText;
    public TextMeshProUGUI playPauseButtonText;
    public Scrollbar panelTextVerticalScroll;

    private bool isPlaying;
    private string clipLengthInfo;
    private int previousSecondsValue;


    // Start is called before the first frame update
    void Start()
    {
        isPlaying = false;

        AudioClip clip = transcription.clip;

        //set progress bar initial values
        clipLengthInfo = GetClipLengthInfo(clip);
        int lengthInSeconds = Mathf.FloorToInt(clip.length);

        //progress bar values
        transcriptionProgress.wholeNumbers = true;
        transcriptionProgress.maxValue = lengthInSeconds;
        transcriptionProgress.value = 0;

        string transcriptionProgressInfo = String.Format("0:00 / {0}", clipLengthInfo);
        transcriptionProgressTimingText.SetText(transcriptionProgressInfo);

        previousSecondsValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            int secondsValue = Mathf.FloorToInt(transcription.time);
            if(secondsValue > previousSecondsValue)
            {
                transcriptionProgress.SetValueWithoutNotify(secondsValue);

                String timingInfo = GetLengthInMinutesAndSecondsInfo(secondsValue);

                string transcriptionProgressInfo = String.Format("{0} / {1}", timingInfo, clipLengthInfo);
                transcriptionProgressTimingText.SetText(transcriptionProgressInfo);
                previousSecondsValue = secondsValue;
            } else if(secondsValue == 0 && previousSecondsValue > 0)
            {
                transcription.Stop();
                isPlaying = false;
                playPauseButtonText.SetText("Play");
                transcriptionProgress.value = 0;
                previousSecondsValue = 0;
            }
        }
    }

    String GetLengthInMinutesAndSecondsInfo(int lengthInSeconds)
    {
        int minutes = lengthInSeconds / 60;
        int secondsRest = lengthInSeconds % 60;
        String lengthInfo = String.Format("{0:D}:{1:D2}", minutes, secondsRest);
        return lengthInfo;
    }

    /**
     * TODO: export to some utility library
     */
    String GetClipLengthInfo(AudioClip clip)
    {
        float clipLength = clip.length;
        int lengthInSeconds = Mathf.FloorToInt(clipLength);
        String lengthInfo = GetLengthInMinutesAndSecondsInfo(lengthInSeconds);
        return lengthInfo;
    }

    public void OnPlayPauseButtonClick()
    {
        if (isPlaying)
        {
            playPauseButtonText.SetText("Play");
            transcription.Pause();
            isPlaying = false;
        }
        else
        {
            playPauseButtonText.SetText("Pause");
            transcription.Play();
            isPlaying = true;
        }
    }

    public void OnTranscriptionProgressSliderValueChange()
    {
        float newValue = transcriptionProgress.value;
        int secondsValue = Mathf.FloorToInt(newValue);

        transcription.time = secondsValue;

        String timingInfo = GetLengthInMinutesAndSecondsInfo(secondsValue);

        string transcriptionProgressInfo = String.Format("{0} / {1}", timingInfo, clipLengthInfo);
        transcriptionProgressTimingText.SetText(transcriptionProgressInfo);
    }
}
