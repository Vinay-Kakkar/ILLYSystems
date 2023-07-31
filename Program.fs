namespace ILLYSystems

module Program = 

    open ILLYSystems.Validation
    open ILLYSystems.Model
    open System

    let runExample () =
        let input : PersonInput = {
            Name = Some "Vinay";
            DOB = Some (DateTime(2000, 7, 19));
            Borough = Some 12
        }
        match Validation.validate input with
        | OK (person : ValidPerson) ->
            printfn "Validation successful. Person: %A" person
        | Error errors ->
            printfn "Validation failed. Errors: %A" errors

    [<EntryPoint>]
    let main argv =
        runExample ()
        0 
