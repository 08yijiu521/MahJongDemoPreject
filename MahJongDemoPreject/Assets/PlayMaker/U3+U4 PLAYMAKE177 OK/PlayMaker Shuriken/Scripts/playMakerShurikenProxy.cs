using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class playMakerShurikenProxy : MonoBehaviour {
	

	private ParticleCollisionEvent[] collisionEvents = new ParticleCollisionEvent[16];
	
	private PlayMakerFSM _fsm;
	
	void Start()
	{
		_fsm = this.GetComponent<PlayMakerFSM>();
		
		if (_fsm==null)
		{
			Debug.LogError("No Fsm found",this);
		}

	}
	
	public ParticleCollisionEvent[] GetCollisionEvents()
	{
		return collisionEvents;
	}
	
	
    void OnParticleCollision(GameObject other) {
		
        ParticleSystem particleSystem;
		
        particleSystem = other.GetComponent<ParticleSystem>();
		
        int safeLength = particleSystem.GetSafeCollisionEventSize();
       // if (collisionEvents.Length < safeLength)
           
		
		collisionEvents = new ParticleCollisionEvent[safeLength];
		int numCollisionEvents = particleSystem.GetCollisionEvents(gameObject, collisionEvents);
		
	
        
		FsmEventData _data = new FsmEventData();
		_data.GameObjectData = other;
		_data.IntData = numCollisionEvents;
		PlayMakerUtils.SendEventToGameObject(_fsm,this.gameObject,"ON PARTICLE COLLISION");
		
		
		/*
       
        int i = 0;
        while (i < numCollisionEvents) {
            if (gameObject.rigidbody) {
                Vector3 pos = collisionEvents[i].intersection;
                Vector3 force = collisionEvents[i].velocity * 10;
                gameObject.rigidbody.AddForce(force);
            }
            i++;
        }
        
		*/
	}
}
