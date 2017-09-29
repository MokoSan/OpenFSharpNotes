module BotFramework

(*
    1. Async Impedence mismatch between C# and F#
    2. Gateway to multiple channel using a Web API service 
    3. Interchange text messages 
    4. Generic protocol from your service to the Bot Connector Service
    5. Flow of conversation as dialogs 
    6. Heavy OO and Async - Await
    7. Root Dialog <- Starting Point
    8. New order dialogs from the root 
    9. Annotate everything
*)

[<EntryPoint>]
let main argv =
    printfn "%A" argv
    0 // return an integer exit code