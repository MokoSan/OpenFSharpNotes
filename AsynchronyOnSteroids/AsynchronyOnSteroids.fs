module AsynchronyOnSteroids

(*
    1. Eric Lynn - Delta Point @deltapoint
    2. Start the cool bits -> Learn the language from that. 
    3. The Goal book from Goldratt's Dice Game -- Theory of Constraints
        1. Running everything in max capacity is not a good idea. 
        2. Dependency and variability => statistically unintuitive
        3. Even though we have capacity, we won't have the resources.
    4. Mailbox Processor
        1. Light weight thread 
            a. State, Thread of Execution, Mechanism to Communicate 
            b. Inorder Message Queue for Communication 
            c. Asynchronous workflow - message loop -- 
                1. look up the message queue, nothing there? Go to sleep
        2. F# Snippet  
        3. Out of order because of Async
        4. Providing isolation layer -- treat it as its single threaded in there.
        5. Post a message to get state.
        6. AsyncReplyChannel - pistol with a single shot -- handle to return method. 
            for looking up state.
*)

type BinMessage =
    | Add of int
    | GetInventoryCount of AsyncReplyChannel<int>

let createBin startingInventory = 
    // This mailbox processor only accepts ints.
    MailboxProcessor<BinMessage>.Start(fun inbox ->
        async {
            let mutable widgetCount = startingInventory

            while true do
                let! msg = mbox.Receive()
                match msg with
                | Add updateAmount ->
                    widgetCount <- widgetCount + updateAmount
                | GetInventoryCount chnl ->
                    chnl.Reply widgetCount
        })

[<EntryPoint>]
let main argv =
    printfn "%A" argv
    0 // return an integer exit code