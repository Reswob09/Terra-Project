using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundBehavior : MonoBehaviour
{
    public Controller controller;
    public AudioSource audioSource;
    public AudioClip slowSpeedAudioClip;
    public AudioClip fastSpeedAudioClip;
    public AudioClip raycastHitAudioClip;
    private float currentSpeed;
    private float currentVelocity;
    private float shootHit;
    private float maxSpeed;
    public float musicTimer = 60f;
    void Start()
    {
      audioSource = gameObject.GetComponent<AudioSource>();
      audioSource.Play(0);
      Debug.Log("Audio Script Started");
    musicTimer -= Time.fixedDeltaTime;
    }

    void Update()
    {
        if(currentSpeed > maxSpeed)
        {
           OnPlayerMove(); 
        }
    }

    void OnPlayerMove()
    {
        GetComponent<Rigidbody>();
       // audioSource.(slowSpeedAudioClip,1);
    }
// put audio clip here ^^^^

    void OnHUD()
    {
        if( GUI.Button(new Rect(10, 70, 150, 30), "Pause"))
        {
            audioSource.Pause();
            Debug.Log("Pause:" + audioSource.time);
        }
      if (GUI.Button(new Rect(10, 170, 150, 30), "Continue"))
        {
            audioSource.UnPause();
        }
    }
}
