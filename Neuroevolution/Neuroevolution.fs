module Neuroevolution

(*
    1. Jeremy Bellows - JeremyBellows.com
    2. Handbook to Neural Evolution through Erlang. 
    3. Built: NeuralFish - Genetic Algorithm 
    4. Neuroevolution - Taking a brain, putting in digital realm - evolving it  
    5. Mailbox processor is like a Neuron 
    6. Barrier -> Sum of Signal -> Activation Function
    7. Bird Sees -> Data to Neuron -> Excitement -> Wings respond 
    8. Sensors -> Neural Layer 1 -> Neural Layer 2 -> Actuator
    9. Evolution - mutations 
        a. Sensors added and connected / disconnected
        b. Mutations added and connection / disconnected
*)

[<EntryPoint>]
let main argv =
    printfn "%A" argv
    0 // return an integer exit code
