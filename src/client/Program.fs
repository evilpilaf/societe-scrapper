open ReadExcel
open CompanySearch

type CompanyInfo ={
    Name: string
    SIRET: string
}

[<EntryPoint>]
let main args =
    let searchResult = getCompanyResults "sci long fleuve" |> searchForCompany
    match searchResult with
    | Some x -> printfn "the link is %s" x
    | None -> printfn "Nothing found 😢"
    0 // return an integer exit code
