using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    [SerializeField] private float _currentTime = 0f;
    [SerializeField] private bool startOnAwake = false;
    private float _stopTime = 0.01f;

    private WaitForSeconds _timer;
    private Coroutine _stopWatchingCoroutine;
    private TMP_Text _textStopwatch;

    private void Awake()
    {
        _timer = new WaitForSeconds(_stopTime);
    }

    void Start()
    {

        _timer = new WaitForSeconds(_stopTime);
        _textStopwatch = GameObject.Find("textTimer").GetComponent<TMP_Text>();
        if (startOnAwake) StartTimer();
    }

    public float GetCurrentTime() => _currentTime;

    public void StartTimer()
    {
        _currentTime = 0f;
        _stopWatchingCoroutine = StartCoroutine(StopWatching());
    }

    public void ClearTimer()
    {
        _currentTime = 0f;
    }

    public void StopTimer()
    {
        StopCoroutine(_stopWatchingCoroutine);
    }

    IEnumerator StopWatching()
    {
        for (; ; )
        {
            _currentTime = MathF.Round(_currentTime + _stopTime, 2);
            string[] timeString = _currentTime.ToString().Split(',');
            try
            {
                _textStopwatch.text = $"{timeString[0]}:{timeString[1]}";
            }
            catch (IndexOutOfRangeException e)
            {
                _textStopwatch.text = $"{_currentTime}:00";
            }

            yield return _timer;
        }
    }
}
