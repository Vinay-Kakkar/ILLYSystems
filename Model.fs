namespace ILLYSystems

module Model =
    open System
    
    type PersonInput = {
        Name: string option;
        DOB: DateTime option;
        Borough: int option
    }

    type ValidPerson = {
        Name: string
        DOB: DateTime
        Borough: int option
    }
