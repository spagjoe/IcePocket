using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject helpPanel;
    public GameObject settingsPanel;
    public static float masterVolume, musicVolume, sfxVolume;

    public void startGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void exitGame() {
        Application.Quit();
    }

    public void toggleHelp() {
        if (helpPanel.activeInHierarchy) {
            closeHelp();
        }
        else {
            openHelp();
        }
    }

    public void toggleSettings() {
        if (settingsPanel.activeInHierarchy) {
            closeSettings();
        }
        else {
            openSettings();
        }
    }

    private void openHelp() {
        helpPanel.SetActive(true);
    }

    private void closeHelp() {
        helpPanel.SetActive(false);
    }

    private void openSettings() {
        settingsPanel.SetActive(true);
    }

    private void closeSettings() {
        settingsPanel.SetActive(false);
    }
}
