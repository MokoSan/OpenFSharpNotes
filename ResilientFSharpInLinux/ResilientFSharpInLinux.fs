module ResilientFSharpInLinux

(*
    1. Resilient: Able to withstand or recover quickly from difficult conditions especially fucked up processes
    2. Unlikely events happen all the time.
    3. systemd - keeps processes running by PID
        1. c-groups: constraints on resources
        2. Interact with signals
        3. Track processes
    4. jounalid - structured logging daemon
    5. docker - standarized immutable infrastructure containers
    6. kubernetes - distributed scheduler
        1. Robust for docker containers 
        2. 3 instances of jobs for backups
    7. Need to latch on the Shutdown handler to clean up on application exit to use systemd 
    8. Suave used as a Restful api to check the status of the processes 
    9. minikube - bootstraps a kubernetes environment written in Go
    10. Include healthcheck APIs - http if possible
    11. Aware of target Runtime differences
*)

[<EntryPoint>]
let main argv =
    printfn "%A" argv
    0 // return an integer exit code