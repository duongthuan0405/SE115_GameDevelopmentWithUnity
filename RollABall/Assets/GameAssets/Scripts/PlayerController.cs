using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float forceMove = 10f;
    [SerializeField] float forceJump = 3f;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject notificationText;
    [SerializeField] GameObject listPickUp;
    [SerializeField] AudioClip collectionSound;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] int totalSecond = 10;
    [SerializeField] AudioClip colidWithEnemy;
    [SerializeField] AudioClip victorySound;
    [SerializeField] AudioClip gameOverSound;

    private int jumpTimes = 0;
    private float normalForceMove;
    private int score = 0;
    private int winScore;
    private System.Timers.Timer timer;

    private bool isGameFinished;

    private Rigidbody rb;
    private Vector2 movementInput;
    void Start()
    {
        isGameFinished = false;
        rb = this.gameObject.GetComponent<Rigidbody>();
        normalForceMove = forceMove;
        score = 0;
        SetScore();
        winScore = listPickUp.transform.childCount;
        SetScore();
        notificationText.SetActive(false);
        StartCoroutine(CountDownCoroutine());
    }

    private IEnumerator CountDownCoroutine()
    {
        int remainTime = totalSecond;
        TimeSpan timeSpan;

        while (!isGameFinished)
        {
            timeSpan = TimeSpan.FromSeconds(remainTime);
            timeText.text = timeSpan.ToString(@"mm\:ss");

            if (remainTime <= 0)
                break;

            yield return new WaitForSeconds(1f);
            remainTime--;
        }

        if (score < winScore && !isGameFinished)
        {
            isGameFinished = true;
            OnLose();
        }
    }

    private async Task OnLose()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;

        await Task.Delay(1000);

        notificationText.GetComponentInChildren<TextMeshProUGUI>().text = "You Lose";
        AudioSource.PlayClipAtPoint(gameOverSound, transform.position, 1f);
        notificationText.SetActive(true);
        Destroy(gameObject); 
    }

    private void FixedUpdate()
    {

        Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);
        rb.AddForce(move * forceMove);
    }
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnJump(InputValue jumpValue)
    {
        jumpTimes++;
        if (jumpTimes <= 2)
        {
            rb.AddForce(Vector3.up * forceJump, ForceMode.Impulse);
        }
    }

    async Task OnBoost(InputValue boostValue)
    {
        if (forceMove != normalForceMove) return;
        forceMove *= 2f;
        await Task.Delay(1000);
        forceMove = normalForceMove;       
    }

    private async void OnCollisionEnter(Collision collision)
    {
        jumpTimes = 0;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
            isGameFinished = true;
            AudioSource.PlayClipAtPoint(colidWithEnemy, transform.position, 1f);
            await OnLose();
        }
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp") && !isGameFinished)
        {
            other.gameObject.SetActive(false);
            score++;
            if(collectionSound)
            {
                AudioSource.PlayClipAtPoint(collectionSound, transform.position, 1f);
            }
            SetScore();
            await OnWin();
        } 
           
    }

    private async Task OnWin()
    {

        if (score >= winScore)
        {
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            isGameFinished = true;

            notificationText.GetComponentInChildren<TextMeshProUGUI>().text = "You Win";
            notificationText.SetActive(true);
            await Task.Delay(1000);
            AudioSource.PlayClipAtPoint(victorySound, transform.position, 1f);
        }
    }

    private void SetScore()
    {
        scoreText.text = "Score: " + score.ToString() + " / " + winScore.ToString();
    }
}
