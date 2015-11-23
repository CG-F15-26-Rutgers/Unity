using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class NewConvo : MonoBehaviour {

    public GameObject person1;
    public GameObject person2;

    private BehaviorAgent agent;


    void Start()
    {
       
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.E) == true)
        {
            agent = new BehaviorAgent(conversationTree());
            BehaviorManager.Instance.Register(agent);
            agent.StartBehavior();
        }
    }


    #region
    //
    //Root Node
    //
    protected Node conversationTree()
    {
        Val<Vector3> P1Pos = Val.V(() => person1.transform.position);
        Val<Vector3> P2Pos = Val.V(() => person2.transform.position);

        return new Sequence(OrientAndWave(P1Pos, P2Pos), WalkAndTalk(P1Pos));
    }
    #endregion

    #region
    //
    //Root Children
    //
    protected Node OrientAndWave( Val<Vector3> P1Pos, Val<Vector3> P2Pos )
    {
        return new Sequence(person1.GetComponent<BehaviorMecanim>().Node_OrientTowards(P2Pos) , P1Wave(),
                            person2.GetComponent<BehaviorMecanim>().Node_OrientTowards(P1Pos) , P2Wave());
    }


    protected Node WalkAndTalk( Val<Vector3> P1Pos )
    {
        return new Sequence(person2.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(P1Pos, 3.0f), Talk());
    }
    #endregion



    #region
    //
    //Control Nodes
    //
    protected Node P1Wave()
    {
        return new Sequence(person1.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 1000));
    }

    protected Node P2Wave()
    {
        return new Sequence(person2.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 1000));
    }

    protected Node Talk()
    {
        return new DecoratorLoop(3, 
            new SequenceShuffle(
                person1.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("/ADAPTMan/ADAPTMan@happy_hand_gesture", 5000),
                person1.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("ADAPTMan@being_cocky", 5000),
                person2.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("ADAPTMan@shaking_head_no", 5000)
            ));
    }
    #endregion



}
