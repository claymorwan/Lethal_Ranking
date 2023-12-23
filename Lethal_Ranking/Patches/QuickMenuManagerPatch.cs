using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using TMPro;
using System.Xml.Linq;
using System.Net;

namespace Lethal_Ranking.Patches
{
    [HarmonyPatch(typeof(QuickMenuManager))]
    internal class QuickMenuManagerPatch
    {
        private static GameObject _infoDisplay;
        private static GameObject _xpProgDisplay;
        private static TextMeshProUGUI _xpDisplayText;
        private static TextMeshProUGUI _rankDisplayText;
        [HarmonyPostfix]
        [HarmonyPatch(typeof(QuickMenuManager), "OpenQuickMenu")]

        // check is menucontainer is is active
        private static void QuickMenuXP(QuickMenuManager __instance) 
        {
            if (!__instance.isMenuOpen)
                return;
            if (!_infoDisplay || !_xpProgDisplay)
            {
                MakeNewXPBar();
            }
            _infoDisplay.SetActive(true);
            _xpProgDisplay.SetActive(true);
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(QuickMenuManager), "Update")]
        private static void XPDisplay(QuickMenuManager __instance)
        {
            if (!_infoDisplay || !_xpProgDisplay)
                return;
            // checkin if the the settings menu or if the pause menu is exited, if it does, it won't be showed
            if (__instance.mainButtonsPanel.activeSelf)  
            {
                _infoDisplay.SetActive(true);
                _xpProgDisplay.SetActive(true);
                _rankDisplayText.gameObject.SetActive(true);
            }
            else
            {
                _infoDisplay.SetActive(false);
                _xpProgDisplay.SetActive(false);
                _rankDisplayText.gameObject.SetActive(false);
            }
            /* Yoinking the localPlayerXP (the xp value) from the file it's located in (HUDManager) 
             and turning it into a string */
            _xpDisplayText.text = "EXP: " + HUDManager.Instance.localPlayerXP.ToString();
            int _rankValue = HUDManager.Instance.localPlayerLevel; // yoinking Rank Value and "translating" it (there's probably a better way to do so but i'm dum)
            if (_rankValue == 0)
            {
                _rankDisplayText.text = "Intern";
            }
            else if (_rankValue == 1)
            {
                _rankDisplayText.text = "Part-Timer";
            }
            else if ( _rankValue == 2)
            {
                _rankDisplayText.text = "Employee";
            }
            else if (_rankValue == 3)
            {
                _rankDisplayText.text = "Leader";
            }
            else if (_rankValue == 4)
            {
                _rankDisplayText.text = "Boss";
            }


        }
        public static void MakeNewXPBar()
        {
            GameObject _pauseMenu = GameObject.Find("/Systems/UI/Canvas/QuickMenu");

            int _XPBarScaleValue = HUDManager.Instance.localPlayerXP;
            float _XPBarScale = _XPBarScaleValue * 0.002f;

            if (!_infoDisplay)
            {
                if (UnityEngine.Screen.fullScreen)
                {
                    // Display XP Bar // 
                    GameObject _gameXPBar = GameObject.Find("/Systems/UI/Canvas/EndgameStats/LevelUp/LevelUpBox");
                    _infoDisplay = GameObject.Instantiate(_gameXPBar);
                    _infoDisplay.name = "XPDisplay";
                    _infoDisplay.transform.SetParent(_pauseMenu.transform, false);
                    _infoDisplay.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    _infoDisplay.transform.Translate(-2.05f, 1.05f, 0f);

                    // Display XP Bar Progression //
                    GameObject _gameXPBarProgress = GameObject.Find("/Systems/UI/Canvas/EndgameStats/LevelUp/LevelUpMeter");
                    _xpProgDisplay = GameObject.Instantiate(_gameXPBarProgress);
                    _xpProgDisplay.name = "XPBarProgress";
                    _xpProgDisplay.transform.SetParent(_infoDisplay.transform, false);
                    _xpProgDisplay.transform.localScale = new Vector3(0.597f, 5.21f, 1f);
                    _xpProgDisplay.GetComponent<Image>().fillAmount = _XPBarScale;
                    _xpProgDisplay.transform.Translate(-0.775f, 0.186f, 0f);
                    Vector3 pos = _xpProgDisplay.transform.localPosition;


                    // Display XP //
                    GameObject _gameXPText = GameObject.Find("/Systems/UI/Canvas/EndgameStats/LevelUp/Total");
                    _xpDisplayText = GameObject.Instantiate(_gameXPText).GetComponent<TextMeshProUGUI>();
                    _xpDisplayText.name = "XPText";
                    _xpDisplayText.alignment = TextAlignmentOptions.Center;
                    _xpDisplayText.transform.SetParent(_infoDisplay.transform, false);
                    _xpDisplayText.transform.localScale = new Vector3(1f, 1f, 1f);
                    _xpDisplayText.transform.Translate(-1.06f, 0f, 0f);

                    // Display rank  //
                    GameObject _gameRankText = GameObject.Find("/Systems/UI/Canvas/EndgameStats/LevelUp/Total");
                    _rankDisplayText = GameObject.Instantiate(_gameRankText).GetComponent<TextMeshProUGUI>();
                    _rankDisplayText.name = "RankText";
                    _rankDisplayText.alignment = TextAlignmentOptions.Center;
                    _rankDisplayText.transform.SetParent(_infoDisplay.transform, false);
                    _rankDisplayText.transform.localScale = new Vector3(1f, 1f, 1f);
                    _rankDisplayText.transform.Translate(-1.11f, -0.08f, 0f);
                } // for fullscreen
                else
                {
                    // Display XP Bar // 
                    GameObject _gameXPBar = GameObject.Find("/Systems/UI/Canvas/EndgameStats/LevelUp/LevelUpBox");
                    _infoDisplay = GameObject.Instantiate(_gameXPBar);
                    _infoDisplay.name = "XPDisplay";
                    _infoDisplay.transform.SetParent(_pauseMenu.transform, false);
                    _infoDisplay.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    _infoDisplay.transform.Translate(-2.30f, 1f, 0f);

                    // Display XP Bar Progression //
                    GameObject _gameXPBarProgress = GameObject.Find("/Systems/UI/Canvas/EndgameStats/LevelUp/LevelUpMeter");
                    _xpProgDisplay = GameObject.Instantiate(_gameXPBarProgress);
                    _xpProgDisplay.name = "XPBarProgress";
                    _xpProgDisplay.transform.SetParent(_infoDisplay.transform, false);
                    _xpProgDisplay.transform.localScale = new Vector3(0.597f, 5.21f, 1f);
                    _xpProgDisplay.GetComponent<Image>().fillAmount = _XPBarScale;
                    _xpProgDisplay.transform.Translate(-0.845f, 0.205f, 0f);
                    Vector3 pos = _xpProgDisplay.transform.localPosition;


                    // Display XP //
                    GameObject _gameXPText = GameObject.Find("/Systems/UI/Canvas/EndgameStats/LevelUp/Total");
                    _xpDisplayText = GameObject.Instantiate(_gameXPText).GetComponent<TextMeshProUGUI>();
                    _xpDisplayText.name = "XPText";
                    _xpDisplayText.alignment = TextAlignmentOptions.Center;
                    _xpDisplayText.transform.SetParent(_infoDisplay.transform, false);
                    _xpDisplayText.transform.localScale = new Vector3(1f, 1f, 1f);
                    _xpDisplayText.transform.Translate(-1.15f, 0f, 0f);

                    // Display rank  //
                    GameObject _gameRankText = GameObject.Find("/Systems/UI/Canvas/EndgameStats/LevelUp/Total");
                    _rankDisplayText = GameObject.Instantiate(_gameRankText).GetComponent<TextMeshProUGUI>();
                    _rankDisplayText.name = "RankText";
                    _rankDisplayText.alignment = TextAlignmentOptions.Center;
                    _rankDisplayText.transform.SetParent(_infoDisplay.transform, false);
                    _rankDisplayText.transform.localScale = new Vector3(1f, 1f, 1f);
                    _rankDisplayText.transform.Translate(-1.2f, -0.08f, 0f);
                } // for windowed
            }
        }
    }
}
