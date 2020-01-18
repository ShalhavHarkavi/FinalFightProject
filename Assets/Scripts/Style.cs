using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Style : MonoBehaviour
{
    
  [SerializeField] float styleMeter; //Doesn't need to be serialized
  [SerializeField] int[] styleMaxArray = {1000, 2500, 6000, 10000, 20000}; //maybe add s->ss and ss->sss
  [SerializeField] int styleMaxInd;
  [SerializeField] int attackCounter; //temp; need to set seperate counters for each kind of attack
  [SerializeField] int prevAttackCounter; //temp; need to set seperate counters for each kind of attack
  [SerializeField] int styleHitBonus = 100; //maybe different bonuses for each kind of attack?
  [SerializeField] float styleHitBonusModifier = 1f;
  bool styleActive;


  //Visual Meter - for now text, later meter
  Text styleDisplayer;
  string currentStyleLetter; //Temp until proper meter


  void Start()
  {
    styleMeter = 0f;
    styleMaxInd = 0;
    attackCounter = 0;
    prevAttackCounter = 0;
    styleActive = false;


    styleDisplayer = GameObject.Find("Canvas").GetComponentInChildren<Text>();
    currentStyleLetter = "D";
  }

  void Update()
  {
    if (attackCounter > 1 && styleMaxInd == 0)
    {
      styleActive = true;
      styleMeter = styleMaxArray[styleMaxInd] - 1;
      prevAttackCounter = attackCounter;
    }
    if (styleActive)
    {
      styleMeter--;
      if (attackCounter > prevAttackCounter)
      {
        prevAttackCounter = attackCounter;
        styleMeter += (styleHitBonus * styleHitBonusModifier);
      }
    }
    if (styleMeter >= styleMaxArray[styleMaxInd])
    {
      //need to account for a case in which a hit perfectly brings meter to max and does not go over it, or if the difference is too small
      if (styleMaxInd < styleMaxArray.Length - 1)
      {
        styleMeter -= styleMaxArray[styleMaxInd];
        styleMaxInd++;


        switch(styleMaxInd)
        {
          case 1:
            currentStyleLetter = "C";
            break;
          case 2:
            currentStyleLetter = "B";
            break;
          case 3:
            currentStyleLetter = "A";
            break;
          case 4:
            currentStyleLetter = "S";
            break;
        }
      }
      else
        styleMeter = styleMaxArray[styleMaxInd];
    }
    if (styleMeter <= 0)
    {
      styleActive = false;
      styleMaxInd = 0;
      styleMeter = 0f;
      attackCounter = 0;
    }


    styleDisplayer.text = currentStyleLetter + " - " + styleMeter.ToString();
  }
  public void AddToAttackCounter() { attackCounter++; }
}
