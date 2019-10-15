using UnityEngine;
using System.Collections;

public class Lprince_ctrl : MonoBehaviour {
	
	
	private Animator anim;
	private CharacterController controller;
	private int battle_state = 1;
	public float speed = 1.0f;
	public float runSpeed = 3.0f;
	public float turnSpeed = 90.0f;
	public float gravity = 20.0f;
	private Vector3 moveDirection = Vector3.zero;
	private float w_sp = 0.0f; //walk speed
	private float r_sp = 0.0f; //run speed
	private float timer = 10f;

	
	// Use this for initialization
	void Start () 
	{						
		anim = GetComponent<Animator>();
		controller = GetComponent<CharacterController> ();

		w_sp = speed; //read walk speed
		r_sp = runSpeed; //read run speed

	}
	
	// Update is called once per frame
	void Update () 
	{		

		if (Input.GetKey ("1"))  // turn to still state
		{ 		
			anim.SetInteger ("battle", 1);
			battle_state = 1; // without weapon
		}
		if (Input.GetKey ("2")) // turn to battle state with walking
		{ 
			anim.SetInteger ("battle", 2);
			battle_state = 2; //with weapon
			
		}
		if (Input.GetKey ("3")) // turn to battle state with run
		{ 
			anim.SetInteger ("battle", 0);
			battle_state = 0; //still
						
		}

//---------------------------------------------------------------- MOVING FORWARD
		if (Input.GetKey ("up")) 
		{	
			if (battle_state == 0) 
			{
				runSpeed = 0;
			}

			if (battle_state == 1 ) // NO WEAPON
			{	
				if (Input.GetKey (KeyCode.LeftShift))
				{		
					anim.SetInteger ("moving", 2);//run
					runSpeed = r_sp;

				}
				else if(Input.GetKey (KeyCode.LeftControl))
				{
					anim.SetInteger ("moving", 20);//push
					runSpeed = r_sp/3;
				}
				else
				{
					anim.SetInteger ("moving", 1);//walk
					runSpeed = w_sp;
				}
			} 	

			if (battle_state == 2) //WITH WEAPON
			{
				if (Input.GetKey (KeyCode.LeftShift))
				{		
					anim.SetInteger ("moving", 4);//run
					runSpeed = r_sp;
					
				}else
				{
					anim.SetInteger ("moving", 3);//walk
					runSpeed = w_sp;
				}
			}
		}
		else if (Input.GetKey ("down")) 
		{			
			anim.SetInteger ("moving", 22);//run
			runSpeed = w_sp/2;	
		}
		else
		{
			anim.SetInteger ("moving", 0);
		}

		if (Input.GetMouseButtonDown (0)) //action1
		{
			if (battle_state==0) 
			{
				anim.SetInteger ("moving", 5);
			}
			if (battle_state==1) 
			{
				anim.SetInteger ("moving", 6);
			}
			if (battle_state==2) 
			{
				anim.SetInteger ("moving", 7);
			}
		}
		if (Input.GetMouseButtonDown (1)) //action2
			{
			if (battle_state==1) 
			{
				anim.SetInteger ("moving", 8);
			}
			if (battle_state==2) 
			{
				anim.SetInteger ("moving", 9);
			}
		}
		if (Input.GetMouseButtonDown (2)) //action3
		{
			if (battle_state==1) 
			{
				anim.SetInteger ("moving", 10);
			}
			if (battle_state==2) 
			{
				anim.SetInteger ("moving", 11);
			}
		}

		if (Input.GetKeyDown ("u")) //hit
		{   
			if (battle_state == 1)
			{
				int n = Random.Range (0, 2);
				if (n == 0) 
				{
					anim.SetInteger ("moving", 12);
				} 
				else 
				{
					anim.SetInteger ("moving", 13);
				}
			} 
			if (battle_state == 2)
			{
				int n = Random.Range (0, 2);
				if (n == 0) 
				{
					anim.SetInteger ("moving", 14);
				} 
				else 
				{
					anim.SetInteger ("moving", 15);
				}
				
			}
		}
		if (Input.GetKeyDown ("space")) //jump
		{ 
			if (battle_state == 1) 
			{
				anim.SetInteger ("moving", 16);
			}
			if (battle_state == 2) 
			{
				anim.SetInteger ("moving", 17);
			}
		}

		if (battle_state == 2) 
		{
			if (Input.GetKeyDown ("l")) { //defence_start
				anim.SetInteger ("moving", 18);
			}
			if (Input.GetKeyUp ("l")) { //defence_end
				anim.SetInteger ("moving", 19);
			}	 
		}

		if (Input.GetKeyDown ("o")) { //death
			anim.SetInteger ("moving", 99);
		}
		if (Input.GetKeyDown ("p")) { //RESURECT ( TECHNICAL )
			anim.SetInteger ("moving", 98);
			anim.SetInteger ("battle", 1);
			battle_state = 1;
		}

		if (Input.GetKeyDown ("m")) { //PICK UP
			anim.SetInteger ("moving", 21);
		}
		//--------------------------------------------------------------HANDS_UP
		
		if (Input.GetKeyDown ("g")) 
		{
			anim.SetLayerWeight(1,1f);
			anim.SetInteger ("moving", 23);
			timer=30;
		}
		if (Input.GetKeyUp ("g")) 
		{
			anim.SetInteger ("moving", 24);
			timer-= Time.deltaTime;
			//anim.SetLayerWeight(1,0f);
		}	 

		if (timer <= 0f )
		{
			anim.SetLayerWeight(1,0f);
		}
		//----------------------------------------------------------------------------------------


		if (controller.isGrounded) 
		{
			moveDirection=transform.forward * Input.GetAxis ("Vertical") * speed * runSpeed;
			float turn = Input.GetAxis("Horizontal");
			transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);						
		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move (moveDirection * Time.deltaTime);
		}
}



