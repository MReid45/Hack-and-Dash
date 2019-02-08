using UnityEngine;
using UnityEngine.SceneManagement;

    public class FallTrigger : MonoBehaviour {

    private Scene scene;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collision");
            SceneManager.LoadScene(scene.name);
        }

    }
}
