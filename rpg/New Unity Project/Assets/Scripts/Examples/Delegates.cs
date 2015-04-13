using System;
using System.Collections.Generic;
using UnityEngine;
public class Delegates
{

    #region ConfigurablePattern
    //define delegate method signature
    delegate void RobotAction();
    //private property for delegate use
    RobotAction myRobotAction;

    void Start () 
    {
        //Set the default method for the delegate
        myRobotAction = RobotWalk;
    }

    void Update()
    {
        //Run the selected delegate method on update
        myRobotAction();
    }

    //public method to tell the robot to walk
    public void DoRobotWalk()
    {
        //set the delegate method to the walk function
        myRobotAction = RobotWalk;
    }
    
    void RobotWalk()
    {
        Debug.Log("Robot walking");
    }

    //public method to tell the robot to run
    public void DoRobotRun()
    {
        //set the delegate method to the run function
        myRobotAction = RobotRun;
    }
    
    void RobotRun()
    {
        Debug.Log("Robot running");
    }
    #endregion

    #region Delegation pattern
    public class Worker
    {
        List<string> WorkCompletedfor = new List<string>();
        public void DoSomething(string ManagerName, Action myDelegate)
        {
            //Audit that work was done for which manager
            WorkCompletedfor.Add(ManagerName);
            
            //Begin work
            myDelegate();
        }
    }

    public class Manager
    {
        private Worker myWorker = new Worker();

        public void PeiceOfWork1()
        {
            //A piece of very long tedious work
        }

        public void PeiceOfWork2()
        {
            //You guessed it, yet more tedious work
        }

        public void DoWork()
        {
            //Send worker to do job 1
            myWorker.DoSomething("Manager1",PeiceOfWork1);

            //Send worker to do Job 2
            myWorker.DoSomething("Manager1", PeiceOfWork2);
        }

        public void DoWork2()
        {
            //Send worker to do job 1
            myWorker.DoSomething("Manager1", () =>
                {
                    //A peice of very long tedious work
                });

            //Send worker to do Job 2
            myWorker.DoSomething("Manager2", () =>
                {
                    //You guessed it, yet more tedious work
                });
        }
    }
    #endregion

    #region Dynamic pattern

    public class WorkerManager
    {
        void DoWork()
        {
            DoJob1();
            DoJob2();
            DoJob3();
        }

        private void DoJob1()
        {
            //Do some filing
        }

        private void DoJob2()
        {
            //Make coffee for the office
        }

        private void DoJob3()
        {
            //Stick it to the man
        }
    }

    //A more intellegent WorkerManager
    public class WorkerManager2
    {
        //WorkerManager delegate
        delegate void MyDelegateHook();
        MyDelegateHook ActionsToDo;
        
        public string WorkerType = "Peon";

        //On Startup, assign jobs to the worker, note this is configurable instead of fixed
        void Start()
        {
            //Peons get lots of work to do
            if (WorkerType == "Peon")
            {
                ActionsToDo += DoJob1;
                ActionsToDo += DoJob2;
            }
            //Everyone else plays golf
            else
            {
                ActionsToDo += DoJob3;
            }
        }

        //On Update do the actions set on ActionsToDo
        void Update()
        {
            ActionsToDo();
        }

        private void DoJob1()
        {
            //Do some filing
        }

        private void DoJob2()
        {
            //Make coffee for the office
        }

        private void DoJob3()
        {
            //Play Golf
        }
    }


    #endregion

}
