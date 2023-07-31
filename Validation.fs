namespace ILLYSystems

module Validation =
    open System
    open ILLYSystems.Model

    type ValidationResult<'t, 'err> =
        | OK of 't
        | Error of 'err

    type ValidationError = 
        | MustBeEntered
        | MaxLength
        | FutureDate
        | Before1905

    type PersonValidations =
        | Name of ValidationError list
        | DOB of ValidationError list

    let private validateName (input: string option) =
        printfn "Validating Name: %A" input 
        match input with
        | Some name ->
            printfn "Name: %s, Length: %d" name (String.length name)
            let errors =
                if String.IsNullOrEmpty name then [MustBeEntered]
                else if String.length name > 100 then [MaxLength]
                else []
            OK (Name errors)
        | None -> OK (Name [MustBeEntered])

    let private validateDOB (input: DateTime option) =
        match input with
        | Some dob ->
            let now = DateTime.Now.Date
            let errors =
                if dob > now then [FutureDate]
                else if dob.Year < 1905 then [Before1905]
                else []
            OK (DOB errors)
        | None -> OK (DOB [MustBeEntered])

    let private combineErrors (nameErrors: ValidationError list) (dobErrors: ValidationError list) =
        let combinedErrors = Name (nameErrors @ dobErrors)
        Error combinedErrors

    let validate (input: PersonInput) =
        let nameValidation = validateName input.Name
        let dobValidation = validateDOB input.DOB

        match nameValidation, dobValidation with
        | OK (Name nameErrors), OK (DOB dobErrors) ->
            if List.isEmpty nameErrors && List.isEmpty dobErrors then
                OK {
                    Name = Option.defaultValue "" input.Name;
                    DOB = Option.defaultValue DateTime.MinValue input.DOB;
                    Borough = input.Borough
                }
            else
                combineErrors nameErrors dobErrors
        | OK (Name nameErrors), Error dobErrors ->
            combineErrors nameErrors dobErrors
        | Error nameErrors, OK (DOB dobErrors) ->
            combineErrors nameErrors dobErrors
        | Error nameErrors, Error dobErrors ->
            combineErrors nameErrors dobErrors
