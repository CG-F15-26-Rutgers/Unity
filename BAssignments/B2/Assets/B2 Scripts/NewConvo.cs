using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class NewConvo : MonoBehaviour {

    public GameObject person1;
    public GameObject person2;
    public GameObject deltrese;
    public Transform DeltreseGoTo;

    private BehaviorAgent agent;


    void Start()
    {
       
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.E) == true)
        {
            Val<Vector3> P1Pos = Val.V(() => person1.transform.position);
            Val<Vector3> P2Pos = Val.V(() => person2.transform.position);
            Val<Vector3> P3Pos = Val.V(() => deltrese.transform.position);

            agent = new BehaviorAgent(conversationTree(P1Pos, P2Pos, P3Pos));
            BehaviorManager.Instance.Register(agent);
            agent.StartBehavior();
        }
    }


    #region
    //
    //Root Node
    //
    protected Node conversationTree(Val<Vector3> P1Pos, Val<Vector3> P2Pos, Val<Vector3> P3Pos)
    {
        return new Sequence(OrientAndWave(P1Pos, P2Pos), WalkAndTalk(P1Pos, P2Pos), 
            new SequenceParallel( DeltreseWalkTo(DeltreseGoTo) , ThatDamnDeltrese(P3Pos)),
                Argue(P1Pos , P2Pos, P3Pos)
            );
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


    protected Node WalkAndTalk( Val<Vector3> P1Pos , Val<Vector3> P2Pos)
    {
        return new Sequence(person2.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(P1Pos, 3.0f),
                            person2.GetComponent<BehaviorMecanim>().Node_OrientTowards(P1Pos) , 
                            person1.GetComponent<BehaviorMecanim>().Node_OrientTowards(P2Pos) , Talk());
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


    protected Node P1isBored()
    {
        return new Sequence( person1.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("texting", 5000) );
    }


    protected Node ThatDamnDeltrese(Val<Vector3> P3Pos)
    {
        return new SequenceParallel( person1.GetComponent<BehaviorMecanim>().Node_HeadLook(P3Pos) , 
                                     person2.GetComponent<BehaviorMecanim>().Node_HeadLook(P3Pos) , 
                                     person1.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("angry", 1000),
                                     person2.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("angry", 1000)
                                     );
    }


    protected Node DeltreseWalkTo(Transform target)
    {
        return new Sequence(deltrese.GetComponent<BehaviorMecanim>().Node_GoTo(target.position));
    }


    protected Node Talk()
    {
            return new SequenceShuffle(
                person1.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("beingcocky", 5000),
                person1.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("cheer", 5000),
                person2.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("clap", 5000),
                person1.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("think", 5000),
                new SequenceParallel(P1isBored() , person2.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("cry" , 5000))
                //person2.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("lookawaygesture", 5000) 
            );
    }


    protected Node Argue( Val<Vector3> P1Pos, Val<Vector3> P2Pos, Val<Vector3> P3Pos )
    {
        return new DecoratorLoop(
            new SequenceParallel(
            person1.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("dismissinggesture" , 3000),
            person2.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("shakingheadno" , 3000),
            deltrese.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("headnodyes" , 3000)));
    }
    #endregion



}

