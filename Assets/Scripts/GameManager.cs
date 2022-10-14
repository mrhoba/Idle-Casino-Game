using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour
{
    #region Variables
    public Text dollarText;
    public Text dollarTextUpgradePage;
    public Text dollarPerSecText;
    public Text dollarPerSecTextUpgradePage;
    public Text dollarPerPersonBJText;
    public Text UpgradeSpeedBJText;

    public double dollar;
    public double dollarPerPersonBJ;
    public double UpgradeSpeedBJLevel;
    public double UpgradeSpeedBJPower;
    public double timeBonusPerClick;
    public double entryMoney;

    public int timeUntilMoneyBJ;
    public float timeCountDownBJ;

    public int timeUntilNextPerson;
    public float timeEntryMoney;

    public ArrayList persons = new ArrayList();

    //public Image timeUntilMoneyBarBJ;
    #endregion
    #region Start/Load/Save
    public void Start()
    {
        Application.targetFrameRate = 60;
        Load();
    }

    public void Load()
    {
        dollar = double.Parse(PlayerPrefs.GetString("dollar", "0"));
        dollarPerPersonBJ = double.Parse(PlayerPrefs.GetString("dollarPerPersonBJ", "10"));
        entryMoney = double.Parse(PlayerPrefs.GetString("entryMoney", "10"));
        timeBonusPerClick = double.Parse(PlayerPrefs.GetString("timeBonusPerClick", "1"));
        timeUntilMoneyBJ = int.Parse(PlayerPrefs.GetString("timeUntilMoneyBJ", "30"));
        timeUntilNextPerson = int.Parse(PlayerPrefs.GetString("timeUntilNextPerson", "30"));
        UpgradeSpeedBJLevel = double.Parse(PlayerPrefs.GetString("UpgradeSpeedBJLevel", "0"));
        UpgradeSpeedBJPower = double.Parse(PlayerPrefs.GetString("UpgradeSpeedBJPower", "1"));
    }

    public void Save()
    {
        PlayerPrefs.SetString("dollar", dollar.ToString());
        PlayerPrefs.SetString("dollarPerPersonBJ", dollarPerPersonBJ.ToString());
        PlayerPrefs.SetString("entryMoney", entryMoney.ToString());
        PlayerPrefs.SetString("timeBonusPerClick", timeBonusPerClick.ToString());
        PlayerPrefs.SetString("timeUntilMoneyBJ", timeUntilMoneyBJ.ToString());
        PlayerPrefs.SetString("timeUntilNextPerson", timeUntilNextPerson.ToString());
        PlayerPrefs.SetString("UpgradeSpeedBJLevel", UpgradeSpeedBJLevel.ToString());
        PlayerPrefs.SetString("UpgradeSpeedBJPower", UpgradeSpeedBJPower.ToString());
    }
    #endregion
    #region Update
    public void Update()
    {
        //Upgrade Cost
        var UpgradeSpeedBJCost = 10 * System.Math.Pow(1.07, UpgradeSpeedBJLevel);
        //Dollar Per Second Equasion
        double dollarPerSecond = (dollarPerPersonBJ / timeUntilMoneyBJ) + (entryMoney / timeUntilNextPerson);
        //Normal Text Outputs    
        dollarText.text = TextFormat(dollar, "F0") + " $";
        dollarTextUpgradePage.text = dollarText.text;
        dollarPerSecText.text = TextFormat(dollarPerSecond, "F2") + " $/Sec Idle";
        dollarPerSecTextUpgradePage.text = dollarPerSecText.text;
        dollarPerPersonBJText.text = TextFormat(dollarPerPersonBJ, "F2") + " $";
        UpgradeSpeedBJText.text = "Blackjack Table Speed Upgrade\n-" + UpgradeSpeedBJPower + " Sec Max Time\nCost: " + TextFormat(UpgradeSpeedBJCost, "F2") + " $";
        //Blackjack dollar addition
        BlackJack();
        //Entry Gate Method
        EntryMoney();
        Save();
    }
    #endregion
    #region Methods
    public string TextFormat(double x, string y)
    {
        if (x > 1000)
        {
            var exponent = System.Math.Floor(System.Math.Log10(System.Math.Abs(x)));
            var mantissa = x / System.Math.Pow(10, exponent);
            return mantissa.ToString("F2") + "e" + exponent;
        }
        return x.ToString(y);
    }

    public void EntryMoney()
    {
        timeEntryMoney += Time.deltaTime;
        if (timeEntryMoney >= timeUntilNextPerson)
        {
            dollar += 10;
            persons.Add(true);
            timeEntryMoney = 0;
        }
    }
    bool bj = false;
    public void BlackJack()
    {
        if (!bj /*&& personEntered==true */&& persons.Contains(true))
        {
            timeCountDownBJ = timeUntilMoneyBJ;
            bj = true;
        }
        if (timeCountDownBJ >= 0/* && personEntered == true*/ && persons.Contains(true))
        {
            timeCountDownBJ -= Time.deltaTime;
        }
        else if(/*personEntered == true && */persons.Contains(true))
        {
            dollar += dollarPerPersonBJ;
            timeCountDownBJ = timeUntilMoneyBJ;
            persons.Remove(true);
            //personEntered = false;
        }
    }

    //Buttons
    public void Click()
    {
        if (timeCountDownBJ - 1 != -1 && persons.Contains(true))
        {
            timeCountDownBJ--;
        }
    }
    int clicked = 0;
    public void MenuButton(GameObject gameObj)
    {
        clicked++;

        if (clicked == 1)
        {
            gameObj.SetActive(true);
        }
        else
        {
            gameObj.SetActive(false);
            clicked = 0;
        }
    }

    public void Upgrade(string upgradeID)
    {
        switch (upgradeID)
        {
            case "BJSpeed":
                var cost1 = 10 * System.Math.Pow(1.07, UpgradeSpeedBJLevel);
                if (dollar >= cost1)
                {
                    if (timeUntilMoneyBJ - 1 != 0)
                    {
                        UpgradeSpeedBJLevel++;
                        dollar -= cost1;
                        timeUntilMoneyBJ--;
                    }
                }
                break;
            default:
                Debug.Log("I'm not assigned to a proper upgrade!");
                break;
        }
    }
        #endregion
        /*public Text dollarText;
        public Text dollarPerClickText;
        public Text dollarPerSecText;
        public Text clickUpgrade1Text;
        public Text clickUpgrade2Text;
        public Text productionUpgrade1Text;
        public Text productionUpgrade2Text;
        public Text goldText;
        public Text goldBoostText;
        public Text goldToGetText;
        public Text clickUpgrade1MaxText;

        public double dollar;
        public double dollarPerSecond;
        public double dollarPerClick;

        public double gold;
        public double goldBoost;
        public double goldToGet;

        public double clickUpgrade2Cost;
        public int clickUpgrade1Level;
        public int clickUpgrade2Level;
        public double clickUpgrade1Power;
        public double clickUpgrade2Power;

        public double productionUpgrade1Cost;
        public double productionUpgrade2Cost;
        public int productionUpgrade1Level;
        public int productionUpgrade2Level;
        public double productionUpgrade1Power;
        public double productionUpgrade2Power;

        public Image clickUpgradeBar;

        public void Start()
        {
            Application.targetFrameRate = 60;
            Load();
        }

        public void Load()
        {
            dollar = double.Parse(PlayerPrefs.GetString("dollar", "0"));
            dollarPerClick = double.Parse(PlayerPrefs.GetString("dollarPerClick", "1"));

            gold = double.Parse(PlayerPrefs.GetString("gold", "0"));

            clickUpgrade2Cost = double.Parse(PlayerPrefs.GetString("clickUpgrade2Cost", "100"));
            productionUpgrade1Cost = double.Parse(PlayerPrefs.GetString("productionUpgrade1Cost", "25"));
            productionUpgrade2Cost = double.Parse(PlayerPrefs.GetString("productionUpgrade2Cost", "250"));

            clickUpgrade1Power = double.Parse(PlayerPrefs.GetString("clickUpgrade1Power", "1"));
            clickUpgrade2Power = double.Parse(PlayerPrefs.GetString("clickUpgrade2Power", "5"));
            productionUpgrade1Power = double.Parse(PlayerPrefs.GetString("productionUpgrade1Power", "1"));
            productionUpgrade2Power = double.Parse(PlayerPrefs.GetString("productionUpgrade2Power", "5"));

            clickUpgrade1Level = PlayerPrefs.GetInt("clickUpgrade1Level", 0);
            clickUpgrade2Level = PlayerPrefs.GetInt("clickUpgrade2Level", 0);
            productionUpgrade1Level = PlayerPrefs.GetInt("productionUpgrade1Level", 0);
            productionUpgrade2Level = PlayerPrefs.GetInt("productionUpgrade2Level", 0);
        }

        public void Save()
        {
            PlayerPrefs.SetString("dollar", dollar.ToString());
            PlayerPrefs.SetString("dollarPerClick", dollarPerClick.ToString());

            PlayerPrefs.SetString("gold", gold.ToString());

            PlayerPrefs.SetString("clickUpgrade2Cost", clickUpgrade2Cost.ToString());
            PlayerPrefs.SetString("productionUpgrade1Cost", productionUpgrade1Cost.ToString());
            PlayerPrefs.SetString("productionUpgrade2Cost", productionUpgrade2Cost.ToString());

            PlayerPrefs.SetString("clickUpgrade1Power", clickUpgrade1Power.ToString());
            PlayerPrefs.SetString("clickUpgrade2Power", clickUpgrade2Power.ToString());
            PlayerPrefs.SetString("productionUpgrade1Power", productionUpgrade1Power.ToString());
            PlayerPrefs.SetString("productionUpgrade2Power", productionUpgrade2Power.ToString());

            PlayerPrefs.SetInt("clickUpgrade1Level", clickUpgrade1Level);
            PlayerPrefs.SetInt("clickUpgrade2Level", clickUpgrade2Level);
            PlayerPrefs.SetInt("productionUpgrade1Level", productionUpgrade1Level);
            PlayerPrefs.SetInt("productionUpgrade2Level", productionUpgrade2Level);
        }
        public void Update()
        {
            goldToGet = 150 * System.Math.Sqrt(dollar / 1e15) + 1;
            goldBoost = gold * 0.001 + 1;
            goldToGetText.text = "Prestige:\n+" + System.Math.Floor(goldToGet).ToString("F0") + " Gold";
            goldText.text = "Gold: " + System.Math.Floor(gold).ToString("F0");
            goldBoostText.text = goldBoost.ToString("F2") + "x boost";
            dollarPerSecond = ((productionUpgrade1Power * productionUpgrade1Level) + (productionUpgrade2Power * productionUpgrade2Level)) * goldBoost;
            //*********************************************************Exponents for Coins Per Click**********************************************************************
            dollarPerClickText.text = "Click\n" + ExponentMethod(dollarPerClick, "F0") + " $";
            //*********************************************************Exponents for Coins *******************************************************************************
            dollarText.text = ExponentMethod(dollar, "F0") + " $";
            //*********************************************************Exponents for Coins Per Second*********************************************************************
            dollarPerSecText.text = ExponentMethod(dollarPerSecond, "F0") + " $/sec";
            //*********************************************************Exponents for Click Upgrade 1 Cost*****************************************************************
            var clickUpgrade1Cost = 10 * System.Math.Pow(1.07, clickUpgrade1Level);
            string clickUpgrade1CostString = ExponentMethod(clickUpgrade1Cost, "F0");
            //*********************************************************Exponents for Click Upgrade 1 Level****************************************************************
            string clickUpgrade1LevelString = ExponentMethod(clickUpgrade1Level, "F0");
            //*********************************************************Exponents for Click Upgrade 2 Cost*****************************************************************
            string clickUpgrade2CostString = ExponentMethod(clickUpgrade2Cost, "F0");
            //*********************************************************Exponents for Click Upgrade 1 Level****************************************************************
            string clickUpgrade2LevelString = ExponentMethod(clickUpgrade2Level, "F0");
            //*********************************************************Exponents for Production Upgrade 1 Cost*****************************************************************
            string productionUpgrade1CostString = ExponentMethod(productionUpgrade1Cost, "F0");
            //*********************************************************Exponents for Production Upgrade 1 Level****************************************************************
            string productionUpgrade1LevelString = ExponentMethod(productionUpgrade1Level, "F0");
            //*********************************************************Exponents for Production Upgrade 2 Cost*****************************************************************
            string productionUpgrade2CostString = ExponentMethod(productionUpgrade2Cost, "F0");
            //*********************************************************Exponents for Production Upgrade 2 Level****************************************************************
            string productionUpgrade2LevelString = ExponentMethod(productionUpgrade2Level, "F0");

            clickUpgrade1Text.text = "Click Upgrade 1\nCost: " + clickUpgrade1CostString + " $\nPower: +" + clickUpgrade1Power + " Click\nLevel: " + clickUpgrade1LevelString;
            clickUpgrade1MaxText.text = "Buy Max (" + BuyClickUpgrade1MaxCount() + ")";
            clickUpgrade2Text.text = "Click Upgrade 2\nCost: " + clickUpgrade2CostString + " $\nPower: +" + clickUpgrade2Power + " Click\nLevel: " + clickUpgrade2LevelString;
            productionUpgrade1Text.text = "Production Unit 1\nCost: " + productionUpgrade1CostString + " coins\nPower: +" + (productionUpgrade1Power * goldBoost).ToString("F2") + " $/sec\nLevel: " + productionUpgrade1LevelString;
            productionUpgrade2Text.text = "Production Unit 2\nCost: " + productionUpgrade2CostString + " coins\nPower: +" + (productionUpgrade2Power * goldBoost).ToString("F2") + " $/sec\nLevel: " + productionUpgrade2LevelString;
            dollar += dollarPerSecond * Time.deltaTime;
            //Progress Bar
            if (dollar / clickUpgrade1Cost < 0.01)
            {
                clickUpgradeBar.fillAmount = 0;
            }
            else if (dollar / clickUpgrade1Cost > 10)
            {
                clickUpgradeBar.fillAmount = 1;
            }
            else clickUpgradeBar.fillAmount = (float)(dollar / clickUpgrade1Cost);

            Save();
        }

        public string ExponentMethod(double x, string y)
        {
            if (x > 1000)
            {
                var exponent = System.Math.Floor(System.Math.Log10(System.Math.Abs(x)));
                var mantissa = x / System.Math.Pow(10, exponent);
                return mantissa.ToString("F2") + "e" + exponent;

            }
            return x.ToString(y);
        }

        public string ExponentMethod(float x, string y)
        {
            if (x > 1000)
            {
                var exponent = Mathf.Floor(Mathf.Log10(Mathf.Abs(x)));
                var mantissa = x / Mathf.Pow(10, exponent);
                return mantissa.ToString("F2") + "e" + exponent;

            }
            return x.ToString(y);
        }

        //Prestige
        public void Prestige()
        {
            if (dollar > 1000)
            {
                dollar = 0;
                dollarPerClick = 1;
                clickUpgrade2Cost = 100;
                productionUpgrade1Cost = 25;
                productionUpgrade2Cost = 250;
                clickUpgrade1Power = 1;
                clickUpgrade2Power = 5;
                productionUpgrade1Power = 1;
                productionUpgrade2Power = 5;
                clickUpgrade1Level = 0;
                clickUpgrade2Level = 0;
                productionUpgrade1Level = 0;
                productionUpgrade2Level = 0;
                gold += goldToGet;
            }
        }

        //Buttons
        public void Click()
        {
            dollar += dollarPerClick;
        }

        public double BuyClickUpgrade1MaxCount()
        {
            var b = 10;
            var c = dollar;
            var r = 1.07;
            var k = clickUpgrade1Level;
            var n = System.Math.Floor(System.Math.Log(((c * (r - 1)) / (b * System.Math.Pow(r, k))) + 1, r));
            return n;
        }

        public void BuyUpgrade(string upgradeID)
        {
            switch (upgradeID)
            {
                case "C1":
                    var cost1 = 10 * System.Math.Pow(1.07, clickUpgrade1Level);
                    if (dollar >= cost1)
                    {
                        clickUpgrade1Level++;
                        dollar -= cost1;
                        dollarPerClick++;
                    }
                    break;
                case "C1MAX":
                    var b = 10;
                    var c = dollar;
                    var r = 1.07;
                    var k = clickUpgrade1Level;
                    var n = System.Math.Floor(System.Math.Log(((c * (r - 1)) / (b * System.Math.Pow(r, k))) + 1, r));
                    var cost2 = b * ((System.Math.Pow(r, k) * (System.Math.Pow(r, n) - 1)) / (r - 1));
                    if (dollar >= cost2)
                    {
                        clickUpgrade1Level += (int)n;
                        dollar -= cost2;
                        dollarPerClick += n;

                    }
                    break;
                case "C2":
                    if (dollar >= clickUpgrade2Cost)
                    {
                        clickUpgrade2Level++;
                        dollar -= clickUpgrade2Cost;
                        clickUpgrade2Cost *= 1.09;
                        dollarPerClick += 5;
                    }
                    break;
                case "P1":
                    if (dollar >= productionUpgrade1Cost)
                    {
                        productionUpgrade1Level++;
                        dollar -= productionUpgrade1Cost;
                        productionUpgrade1Cost *= 1.07;
                    }
                    break;
                case "P2":
                    if (dollar >= productionUpgrade2Cost)
                    {
                        productionUpgrade2Level++;
                        dollar -= productionUpgrade2Cost;
                        productionUpgrade2Cost *= 1.09;
                    }
                    break;
                default:
                    Debug.Log("I'm not assigned to a proper upgrade!");
                    break;
            }
        }*/

    }
