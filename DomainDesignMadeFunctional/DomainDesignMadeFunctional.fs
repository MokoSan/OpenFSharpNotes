module DomainDesignMadeFunctional

(*
    1. Not understanding the problem -- nothing to do with programming language.

    2. Reduce Garbage In -- less garbage out.

    3. F# Type system wins all.

    4. Domain Driven Design - Eric Evans -- Focus on Business need based on the domain.

    5. F# is great Boring Line of Business Applications [ BLOBA ] 
        1. Express requirements clearly
            a. Define Requirements in Code itself
        2. Rapid development cycle
            a. REPL great for Protoyping
        3. High quality deliverables
            a. Type system.    

    6. Domain Driven Design
        1. Communication is key. Everyone shares the same model. 
        2. Domain Export -> Developer -> Code all in sync.
        3. Same word means two different things - context dependent meaning : Bounded Context. 
            a. Spam -- SuperMarket vs. Spam -- Email
        4. Use same language as domain experts do. 
        5. Persistence Ignore - No databases.
        6. The design is the code and code is the design.
        7. Domain == Code == Execution

    7. F# Type System
        1. Composable Type System since types don't have behavior
        2. 2 different ways of making types
            a. Multiply - first pile x second pile -- tuple eg. int * int 
                i. Birthdays = Set of People x Set of Dates 
            b. Add - first pile or second pile -- choice via discriminated unions 
                i. Temperatures as either F or C 

        3. What are types for:
            a. Annotation for type checking.
            b. Domain Modelling
            c. BOTH THE SAME THING..

        4. Good static type system is like having compile time unit tests.

        5. TYPE ALL THE THINGS.

    8. Domain Modelling with Types
        1. Nulls shouldn't be in the type system as it is pretentious
        2. Use options 
        3. Null is the Saruman of Static Typing.
        4. Option<'T> has Some or None
        5. Single Choice types to keep distinct types
            a. type EmailAddress = EmailAddress of string
        6. Constraints can be added as well through options.
            a. Not all strings can be turned into Email Addresses. 
            b. 9999 things in your shopping cart -- OrderLineQuantity of int with constaints of size.
            c. There is no problem that can't be solved by wraping it in another type.
            d. Make illegal states unrepresentable.
            e. Requirements encoded in the type system.

    9. Conclusion
        1. Types are self documentation on the basis of the Domain 
        2. Constraints are explicits
        3. Types can encode business rules

    fsharpforfunandprofit.com/ddd
    Domain Modeling Made Functional -- book..
*)

[<EntryPoint>]
let main argv =
    printfn "%A" argv
    0 // return an integer exit code