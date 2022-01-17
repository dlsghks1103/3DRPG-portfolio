using UnityEngine;

public class bl_ControllerExample : MonoBehaviour {

   [SerializeField]private bl_Joystick Joystick;//Joystick reference for assign in inspector

    [SerializeField]private float Speed = 5;

    private float horizontal;
    public float Horizontal
    {
        get 
        {
            return horizontal;
        }
    }

    private float vertical;
    public float Vertical
    {
        get
        {
            return vertical;
        }
    }

    void Update()
    {

        vertical = Joystick.Vertical; //get the vertical value of joystick
        horizontal = Joystick.Horizontal;//get the horizontal value of joystick

        //Vector3 translate = (new Vector3(h, 0, v) * Time.deltaTime) * Speed;
        //transform.Translate(translate);
    }
}