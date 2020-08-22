using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{

    /// <summary>
    /// ゲームの終了
    /// </summary>
    public void GameExit()
    {
        Application.Quit();
    }

    /// <summary>
    /// ゲームシーンの読み込み
    /// </summary>
    public void LoadGameScene()
    {
        //ロード後のイベント登録
        SceneManager.sceneLoaded += GameSceneLoaded;

        //ゲームシーンのロード(シーン名は仮置き)
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// シーン読み込み後に何か処理させるための関数
    /// </summary>
    /// <param name="nextscene"></param>
    /// <param name="mode"></param>
    public void GameSceneLoaded(Scene nextscene,LoadSceneMode mode)
    {
        //何かしたければここに書く

        //ロード後イベントからこの関数の削除
        SceneManager.sceneLoaded -= GameSceneLoaded;
    }
}
