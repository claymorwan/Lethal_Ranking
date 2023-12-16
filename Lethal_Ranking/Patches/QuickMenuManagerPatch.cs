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

namespace Lethal_Ranking.Patches
{
    [HarmonyPatch(typeof(QuickMenuManager))]
    internal class QuickMenuManagerPatch
    {
        private static GameObject _xpDisplay;
        private static GameObject _xpProgDisplay;
        private static TextMeshProUGUI _xpDisplayText;
        [HarmonyPostfix]
        [HarmonyPatch(typeof(QuickMenuManager), "OpenQuickMenu")]

        // check is menucontainer is is active
        private static void QuickMenuXP(QuickMenuManager __instance) 
        {
            if (!__instance.isMenuOpen)
                return;
            if (!_xpDisplay || !_xpProgDisplay)
            {
                MakeNewXPBar();
            }
            _xpDisplay.SetActive(true);
            _xpProgDisplay.SetActive(true);
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(QuickMenuManager), "Update")]
        private static void XPDisplay(QuickMenuManager __instance)
        {
            if (!_xpDisplay || !_xpProgDisplay)
                return;
            // checkin if the the settings menu or if the pause menu is exited, if it does, it won't be showed
            if (__instance.mainButtonsPanel.activeSelf)  
            {
                _xpDisplay.SetActive(true);
                _xpProgDisplay.SetActive(true);
            }
            else
            {
                _xpDisplay.SetActive(false);
                _xpProgDisplay.SetActive(false);
            }
            /* Yoinking the localPlayerXP (the xp value) from the file it's located in (HUDManager) 
             and turning it into a string */
            _xpDisplayText.text = "XP: " + HUDManager.Instance.localPlayerXP.ToString(); 
        }
        public static void MakeNewXPBar()
        {
            GameObject _pauseMenu = GameObject.Find("/Systems/UI/Canvas/QuickMenu");
            if (!_xpDisplay)
            {
                // Display XP Bar // 
                GameObject _gameXPBar = GameObject.Find("/Systems/UI/Canvas/EndgameStats/LevelUp/LevelUpBox");
                _xpDisplay = GameObject.Instantiate(_gameXPBar);
                _xpDisplay.name = "XPDisplay";
                _xpDisplay.transform.SetParent(_pauseMenu.transform, false);
                _xpDisplay.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                _xpDisplay.transform.Translate(-2f, 1f, 0f);

                // Display XP Bar Progression //
                GameObject _gameXPBarProgress = GameObject.Find("/Systems/UI/Canvas/EndgameStats/LevelUp/LevelUpMeter");
                _xpProgDisplay = GameObject.Instantiate(_gameXPBarProgress);
                _xpProgDisplay.name = "XPBarProgress";
                _xpProgDisplay.transform.SetParent(_xpDisplay.transform, false);
                _xpProgDisplay.GetComponent<Image>().fillAmount = 0f;
                _xpProgDisplay.transform.localScale = new Vector3(0.597f, 5.21f, 1f);
                _xpProgDisplay.transform.Translate(-0.8f, 0.2f, 0f);
                Vector3 pos = _xpProgDisplay.transform.localPosition;
                _xpProgDisplay.transform.localPosition = new Vector3(pos.x + 7, pos.y - 3.5f, 0f);


                // Display XP //
                GameObject _gameXPText = GameObject.Find("/Systems/UI/Canvas/EndgameStats/LevelUp/Total");
                _xpDisplayText = GameObject.Instantiate(_gameXPText).GetComponent<TextMeshProUGUI>();
                _xpDisplayText.name = "XPText";
                _xpDisplayText.alignment = TextAlignmentOptions.Center;
                _xpDisplayText.SetText("0/1000");
                _xpDisplayText.transform.SetParent(_xpDisplay.transform, false);
            }
        }
    }
}
