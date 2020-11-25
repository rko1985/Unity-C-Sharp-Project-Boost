using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    Rigidbody rigidBody;
    AudioSource audioSource;
    int speed = 10;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //todo somewhere stop sound on death
        if(state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive) { return; } // ignore collisions when dead       

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                print("OK");
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f); //parameterise time
                break;
            default:
                print("Hit something deadly");
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f); //parameterise time
                //kill player
                break;
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); //todo allow for more than 2 levels
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) //can thrust while rotating
        {
            print("Thrusting");
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()    {

        rigidBody.freezeRotation = true; //take manual control of rotation
                
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            print("Rotating left");
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("Rotating right");
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //resume physics control of rotation
    }

    
}
