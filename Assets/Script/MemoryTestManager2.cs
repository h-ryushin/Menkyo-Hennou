using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class MemoryTestmanager2 : MonoBehaviour
{
    public GameObject[] questionPanelArray; 
    public Button nextButton;                 
    public InputField[] inputFields; 
    // public Text resultText; 

    int currentPanelIndex = 0;
    private int correctCount = 0;
    
    private string[] correctAnswers = new string[5]; 
    private const int QUESTION_COUNT = 5;
    
    void Start()
    {
        SetCorrectAnswers(); 
        
        questionPanelArray[0].SetActive(true);
        for (int i = 1; i < questionPanelArray.Length; i++)
        {
            questionPanelArray[i].SetActive(false);
        }
        
        UpdateLayout();
        // resultText.gameObject.SetActive(false); 
        
        Debug.Log($"【設定された正解】 年:{correctAnswers[0]}, 月:{correctAnswers[1]}, 日:{correctAnswers[2]}, 曜日:{correctAnswers[3]}, 時分:{correctAnswers[4]}");
    }
    
    void SetCorrectAnswers()
    {
        DateTime now = DateTime.Now;
        
        string dayOfWeekJapanese;
        switch (now.DayOfWeek)
        {
            case DayOfWeek.Monday: dayOfWeekJapanese = "月曜日"; break;
            case DayOfWeek.Tuesday: dayOfWeekJapanese = "火曜日"; break;
            case DayOfWeek.Wednesday: dayOfWeekJapanese = "水曜日"; break;
            case DayOfWeek.Thursday: dayOfWeekJapanese = "木曜日"; break;
            case DayOfWeek.Friday: dayOfWeekJapanese = "金曜日"; break;
            case DayOfWeek.Saturday: dayOfWeekJapanese = "土曜日"; break;
            case DayOfWeek.Sunday: dayOfWeekJapanese = "日曜日"; break;
            default: dayOfWeekJapanese = ""; break;
        }

        correctAnswers[0] = now.ToString("yyyy");         
        correctAnswers[1] = now.ToString("MM");           
        correctAnswers[2] = now.ToString("dd");           
        correctAnswers[3] = dayOfWeekJapanese;            
        correctAnswers[4] = now.ToString("HH:mm");        
    }
    
    public void GoNextPanel() 
    {
        if (currentPanelIndex > 0 && currentPanelIndex <= QUESTION_COUNT)
        {
            CheckAnswer(currentPanelIndex - 1); 
        }

        questionPanelArray[currentPanelIndex].SetActive(false);

        currentPanelIndex++; 
        
        if (currentPanelIndex > QUESTION_COUNT)
        {
            questionPanelArray[QUESTION_COUNT + 1].SetActive(true); 
            UpdateLayout();
            return;
        }

        questionPanelArray[currentPanelIndex].SetActive(true);
        
        UpdateLayout();
    }
    
    void UpdateLayout()
    {
        if (currentPanelIndex > QUESTION_COUNT)
        {
            if(nextButton != null) nextButton.gameObject.SetActive(false); 
            // resultText.gameObject.SetActive(false); 
            
            Debug.Log($"【最終結果】正解数：{correctCount} / {QUESTION_COUNT}");
        }
        else 
        {
            if(nextButton != null) nextButton.gameObject.SetActive(true); 
            // resultText.gameObject.SetActive(false); 
        }
    }

    private void CheckAnswer(int index)
    {
        string userInput = inputFields[index].text;
        string normalizedInput = NormalizeInput(userInput);
        string correctAnswer = correctAnswers[index];
        
        bool isCorrect = false;

        if (index == 4) 
        {
            isCorrect = CheckTimeRange(normalizedInput);
        }
        else
        {
            if (index == 3) 
            {
                normalizedInput = GetFullDayOfWeek(normalizedInput);
            }
            isCorrect = (normalizedInput == correctAnswer);
        }

        if (isCorrect)
        {
            correctCount++;
        }
        
        Debug.Log($"質問 {index + 1}: 入力='{userInput}' (正規化='{normalizedInput}') | 正解='{correctAnswer}' | 結果: {isCorrect}");
        Debug.Log($"現在の正解数: {correctCount}");
    }
    
    private bool CheckTimeRange(string input)
    {
        if (DateTime.TryParseExact(input, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime inputTime))
        {
            DateTime now = DateTime.Now;
            long inputMinutes = inputTime.Hour * 60 + inputTime.Minute;
            long correctMinutes = now.Hour * 60 + now.Minute;

            if (inputMinutes >= correctMinutes - 60 && inputMinutes <= correctMinutes + 60)
            {
                return true;
            }
        }
        return false;
    }
    
    private string NormalizeInput(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        string normalized = input.Replace("０", "0").Replace("１", "1").Replace("２", "2")
                                 .Replace("３", "3").Replace("４", "4")
                                 .Replace("５", "5").Replace("６", "6")
                                 .Replace("７", "7").Replace("８", "8")
                                 .Replace("９", "9")
                                 .Replace("：", ":")
                                 .Replace("　", " ");
        return normalized.Trim().ToLower();
    }
    
    private string GetFullDayOfWeek(string day)
    {
        string d = day.ToLower().Trim();
        if (d.Contains("月")) return "月曜日";
        if (d.Contains("火")) return "火曜日";
        if (d.Contains("水")) return "水曜日";
        if (d.Contains("木")) return "木曜日";
        if (d.Contains("金")) return "金曜日";
        if (d.Contains("土")) return "土曜日";
        if (d.Contains("日")) return "日曜日";
        return day;
    }
}